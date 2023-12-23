using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Zlodey.Actors
{
	public sealed class ProgressAttackView : ConverterMonoComponent
	{
		[SerializeField] private Image _backgroundProgress;
		[SerializeField] private Image _mainProgressColor;

		public void SetState(bool state)
		{
			_backgroundProgress.gameObject.SetActive(state);
		}
		public void Full(float duration)
		{
			_mainProgressColor.DOFillAmount(1f, duration).onComplete += () =>
			{
				_mainProgressColor.fillAmount = 0f;
				_backgroundProgress.gameObject.SetActive(false);
			};
		}

		public override void Convert(EcsEntity entity)
		{
			entity.Get<ProgressAttackViewRef>().value = this;
		}
	}
}