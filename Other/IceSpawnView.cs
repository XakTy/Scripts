using UnityEngine;

namespace InterfaceWay
{
	public sealed class IceSpawnView : SpawnView
	{
		public ParticleSystem[] ParticleExplosion;
		public GameObject IceUnit;
		public override void Play()
		{
			foreach (var system in ParticleExplosion)
			{
				system.Play();
			}
			IceUnit.gameObject.SetActive(false);
		}
	}
}