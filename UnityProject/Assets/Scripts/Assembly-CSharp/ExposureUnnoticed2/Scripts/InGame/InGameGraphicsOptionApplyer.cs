using ExposureUnnoticed2.ObjectUI.OptionMenu;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class InGameGraphicsOptionApplyer : MonoBehaviour
	{
		[SerializeField]
		private PostProcessLayer[] postProcessLayers;

		[SerializeField]
		private PostProcessVolume postProcessVolume;

		[SerializeField]
		private PostProcessVolume postProcessVolumeSelfCamera;

		[SerializeField]
		private bool isTitle;

		private float initBrightness;

		private OptionData optionData;

		private ColorGrading ColorGrading;

		private ColorGrading ColorGradingSelfCamera;

		private AmbientOcclusion AmbientOcclusion;

		private ScreenSpaceReflections ScreenSpaceReflections;

		private Bloom Bloom;

		private Bloom BloomSelf;

		private MotionBlur MotionBlur;

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x494DC0
			optionData = GameState.OptionData;
			EventManager.AddListener<GraphicsOptionChangeEvent>(OnChange);
			EventManager.AddListener<ChangeTimeEvent>(OnChangeTime);
			EventManager.AddListener<CompleteTransitionStageEvent>(OnChangeStage);
			if (!isTitle)
			{
			    ColorGradingSelfCamera = postProcessVolumeSelfCamera.profile.GetSetting<ColorGrading>();
			    BloomSelf = postProcessVolumeSelfCamera.profile.GetSetting<Bloom>();
			    ColorGrading = postProcessVolume.profile.GetSetting<ColorGrading>();
			    AmbientOcclusion = postProcessVolume.profile.GetSetting<AmbientOcclusion>();
			    ScreenSpaceReflections = postProcessVolume.profile.GetSetting<ScreenSpaceReflections>();
			    Bloom = postProcessVolume.profile.GetSetting<Bloom>();
			    MotionBlur = postProcessVolume.profile.GetSetting<MotionBlur>();
			}
			OnChange(null);
		}

		private void OnDestroy()
		{
			// Recovered from GameAssembly.dll RVA 0x4953C0
			EventManager.RemoveListener<GraphicsOptionChangeEvent>(OnChange);
			EventManager.RemoveListener<ChangeTimeEvent>(OnChangeTime);
			EventManager.RemoveListener<CompleteTransitionStageEvent>(OnChangeStage);
		}

		private void OnChangeTime(ChangeTimeEvent evt)
		{
			// Recovered from GameAssembly.dll RVA 0x495550
			SetBloomThreshold();
		}

		private void OnChangeStage(CompleteTransitionStageEvent evt)
		{
			// Recovered from GameAssembly.dll RVA 0x495550
			SetBloomThreshold();
		}

		private void OnChange(GraphicsOptionChangeEvent evt)
		{
			// Recovered from GameAssembly.dll RVA 0x495560
			QualitySettings.vSyncCount = optionData.VSync ? 1 : 0;
			Application.targetFrameRate = GameState.OptionData.MaxFrameRate;
			QualitySettings.SetQualityLevel(optionData.Quality, true);
			if (isTitle) return;
			ColorGradingSelfCamera.postExposure.value = (optionData.Brightness - 50) * 0.05f;
			ColorGradingSelfCamera.contrast.value = optionData.Contrast * 2 - 100;
			ColorGrading.temperature.value = ColorGradingSelfCamera.temperature.value = optionData.ColorTemperature - 6;
			foreach (PostProcessLayer layer in postProcessLayers)
			    layer.antialiasingMode = optionData.AntiAliasing ? PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing : PostProcessLayer.Antialiasing.None;
			ColorGrading.postExposure.value = (optionData.Brightness - 50) * 0.05f;
			ColorGrading.contrast.value = optionData.Contrast * 2 - 100;
			AmbientOcclusion.active = optionData.SSAO;
			ScreenSpaceReflections.active = optionData.SSR;
			Bloom.active = optionData.BloomStrength != 0;
			Bloom.intensity.value = optionData.BloomStrength * 0.1f;
			SetBloomThreshold();
			MotionBlur.active = optionData.MotionBlur;
			var cameraController = InGameManager.Instance.InGameReferencer.BaseCameraController;
			int fov = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.IsStrafe
			    ? optionData.CameraAngle1st : optionData.CameraAngle3rd;
			cameraController.SetDirectCameraFOV(fov);
		}

		private void SetBloomThreshold()
		{
			// Recovered from GameAssembly.dll RVA 0x495B30
			GameStateData state = GameState.GameStateData;
			int stage = (int)state.CurrentStageType;
			bool light = state.IsDaytime || stage == 11 || stage == 0 || stage == 6 || stage == 7;
			int value = light ? optionData.BloomBiasLight : optionData.BloomBiasDark;
			Bloom.threshold.value = value * 0.01f;
		}
	}
}
