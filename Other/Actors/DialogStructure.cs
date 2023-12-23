using System;
using UnityEngine;
using UnityEngine.Events;

namespace Zlodey.Actors
{
	[Serializable]
	public struct DialogStructure
	{
		public CharacterData Character;
		public Transform Unit;
		public UnityEvent Events;
		public UnityEvent EventsEndDialog;
		public string Description;
	}
}