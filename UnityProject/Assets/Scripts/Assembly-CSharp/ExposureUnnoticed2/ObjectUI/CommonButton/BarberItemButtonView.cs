using System;
using ExposureUnnoticed2.ObjectUI.PcMenuPanel.OnlineShopPanel;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.CommonButton
{
	public class BarberItemButtonView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI text;

		[SerializeField]
		private TextMeshProUGUI price;

		[SerializeField]
		private GameObject purchasedMask;

		[SerializeField]
		private CanvasGroup emitCanvasGroup;

		private Action<IOnlineShopItemRecord, bool> onClick;

		private Action onClickPurchase;

		private IOnlineShopItemRecord record;

		private bool isBring;

		public void Initialize(IOnlineShopItemRecord record, Action<IOnlineShopItemRecord, bool> onClick, bool isPurchased, Action onClickPurchase)
		{
            this.record = record;
            this.onClickPurchase = onClickPurchase;
            isBring = isPurchased;
            text.text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(record.NameKey);
            price.text = record.Price.ToString();
            this.onClick = onClick;
            purchasedMask.SetActive(isPurchased);
		}

		public void OnClick()
		{
            onClick?.Invoke(record, isBring);
		}

		public void OnClickPurchase()
		{
            if (OnlineShopPanelView.Instance.CurrentRecord != record) onClick?.Invoke(record, isBring);
            onClickPurchase?.Invoke();
		}

		public void ForceSetEmit(int alpha)
		{
            emitCanvasGroup.alpha = alpha;
		}
	}
}
