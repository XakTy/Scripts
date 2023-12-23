using Leopotam.Ecs;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Zlodey.Actors;
using UnityEngine.UIElements;
using System;
using Zlode.States;

namespace Zlodey
{
	public sealed class StateMoveSystem : IEcsRunSystem
	{
		private readonly EcsFilter<AttackMeleeDataRef, TransformRotate, Target>.Exclude<Reload, DiedState> _filter;
		private readonly EcsFilter<AttackRangeDataRef, TransformRotate, Target>.Exclude<Reload, DiedState> _filterRange;

		private readonly RuntimeData _runtimeData;
		public void Run()
		{
			foreach (var i in _filter)
			{
				var attackData = _filter.Get1(i).value;
				var transformOrc = _filter.Get2(i).value;
				var transformTarget = _filter.Get3(i).value;

				var fromOrcToTarget = (transformTarget.position - transformOrc.position);
				fromOrcToTarget.y = 0f;

				var entity = _filter.GetEntity(i);

				if (fromOrcToTarget.sqrMagnitude < attackData.DistanceToAttack * attackData.DistanceToAttack)
				{
					entity.Get<AttackState>();


					var normalized = fromOrcToTarget.normalized;
					var dot = Vector3.Dot(transformOrc.forward, normalized);
					if (dot < 0.9f)
					{
						transformOrc.rotation = Quaternion.RotateTowards(transformOrc.rotation, Quaternion.LookRotation(normalized), _runtimeData.deltaTime * 200f);
						continue;
					}

					entity.Get<AttackEvent>();
				}
				else
				{
					entity.Del<AttackState>();


					var count = Physics.OverlapSphereNonAlloc(transformOrc.position, 10f, _runtimeData.CollidersPool);
					Span<Collider> colliders = new Span<Collider>(_runtimeData.CollidersPool, 0, count);
					var idFraction = entity.Get<IDFraction>().id;
					if (colliders.Length > 0)
					{
						var positionTr = transformOrc.position;

						var newTarget = FindNear(colliders, transformOrc, idFraction, positionTr);


						if (newTarget && newTarget.Entity.IsAlive())
						{
							entity.Get<Target>().TargetEntity = newTarget.Entity;
							entity.Get<Target>().value = newTarget.transform;
						}
					}
				}
			}

			foreach (var i in _filterRange)
			{
				var attackDistance = _filterRange.Get1(i).value.Range;
				var transformOrc = _filterRange.Get2(i).value;
				var transformTarget = _filterRange.Get3(i).value;

				var orcToPlayer = (transformTarget.position - transformOrc.position);
				var dist = orcToPlayer.sqrMagnitude;


				if (dist < attackDistance * attackDistance)
				{
					var entity = _filterRange.GetEntity(i);
					entity.Get<AttackState>();

					var normalized = orcToPlayer.normalized;
					normalized.y = 0f;

					var dot = Vector3.Dot(transformOrc.forward, normalized);
					if (dot < 0.8f)
					{
						transformOrc.rotation = Quaternion.RotateTowards(transformOrc.rotation, Quaternion.LookRotation(normalized), _runtimeData.deltaTime * 200f);
						continue;
					}

					entity.Get<AttackDir>().value = normalized;
					entity.Get<AttackEvent>();
				}
				else
				{
					var entity = _filterRange.GetEntity(i);

					entity.Del<AttackState>();

					var count = Physics.OverlapSphereNonAlloc(transformOrc.position, 10f, _runtimeData.CollidersPool);
					Span<Collider> colliders = new Span<Collider>(_runtimeData.CollidersPool, 0, count);

					var idFraction = entity.Get<IDFraction>().id;
					if (colliders.Length > 0)
					{
						var positionTr = transformOrc.position;

						var newTarget = FindNear(colliders, transformOrc, idFraction, positionTr);


						if (newTarget && newTarget.Entity.IsAlive())
						{
							entity.Get<Target>().TargetEntity = newTarget.Entity;
							entity.Get<Target>().value = newTarget.transform;
						}
					}
				}
			}
		}

		private static EntityActor FindNear(Span<Collider> colliders, Transform transformOrc, int idFraction, Vector3 positionTr)
		{
			var minPosition = Mathf.Infinity;
			EntityActor newTarget = null;

			foreach (var collider in colliders)
			{
				if (
					collider.transform == transformOrc ||
					!collider.TryGetComponent<EntityActor>(out var actor) ||
					!actor.Entity.IsAlive() ||
					!actor.Entity.Has<IDFraction>() ||
					actor.Entity.Get<IDFraction>().id == idFraction
				)
				{
					continue;
				}

				var dist = (positionTr - collider.transform.position).sqrMagnitude;

				if (dist < minPosition)
				{
					minPosition = dist;
					newTarget = actor;
				}
			}

			return newTarget;
		}
	}
}