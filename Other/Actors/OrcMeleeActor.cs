using Leopotam.Ecs;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

namespace Zlodey.Actors
{
	public sealed class OrcMeleeActor : Enemy
	{
		public float HealthValue;
		public RendersDissolve Renderers;
		public AttackMeleeData AttackData;
		public Animator Animator;
		public AIBase AIBase;
		protected override void InitComponents()
		{
			base.InitComponents();


			Entity.Get<OrcTag>();
			Entity.Get<AttackMeleeDataRef>().value = AttackData;
			Entity.Get<AnimatorRef>().value = Animator;
			Entity.Get<AI>().value = AIBase;
			Entity.Get<RendersDissolve>() = Renderers;
			Entity.Get<Health>() = new Health().SetValue(HealthValue);
		}


		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(TrRotate.position + TrRotate.forward * AttackData.Range, AttackData.Radius);
		}
	}
}