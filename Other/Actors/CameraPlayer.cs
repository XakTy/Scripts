using NaughtyAttributes;
using UnityEngine;

namespace Zlodey.Actors
{
	public sealed class CameraPlayer : MonoBehaviour
	{
		public Camera Main;
		public Transform Target;
		public Vector3 Offset;

		[Button("Set offset")]
		public void UpdateOffset()
		{
			Offset = Main.transform.position - Target.position;
		}
	}
}