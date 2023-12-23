using UnityEngine;

namespace Zlodey.Actors
{
	[CreateAssetMenu(fileName = "Character Dialog")]
	public sealed class DropData : ScriptableObject
	{
		[field: SerializeField] public PickUpView View { get; private set; }
		[field: SerializeField] public float Chance { get; private set; }
	}
}