using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;

namespace Zlodey
{
	public sealed class AttackPlayerView : IEcsRunSystem
	{
		private readonly EcsFilter<RangeView> _filter;

		private readonly RuntimeData _runtimeData;

		private readonly GameScreen _gameScreen = Service<UI>.Get().GameScreen;
		public void Run()
		{
			if (_filter.IsEmpty()) return;

			if (_gameScreen.AttackJoystics.IsPressed && _gameScreen.AttackJoystics.Input.magnitude > 0.8f)
			{
				ref var rangeView = ref _filter.Get1(0);
				rangeView.value.gameObject.SetActive(true);
				rangeView.value.rotation = Quaternion.LookRotation(rangeView.Direction);
			}
			else
			{
				var rangeView = _filter.Get1(0).value;
				rangeView.gameObject.SetActive(false);
			}
		}
	}
}