using UnityEngine;

namespace Zlodey.Actors
{
	public sealed class DialogStartEvent : MonoBehaviour
	{
		public DialogMono Dialog;
		private void Start()
		{
			Dialog.Show();
		}
	}
}