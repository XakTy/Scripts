using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Zlodey.Actors
{
	public sealed class BulletActor : EntityActor
	{
		public BulletData data;

		public ParticleSystem ParticleBullet;
		public ParticleSystem ParticleHit;

		public ArrowType Type;

		public enum ArrowType
		{
			Magic,
			Common,
		}

		protected override void InitComponents()
		{
			Entity.Get<BulletData>() = data;

			if (Type == ArrowType.Magic)
			{
				Entity.Get<MagicView>().Bullet = ParticleBullet;
				Entity.Get<MagicView>().Hit = ParticleHit;
			}
			else
			{
				Entity.Get<ArrowView>();
			}

		
			Entity.Get<TransformRef>().value = transform;
		}

		private void OnTriggerEnter(Collider other)
		{
			var bulletEvent = Service<EcsWorld>.Get().NewEntity();
			bulletEvent.Get<BulletEvent>() = new BulletEvent { ColliderEnter = other, EntityBullet = Entity };
		}

	}
}