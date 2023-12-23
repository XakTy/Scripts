using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Serialization;

namespace Zlodey.Actors
{
	public sealed class Player : EntityActor
	{
		public WeaponActor weapon;
		public Transform value;
		public Animator animator;
		public CharacterController character;

		[FormerlySerializedAs("_fractionParticlesView")] [FormerlySerializedAs("OrcView")] public FractionView _fractionView;
		public HealthView HealthView;

		public float Health;
		protected override void InitComponents()
		{
			Entity.Get<Health>() = new Health().SetValue(Health);
			Entity.Get<InputPlayer>();
			Entity.Get<TransformRef>().value = transform;
			Entity.Get<TransformRotate>().value = value;
			Entity.Get<CharacterControllerRef>().value = character;
			Entity.Get<IDFraction>().id = 0;
			Entity.Get<Velocity>();
			Entity.Get<AnimatorRef>().value = animator;

			Entity.Get<FractionViewRef>().value = _fractionView;
			Entity.Get<HealthViewRef>().value = HealthView;

			weapon.Init();
			weapon.Entity.Get<Owner>().Entity = Entity;
			
			Entity.Get<CurrentWeapon>().Entity = weapon.Entity;
		}
	}
}