using Leopotam.Ecs;

namespace Zlodey
{
	public sealed class ReloadWeaponSystem : IEcsRunSystem
	{
		private readonly EcsFilter<Reload> _filter;
		private readonly RuntimeData _runtimeData;
		public void Run()
		{
			if (_filter.IsEmpty() || _runtimeData.GameState != GameState.Playing) return;

			foreach (var i in _filter)
			{
				ref var reload = ref _filter.Get1(i).value;

				reload -= _runtimeData.deltaTime;

				if (reload <= 0)
				{
					var entity = _filter.GetEntity(i);
					entity.Del<Reload>();
				}
			}

		}
	}
}