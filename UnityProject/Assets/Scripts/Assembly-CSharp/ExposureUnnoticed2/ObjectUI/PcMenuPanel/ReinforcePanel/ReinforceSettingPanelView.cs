using System;
using ExposureUnnoticed2.ObjectUI.OptionMenu;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.PcMenuPanel.ReinforcePanel
{
	public class ReinforceSettingPanelView : MonoBehaviour
	{
		[SerializeField]
		private CustomizeCheckItemView slot1View;

		[SerializeField]
		private CustomizeCheckItemView slot2View;

		[SerializeField]
		private CustomizeCheckItemView slot3View;

		[SerializeField]
		private CustomizeCheckItemView onlyNegativeView;

		[SerializeField]
		private TextMeshProUGUI slot1Text;

		[SerializeField]
		private TextMeshProUGUI slot2Text;

		[SerializeField]
		private TextMeshProUGUI slot3Text;

		[SerializeField]
		private TextMeshProUGUI costScaleText;

		[SerializeField]
		private TextMeshProUGUI title;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		private int currentSelectIndex;

		private bool[] settings;

		private Action<bool[]> onClose;

		private bool isOnInit;

		public void Initialize(IReinforcableRecord record, bool[] settings, Action<bool[]> onClose)
		{
		}

		private void Refresh()
		{
		}

		private void ToPadModeIfNeed()
		{
		}

		private void Update()
		{
		}

		public void OnClickDecide()
		{
		}

		public void OnAnimCloseEnd()
		{
		}
	}
}
