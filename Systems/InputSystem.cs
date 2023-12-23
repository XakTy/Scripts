using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;

namespace Zlodey
{
	public sealed class InputSystem : IEcsRunSystem
	{
		private readonly EcsFilter<InputPlayer, Velocity> _filter;
		private readonly RuntimeData _runtimeData;

		private readonly GameScreen _gameScreen = Service<UI>.Get().GameScreen;
		public void Run()
		{
			if (_filter.IsEmpty()) return;
			ref var velocity = ref _filter.Get2(0).value;

			var dir = _gameScreen.MoveJoystics.Direction;

			if (dir != Vector2.zero)
			{
				var rot = Quaternion.Euler(0f, _runtimeData.CurrentCamera.transform.eulerAngles.y, 0) *  new Vector3(dir.x, 0f, dir.y);
				velocity = rot;
			}
			else
			{
				velocity = Vector3.zero;
			}
		}
	}
}