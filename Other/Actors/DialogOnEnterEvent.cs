using UnityEngine;

namespace Zlodey.Actors
{
	public sealed class DialogOnEnterEvent : MonoBehaviour
	{
		public DialogMono Dialog;
		public Collider ColliderTrigger;
		private void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent<Player>(out _)) return;

			Dialog.Show();
			ColliderTrigger.enabled = false;
		}
	}
}