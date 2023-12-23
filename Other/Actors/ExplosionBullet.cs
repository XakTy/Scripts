using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Zlodey.Actors
{
	public sealed class ExplosionBullet : EntityActor
	{
		public float Radius;
		public float TimeToExplosion;
		public float Damage;

		public Transform View;
		public ParticleSystem ExplosionParticle;
		protected override void InitComponents()
		{
			Entity.Get<ExplosionRef>().value = this;
			Entity.Get<ExplosionData>().RadiusExplosion = Radius;
			Entity.Get<ExplosionData>().TimeToExplosion = TimeToExplosion;
			Entity.Get<ExplosionData>().Damage = Damage;
		}


		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			View.localScale = new Vector3(Radius, Radius, 0);
			Gizmos.DrawWireSphere(transform.position, Radius);
		}
	}
}