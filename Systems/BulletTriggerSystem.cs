using Cysharp.Threading.Tasks;
using Leopotam.Ecs;
using UnityEngine;
using Zlode.States;
using static UnityEngine.EventSystems.EventTrigger;
using Zlodey.Actors;

namespace Zlodey
{
	public sealed class BulletViewDiedSystem : IEcsRunSystem
	{
		private readonly EcsFilter<TransformRef, MagicView, DiedEvent> _viewMagic;

		private readonly EcsFilter<TransformRef, ArrowView, DiedEvent> _viewArrow;
		public void Run()
		{
			foreach (var i in _viewArrow)
			{
				var tr = _viewArrow.Get1(i).value;
				tr.gameObject.SetActive(false);

				var entity = _viewArrow.GetEntity(i);
				entity.Del<DiedEvent>();

				DestroyDelay(tr, entity).Forget();
			}

			foreach (var i in _viewMagic)
			{
				var tr = _viewMagic.Get1(i).value;
				ref var view = ref _viewMagic.Get2(i);

				if (view.isPlay) continue;
				view.isPlay = true;

				view.Bullet.Stop();

				view.Hit.transform.rotation = Quaternion.Euler(tr.forward);
				view.Hit.transform.SetParent(null);
				view.Hit.Play();

				DestroyDelay(tr, _viewMagic.GetEntity(i)).Forget();

				var entity = _viewMagic.GetEntity(i);
				entity.Del<DiedEvent>();
			}
		}

		private async UniTaskVoid DestroyDelay(Transform tr, EcsEntity entity)
		{
			await UniTask.Delay(1000, cancellationToken: tr.GetCancellationTokenOnDestroy());
			
			tr.gameObject.SetActive(false);

			if (entity.IsAlive())
			{
				entity.Destroy();
			}
		}
	}

	public sealed class BulletTriggerSystem : IEcsRunSystem
	{
		private readonly EcsFilter<BulletEvent> _bulletsTrigger;

		private readonly EcsWorld _world;

		private readonly int _obstacle = LayerMask.GetMask("Obstacle");
		public void Run()
		{
			foreach (var i in _bulletsTrigger)
			{
				var bulletEvent = _bulletsTrigger.Get1(i);
				var entityBullet = bulletEvent.EntityBullet;

				if (bulletEvent.ColliderEnter.gameObject.layer == 6)
				{
					entityBullet.Get<DiedEvent>();
					entityBullet.Del<AttackDir>();

					bulletEvent.EntityBullet.Get<TransformRef>().value.GetComponent<Collider>().enabled = false;
				}
				else if (bulletEvent.ColliderEnter.TryGetComponent<EntityActor>(out var actor))
				{
					var ownerEntity = entityBullet.Get<Owner>().Entity;
					if (ownerEntity.IsAlive() && actor.Entity.IsAlive() && actor.Entity.Has<Health>() && !actor.Entity.Has<DiedState>())
					{
						var bulletData = entityBullet.Get<BulletData>();

						ref var eventDamage = ref _world.NewEntity().Get<DamageEvent>();
						eventDamage.value = bulletData.damage;
						eventDamage.EntityA = entityBullet;
						eventDamage.EntityB = actor.Entity;

						entityBullet.Get<DiedEvent>();
						entityBullet.Del<AttackDir>();

						bulletEvent.EntityBullet.Get<TransformRef>().value.GetComponent<Collider>().enabled = false;
					}
				}

				var entityEvent = _bulletsTrigger.GetEntity(i);

				entityEvent.Destroy();
			}
		}
	}
}