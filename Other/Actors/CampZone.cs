using UnityEngine;
using UnityEngine.Events;

namespace Zlodey.Actors
{
	public sealed class CampZone : MonoBehaviour
	{
		public EntityActor[] Actors;

		public UnityEvent Events;
		public void Event()
		{
			Events?.Invoke();
		}
	}
}