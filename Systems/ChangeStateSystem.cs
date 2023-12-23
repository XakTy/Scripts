using System;
using Leopotam.Ecs;
using UnityEngine;

namespace Zlodey
{
    class ChangeStateSystem : IEcsRunSystem
    {
        private UI _ui;
        private EcsFilter<ChangeStateEvent> _filter;

        private readonly EcsFilter<AI, AnimatorRef, IDFraction> _unitsPlayer = default;



		private StaticData _staticData;
        private RuntimeData _runtimeData;
        private SceneData _sceneData;
        private EcsWorld _world;
        
        public void Run()
        {
            foreach (var i in _filter)
            {
                var state = _filter.Get1(i).NewGameState;
                _runtimeData.GameState = state;
                switch (state)
                {
                    case GameState.Lose:
                        _ui.LoseScreen.Show(true);
						break;
                    case GameState.Before:
                        _ui.MenuScreen.Show(true);
                        _ui.MenuScreen.Level.text = $"LEVEL {_runtimeData.Level + 1}";
                        
                        _ui.GameScreen.Show(false);
                        break;
                    case GameState.Playing:
                        _runtimeData.LevelStartedTime = Time.realtimeSinceStartup;
                        
                        _ui.MenuScreen.Show(false);
                        
                        _ui.GameScreen.Level.text = $"LEVEL {_runtimeData.Level + 1}";
                        _ui.GameScreen.Show(true);
                        break;
                    case GameState.Win:

	                    foreach (var unityIndex in _unitsPlayer)
	                    {
		                    var id = _unitsPlayer.Get3(unityIndex).id;
		                    if (id != 0) continue;

		                    var animator = _unitsPlayer.Get2(unityIndex).value;
		                    animator.Play("Win");

                            var entity = _unitsPlayer.GetEntity(unityIndex);
                            entity.Del<AI>();
                            entity.Del<MovePlayer>();
                            entity.Del<AnimatorRef>();
	                    }

						break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                _filter.GetEntity(i).Destroy();
            }
        }
    }
}