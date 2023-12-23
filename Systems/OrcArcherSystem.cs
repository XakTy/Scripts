using Cysharp.Threading.Tasks;
using Leopotam.Ecs;
using System;
using UnityEngine;
using Zlodey.Actors;
using Object = UnityEngine.Object;

namespace Zlodey
{
	public sealed class TimerAttackSystem : IEcsRunSystem
	{
		private readonly RuntimeData _runtimeData;

		private readonly EcsFilter<PreAttackInterval> _preAttack;
		private readonly EcsFilter<AfterAttackInterval> _afterAttack;

		private readonly EcsFilter<PreAttackInterval, TransformRotate, AttackMeleeDataRef> _preMeleeAttack;
		private readonly EcsFilter<PreAttackInterval, AttackDir, AttackRangeDataRef, OrcArcher> _preArcherAttack;

		private readonly EcsFilter<AI, AttackRangeDataRef, AfterAttackInterval> _afterAIRangeAttack;
		private readonly EcsFilter<AI, AttackMeleeDataRef, AfterAttackInterval> _afterAIMeleeAttack;

		private readonly EcsWorld _world;
		public void Run()
		{
			foreach (var i in _preAttack)
			{
				ref var time = ref _preAttack.Get1(i).value;
				time -= _runtimeData.deltaTime;
			}

			foreach (var i in _afterAttack)
			{
				ref var time = ref _afterAttack.Get1(i).value;
				time -= _runtimeData.deltaTime;
			}



			foreach (var i in _afterAIRangeAttack)
			{
				ref var time = ref _afterAIRangeAttack.Get3(i).value;

				if (time <= 0f)
				{
					var entity = _afterAIRangeAttack.GetEntity(i);
					var ai = _afterAIRangeAttack.Get1(i).value;
					var reload = _afterAIRangeAttack.Get2(i).value.Reload;

					entity.Get<Reload>().value = reload;
					entity.Del<AfterAttackInterval>();

					ai.canMove = true;
					ai.canSearch = true;
				}
			}

			foreach (var i in _afterAIMeleeAttack)
			{
				ref var time = ref _afterAIMeleeAttack.Get3(i).value;

				if (time <= 0f)
				{
					var entity = _afterAIMeleeAttack.GetEntity(i);
					var ai = _afterAIMeleeAttack.Get1(i).value;
					var reload = _afterAIMeleeAttack.Get2(i).value.ReloadTime;

					entity.Get<Reload>().value = reload;
					entity.Del<AfterAttackInterval>();

					ai.canMove = true;
					ai.canSearch = true;
				}
			}

			foreach (var i in _preMeleeAttack)
			{
				ref var time = ref _preMeleeAttack.Get1(i).value;

				if (time <= 0f)
				{
					var attackMeleeDataRef = _preMeleeAttack.Get3(i);
					var tr = _preMeleeAttack.Get2(i).value;
					var attackData = attackMeleeDataRef.value;
					var entityOrc = _preMeleeAttack.GetEntity(i);
					var idFraction = entityOrc.Get<IDFraction>().id;

					var count = Physics.OverlapSphereNonAlloc(tr.position + tr.forward * attackData.Range, attackData.Radius, _runtimeData.CollidersPool);

					Span<Collider> colliders = new Span<Collider>(_runtimeData.CollidersPool, 0, count);

					foreach (var collider in colliders)
					{
						if (
							collider.transform == tr ||

							!collider.TryGetComponent<EntityActor>(out var actor) ||

							!actor.Entity.IsAlive() ||

							!actor.Entity.Has<IDFraction>() ||

							actor.Entity.Get<IDFraction>().id == idFraction
						)
						{
							continue;
						}


						var damageEvent = new DamageEvent();
						damageEvent.EntityA = entityOrc;
						damageEvent.EntityB = actor.Entity;
						damageEvent.value = attackData.Damage;

						var damageEntity = _world.NewEntity();
						damageEntity.Add(damageEvent);
					}

					entityOrc.Del<PreAttackInterval>();
					entityOrc.Get<AfterAttackInterval>().value = 0.5f;
				}
			}

			foreach (var i in _preArcherAttack)
			{
				var entityOrc = _preArcherAttack.GetEntity(i);
				var time = _preArcherAttack.Get1(i).value;
				var dir = _preArcherAttack.Get2(i).value;
				var attackRangeDataRef = _preArcherAttack.Get3(i);

				if (time <= 0f)
				{
					var attackData = attackRangeDataRef.value;
					var arrow = Object.Instantiate(attackData.Bullet, attackRangeDataRef.StartPoint.position, Quaternion.identity);
					arrow.Init(_world);

					arrow.Entity.Get<Owner>().Entity = entityOrc;

					var attackDir = new AttackDir() { value = dir };

					arrow.transform.rotation = Quaternion.LookRotation(attackDir.value);
					arrow.Entity.Get<AttackDir>() = attackDir;
					arrow.Entity.Get<LifeTimer>().value = Time.time + attackData.LifeTime;


					entityOrc.Del<PreAttackInterval>();
					entityOrc.Get<AfterAttackInterval>().value = 0.5f;
				}
			}
		}
	}

	public sealed class OrcArcherSystem : IEcsRunSystem
	{
		private readonly EcsFilter<OrcArcher, AttackRangeDataRef, AI, AttackEvent> _archer;
		private readonly EcsFilter<ProgressAttackViewRef, AttackRangeDataRef, AttackDir, AttackEvent> _archerView;

		private readonly EcsWorld _world;

		public void Run()
		{
			if (_archer.IsEmpty()) return;

			foreach (var i in _archerView)
			{
				var view = _archerView.Get1(i).value;
				var data = _archerView.Get2(i);
				var dir = _archerView.Get3(i).value;

				view.transform.rotation = Quaternion.LookRotation(dir);

				view.SetState(true);
				view.Full(data.value.AttackInterval);
			}


			foreach (var i in _archer)
			{
				var data = _archer.Get2(i);

				var archerEntity = _archer.GetEntity(i);

				var ai = _archer.Get3(i).value;
				ai.canMove = false;
				ai.canSearch = false;

				archerEntity.Get<PreAttackInterval>().value = data.value.AttackInterval;

				archerEntity.Get<Reload>().value = 999999f;
			}
		}
	}
}