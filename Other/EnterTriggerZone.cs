using UnityEngine;
using UnityEngine.Events;
using Zlodey.Actors;

namespace InterfaceWay
{
	public sealed class EnterTriggerZone : MonoBehaviour
	{
		public UnityEvent Events;
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent<Player>(out var pl))
			{
				Events?.Invoke();
				gameObject.SetActive(false);
			}
		}
	}
}