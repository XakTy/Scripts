using Cysharp.Threading.Tasks;
using DG.Tweening;
using Leopotam.Ecs;
using System;
using UnityEngine;
using Zlodey.Actors;
using Object = UnityEngine.Object;

namespace Zlodey
{
	public sealed class OrcRangeExplosionSystem : IEcsRunSystem
	{
		private readonly EcsFilter<OrcExplosionView, Target, AttackEvent> _view;

		private readonly EcsFilter<OrcExplosion, AttackRangeDataRef, Target, AttackEvent> _explosion;

		private readonly EcsWorld _world;
		public void Run()
		{
			foreach (var i in _view)
			{
				var view = _view.Get1(i).View;

				var target = _view.Get2(i).value;

				var posSpawn = target.position;
				posSpawn.y = 0;

				view.Bullet.transform.SetParent(null);

				view.Bullet.transform.DOJump(posSpawn, 1f, 1, 1.7f).OnComplete(() =>
				{
					view.Bullet.gameObject.SetActive(false);
					view.Bullet.transform.position = view.SpawnPoint.position;
					view.Bullet.gameObject.SetActive(true);
				}).SetDelay(0.3f);
			}

			foreach (var i in _explosion)
			{
				var data = _explosion.Get2(i);
				var target = _explosion.Get3(i).value;

				var posSpawn = target.position;
				posSpawn.y = 0;

				var explosionBullet = Object.Instantiate(data.value.Bullet, posSpawn, Quaternion.identity);
				explosionBullet.Init(_world);

				explosionBullet.Entity.Get<Timer>().value = explosionBullet.Entity.Get<ExplosionData>().TimeToExplosion;

				var entity = _explosion.GetEntity(i);

				explosionBullet.Entity.Get<Owner>().Entity = entity;

				entity.Get<AI>().value.canMove = false;
				entity.Get<AI>().value.canSearch = false;

				entity.Get<Reload>().value = 9999999f;
				entity.Get<AfterAttackInterval>().value = 0.5f;
			}
		}
	}
}