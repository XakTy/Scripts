using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Zlodey
{
	public sealed class TransformMoveSystem : IEcsRunSystem
	{
		private readonly EcsFilter<CharacterControllerRef, Velocity> _filter;
		private readonly RuntimeData _runtimeData;

		private Vector3 offset;
		public void Run()
		{
			if (_filter.IsEmpty()) return;
			var characterController = _filter.Get1(0).value;
			var velocity = _filter.Get2(0).value;
			characterController.Move(velocity.normalized * 5f * _runtimeData.deltaTime);

			_runtimeData.CurrentCamera.transform.position = characterController.transform.position + _runtimeData.CurrentCamera.Offset;
		}
	}
}