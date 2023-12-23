using Leopotam.Ecs;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

namespace Zlodey.Actors
{
	public abstract class Enemy : EntityActor
	{
		public DropData Drop;

		public int IDFraction;

		public bool Hitable = true;

		public Transform TrRotate;
		protected override void InitComponents()
		{
			Entity.Get<EnemyTag>();
			Entity.Get<TransformRef>().value = transform;
			Entity.Get<TransformRotate>().value = TrRotate;
			Entity.Get<IDFraction>().id = IDFraction;

			if (Hitable)
			{
				Entity.Get<HitableTag>();
			}

			if (Drop)
			{
				Entity.Get<Drop>().Data = Drop;
			}
		}

		private void OnValidate()
		{
			if (!TrRotate)
			{
				TrRotate = transform.Find("Visual");
			}
		}
	}
}