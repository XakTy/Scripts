using DG.Tweening;
using UnityEngine;

namespace Zlodey.Actors
{
	public class CircleProgressionView : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _backgroundImage;
		[SerializeField] private SpriteRenderer _mainImage;
		public void Full(float duration)
		{
			_backgroundImage.gameObject.SetActive(true);
			_mainImage.transform.DOScale(Vector3.zero, duration).From().onComplete += () =>
			{
				_backgroundImage.gameObject.SetActive(false);
			};
		}
	}
}