using Leopotam.Ecs;

namespace Zlodey
{
	public sealed class OwnerUpdateSystem : IEcsRunSystem
	{
		private readonly EcsFilter<Owner, AttackDir, SingleAttackTag> _filter;
		private readonly RuntimeData _runtimeData;
		private readonly EcsWorld _world;
		public void Run()
		{
			if (_filter.IsEmpty() || _runtimeData.GameState != GameState.Playing) return;


			var entity = _filter.Get1(0).Entity;
			var dir = _filter.Get2(0).value;

			entity.Get<AttackDir>().value = dir;
		}
	}
}