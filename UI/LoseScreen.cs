using UnityEngine.UI;

namespace Zlodey
{
	public class LoseScreen : Screen
	{
		public Button RestartButton;

		private void OnEnable()
		{
			RestartButton.onClick.AddListener(RestartLevel);
		}

		private void OnDisable()
		{
			RestartButton.onClick.RemoveListener(RestartLevel);
		}
		public void RestartLevel()
		{
			Levels.LoadCurrent();
		}
	}
}