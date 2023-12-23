using Cysharp.Threading.Tasks;
using LeopotamGroup.Globals;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace Zlodey.Actors
{
	public sealed class DialogMono : MonoBehaviour
	{
		public DialogStructure[] Dialog;

		private int _currentDialog;

		public float TextInterval;

		private WaitForSeconds _seconds;
		private Coroutine _currentCoroutine;

		public Camera RawCamera;

		private static Vector3 OffsetCamera = new Vector3(0, 1.36699998f, 1.08500004f);

		private void Start()
		{
			_seconds = new WaitForSeconds(TextInterval);
		}
		public void Next()
		{
			var dialogStructure = Dialog[_currentDialog];

			if (_currentCoroutine != null)
			{
				StopCoroutine(_currentCoroutine);
				_currentCoroutine = null;
				Service<UI>.Get().DialogScreen.SetText(dialogStructure.Description);
				return;
			}

			dialogStructure.EventsEndDialog?.Invoke();

			_currentDialog++;

			if (_currentDialog >= Dialog.Length)
			{
				Hide();
				return;
			}

			dialogStructure = Dialog[_currentDialog];

			RawCamera.transform.parent = dialogStructure.Unit;
			RawCamera.transform.localPosition = OffsetCamera;
			RawCamera.transform.localRotation = Quaternion.Euler(0, 180, 0);


			Service<UI>.Get().DialogScreen.SetValue(dialogStructure.Character, "");

			_currentCoroutine = StartCoroutine(MathText(dialogStructure.Description));
			dialogStructure.Events?.Invoke();
		}

		public IEnumerator MathText(string text)
		{
			int index = 0;

			while (index < text.Length)
			{
				yield return _seconds;
				Service<UI>.Get().DialogScreen.SetText(text.AsSpan()[..index].ToString());

				index++;
			}
		}

		public void Show()
		{
			var gameScreen = Service<UI>.Get().GameScreen;

			gameScreen.AttackJoystics.gameObject.SetActive(false);
			gameScreen.MoveJoystics.gameObject.SetActive(false);

			gameScreen.MoveJoystics.OnPointerUp(null);

			gameScreen.AttackJoystics.OnPointerUp(null);


			var dialogStructure = Dialog[_currentDialog];

			var dialogScreen = Service<UI>.Get().DialogScreen;
			dialogScreen.Show();
			dialogScreen.ButtonDialog.onClick.AddListener(Next);
			dialogScreen.SetValue(dialogStructure.Character, dialogStructure.Description);
			dialogStructure.Events?.Invoke();

			RawCamera.transform.parent = dialogStructure.Unit;
			RawCamera.transform.localPosition = OffsetCamera;
			RawCamera.transform.localRotation = Quaternion.Euler(0, 180, 0);

			RawCamera.gameObject.SetActive(true);
		}
		public void Hide()
		{
			var gameScreen = Service<UI>.Get().GameScreen;


			var staticData = Service<StaticData>.Get();
			
			if (!staticData.AutoAttack)
			{
				gameScreen.AttackJoystics.gameObject.SetActive(true);
			}

			gameScreen.MoveJoystics.gameObject.SetActive(true);


			var dialogScreen = Service<UI>.Get().DialogScreen;
			dialogScreen.ButtonDialog.onClick.RemoveListener(Next);
			dialogScreen.Show(false);

			RawCamera.gameObject.SetActive(false);
		}
	}
}