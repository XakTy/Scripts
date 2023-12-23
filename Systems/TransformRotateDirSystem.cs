using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;

namespace Zlodey
{
	public sealed class TransformRotateDirSystem : IEcsRunSystem
	{
		private readonly EcsFilter<InputPlayer, TransformRotate, Velocity> _filter;
		private readonly RuntimeData _runtimeData;

		private readonly EcsWorld _world;

		private readonly StaticData _staticData;

		private readonly GameScreen _gameScreen = Service<UI>.Get().GameScreen;

		public void Run()
		{
			if (_filter.IsEmpty()) return;

			if (_gameScreen.AttackJoystics.IsPressed) return;

			var transform = _filter.Get2(0).value;
			var dir = _filter.Get3(0).value;

			var entity = _filter.GetEntity(0);

			if (_staticData.AutoAttack && entity.Has<AttackDir>())
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(entity.Get<AttackDir>().value), _runtimeData.deltaTime * 10f);
				entity.Del<AttackDir>();
				return;
			}

			entity.Del<AttackDir>();

			if (dir == Vector3.zero)
			{
				return;
			}

			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), _runtimeData.deltaTime * 10f);
		}
	}
}