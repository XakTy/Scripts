using Leopotam.Ecs;
using UnityEngine;

namespace Zlodey.Actors
{
	public abstract class HealthView : ConverterMonoComponent
	{
		public abstract void SetValue(float value, float maxHealth);
		public override void Convert(EcsEntity entity)
		{
			entity.Get<HealthViewRef>().value = this;
		}
	}
}