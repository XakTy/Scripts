using Leopotam.Ecs;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEngine.EventSystems.EventTrigger;

namespace Zlodey.Actors
{
	public sealed class OrcWarriorBoss : Enemy
	{
		public float HealthValue;
		public RendersDissolve Renderers;
		public AttackMeleeData Attack;
		public Animator Animator;
		public AIBase AIBase;
		public CircleProgressionView CircleView;
		[FormerlySerializedAs("ArcherView")] public ProgressAttackView _progressAttackView;
		public ParticleSystem ParticleRotate;
		protected override void InitComponents()
		{
			base.InitComponents();
			Entity.Get<OrcTag>();
			Entity.Get<OrcWarriorTag>().ParticleRotate = ParticleRotate;
			Entity.Get<SpellTornado>();
			Entity.Get<CircleViewRef>().value = CircleView;
			Entity.Get<ProgressAttackViewRef>().value = _progressAttackView;
			Entity.Get<AttackMeleeDataRef>().value = Attack;
			Entity.Get<AnimatorRef>().value = Animator;
			Entity.Get<AI>().value = AIBase;
			Entity.Get<RendersDissolve>() = Renderers;
			Entity.Get<Health>() = new Health().SetValue(HealthValue);
		}


		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(transform.position + transform.forward * Attack.Range, Attack.Radius);
		}
	}
}