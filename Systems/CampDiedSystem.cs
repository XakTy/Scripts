using Leopotam.Ecs;

namespace Zlodey
{
	public sealed class CampDiedSystem : IEcsRunSystem
	{
		private readonly EcsFilter<Camp, DestroyCamp> _die;
		private readonly EcsFilter<Camp> _camps;
		private readonly UI _ui;
		public void Run()
		{
			foreach (var i in _die)
			{
				var entity = _die.GetEntity(i);

				var camp = _die.Get1(i).Events;
				camp?.Invoke();

				if (_camps.GetEntitiesCount() > 1)
				{
					_ui.GameScreen.ClearClamp();
					_ui.GameScreen.Next();
				}

				entity.Destroy();
			}
		}
	}
}