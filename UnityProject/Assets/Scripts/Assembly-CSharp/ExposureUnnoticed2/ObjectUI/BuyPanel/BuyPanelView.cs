using System;
using Common.Scripts.UI;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.BuyPanel
{
	public class BuyPanelView : BasePanelView
	{
		public enum BuyPanelType
		{
			Clothes = 0,
			AdultGoods = 1
		}

		[SerializeField]
		private Transform itemParent;

		[SerializeField]
		private GameObject itemPrefab;

		private BuyPanelType type;

		private void Initialize(BuyPanelType type, Action onClose)
		{
		}

		private void LateUpdate()
		{
		}

		public void OnClickBack()
		{
		}

		private void ClothesInitialize()
		{
		}

		private void AdultGoodsInitialize()
		{
		}

		public static void Open(BuyPanelType type, Action onClose, Transform parent = null)
		{
		}
	}
}
