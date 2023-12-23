using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Zlodey.Actors
{
	public sealed class HealthBar : HealthView
	{
		[SerializeField] private Image _backgroundImage;
		[SerializeField] private Image _mainHealthImage;
		public override void SetValue(float currentHealth, float maxHealth)
		{
			_backgroundImage.gameObject.SetActive(currentHealth < maxHealth && currentHealth > 0);
			_mainHealthImage.fillAmount = currentHealth / maxHealth;
		}
	}
}