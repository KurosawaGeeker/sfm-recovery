using System;
using System.Collections.Generic;
using Common.Scripts.UI;
using ExposureUnnoticed2.Master.Achievement;
using HadakaCoat.ObjectsUi.Common.Button;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.AchievementPanel
{
	public class AchievementPanelView : BasePanelView
	{
		[SerializeField]
		private GameObject itemPrefab;

		[SerializeField]
		private Transform itemParent;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		private int currentSelectIndex;

		private List<AchievementPanelItemView> viewList;

		private void Initialize(Action onClose)
		{
		}

		private void ToPadModeIfNeed()
		{
		}

		private void RefreshView()
		{
		}

		private void OnClickReceive(RAchievement record)
		{
		}

		private void LateUpdate()
		{
		}

		public static void Open(Action onClose)
		{
		}
	}
}
