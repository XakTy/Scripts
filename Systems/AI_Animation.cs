using Leopotam.Ecs;
using UnityEngine;
using Zlode.States;

namespace Zlodey
{
	public sealed class AI_Animation : IEcsRunSystem
	{
		private readonly EcsFilter<AI, AnimatorRef> _filterAI = default;
		private readonly EcsFilter<AI, AnimatorRef, AttackEvent>.Exclude<DiedState, HitTimer> _filter = default;
		private readonly EcsFilter<AI, AnimatorRef, DiedEvent> _dieds = default;
		private readonly EcsFilter<EnemyTag, AnimatorRef, DiedEvent> _diedAiAnimation;
		private readonly EcsFilter<InputPlayer, AnimatorRef, DiedEvent> _diedAnimation;
		private readonly EcsFilter<AnimatorRef, HitTimer, HitReaction>.Exclude<DiedState> _hit;

		public void Run()
		{

			foreach (var died in _dieds)
			{
				var animator = _dieds.Get2(died).value;

				animator.SetBool(AnimationConst.Walk, false);
				animator.SetBool(AnimationConst.Attack, false);
			}



			foreach (var i in _hit)
			{
				var animator = _hit.Get1(i).value;
				ref var hitting = ref _hit.Get2(i).IsHitting;

				if (hitting) continue;

				Debug.Log("Hit anim0");

				hitting = true;

				animator.Play(AnimationConst.Hit);
			}

			foreach (var i in _diedAiAnimation)
			{
				var animator = _diedAiAnimation.Get2(i).value;
				animator.applyRootMotion = true;
				animator.SetLayerWeight(1, 0);




				animator.Play(AnimationConst.DieArray[Random.Range(0, 3)]);
			}


			foreach (var i in _diedAnimation)
			{
				var animator = _diedAnimation.Get2(i).value;
				animator.applyRootMotion = true;
				animator.SetLayerWeight(1, 0);


				animator.Play(AnimationConst.Die);
			}

			foreach (var i in _filterAI)
			{
				var ai = _filterAI.Get1(i).value;

				var entity = _filterAI.GetEntity(i);
				var animator = _filterAI.Get2(i).value;
				if (entity.Has<HitTimer>() || entity.Has<AttackState>() || entity.Has<DiedState>())
				{
					animator.SetBool(AnimationConst.Walk, false);
					continue;
				}

				var velocity = ai.velocity;
				velocity.y = 0f;
				animator.SetBool(AnimationConst.Walk, velocity.sqrMagnitude > 0.01f);
			}

			foreach (var i in _filter)
			{
				var animator = _filter.Get2(i).value;
				animator.Play(AnimationConst.Attack);
			}
		}
	}
}