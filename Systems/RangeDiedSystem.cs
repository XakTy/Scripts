using System;
using Cysharp.Threading.Tasks;
using Leopotam.Ecs;
using UnityEngine;

namespace Zlodey
{
	public sealed class RangeDiedSystem : IEcsRunSystem
	{
		private readonly EcsFilter<AttackRangeDataRef, RangeView, AttackEvent> _attack;
		private readonly EcsFilter<CircleView> _playerAttack;
		private readonly EcsFilter<RangeView, DiedEvent> _dieds;

		private readonly StaticData _staticData;
		public void Run()
		{
			foreach (var i in _playerAttack)
			{
				var view = _playerAttack.Get1(i);

				view.sprite.gameObject.SetActive(_staticData.AutoAttack);
			}

			foreach (var i in _attack)
			{
				var rangeView = _attack.Get2(i);
				var attackData = _attack.Get1(i);

				rangeView.value.gameObject.SetActive(true);

				Delay(rangeView.value.gameObject, attackData.value.Reload).Forget();
			}

			foreach (var i in _dieds)
			{
				var rangeView = _dieds.Get1(i);

				rangeView.value.gameObject.SetActive(false);
			}
		}

		public async UniTaskVoid Delay(GameObject gameObject, float time)
		{
			await UniTask.Delay(TimeSpan.FromSeconds(time));
			gameObject.SetActive(false);
		}
	}
}