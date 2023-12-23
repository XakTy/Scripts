using Leopotam.Ecs;
using UnityEngine;
using Zlodey;
using Zlodey.Actors;

namespace InterfaceWay
{
	public sealed class UnitZoneInit : MonoBehaviour
	{
		public EntityActor[] Actors;
		public void Init()
		{
			foreach (var entityActor in Actors)
			{
				entityActor.Init();
				var angryZone = entityActor.GetComponentInChildren<AngryZone>();
				angryZone.Collider.enabled = false;
				angryZone.Collider.enabled = true;
			}
		}

		public void MovePlayer()
		{
			foreach (var entityActor in Actors)
			{
				if (entityActor.Entity.IsAlive())
				{
					entityActor.Entity.Get<MovePlayer>();
				}
			}
		}
	}
}