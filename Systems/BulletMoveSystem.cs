using Leopotam.Ecs;

namespace Zlodey
{
	public sealed class BulletMoveSystem : IEcsRunSystem
	{
		private readonly EcsFilter<BulletData, TransformRef, AttackDir> _filter;
		private RuntimeData _runtimeData;

		public void Run()
		{
			foreach (var i in _filter)
			{
				var data = _filter.Get1(i);
				var transform = _filter.Get2(i).value;
				var dir = _filter.Get3(i).value;

				transform.position += _runtimeData.deltaTime * dir * data.speed;
			}
		}
	}
}