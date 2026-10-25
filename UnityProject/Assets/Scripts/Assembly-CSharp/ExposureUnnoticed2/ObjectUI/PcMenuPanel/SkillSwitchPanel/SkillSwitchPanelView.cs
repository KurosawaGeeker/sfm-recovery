using System.Collections.Generic;
using Common.CommonUI.CommonScrollView;
using Common.Scripts.UI;
using ExposureUnnoticed2.Master.Skill;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.PcMenuPanel.SkillSwitchPanel
{
	public class SkillSwitchPanelView : BasePanelView
	{
		[SerializeField]
		private Transform itemParent;

		[SerializeField]
		private GameObject itemPrefab;

		[SerializeField]
		private TextMeshProUGUI currentSkillName;

		[SerializeField]
		private TextMeshProUGUI currentSkillDescription;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		[SerializeField]
		private CommonScrollRectView commonScrollRectView;

		private List<SkillSwitchItemView> itemList;

		private int currentSelectIndex;

		private void Initialize()
		{
		}

		private void ToPadModeIfNeed(bool isForce = false)
		{
		}

		private void Update()
		{
		}

		private void RefreshItem()
		{
		}

		private void OnClickItem(MSkillRecord record)
		{
		}

		private void OnClose()
		{
		}

		public static void Open()
		{
		}
	}
}
