using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;

namespace Zlodey
{
	public sealed class CalculateAttackJoystick : IEcsRunSystem
	{
		private readonly RuntimeData _runtimeData;
		private readonly GameScreen _gameScreen = Service<UI>.Get().GameScreen;

		private readonly EcsFilter<SingleAttackTag, RangeView> _weapon;
		private readonly EcsFilter<InputPlayer, TransformRotate> _player;
		public void Run()
		{
			if (!_gameScreen.AttackJoystics.IsPressed || _player.IsEmpty()) return;

			var dirInput = _gameScreen.AttackJoystics.Direction;
			var rot = Quaternion.Euler(0f, _runtimeData.CurrentCamera.transform.eulerAngles.y, 0) * new Vector3(dirInput.x, 0f, dirInput.y).normalized;

			_weapon.Get2(0).Direction = rot;

			if (rot == Vector3.zero) return;
			_player.Get2(0).value.rotation = Quaternion.LookRotation(rot);
		}
	}
}