using Common.Scripts.UI;
using HadakaCoat.ObjectsUi.Common.Button;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.ChooseDildoPanelView
{
	public class ChooseDildoPanelView : BasePanelView
	{
		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		private int currentSelectIndex;

		private void Initialize()
		{
            // Complete original 7-byte tail at RVA 0x53E260.
            ToPadModeIfNeed();
        }

		private void ToPadModeIfNeed()
		{
            // Complete original RVA 0x53E270; no active-panel gate or index clamp.
            if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad)
                buttonGroupManager.ForceSelect(currentSelectIndex);
        }

		private void LateUpdate()
		{
		}

		public void OnClickCancel()
		{
		}

		public void OnClickPutFloor()
		{
		}

		public void OnClickPutWall()
		{
		}

		public static void Open()
		{
            // Complete original RVA 0x53EA60; preserve wrong-type/null NRE.
            var panel = global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.Open(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.ChooseDildoPanel, false) as ChooseDildoPanelView;
            panel.Initialize();
        }
	}
}
