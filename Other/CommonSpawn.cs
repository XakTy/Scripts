using UnityEngine;
using Zlodey.Actors;

namespace InterfaceWay
{
	public sealed class CommonSpawn : SpawnView
	{
		public GameObject ObjectDisable;
		public override void Play()
		{
			ObjectDisable.SetActive(false);
		}
	}
}