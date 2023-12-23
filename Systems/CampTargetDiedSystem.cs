using Leopotam.Ecs;

namespace Zlodey
{
	public sealed class CampTargetDiedSystem : IEcsRunSystem
	{
		private readonly EcsFilter<CampEntity, DiedEvent> _dieds;

		public void Run()
		{
			foreach (var died in _dieds)
			{
				var campEntity = _dieds.Get1(died).Entity;

				ref var camp = ref campEntity.Get<Camp>();
				camp.countEntities--;

				if (camp.countEntities == 0)
				{
					campEntity.Get<DestroyCamp>();
				}
			}
		}
	}
}