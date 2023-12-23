using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;

namespace Zlodey
{
	public sealed class AttackPlayerSystem : IEcsRunSystem
	{
		private readonly EcsFilter<JoystickDir> _joystickDir;

		private readonly EcsFilter<SingleAttackTag> _attackEvent;

		private readonly StaticData _staticData;

		private readonly RuntimeData _runtimeData;

		private readonly GameScreen _gameScreen = Service<UI>.Get().GameScreen;

		public void Run()
		{
			//if (_joystickDir.IsEmpty() || _attackEvent.IsEmpty()) return;
			if (_attackEvent.IsEmpty()) return;

			if (_gameScreen.AttackJoystics.IsPressed)
			{
				var magnitudeJ = _gameScreen.AttackJoystics.Input.magnitude;
				if (magnitudeJ < 0.8f)
				{
					return;
				}

				var dirJ = _gameScreen.AttackJoystics.Direction;

				var entityWeaponJ = _attackEvent.GetEntity(0);

				var rotJ = Quaternion.Euler(0f, _runtimeData.CurrentCamera.transform.eulerAngles.y, 0) * new Vector3(dirJ.x, 0f, dirJ.y);

				entityWeaponJ.Get<AttackDir>().value = rotJ.normalized;
			}

			//var dirInput = _joystickDir.Get1(0).value;
			//var magnitude = _joystickDir.Get1(0).magnitude;

			//if (magnitude < 0.8f)
			//{
			//	Service<EcsWorld>.Get().NewEntity().Get<AutoAttack>();
			//	_joystickDir.GetEntity(0).Destroy();
			//	return;
			//}

			//var entityWeapon = _attackEvent.GetEntity(0);

			//var rot = Quaternion.Euler(0f, _runtimeData.CurrentCamera.transform.eulerAngles.y, 0) * new Vector3(dirInput.x, 0f, dirInput.y);

			//entityWeapon.Get<AttackDir>().value = rot.normalized;

			//_joystickDir.GetEntity(0).Destroy();
		}
	}
}