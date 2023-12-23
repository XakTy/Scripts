using UnityEngine;

namespace Zlodey.Actors
{
	[CreateAssetMenu(fileName ="AttackDataMelee")]
	public sealed class AttackMeleeData : ScriptableObject
	{
		public float Radius;
		public float Damage;
		public float DelayToAttack;
		public float ReloadTime;
		public float Range;
		public float DistanceToAttack;
	}
}