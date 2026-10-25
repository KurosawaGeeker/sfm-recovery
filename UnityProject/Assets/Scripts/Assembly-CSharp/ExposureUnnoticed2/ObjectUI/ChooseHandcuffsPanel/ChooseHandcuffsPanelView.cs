using Common.Scripts.UI;
using ExposureUnnoticed2.Master.Action;
using ExposureUnnoticed2.Master.AdultGoods;
using HadakaCoat.ObjectsUi.Common.Button;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.ChooseHandcuffsPanel
{
	public class ChooseHandcuffsPanelView : BasePanelView
	{
		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		private MAdultGoodsType handcuffType;

		private int currentSelectIndex;

		private void Initialize(MAdultGoodsType handcuffType)
		{
		}

		private void ToPadModeIfNeed()
		{
		}

		private void LateUpdate()
		{
		}

		public void OnClickCancel()
		{
		}

		public void OnClickPutBack()
		{
		}

		public void OnClickPutMap()
		{
		}

		private void TransAction(ActionType type)
		{
		}

		public static void Open(MAdultGoodsType handcuffType)
		{
		}
	}
}
