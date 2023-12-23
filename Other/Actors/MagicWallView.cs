using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Zlodey.Actors
{
	public sealed class MagicWallView : MonoBehaviour
	{
		public ParticleSystem[] Particles;
		public Image Closer;
		public void Show()
		{
			foreach (var particle in Particles)
			{
				particle.Play();
			}

			Closer.transform.DOScale(Vector3.one, 0.2f);
		}
		public void Hide()
		{
			foreach (var particle in Particles)
			{
				particle.Stop();
			}

			Closer.transform.DOScale(Vector3.zero, 0.2f);
		}
	}
}