using Cysharp.Threading.Tasks;
using DG.Tweening;
using Leopotam.Ecs;
using Pathfinding.RVO;
using UnityEngine;
using Zlode.States;
using Zlodey.Actors;
using static UnityEngine.EventSystems.EventTrigger;

namespace Zlodey
{
	public sealed class SoundDieSystem : IEcsRunSystem
	{
		private readonly EcsFilter<OrcTag, DiedEvent> _orcDies;
		private readonly EcsFilter<FractionViewRef, DiedEvent> _orcViewDies;

		private readonly SceneData _sceneData;

		private readonly StaticData _staticData;
		public void Run()
		{
			foreach (var i in _orcDies)
			{
				_sceneData.Sounder.PlayOneShot(_staticData.OrcDie);
			}

			foreach (var i in _orcViewDies)
			{
				var view = _orcViewDies.Get1(i).value;
				view.Died();
			}
		}
	}

	public sealed class FindAllySystem : IEcsRunSystem
	{
		private readonly EcsFilter<TransformRef, Target, IDFraction, GetTargetRequest> _getTarget;
		public void Run()
		{
			foreach (var i in _getTarget)
			{
				var tr = _getTarget.Get1(i).value;
				var target = _getTarget.Get2(i);
				var id = _getTarget.Get3(i).id;

				var colliders = Physics.OverlapSphere(tr.position, 7f);

				if (colliders.Length > 0)
				{
					foreach (var collider in colliders)
					{
						if (collider.TryGetComponent<Enemy>(out var entityActor) && entityActor.Entity.IsAlive())
						{
							if (id == entityActor.Entity.Get<IDFraction>().id && !entityActor.Entity.Has<Target>())
							{
								entityActor.Entity.Get<Target>() = target;
							}
						}
					}
				}

				var entity = _getTarget.GetEntity(i);
				entity.Del<GetTargetRequest>();
			}
		}
	}

	public sealed class CheckerHealth : IEcsRunSystem
	{
		private readonly EcsFilter<Health, HitReaction>.Exclude<DiedState> _hitReaction = default;
		public void Run()
		{
			foreach (var i in _hitReaction)
			{
				var health = _hitReaction.Get1(i).CurrentHealth;

				if (health <= 0f)
				{
					var entity = _hitReaction.GetEntity(i);

					entity.Get<DiedState>();
					entity.Get<DiedEvent>();
				}
			}

		}
	}

	public sealed class DiedSystem : IEcsRunSystem
	{
		private readonly EcsFilter<ProgressAttackViewRef, DiedEvent> _diedsView = default;
		private readonly EcsFilter<EnemyTag, AI, DiedEvent> _dieds = default;
		private readonly EcsFilter<Targets> _filterTargets = default;
		public void Run()
		{
			foreach (var i in _diedsView)
			{
				var view = _diedsView.Get1(i).value;
				view.SetState(false);
			}

			foreach (var i in _dieds)
			{
				var entity = _dieds.GetEntity(i);

				var transform = entity.Get<TransformRef>().value;

				if (!_filterTargets.IsEmpty())
				{
					var targets = _filterTargets.Get1(0).value;
					if (targets.Contains(transform))
					{
						targets.Remove(transform);
					}

					_filterTargets.GetEntity(0).Get<UpdateInfo>();
				}

				_dieds.Get2(i).value.GetComponent<RVOController>().enabled = false;


				var colliders = transform.GetComponentsInChildren<Collider>();

				foreach (var collider in colliders)
				{
					collider.enabled = false;
				}

				transform.DOMove(transform.position - new Vector3(0, 3f, 0), 2f).SetDelay(5f);

				entity.Destroy();
			}
		}
	}

	public sealed class DamageSystem : IEcsRunSystem
	{
		private readonly EcsFilter<AsyncDamageEvent> _filterAsyncDamage = default;
		private readonly EcsFilter<DamageEvent> _filterDamage = default;



		private static readonly int DissolveValue = Shader.PropertyToID("_Color");

		public void Run()
		{
			foreach (var i in _filterAsyncDamage)
			{
				var asyncDamage = _filterAsyncDamage.Get1(i);
				var entity = _filterAsyncDamage.GetEntity(i);

				if (!asyncDamage.EntityA.IsAlive() || !asyncDamage.EntityB.IsAlive())
				{
					entity.Destroy();
					continue;
				}

				var damageEvent = new DamageEvent();

				damageEvent.EntityA = asyncDamage.EntityA;
				damageEvent.EntityB = asyncDamage.EntityB;
				damageEvent.value = asyncDamage.value;

				entity.Add(damageEvent);

				entity.Del<AsyncDamageEvent>();
			}

			foreach (var indexEvent in _filterDamage)
			{
				var eventDamage = _filterDamage.Get1(indexEvent);

				if (eventDamage.EntityB.IsAlive() && eventDamage.EntityB.Has<RendersDissolve>())
				{
					var renders = eventDamage.EntityB.Get<RendersDissolve>().value;
					BlinkWhite(renders).Forget();
				}

			}

			foreach (var indexEvent in _filterDamage)
			{
				var eventDamage = _filterDamage.Get1(indexEvent);

				if (!eventDamage.EntityA.IsAlive() || !eventDamage.EntityB.IsAlive() || !eventDamage.EntityA.Has<Owner>()) continue;

				if (!eventDamage.EntityB.Has<Target>())
				{
					var ownerEntity = eventDamage.EntityA.Get<Owner>().Entity;

					if (!ownerEntity.IsAlive())
					{
						continue;
					}

					var tr = ownerEntity.Get<TransformRef>().value;

					if (!tr) continue;

					eventDamage.EntityB.Get<Target>().TargetEntity =  ownerEntity;
					eventDamage.EntityB.Get<Target>().value = tr;

					eventDamage.EntityB.Get<GetTargetRequest>();
				}

			}

			foreach (var indexEvent in _filterDamage)
			{
				var eventDamage = _filterDamage.Get1(indexEvent);

				if (eventDamage.EntityB.IsAlive())
				{
					if (eventDamage.EntityB.Has<Health>() && !eventDamage.EntityB.Has<DiedState>())
					{
						ref var health = ref eventDamage.EntityB.Get<Health>().CurrentHealth;
						health -= eventDamage.value;

						eventDamage.EntityB.Get<HitReaction>();

						_filterDamage.GetEntity(indexEvent).Destroy();
					}
				}
			}
		}

		private async UniTask BlinkWhite(Renderer[] renders)
		{
			foreach (var renderer in renders)
			{
				renderer.material.SetColor(DissolveValue, new Color(1f, 0.6273585f, 0.6273585f));
			}

			await UniTask.Delay(100, cancellationToken: renders[0].GetCancellationTokenOnDestroy());

			foreach (var renderer in renders)
			{
				renderer.material.SetColor(DissolveValue, Color.white);
			}
		}
	}
}