using Leopotam.Ecs;
using UnityEngine;
using Zlode.States;
using static UnityEngine.EventSystems.EventTrigger;

namespace Zlodey
{
	public sealed class HealthViewSystem : IEcsRunSystem
	{
		private readonly EcsFilter<Health, HealthViewRef, HitReaction> _health;
		private readonly EcsFilter<HealthViewRef> _healthView;

		private readonly RuntimeData _runtimeData;
		public void Run()
		{
			foreach (var i in _healthView)
			{
				var health = _healthView.Get1(i).value;

				if (health == null) continue;
				health.transform.LookAt(_runtimeData.CurrentCamera.transform);
			}


			foreach (var i in _health)
			{
				var health = _health.Get1(i);
				var healthView = _health.Get2(i).value;
				healthView.SetValue(health.CurrentHealth, health.MaxValue);
			}
		}
	}

	public sealed class HitSystem : IEcsRunSystem
	{
		private readonly EcsFilter<HitableTag, AI, HitReaction>.Exclude<HitTimer> _hitBeginTimer;
		private readonly EcsFilter<HitTimer, AI> _hitTimer;
		private readonly EcsFilter<ProgressAttackViewRef, HitTimer> _hitArcherTimer;
		public void Run()
		{


			foreach (var i in _hitBeginTimer)
			{

				var randomChance = Random.Range(0f, 1f);

				if (randomChance < 0.1f)
				{
					var ai = _hitBeginTimer.Get2(i).value;
					ai.canMove = false;


					var entity = _hitBeginTimer.GetEntity(i);
					entity.Get<HitTimer>().value = Time.time + 1.2f;
					entity.Del<AttackEvent>();
				}

			}

			foreach (var i in _hitArcherTimer)
			{
				var view = _hitArcherTimer.Get1(i).value;
				view.SetState(false);
			}

			foreach (var i in _hitTimer)
			{
				ref var hitTimer = ref _hitTimer.Get1(i).value;
				if (hitTimer < Time.time)
				{
					var ai = _hitTimer.Get2(i).value;

					ai.canMove = true;

					var entity = _hitTimer.GetEntity(i);

					entity.Del<HitTimer>();
				}
			}
		}
	}

	public sealed class AnimationSystem : IEcsRunSystem
	{
		private readonly EcsFilter<InputPlayer , AnimatorRef, AttackDir>.Exclude<DiedState> _attackAnimation;

		private readonly EcsFilter<AnimatorRef, Velocity>.Exclude<AttackDir, DiedState> _moveAnimation;
		public void Run()
		{
			foreach (var i in _attackAnimation)
			{

				var entity = _attackAnimation.GetEntity(i);
				if (entity.Has<DiedState>() || entity.Has<DiedEvent>())
				{
					continue;
				}

				if (entity.Has<CurrentWeapon>())
				{
					var weapon = entity.Get<CurrentWeapon>().Entity;

					if (weapon.Has<Reload>())
					{
						continue;
					}

				}

				var animator = _attackAnimation.Get2(i).value;
				animator.SetTrigger(AnimationConst.Attack);
			}


			foreach (var i in _moveAnimation)
			{
				var animator = _moveAnimation.Get1(i).value;
				var velocity = _moveAnimation.Get2(i).value;
				animator.SetBool(AnimationConst.Walk, velocity != Vector3.zero);
			}

		}
	}
}