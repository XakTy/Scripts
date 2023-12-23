using System;
using System.Numerics;
using Cysharp.Threading.Tasks;
using Leopotam.Ecs;
using UnityEngine;
using Zlodey.Actors;
using static UnityEngine.EventSystems.EventTrigger;
using Quaternion = UnityEngine.Quaternion;

namespace Zlodey
{
	public sealed class OrcMeleeAttackSystem : IEcsRunSystem
	{
		private readonly EcsFilter<OrcTag, AI, AttackMeleeDataRef, AttackEvent> _filter = default;
		private readonly EcsWorld _world;

		private readonly EcsFilter<ProgressAttackViewRef, AttackMeleeDataRef, TransformRotate, AttackEvent> _melee;
		public void Run()
		{

			foreach (var i in _melee)
			{
				var view = _melee.Get1(i).value;
				var data = _melee.Get2(i);
				var dir = _melee.Get3(i).value.forward;

				view.transform.rotation = Quaternion.LookRotation(dir);

				view.SetState(true);
				view.Full(data.value.DelayToAttack);
			}

			foreach (var i in _filter)
			{
				var ai = _filter.Get2(i).value;
				ai.canMove = false;
				ai.canSearch = false;

				var attackData = _filter.Get3(i).value;

				var entity = _filter.GetEntity(i);
				entity.Get<Reload>().value = 9999f;

				entity.Get<PreAttackInterval>().value = attackData.DelayToAttack;

			}
		}
	}
}