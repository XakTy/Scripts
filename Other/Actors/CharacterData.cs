using UnityEngine;

namespace Zlodey.Actors
{
	[CreateAssetMenu(fileName = "Character Dialog")]
	public sealed class CharacterData : ScriptableObject
	{
		[field: SerializeField] public string Name { get; private set; }
		[field: SerializeField] public Sprite Icon { get; private set; }
	}
}