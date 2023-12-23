using Leopotam.Ecs;
using UnityEngine;

namespace Zlodey
{
	public sealed class LifeSystem : IEcsRunSystem
	{
		private readonly EcsFilter<LifeTimer, DiedEvent> _lifeDiedTimer;
		private readonly EcsFilter<LifeTimer> _lifeTimer;

		private readonly RuntimeData _runtimeData;
		public void Run()
		{
			foreach (var i in _lifeDiedTimer)
			{
				var entity = _lifeDiedTimer.GetEntity(i);

				entity.Del<LifeTimer>();
			}

			foreach (var i in _lifeTimer)
			{
				var time = _lifeTimer.Get1(i).value;
				if (time < Time.time)
				{
					var entity = _lifeTimer.GetEntity(i);

					entity.Del<LifeTimer>();
					entity.Del<AttackDir>();
					entity.Get<DiedEvent>();
				}
			}
		}
	}
}