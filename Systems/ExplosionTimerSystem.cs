using Leopotam.Ecs;
using UnityEngine;
using Zlodey.Actors;

namespace Zlodey
{
	public sealed class ExplosionTimerSystem : IEcsRunSystem
	{
		private readonly EcsFilter<ExplosionRef, ExplosionData, Timer> _filter = default;
		private readonly EcsFilter<ExplosionRef, ExplosionData, Timer> _view = default;
		private readonly EcsFilter<ExplosionRef, ExplosionData, Timer> _checkerTimer = default;
		private readonly RuntimeData _runtimeData = default;
		private readonly EcsWorld _world = default;

		public void Run()
		{
			foreach (var i in _filter)
			{
				ref var timer = ref _filter.Get3(i).value;
				var explosionBullet = _filter.Get1(i).value;

				timer -= _runtimeData.deltaTime;

				if (timer <= 0f)
				{
					var colliders = Physics.OverlapSphere(explosionBullet.transform.position, explosionBullet.Radius);

					if (colliders.Length > 0)
					{
						var explosionData = _filter.Get2(i);
						foreach (var collider in colliders)
						{
							if (collider.TryGetComponent<EntityActor>(out var actor))
							{
								if (!actor.Entity.IsAlive())
								{
									continue;
								}

								ref var damageEvent = ref _world.NewEntity().Get<DamageEvent>();

								damageEvent.EntityB = actor.Entity;
								damageEvent.value = explosionData.Damage;
							}
						}
					}
				}
			}

			foreach (var i in _view)
			{
				var timer = _view.Get3(i).value;
				var explosionBullet = _view.Get1(i).value;

				var percent = (explosionBullet.TimeToExplosion - timer) / explosionBullet.TimeToExplosion;
				var scale = new Vector3(percent, percent, 0);
				explosionBullet.View.localScale = scale;

				if (timer <= 0f)
				{
					explosionBullet.ExplosionParticle.Play();
					explosionBullet.ExplosionParticle.GetComponent<AudioSource>().Play();

					explosionBullet.View.parent.gameObject.SetActive(false);
				}
			}

			foreach (var i in _checkerTimer)
			{
				ref var timer = ref _checkerTimer.Get3(i).value;

				if (timer <= 0f)
				{
					var entity = _checkerTimer.GetEntity(i);
					entity.Del<Timer>();
				}
			}
		}
	}
}