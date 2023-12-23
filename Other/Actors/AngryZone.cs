using Leopotam.Ecs;
using UnityEngine;

namespace Zlodey.Actors
{
	public sealed class AngryZone : MonoBehaviour
	{
		public EntityActor Actor;
		public Collider Collider;
		private void OnTriggerEnter(Collider other)
		{

			if (other.TryGetComponent<EntityActor>(out var actor))
			{
				if (!Actor.Entity.IsAlive() || !actor.Entity.IsAlive() || !actor.Entity.Has<IDFraction>()) return;

				if (Actor.Entity.Get<IDFraction>().id == actor.Entity.Get<IDFraction>().id)
				{
					return;
				}

				if (Actor.Entity.Has<Target>())
				{
					var targetTr = Actor.Entity.Get<Target>().value;

					var dist = (targetTr.position - transform.position).sqrMagnitude;
					var distNew = (actor.transform.position - transform.position).sqrMagnitude;

					if (distNew < dist)
					{
						return;
					}
				}

				Actor.Entity.Get<Target>().TargetEntity = actor.Entity;
				Actor.Entity.Get<Target>().value = actor.transform;
				Actor.Entity.Get<GetTargetRequest>();
			}
		}

		public void Off()
		{
			gameObject.SetActive(false);
		}
	}
}