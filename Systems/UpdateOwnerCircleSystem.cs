using Leopotam.Ecs;

namespace Zlodey
{
	public sealed class UpdateOwnerCircleSystem : IEcsRunSystem
	{
		private readonly EcsFilter<Targets, CircleView, UpdateInfo> _filter;
		public void Run()
		{
			foreach (var i in _filter)
			{
				var targets = _filter.Get1(i).value;
				var view = _filter.Get2(i);

				view.sprite.color = targets.Count > 0 ? view.EnterCircly : view.ExitCircly;

				var entity = _filter.GetEntity(i);

				entity.Del<UpdateInfo>();
			}
		}
	}
}