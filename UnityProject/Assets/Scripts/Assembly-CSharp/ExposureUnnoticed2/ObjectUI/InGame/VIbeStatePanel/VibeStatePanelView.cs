using ExposureUnnoticed2.Master.AdultGoods;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.InGame.VIbeStatePanel
{
	public class VibeStatePanelView : MonoBehaviour
	{
		[SerializeField]
		private Image vibeImage;

		[SerializeField]
		private Image pistonImage;

		private VibrationModeType currentVibeType;

		private int currentPistonMode;

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x4FCC30
			currentVibeType = (VibrationModeType)0;
			currentPistonMode = 0;
			EventManager.AddListener<OptionChangeEvent>(OnChange, false);
			Refresh();
			OnChange(null);
		}

		private void OnDestroy()
		{
			// Recovered from GameAssembly.dll RVA 0x4FCE00
			EventManager.RemoveListener<OptionChangeEvent>(OnChange);
		}

		public void OnLateUpdate()
		{
			// Recovered from GameAssembly.dll RVA 0x4FCEC0
			var capturedMode = currentVibeType;
			if (capturedMode == global::ExposureUnnoticed2.Object3D.AdultGoods.CommonVibratorController.VibrationStrength &&
			    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PistonMachineController.CurrentSpeedType == currentPistonMode)
			    return;
			Refresh();
		}

		private void Refresh()
		{
			// Recovered from GameAssembly.dll RVA 0x4FCF80
			var capturedMode = global::ExposureUnnoticed2.Object3D.AdultGoods.CommonVibratorController.VibrationStrength;
			currentVibeType = capturedMode;
			int nextPistonMode = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PistonMachineController.CurrentSpeedType;
			var capturedVibeImage = vibeImage;
			currentPistonMode = nextPistonMode;
			capturedVibeImage.color = capturedMode switch
			{
			    (VibrationModeType)0 => new Color(0f, 0f, 0f, 1f),
			    (VibrationModeType)1 => new Color(0f, 0f, 1f, 1f),
			    (VibrationModeType)2 => new Color(1f, 0f, 0f, 1f)
			};
			int capturedPistonMode = currentPistonMode;
			var capturedPistonImage = pistonImage;
			capturedPistonImage.color = capturedPistonMode switch
			{
			    0 => new Color(0f, 0f, 0f, 1f),
			    1 => new Color(0f, 0f, 1f, 1f),
			    2 => new Color(0f, 1f, 0f, 1f),
			    3 => new Color(1f, 0f, 0f, 1f)
			};
		}

		private void OnChange(OptionChangeEvent evt)
		{
			// Recovered from GameAssembly.dll RVA 0x4FD1C0
			var firstOption = global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData;
			if (firstOption is null) throw new global::System.NullReferenceException();
			var firstImage = vibeImage;
			bool firstEnabled = firstOption.VibeStateColor;
			firstImage.enabled = firstEnabled;
			var secondOption = global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData;
			if (secondOption is null) throw new global::System.NullReferenceException();
			var secondImage = pistonImage;
			bool secondEnabled = secondOption.VibeStateColor;
			secondImage.enabled = secondEnabled;
		}
	}
}
