using System;
using System.Collections.Generic;
using Common.CommonUI.CommonScrollView;
using Common.Scripts.UI;
using ExposureUnnoticed2.Scripts.Base;
using HadakaCoat.ObjectsUi.Common.Button;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.SaveDataSelectPanel
{
	public class SaveDataSelectPanelView : BasePanelView
	{
		[SerializeField]
		private GameObject itemPrefab;

		[SerializeField]
		private Transform itemParent;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		[SerializeField]
		private CommonScrollRectView commonScrollRectView;

		private static SaveDataSelectPanelView instance;

		private int currentSelectIndex;

		private List<SaveSlotItemView> childViewList;

		private void Initialize(Action onClose)
		{
		}

		private void ToPadModeIfNeed()
		{
		}

		private void LateUpdate()
		{
		}

		private void TransScene()
		{
		}

		private void OnClickItem(SaveRapper.SaveDataType type, int slot)
		{
		}

		public void OnClickSaveCategory()
		{
		}

		public void OnClickManualCategory()
		{
		}

		public void OnClickBack()
		{
		}

		public static void Open(Action onClose, Transform parent = null)
		{
		}
	}
}
