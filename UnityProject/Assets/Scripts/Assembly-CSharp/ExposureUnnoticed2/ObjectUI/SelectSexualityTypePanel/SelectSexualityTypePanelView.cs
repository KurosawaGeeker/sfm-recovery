using Common.Scripts.UI;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.SelectSexualityTypePanel
{
	public class SelectSexualityTypePanelView : BasePanelView
	{
		[SerializeField]
		private ButtonView[] itemButtons;

		[SerializeField]
		private TextMeshProUGUI description;

		[SerializeField]
		private ButtonGroupManager buttonGroup;

		private PlayerStateModel.SexualityType currentSelect;

		private void Initialize()
		{
		}

		private void RefreshView()
		{
		}

		private void Update()
		{
		}

		public void OnClickCancel()
		{
		}

		public void OnClickApply()
		{
		}

		public void OnClickItem(int index)
		{
		}

		public static void Open()
		{
		}
	}
}
