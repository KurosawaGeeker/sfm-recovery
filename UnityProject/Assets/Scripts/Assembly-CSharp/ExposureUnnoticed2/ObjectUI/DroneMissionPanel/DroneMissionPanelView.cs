using System.Collections.Generic;
using Common.CommonUI.CommonScrollView;
using Common.Scripts.UI;
using ExposureUnnoticed2.Master.DroneMission;
using ExposureUnnoticed2.ObjectUI.CommonUI.CommonTab;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.DroneMissionPanel
{
	public class DroneMissionPanelView : BasePanelView
	{
		[SerializeField]
		private GameObject buttonPrefab;

		[SerializeField]
		private Transform buttonParent;

		[SerializeField]
		private TextMeshProUGUI rewardText;

		[SerializeField]
		private TextMeshProUGUI missionLevelText;

		[SerializeField]
		private TextMeshProUGUI missionNameText;

		[SerializeField]
		private TextMeshProUGUI missionDescription;

		[SerializeField]
		private TextMeshProUGUI clearCondition;

		[SerializeField]
		private TextMeshProUGUI stageRpBonusText;

		[SerializeField]
		private TextMeshProUGUI[] startConditions;

		[SerializeField]
		private DroneMissionRecordItemView[] recordPerStageView;

		[SerializeField]
		private GameObject rightContents;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		[SerializeField]
		private CommonTabView tabView;

		[SerializeField]
		private ButtonView startButton;

		[SerializeField]
		private ButtonView interruptButton;

		[SerializeField]
		private ContentSizeFitter ContentSizeFitter;

		[SerializeField]
		private VerticalLayoutGroup VerticalLayoutGroup;

		[SerializeField]
		private GameObject dronePanelParent;

		[SerializeField]
		private RandomMissionPanelView randomMissionPanelView;

		[SerializeField]
		private GameObject dronePanelMask;

		[SerializeField]
		private GameObject randomPanelMask;

		[SerializeField]
		private CommonScrollRectView commonScrollRectView;

		private RDroneMission currentSelect;

		private int displayRank;

		private float stageRpBonus;

		private int currentSelectIndex;

		private int currentTabIndex;

		private List<DroneMissionPanelItemView> childViewList;

		public static DroneMissionPanelView Instance { get; private set; }

		private void Initialize()
		{
		}

		private void OnDestroy()
		{
		}

		private void ToPadModeIfNeed(bool isForce = false)
		{
		}

		private void Update()
		{
		}

		private void OnChangeTab(int index)
		{
		}

		private void OnChangeDisplayRank()
		{
		}

		private void RefreshRight()
		{
		}

		public void OnClickStart()
		{
		}

		public void OnClickInterrupt()
		{
		}

		public static void Open()
		{
            // Complete original RVA 0x517070; private Initialize remains a declared UI boundary.
            var panel = global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.Open(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.DroneMissionPanel, false) as DroneMissionPanelView;
            panel.Initialize();
        }
	}
}
