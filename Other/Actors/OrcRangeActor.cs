using Leopotam.Ecs;
using Pathfinding;
using UnityEngine;

namespace Zlodey.Actors
{
	public sealed class OrcRangeActor : Enemy
	{
		public float HealthValue;
		public RendersDissolve Renderers;
		public AttackRangeData Attack;
		public Animator Animator;
		public AIBase AIBase;
		public Transform SpawnPoint;
		public ParticleSystem Bullet;
		protected override void InitComponents()
		{
			base.InitComponents();
			Entity.Get<OrcTag>();
			Entity.Get<OrcExplosion>();
			Entity.Get<OrcExplosionView>().View = this;
			Entity.Get<AttackRangeDataRef>().value = Attack;
			Entity.Get<AttackRangeDataRef>().StartPoint = SpawnPoint;
			Entity.Get<AnimatorRef>().value = Animator;
			Entity.Get<AI>().value = AIBase;
			Entity.Get<RendersDissolve>() = Renderers;
			Entity.Get<Health>() = new Health().SetValue(HealthValue);
		}


		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, Attack.Range);
		}
	}
}