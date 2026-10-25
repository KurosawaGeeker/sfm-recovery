using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.BuyPanel
{
	public class BuyPanelItemView : MonoBehaviour
	{
		[SerializeField]
		private new TextMeshProUGUI name;

		[SerializeField]
		private TextMeshProUGUI necessaryRp;

		[SerializeField]
		private TextMeshProUGUI buttonName;

		[SerializeField]
		private GameObject purchasedTextObject;

		private IBuyPanelItem master;

		private IBasketController basketController;

		private BuyPanelView.BuyPanelType type;

		private bool isInBasket;

		public void Initialize(IBuyPanelItem master, IBasketController basketController, BuyPanelView.BuyPanelType type, bool isPurchased)
		{
		}

		public void OnClickButton()
		{
		}

		private void AddBasket()
		{
		}

		private void RemoveBasket()
		{
		}

		private void RefreshView()
		{
		}
	}
}
