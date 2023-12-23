using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Zlodey
{
	public sealed class CoinView : MonoBehaviour
	{
		[field:SerializeField] public TMP_Text TextMoney { get; private set; }
		[field: SerializeField] public Image CoinImage { get; private set; }

		private Tween _currentTween;
		public void SetCoin(int coins)
		{
			TextMoney.text = coins.ToString();

			if (_currentTween != null)
			{
				return;
			}

			_currentTween = CoinImage.transform.DOScale(Vector3.one * 1.1f, 0.2f).SetLoops(2, LoopType.Yoyo);

			_currentTween.onComplete += () => { _currentTween = null; };
		}
	}
}