using Cysharp.Threading.Tasks;
using UnityEngine;
using Zlodey;
using Zlodey.Actors;
using Progress = Zlodey.Progress;

namespace InterfaceWay
{
	public sealed class EnterZone : MonoBehaviour
	{
		private bool _exit;
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent<Player>(out var player))
			{
				_exit = false;
				NextLevel().Forget();
			}
		}
		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent<Player>(out var player))
			{
				_exit = true;
			}
		}

		public async UniTaskVoid NextLevel()
		{
			await UniTask.Delay(500);
			if (_exit)
			{
				return;
			}



			Progress.CurrentLevel++;
			Levels.LoadCurrent();
		}
	}
}