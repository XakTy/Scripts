using System.Collections;
using System.Linq;
using Leopotam.Ecs;
using LeopotamGroup.Globals;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Zlodey.Actors;

namespace Zlodey
{
	public sealed class Game : MonoBehaviour
    {
	    private EcsWorld _world;
	    private EcsSystems _systems;

        public UnityEvent Events;

        [SerializeField] private SceneData _sceneData;
		[SerializeField] private RuntimeData _runtimeData;
		[SerializeField] private StaticData _staticData;

		[Button("Find Enemy")]
		public void GetEnemy()
		{
			_sceneData.Enemies = Object.FindObjectsOfType<Enemy>();
		}

		IEnumerator Start()
        {
            // void can be switched to IEnumerator for support coroutines.

            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
#endif


            _runtimeData = new RuntimeData();

            ServiceInject();
            GameInitialization.FullInit();

            _systems
                // register your systems here, for example:
                .Add(new InitializeSystem())
                .Add(new ChangeStateSystem())
                .Add(new StartGameSystem())

                .Add(new InputSystem())
                .Add(new BulletMoveSystem())
                .Add(new TransformMoveSystem())
                .Add(new FindNearTargetsSystem())
                .Add(new OwnerUpdateSystem())
                .Add(new UpdateOwnerCircleSystem())
				.Add(new CalculateAttackJoystick())
				.Add(new TransformRotateDirSystem())
                .Add(new BulletTriggerSystem())
                .Add(new BulletViewDiedSystem())
                .Add(new LifeSystem())

                .OneFrame<HitReaction>()

                .Add(new DamageSystem())
                .Add(new HitSystem())


                .Add(new AnimationSystem())
				// Get DiedEvent
				.Add(new CheckerHealth())

                .Add(new DropSystem())

				.Add(new CampTargetDiedSystem())
                .Add(new CampDiedSystem())

                .Add(new SoundDieSystem())

                .Add(new RangeDiedSystem())

				.Add(new HealthViewSystem())

                .Add(new AIUpdateSystem())

                .OneFrame<AttackEvent>()
                .Add(new StateMoveSystem())
                .Add(new TimerAttackSystem())

                .OneFrame<ActivateSpell>()
                .Add(new SpellTornadoSystem())

				.Add(new AI_Animation())
					 // entity Destroy AI
                .Add(new PlayerDiedSystem())

				.Add(new DiedSystem())

				.Add(new SingleAttackSystem())

                .Add(new ReloadWeaponSystem())


                //.Add(new AttackPlayerView())

                .OneFrame<AutoAttack>()
                .Add(new AttackPlayerSystem())



                .Add(new FindAllySystem())

           
                .Add(new TargetCheckerSystem())

                .Add(new OrcArcherSystem())
                .Add(new OrcMeleeAttackSystem())
				.Add(new OrcRangeExplosionSystem())

                .Add(new ExplosionTimerSystem())





                .Inject(_sceneData)
                .Inject(_runtimeData)
                .Inject(_staticData)
                .Inject(Service<UI>.Get())

                .Init();

            Events?.Invoke();

			yield return null;
        }

        private void ServiceInject()
        {
	        Service<SceneData>.Set(_sceneData);
	        Service<RuntimeData>.Set(_runtimeData);
	        Service<StaticData>.Set(_staticData);
	        Service<EcsWorld>.Set(_world);
        }

        void Update()
        {
	        _runtimeData.deltaTime = Time.deltaTime;
            _systems?.Run();
        }

        public void Win()
        {
            _world.ChangeState(GameState.Win);
        }
        void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
                _world.Destroy();
                _world = null;
            }
        }
    }
}