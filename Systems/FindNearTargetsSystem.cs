using System.Collections.Generic;
using DG.Tweening;
using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;
using UnityEngine.UIElements;

namespace Zlodey
{
	public sealed class FindNearTargetsSystem : IEcsRunSystem
	{
		private readonly EcsFilter<AutoAttack> _autoAttack;
		private readonly EcsFilter<SingleAttackTag, TransformRef, Targets> _filter;
		private readonly EcsFilter<AI, TransformRef, Targets> _finder;
		private readonly RuntimeData _runtimeData;

		private readonly StaticData _staticData;

		private readonly GameScreen _gameScreen = Service<UI>.Get().GameScreen;
		public void Run()
		{
			if (_filter.IsEmpty() || _runtimeData.GameState != GameState.Playing) return;


			if (_autoAttack.IsEmpty() && !_staticData.AutoAttack)
			{
				return;
			}

			if (!_autoAttack.IsEmpty())
			{
				foreach (var i in _autoAttack)
				{
					var autoAttackEntity = _autoAttack.GetEntity(i);
					autoAttackEntity.Destroy();
				}
			}

			var transform = _filter.Get2(0).value;
			var target = _filter.Get3(0).value;

			if (target.Count == 0)
			{
				return;
			}

			var dir = GetNearTarget(target, transform.position);

			dir.y = 0;
			var entity = _filter.GetEntity(0);

			if (_staticData.AutoAttack)
			{
				if (entity.Get<Owner>().Entity.Get<Velocity>().value != Vector3.zero) return;
			}
			else
			{
				var trRotate = entity.Get<Owner>().Entity.Get<TransformRotate>().value;
				trRotate.rotation = Quaternion.LookRotation(dir.normalized);
			}

			entity.Get<AttackDir>().value = dir.normalized;
		}

		private Vector3 GetNearTarget(List<Transform> transforms, Vector3 position)
		{
			var minPosition = Mathf.Infinity;
			var newPosition = Vector3.zero;

			foreach (var transform in transforms)
			{
				var dist = (position - transform.position).sqrMagnitude;

				if (dist < minPosition)
				{
					minPosition = dist;
					newPosition = transform.position;
				}
			}

			var dir = newPosition - position;

			return dir;
		}
	}
}