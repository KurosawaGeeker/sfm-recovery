using System;
using Common.Scripts.UI;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.OptionMenu
{
	public class OptionMenuView : BasePanelView
	{
		private enum MiddleType
		{
			None = 0,
			GameSystem = 1,
			KeyConfig = 2,
			UI = 3,
			Sound = 4,
			Control = 5
		}

		[SerializeField]
		private Transform middleParent;

		[SerializeField]
		private GameObject gameSystemPanel;

		[SerializeField]
		private GameObject uiPanel;

		[SerializeField]
		private GameObject soundPanel;

		[SerializeField]
		private GameObject controlPanel;

		[SerializeField]
		private GameObject keyConfigPanel;

		[SerializeField]
		private TextMeshProUGUI descriptionTitle;

		[SerializeField]
		private TextMeshProUGUI description;

		[SerializeField]
		private GameObject descriptionParent;

		[SerializeField]
		private ButtonGroupManager categoryGroup;

		[SerializeField]
		private ButtonView keyconfigButton;

		private MiddleType currentOpen;

		private IOptionMiddlePanel currentOpenView;

		private bool skipForceSelect;

		private void Initialize()
		{
		}

		private void Update()
		{
		}

		private void CommonOnClickProcess(Action openProcess, MiddleType type)
		{
		}

		public void OnClickGameSystem()
		{
		}

		public void OnClickUi()
		{
		}

		public void OnClickSound()
		{
		}

		public void OnClickControl()
		{
		}

		public void OnClickKeyConfig()
		{
		}

		private void OnCloseChild()
		{
		}

		private void OnClickItem(OptionItemType type)
		{
		}

		public static void Open()
		{
		}
	}
}
