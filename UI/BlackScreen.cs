using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Zlodey
{
	public sealed class BlackScreen : Screen
	{
		[SerializeField] private Image _splash;
		[SerializeField] private float _duration;

		public Tween tweenScreen;

		[Button("On")]
		public void On()
		{
			_splash.DOComplete();
			_splash.gameObject.SetActive(true);
			var color = Color.black;
			color.a = 0f;

			_splash.color = color;
			tweenScreen = _splash.DOColor(Color.black, _duration);

			tweenScreen.onComplete += () =>
			{
				tweenScreen = null;
			};
		}

		[Button("Off")]
		public void Off()
		{
			_splash.DOComplete();
			_splash.color = Color.black;
			var color = Color.black;
			color.a = 0f;

			_splash.DOColor(color, _duration).onComplete += () =>
			{
				_splash.gameObject.SetActive(false);
			};
		}
	}
}