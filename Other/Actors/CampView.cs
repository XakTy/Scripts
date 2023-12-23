using UnityEngine;

namespace Zlodey.Actors
{
	public sealed class CampView : MonoBehaviour
	{
		public GameObject Unactive;
		public GameObject CurrentLevel;
		public GameObject OldLevel;
		public void Set()
		{
			Unactive.SetActive(false);
			OldLevel.SetActive(false);
			CurrentLevel.SetActive(true);
		}
		public void Complete()
		{
			OldLevel.SetActive(true);
			CurrentLevel.SetActive(false);
		}

		public void ResetCamp()
		{

			Unactive.SetActive(true);
			OldLevel.SetActive(false);
			CurrentLevel.SetActive(false);
		}
	}
}