using Leopotam.Ecs;
using UnityEngine;
using Zlode.States;
using Zlodey.Actors;

namespace Zlodey
{

	public sealed class TargetCheckerSystem : IEcsRunSystem
	{
		private readonly EcsFilter<Target> _filter;
		public void Run()
		{
			foreach (var i in _filter)
			{
				var target = _filter.Get1(i).TargetEntity;

				if (!target.IsAlive())
				{
					var entity = _filter.GetEntity(i);

					entity.Del<Target>();
					entity.Del<AttackState>();

					var arg = entity.Get<TransformRef>().value.GetComponentInChildren<AngryZone>();

					if (arg)
					{
						arg.Collider.enabled = false;
						arg.Collider.enabled = true;
					}
				}
			}
		}
	}

	public sealed class PlayerDiedSystem : IEcsRunSystem
	{
		private readonly EcsFilter<InputPlayer, TransformRef, CurrentWeapon, DiedEvent> _filter;
		private readonly EcsFilter<AI, AnimatorRef, IDFraction> _enemies = default;

		private readonly EcsFilter<Target> _filterEnemies;

		private readonly EcsWorld _world;
		public void Run()
		{
			if (_filter.IsEmpty()) return;


			foreach (var indexEnemy in _filterEnemies)
			{
				var entity = _filterEnemies.GetEntity(indexEnemy);
				entity.Del<Target>();
				entity.Del<AttackDir>();
			}

			foreach (var enemy in _enemies)
			{
				var id = _enemies.Get3(enemy).id;
				if (id == 0) continue;

				var animator = _enemies.Get2(enemy).value;
				animator.Play("Win");
			}

			_filter.Get3(0).Entity.Destroy();
			_filter.GetEntity(0).Destroy();

			_world.ChangeState(GameState.Lose);
		}
	}
}