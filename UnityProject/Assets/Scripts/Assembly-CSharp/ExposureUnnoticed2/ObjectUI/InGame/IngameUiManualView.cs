using ExposureUnnoticed2.Scripts.Base;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InGame
{
	public class IngameUiManualView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI openCoat;

		[SerializeField]
		private TextMeshProUGUI closeCoat;

		[SerializeField]
		private TextMeshProUGUI openHip;

		[SerializeField]
		private TextMeshProUGUI dash;

		[SerializeField]
		private TextMeshProUGUI crouch;

		[SerializeField]
		private TextMeshProUGUI viewChange;

		[SerializeField]
		private TextMeshProUGUI ringMenu;

		[SerializeField]
		private TextMeshProUGUI poseRingMenu;

		[SerializeField]
		private TextMeshProUGUI menu;

		[SerializeField]
		private TextMeshProUGUI systemMenu;

		[SerializeField]
		private TextMeshProUGUI sexCumShot;

		[SerializeField]
		private TextMeshProUGUI sexMenu;

		[SerializeField]
		private TextMeshProUGUI sexSystemMenu;

		[SerializeField]
		private GameObject pcParent;

		[SerializeField]
		private GameObject padParent;

		[SerializeField]
		private GameObject pcSexParent;

		[SerializeField]
		private GameObject padSexParent;

		[SerializeField]
		private GameObject hipPc;

		[SerializeField]
		private GameObject hipPad;

		private bool isPad;

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x4FC040
			Refresh(null);
			EventManager.AddListener<OptionChangeEvent>(Refresh, false);
			pcParent.SetActive(true);
			padParent.SetActive(false);
			pcSexParent.SetActive(false);
			padSexParent.SetActive(false);
			var capturedHipPc = hipPc;
			bool pcHipEnabled = !SystemManager.IsTrial;
			capturedHipPc.SetActive(pcHipEnabled);
			var capturedHipPad = hipPad;
			bool padHipEnabled = !SystemManager.IsTrial;
			capturedHipPad.SetActive(padHipEnabled);
		}

		private void OnDestroy()
		{
			// Recovered from GameAssembly.dll RVA 0x4FC370
			EventManager.RemoveListener<OptionChangeEvent>(Refresh);
		}

		public void OnUpdate()
		{
			// Recovered from GameAssembly.dll RVA 0x4FC430
			bool capturedOldPad = isPad;
			if (capturedOldPad == InputManager.IsGamePad) return;
			bool nextPad = InputManager.IsGamePad;
			var capturedPc = pcParent;
			isPad = nextPad;
			capturedPc.SetActive(!nextPad);
			var capturedPad = padParent;
			bool currentPad = isPad;
			capturedPad.SetActive(currentPad);
			pcSexParent.SetActive(false);
			padSexParent.SetActive(false);
		}

		private void Refresh(OptionChangeEvent evt)
		{
			// Recovered from GameAssembly.dll RVA 0x4FC670
			var capturedOpenCoat = openCoat;
			string openCoatLabel = Localizer.Trans("ui_ingame_manual_open_coat", InputManager.GetString((InputManager.InputType)5));
			capturedOpenCoat.text = openCoatLabel;
			var capturedCloseCoat = closeCoat;
			string closeCoatLabel = Localizer.Trans("ui_ingame_manual_close_coat", InputManager.GetString((InputManager.InputType)6));
			capturedCloseCoat.text = closeCoatLabel;
			var capturedOpenHip = openHip;
			string openHipLabel = Localizer.Trans("ui_ingame_manual_open_hip", InputManager.GetString((InputManager.InputType)12));
			capturedOpenHip.text = openHipLabel;
			var capturedDash = dash;
			string dashLabel = Localizer.Trans("ui_ingame_manual_dash", InputManager.GetString((InputManager.InputType)3));
			capturedDash.text = dashLabel;
			var capturedCrouch = crouch;
			string crouchLabel = Localizer.Trans("ui_ingame_manual_crouch", InputManager.GetString((InputManager.InputType)7));
			capturedCrouch.text = crouchLabel;
			var capturedViewChange = viewChange;
			string viewChangeLabel = Localizer.Trans("ui_ingame_manual_viewChange", InputManager.GetString((InputManager.InputType)4));
			capturedViewChange.text = viewChangeLabel;
			var capturedRingMenu = ringMenu;
			string ringMenuFormat = Localizer.Trans("ui_ingame_manual_ring_menu_hold");
			object[] bindings = new object[4];
			bindings[0] = InputManager.GetString((InputManager.InputType)14);
			bindings[1] = InputManager.GetString((InputManager.InputType)23);
			bindings[2] = InputManager.GetString((InputManager.InputType)27);
			bindings[3] = InputManager.GetString((InputManager.InputType)28);
			string ringMenuLabel = string.Format(ringMenuFormat, bindings);
			capturedRingMenu.text = ringMenuLabel;
			var capturedSexMenu = sexMenu;
			var capturedMenu = menu;
			string menuLabel = Localizer.Trans("ui_ingame_manual_menu", InputManager.GetString((InputManager.InputType)2));
			capturedMenu.text = menuLabel;
			capturedSexMenu.text = menuLabel;
			var capturedSexSystemMenu = sexSystemMenu;
			var capturedSystemMenu = systemMenu;
			string systemMenuLabel = Localizer.Trans("ui_ingame_manual_system_menu", InputManager.GetString((InputManager.InputType)16));
			capturedSystemMenu.text = systemMenuLabel;
			capturedSexSystemMenu.text = systemMenuLabel;
			var capturedSexCumShot = sexCumShot;
			string cumShotLabel = Localizer.Trans("ui_manual_cumShot_0", InputManager.GetString((InputManager.InputType)8));
			capturedSexCumShot.text = cumShotLabel;
		}
	}
}
