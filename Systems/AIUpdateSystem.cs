using Cysharp.Threading.Tasks;
using DG.Tweening;
using Leopotam.Ecs;
using System.Numerics;
using UnityEngine;
using Zlode.States;
using Zlodey.Actors;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Zlodey
{
	public sealed class SpellTornadoSystem : IEcsRunSystem
	{
		private readonly EcsFilter<SpellTornado, TransformRef, IDFraction, ActivateSpell> _filter;
		private readonly EcsFilter<CircleViewRef, ActivateSpell> _circleView;
		private readonly EcsFilter<SpellTornado, ActivateSpell, AnimatorRef> _animation;
		private readonly EcsFilter<SpellTornado, AttackEvent> _attackEvent;

		private readonly StaticData _staticData;

		private readonly EcsWorld _world;
		private readonly RuntimeData _runtimeData;

		public void Run()
		{
			foreach (var i in _attackEvent)
			{
				var chanceSpell = Random.Range(0f, 1f);

				if (chanceSpell < 0.6f)
				{
					var entity = _attackEvent.GetEntity(i);
					entity.Del<AttackEvent>();
					entity.Get<Reload>().value = 9999999f;
					entity.Get<ActivateSpell>();
				}
			}

			foreach (var i in _circleView)
			{
				var circleView = _circleView.Get1(i).value;

				circleView.Full(2f);
			}

			foreach (var i in _filter)
			{
				var tr = _filter.Get2(i).value;
				var idFraction = _filter.Get3(i).id;
				var entity = _filter.GetEntity(i);

				Activate(entity, tr, idFraction).Forget();
			}
		}

		private async UniTaskVoid Activate(EcsEntity entity, Transform tr, int idFraction)
		{
			await UniTask.Delay(2000);
			if (!entity.IsAlive())
			{
				return;
			}


			var dir = (entity.Get<Target>().value.position - tr.position).normalized;
			entity.Get<OrcWarriorTag>().ParticleRotate.Play();
			entity.Get<AnimatorRef>().value.Play("Tornado");
			entity.Get<LockMove>();
			entity.Get<AI>().value.canMove = false;
			entity.Get<AI>().value.canSearch = false;

			var trPosition = tr.position + dir * 15f;

			var detect = Physics.Raycast(tr.position, dir, out var hit, 100f , LayerMask.GetMask("Obstacle"));

			entity.Get<TransformRotate>().value.DORotate(new Vector3(0, 360f, 0), 0.3f).SetLoops(8, LoopType.Incremental);

			if (detect)
			{
				var point = hit.point;
				point.y = trPosition.y;

				var distOne = (tr.position - point).sqrMagnitude;
				var distTwo = (tr.position - trPosition).sqrMagnitude;

				if (distOne <= distTwo)
				{
					trPosition.x = point.x;
					trPosition.z = point.z;


					tr.DOMove(trPosition, 0.5f).SetEase(Ease.InSine);
				}
				else
				{
					var di = trPosition - dir * 2f;
					tr.DOMove(di, 0.5f).SetEase(Ease.InSine).onComplete += () =>
					{
						tr.DOMove(trPosition, 1f).SetEase(Ease.InSine);
					};
				}

			}
			else
			{
				var di = trPosition - dir * 2f;
				tr.DOMove(di, 0.5f).SetEase(Ease.InSine).onComplete += () =>
				{
					tr.DOMove(trPosition, 1f).SetEase(Ease.InSine);
				};
			}
			var time = 1.5f;
			var tickConst = 0.1f;
			var tick = 0f;

			while (time > 0f)
			{
				if (tick <= 0f)
				{
					var colliders = Physics.OverlapSphere(tr.position, 2f);
					foreach (var collider in colliders)
					{
						if (
							collider.transform == tr ||

							!collider.TryGetComponent<EntityActor>(out var actor) ||

							!actor.Entity.IsAlive() ||

							!actor.Entity.Has<IDFraction>() ||

							actor.Entity.Get<IDFraction>().id == idFraction
						)
						{
							continue;
						}

						var direction = (actor.transform.position - tr.position).normalized;
						direction.y = actor.transform.position.y;

						var jumpPosition = tr.position + direction * 15f;
						jumpPosition.y = actor.transform.position.y;

						var detectObstacle = Physics.Raycast(tr.position, direction, out var hitObstacle, 100f, LayerMask.GetMask("Obstacle"));

						if (detectObstacle)
						{
							var point = hitObstacle.point - direction * 2f;
							point.y = actor.transform.position.y;

							var distOne = (actor.transform.position - point).sqrMagnitude;
							var distTwo = (actor.transform.position - trPosition).sqrMagnitude;

							if (distOne <= distTwo)
							{
								jumpPosition.x = point.x;
								jumpPosition.z = point.z;

								actor.transform.DOJump(jumpPosition, 2f, 1, 0.8f).SetEase(Ease.InSine);
							}
							else
							{
								actor.transform.DOJump(jumpPosition, 2f, 1, 0.8f).SetEase(Ease.InSine);
							}

						}
						else
						{
							actor.transform.DOJump(jumpPosition, 2f, 1, 0.8f).SetEase(Ease.InSine);
						}


						var damageEvent = new AsyncDamageEvent();
						damageEvent.EntityA = entity;
						damageEvent.EntityB = actor.Entity;
						damageEvent.value = 0.75f;

						var damageEntity = _world.NewEntity();
						damageEntity.Add(damageEvent);
					}

					tick = tickConst;
				}

				tick -= _runtimeData.deltaTime;
				time -= _runtimeData.deltaTime;

				await UniTask.Yield();

				if (!entity.IsAlive())
				{
					return;
				}
			}

			if (!entity.IsAlive())
			{
				return;
			}

			entity.Get<OrcWarriorTag>().ParticleRotate.Stop();
			entity.Get<TransformRotate>().value.DOComplete();
			entity.Del<LockMove>();
			entity.Get<TransformRotate>().value.DOLocalRotate(Vector3.zero, 0.3f);
			entity.Get<AI>().value.canMove = true;
			entity.Get<AI>().value.canSearch = true;
			entity.Get<Reload>().value = 0.3f;
		}
	}

	public sealed class AIUpdateSystem : IEcsRunSystem
	{
		private readonly EcsFilter<AI>.Exclude<LockMove> _filter = default;
		private readonly EcsFilter<AI, TransformRotate>.Exclude<AttackState> _aiRotate = default;
		private readonly EcsFilter<AI, Target> _filterTarget = default;
		private readonly EcsFilter<AI, Waypoints>.Exclude<Target> _filterWaypints = default;

		private readonly EcsFilter<AI, MovePlayer>.Exclude<Target, DiedState> _playerUnits = default;

		private readonly EcsFilter<InputPlayer, TransformRef> _playerFilter = default;

		private readonly RuntimeData _runtimeData = default;
		public void Run()
		{
			foreach (var i in _playerFilter)
			{
				var posPlayer = _playerFilter.Get2(i).value.position;
				int count = 1;

				foreach (var unityIndex in _playerUnits)
				{
					var pi = Mathf.PI * count;

					var circle = new UnityEngine.Vector3(Mathf.Cos(pi) / _playerFilter.GetEntitiesCount(), 0f, Mathf.Sin(pi) / _playerFilter.GetEntitiesCount()) * 2f;

					var ai = _playerUnits.Get1(unityIndex).value;

					ai.destination = posPlayer + circle;

					count++;
				}

			}

			foreach (var i in _filter)
			{
				var entity = _filter.GetEntity(i);
				if (entity.Has<HitTimer>() || entity.Has<DiedState>())
				{
					continue;
				}

				var ai = _filter.Get1(i).value;
				ai.Tick(_runtimeData.deltaTime);
			}

			foreach (var i in _aiRotate)
			{
				var entity = _filter.GetEntity(i);
				if (entity.Has<HitTimer>() || entity.Has<DiedState>())
				{
					continue;
				}


				var ai = _aiRotate.Get1(i).value;
				var trRotate = _aiRotate.Get2(i).value;




				var velocity = ai.velocity.normalized;
				velocity.y = 0f;

				if (velocity != Vector3.zero)
				{
					trRotate.rotation = Quaternion.RotateTowards(trRotate.rotation, Quaternion.LookRotation(velocity), 180f * _runtimeData.deltaTime);
				}
			}

			foreach (var i in _filterWaypints)
			{
				var ai = _filterWaypints.Get1(i).value;
				ref var waypoints = ref _filterWaypints.Get2(i);

				var pointPosition = waypoints.Points[waypoints.index].position;

				var dist = (ai.position - pointPosition).sqrMagnitude;

				if (dist < 3f)
				{
					if (waypoints.Inverse)
					{
						waypoints.index--;
						if (waypoints.index == -1)
						{
							waypoints.Inverse = false;
							waypoints.index = 0;
							continue;
						}
					}
					else
					{
						waypoints.index++;
						if (waypoints.index == waypoints.Points.Length)
						{
							waypoints.Inverse = true;
							waypoints.index = waypoints.Points.Length - 1;
							continue;
						}
					}
				}
				ai.destination = waypoints.Points[waypoints.index].position;
			}

			foreach (var i in _filterTarget)
			{
				var ai = _filterTarget.Get1(i).value;
				var target = _filterTarget.Get2(i).value;

				ai.destination = target.position;
			}
		}
	}
}