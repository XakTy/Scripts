using Leopotam.Ecs;
using UnityEngine;
using Zlodey.Actors;

namespace Zlodey
{
    public sealed class InitializeSystem : IEcsInitSystem
    {
        private readonly UI _ui;
        private readonly EcsWorld _world;
        private readonly RuntimeData _runtimeData;
        private readonly SceneData _sceneData;

        public void Init()
        {
            _ui.GameScreen.ResetCamp();

			_ui.CloseAll();
            _runtimeData.Level = Progress.CurrentLevel;
            _world.ChangeState(GameState.Playing);

            _runtimeData.CurrentCamera = _sceneData.CameraPool[0];

			_sceneData.Player.Init(_world);

            foreach (var sceneDataEnemy in _sceneData.Enemies)
            {
	            sceneDataEnemy.Init(_world);
            }

            var camp = Object.FindObjectsOfType<CampZone>();

            foreach (var campZone in camp)
            {
	            var campEntity = _world.NewEntity();

	            campEntity.Get<Camp>().countEntities = campZone.Actors.Length;
                campEntity.Get<Camp>().Events = campZone.Events;

	            foreach (var campZoneActor in campZone.Actors)
	            {
		            campZoneActor.Entity.Get<CampEntity>().Entity = campEntity;
	            }
            }

		}
	}
}