using Leopotam.Ecs;
using UnityEngine;

namespace Zlodey.Actors
{
	public abstract class ConverterMonoComponent : MonoBehaviour
	{
		public abstract void Convert(EcsEntity entity);
	}

	public class FractionView : ConverterMonoComponent
	{
		public ParticleSystem[] Particles;
		public void Died()
		{
			foreach (var particle in Particles)
			{
				particle.Stop();
			}
		}
		public override void Convert(EcsEntity entity)
		{
			entity.Get<FractionViewRef>().value = this;
		}
	}
}