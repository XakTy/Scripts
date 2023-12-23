using UnityEngine;

namespace Zlodey
{
	public static class AnimationConst
	{
		public static readonly int Idle = Animator.StringToHash("Idle");
		public static readonly int Walk = Animator.StringToHash("Walk");
		public static readonly int Attack = Animator.StringToHash("Attack");
		public static readonly int Hit = Animator.StringToHash("Hit");

		public static readonly int Die = Animator.StringToHash("Die");

		public static readonly int[] DieArray = {Animator.StringToHash("Die_0"), Animator.StringToHash("Die_1"), Animator.StringToHash("Die_2") };


	}
}