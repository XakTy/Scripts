using UnityEngine;

namespace Zlodey.Actors
{
	[CreateAssetMenu(fileName = "AttackDataRange")]
	public sealed class AttackRangeData : ScriptableObject
	{
		public EntityActor Bullet;
		public float LifeTime;
		public float Damage;
		public float AttackInterval;
		public float Reload;
		public float Range;
	}
}