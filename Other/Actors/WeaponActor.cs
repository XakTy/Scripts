using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Serialization;

namespace Zlodey.Actors
{
	public sealed class WeaponActor : EntityActor
	{
		[FormerlySerializedAs("rangeData")] public AttackRangeDataRef _rangeDataRef;
		public CircleView View;
		public RangeView RangeView;
		protected override void InitComponents()
		{
			Entity.Get<SingleAttackTag>();
			Entity.Get<CircleView>() = View;
			Entity.Get<RangeView>() = RangeView;
			Entity.Get<AttackRangeDataRef>() = _rangeDataRef;
			Entity.Get<TransformRef>().value = transform;
			Entity.Get<Targets>().value = new System.Collections.Generic.List<Transform>();
		}


		private void OnTriggerEnter(Collider other)
		{
			if (Entity.IsAlive() && other.TryGetComponent<Enemy>(out var actor))
			{
				if (!actor.Entity.IsAlive())
				{
					return;
				}

				if (Entity.Get<Owner>().Entity.Get<IDFraction>().id == actor.Entity.Get<IDFraction>().id)
				{
					return;
				}

				Entity.Get<Targets>().value.Add(actor.transform);
				Entity.Get<UpdateInfo>();
			}

		}

		private void OnTriggerExit(Collider other)
		{
			if (Entity.IsAlive() && other.TryGetComponent<Enemy>(out var actor))
			{
				if (!actor.Entity.IsAlive())
				{
					return;
				}

				if (Entity.Get<Owner>().Entity.Get<IDFraction>().id == actor.Entity.Get<IDFraction>().id)
				{
					return;
				}

				Entity.Get<Targets>().value.Remove(actor.transform);
				Entity.Get<UpdateInfo>();
			}

		}
	}
}