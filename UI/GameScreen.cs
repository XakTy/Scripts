using Cysharp.Threading.Tasks;
using DG.Tweening;
using Leopotam.Ecs;
using LeopotamGroup.Globals;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zlodey.Actors;

namespace Zlodey
{
	public class GameScreen : Screen
    {
        public TextMeshProUGUI Level;
        public TextMeshProUGUI ClampClearText;

        public FloatingJoystick MoveJoystics;
        public FloatingJoystick AttackJoystics;

        public Toggle AutoAttack;
        public Button CameraChange;

        private int _currentIndexCamera = 0;

        public CampView[] Camps;
        private int _camps;

        public CoinView CoinView;

        [Button("Set position")]
        private void SetPosition()
        {
			AttackJoystics.SetPosition();
			MoveJoystics.SetPosition();
		}
        private void Start()
        {
	  
			//AutoAttack.isOn = Service<StaticData>.Get().AutoAttack;
   //         AutoAttack.onValueChanged.AddListener(OffAttack);
   //         AutoAttack.onValueChanged.Invoke(Service<StaticData>.Get().AutoAttack);


			AttackJoystics.gameObject.SetActive(!Service<StaticData>.Get().AutoAttack);
   //         AttackJoystics.OnUpJoystick += AttackDir;

			//CameraChange.onClick.AddListener(EditCamera);

   //         CameraChange.GetComponentInChildren<TMP_Text>().text = $"Camera N - 0";

		}

        private void OnEnable()
        {
			AttackJoystics.SetPosition();
			MoveJoystics.SetPosition();
		}

        public void ClearClamp()
        {
	        ClampClearText.gameObject.SetActive(true);
	        ClampClearText.DOComplete();
	        ClampClearText.transform.DOScale(Vector3.one * 1.4f, 1f).SetLoops(-1, LoopType.Yoyo);
			AsyncClear();
        }

        private async UniTaskVoid AsyncClear()
        {
            await UniTask.Delay(2100, cancellationToken: destroyCancellationToken);

            ClampClearText.gameObject.SetActive(false);
            await UniTask.Delay(700, cancellationToken: destroyCancellationToken);
			Service<SceneData>.Get().Sounder.Stop();
        }

        public void Next()
        {
            Camps[_camps].Complete();
            _camps++;
            Camps[_camps].Set();
		}

        public void ResetCamp()
        {
	        foreach (var camp in Camps)
            {
                camp.ResetCamp();
            }
			_camps = 0;
            Camps[_camps].Set();
		}

        private void AttackDir(JoystickDir dir)
        {
	        var ecsWorld = Service<EcsWorld>.Get();
            if (ecsWorld != null)
            {
	            ecsWorld.NewEntity().Get<JoystickDir>() = dir;
			}
        }

        private void EditCamera()
        {
            var cameraPool = Service<SceneData>.Get().CameraPool;

            cameraPool[_currentIndexCamera].gameObject.SetActive(false);

			_currentIndexCamera++;

            if (_currentIndexCamera == cameraPool.Length)
            {
                _currentIndexCamera = 0;
            }

			Service<RuntimeData>.Get().CurrentCamera = cameraPool[_currentIndexCamera];

			cameraPool[_currentIndexCamera].gameObject.SetActive(true);

            CameraChange.GetComponentInChildren<TMP_Text>().text = $"Camera N - {_currentIndexCamera}";
		}

        private void OffAttack(bool arg0)
        {
            Service<StaticData>.Get().AutoAttack = arg0;
			AttackJoystics.gameObject.SetActive(!arg0);

            if (arg0)
            {
	            MoveJoystics.MiddlePosition();
			}
            else
            {
	            MoveJoystics.BasicPosition();
			}
		}
    }
}