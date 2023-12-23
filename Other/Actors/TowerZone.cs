using DG.Tweening;
using TMPro;
using UnityEngine;
using static System.TimeZoneInfo;
using static UnityEngine.GraphicsBuffer;

namespace Zlodey.Actors
{
	public sealed class TowerZone : MonoBehaviour
	{
		public TMP_Text Coins;

		public GameObject SpawnObject;
		public GameObject Visual;

		public ParticleSystem Dust;

		public float DurationMove;
		public float DurationSize;
		public float DoSize;

		public Ease EaseMove;
		public Ease EaseScela;
		private void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent<Player>(out _)) return;

			var ctx = 50;
			var zero = 0;

			DOTween.To(() => ctx, x => ctx = x, zero, 0.6f).OnUpdate(() => Coins.text = ctx.ToString()).onComplete += () =>
			{
				Visual.SetActive(false);
				SpawnObject.gameObject.SetActive(true);
				SpawnObject.transform.DOLocalMove(Vector3.up, DurationMove).SetEase(EaseMove).From().onComplete += () =>
				{
					Dust.Play();
				};
				SpawnObject.transform.DOScale(DoSize, DurationSize).SetLoops(2, LoopType.Yoyo).SetEase(EaseScela).SetDelay(DurationMove);
			};
		}
	}
}