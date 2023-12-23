using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Zlodey.Actors
{
	public sealed class DialogScreen : Screen
	{
		[field: SerializeField] public Button ButtonDialog { get; private set; }
		[field: SerializeField] public TMP_Text NameT { get; private set; }
		[field: SerializeField] public TMP_Text DescriptionT { get; private set; }
		[field: SerializeField] public RawImage Portrait { get; private set; }

		public void SetValue(CharacterData character, string text)
		{
			NameT.text = character.Name;
			DescriptionT.text = text;
		}
		public void SetText(string text)
		{
			DescriptionT.text = text;
		}

	}
}