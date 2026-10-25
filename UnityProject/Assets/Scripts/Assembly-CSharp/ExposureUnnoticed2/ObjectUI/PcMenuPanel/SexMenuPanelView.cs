using Common.Scripts.UI;
using ExposureUnnoticed2.Master.Sex;
using ExposureUnnoticed2.ObjectUI.OptionMenu;
using ExposureUnnoticed2.Scripts.Base;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.PcMenuPanel
{
	public class SexMenuPanelView : BasePanelView
	{
		[SerializeField]
		private OptionItemBarView xBar;

		[SerializeField]
		private OptionItemBarView zBar;

		[SerializeField]
		private OptionItemBarView yBar;

		[SerializeField]
		private OptionItemBarView rotateBar;

		[SerializeField]
		private OptionItemCheckView saoHantoumei;

		[SerializeField]
		private OptionItemCheckView saoOnly;

		[SerializeField]
		private Transform itemParent;

		[SerializeField]
		private Transform itemPrefab;

		[SerializeField]
		private ButtonView shotButton;

		[SerializeField]
		private GameObject pcSexParent;

		[SerializeField]
		private GameObject padSexParent;

		[SerializeField]
		private TextMeshProUGUI sexMenu;

		[SerializeField]
		private TextMeshProUGUI sexSystemMenu;

		private bool isOpenChild;

		private int currentSelectIndex;

		private CanvasGroup canvasGroup;

		private SmoothFloat alpha;

		private bool isVisible;

		private bool isExited;

		private RSex currentSex;

		private bool isPad;

		private void Initialize()
		{
		}

		private void OnClickSexItem(RSex rSex)
		{
		}

		public void OnClickReset(int index)
		{
		}

		public void OnClickShot()
		{
		}

		private void Update()
		{
		}

		private void LateUpdate()
		{
		}

		public void OnClickExit()
		{
		}

		public static void Open()
		{
		}
	}
}
