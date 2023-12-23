using Leopotam.Ecs;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;

namespace Zlodey
{
	public sealed class SingleAttackSystem : IEcsRunSystem
	{
		private readonly EcsFilter<SingleAttackTag, AttackRangeDataRef, AttackDir>.Exclude<Reload> _filter;
		private readonly RuntimeData _runtimeData;
		private readonly EcsWorld _world;
		private readonly StaticData _staticData;
		public void Run()
		{
			if (_filter.IsEmpty() || _runtimeData.GameState != GameState.Playing) return;

			var data = _filter.Get2(0);
			var dir = _filter.Get3(0).value;

			var entity = _filter.GetEntity(0);

			if (_staticData.AutoAttack)
			{
				if (entity.Get<Owner>().Entity.Get<Velocity>().value != Vector3.zero)
				{
					entity.Del<AttackDir>();
					return;
				}
			}

			var bullet = Object.Instantiate(data.value.Bullet, data.StartPoint.position, Quaternion.identity);
			bullet.Init(_world);
			bullet.transform.rotation = Quaternion.LookRotation(dir);
			bullet.Entity.Get<AttackDir>().value = dir;
			bullet.Entity.Get<LifeTimer>().value = Time.time + 0.5f;


			bullet.Entity.Get<Owner>().Entity = entity;

			entity.Del<AttackDir>();
			entity.Get<Reload>().value = data.value.Reload;
		}
	}
}