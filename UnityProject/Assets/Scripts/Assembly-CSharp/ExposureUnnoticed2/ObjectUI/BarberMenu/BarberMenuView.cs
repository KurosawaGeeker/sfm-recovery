using Common.Scripts.UI;
using ExposureUnnoticed2.Master.Hair;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.BarberMenu
{
	public class BarberMenuView : BasePanelView
	{
		[SerializeField]
		private TextMeshProUGUI totalRpText;

		[SerializeField]
		private GameObject itemPrefab;

		[SerializeField]
		private GameObject hairScrollParent;

		[SerializeField]
		private Transform hairItemParent;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		[SerializeField]
		private ButtonView purchaseButton;

		private MHairRecord oldHair;

		private int totalRp;

		private MHairRecord currentSelect;

		private void Initialize()
		{
		}

		private void RefreshView()
		{
		}

		private void OnClickItem(MHairRecord record)
		{
		}

		private void LateUpdate()
		{
		}

		public void OnClickBack()
		{
		}

		public void OnClickPurchase()
		{
		}

		public static void Open()
		{
		}
	}
}
