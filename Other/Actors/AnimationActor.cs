using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Zlodey.Actors
{
	public sealed class AnimationActor : MonoBehaviour
	{
		[field: SerializeField] public Animator Animator { get; private set; }
		[field: SerializeField] public bool IsPlayStart { get; private set; }

		public string AnimationID;
		public void Start()
		{
			if (IsPlayStart)
			{
				PlayAnimation(AnimationID);
			}
		}


		public void RootMotion(bool state)
		{
			Animator.applyRootMotion = state;
		}
		public void PlayAnimation(string name)
		{
			Animator.Play(name);
		}

	}
}