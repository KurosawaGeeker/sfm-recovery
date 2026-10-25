using System.Collections.Generic;
using Common.CommonUI.CommonScrollView;
using Common.Scripts.UI;
using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.ObjectUI.CommonUI.CommonTab;
using ExposureUnnoticed2.ObjectUI.InventoryPanel.CosplaySerieseSelectPanel;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.PcMenuPanel.ReinforcePanel
{
	public class ReinforcePanelView : BasePanelView
	{
		private enum CategoryTab
		{
			AdultGoods = 0,
			Cosplay = 1,
			Costume = 2
		}

		private struct Model
		{
			public int Price;

			public string DescriptionKey;

			public string DescriptionNegativeKey;
		}

		[SerializeField]
		private CommonTabView tabView;

		[SerializeField]
		private Transform itemParent;

		[SerializeField]
		private GameObject itemPrefab;

		[SerializeField]
		private GameObject reinforceEffectItemPrefab;

		[SerializeField]
		private GameObject noSelectMask;

		[SerializeField]
		private Transform currentSelectDetailParent;

		[SerializeField]
		private TextMeshProUGUI currentSelectItemName;

		[SerializeField]
		private TextMeshProUGUI currentRankDescription;

		[SerializeField]
		private TextMeshProUGUI currentPrice;

		[SerializeField]
		private ButtonView reinforceButton;

		[SerializeField]
		private GameObject resultPanelPrefab;

		[SerializeField]
		private GameObject settingPanelPrefab;

		[SerializeField]
		private GameObject basicParent;

		[SerializeField]
		private GameObject cosplayParent;

		[SerializeField]
		private Transform cosplayItemParent;

		[SerializeField]
		private ButtonGroupManager basicButtonGroupManager;

		[SerializeField]
		private ButtonGroupManager cosplayButtonGroupManager;

		[SerializeField]
		private ButtonGroupManager rankButtonGroupManager;

		[SerializeField]
		private CosplaySeriesSelectPanelView cosplaySeriesSelectPanelView;

		[SerializeField]
		private ButtonView buttonRank2;

		[SerializeField]
		private ButtonView buttonRank3;

		[SerializeField]
		private ScrollRect scrollRect;

		[SerializeField]
		private ScrollRect scrollRectCosplay;

		[SerializeField]
		private CommonScrollRectView commonScrollRectView;

		[SerializeField]
		private CommonScrollRectView cosplayCommonScrollRectView;

		private int currentRank;

		private IReinforcableRecord currentRecord;

		private ReinforceEffectGacha gacha;

		private bool isOpenChild;

		private int currentTabIndex;

		private IReinforcableRecord first;

		private int currentSeriesIndex;

		private List<CosplayType> bringCosplaySeriesList;

		private float EscLockTime;

		private int currentSelectIndex;

		private int currentSelectRankIndex;

		private List<ReinforceItemView> itemList;

		private Model[] model;

		public static ReinforcePanelView Instance { get; private set; }

		public bool IsCompleteSave { get; private set; }

		private void Initialize()
		{
		}

		private void ToPadModeIfNeed(bool isForce = false)
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private void RefreshCurrentSelectDetail()
		{
		}

		private void RefreshReinforceDetail()
		{
		}

		private int SettingCostScale(IReinforcableRecord record)
		{
			return 0;
		}

		private void OnClickTab(int index)
		{
		}

		private void ChangeSeries(int changeAmount)
		{
		}

		private void OnClickItem(IReinforcableRecord record)
		{
		}

		public void OnClickSelectRank(int rank)
		{
		}

		public void OnClickReinforce()
		{
		}

		public void OnClickSetting()
		{
		}

		public static void Open()
		{
		}
	}
}
