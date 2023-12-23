using Leopotam.Ecs;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

namespace Zlodey.Actors
{
	public sealed class OrcArrowActor : Enemy
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

			Entity.Get<OrcArcher>();

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