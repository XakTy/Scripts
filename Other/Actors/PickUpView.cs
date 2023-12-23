using LeopotamGroup.Globals;
using UnityEngine;

namespace Zlodey.Actors
{
	public sealed class PickUpView : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent<Player>(out var player))
			{
				var money = PlayerPrefs.GetInt("Money");
				money += 25;
				PlayerPrefs.SetInt("Money", money);
				Service<UI>.Get().GameScreen.CoinView.SetCoin(money);

				gameObject.SetActive(false);
			}
		}
	}
}