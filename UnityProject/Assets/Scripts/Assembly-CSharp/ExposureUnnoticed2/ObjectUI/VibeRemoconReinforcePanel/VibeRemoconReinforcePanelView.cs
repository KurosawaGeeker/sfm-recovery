using Common.Scripts.UI;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.VibeRemoconReinforcePanel
{
	public class VibeRemoconReinforcePanelView : BasePanelView
	{
		private const int MaxLevel = 3;

		[SerializeField]
		private ButtonView reinforceButton;

		[SerializeField]
		private TextMeshProUGUI currentLevelText;

		[SerializeField]
		private TextMeshProUGUI costText;

		private int reinforceCost;

		private void Initialize()
		{
		}

		private void RefreshView()
		{
		}

		private void Update()
		{
		}

		public void OnClickReinforce()
		{
		}

		public static void Open()
		{
		}
	}
}
