using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;
using UnityEngine.Audio;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class UiOptionApplyer : MonoBehaviour
	{
		private GameObject faceCamera;

		private GameObject bodyCamera;

		[SerializeField]
		private GameObject faceCameraPanel;

		[SerializeField]
		private GameObject bodyCameraPanel;

		[SerializeField]
		private GameObject playerInfoPanel;

		[SerializeField]
		private GameObject heartRatePanel;

		[SerializeField]
		private GameObject ecstasyGauge;

		[SerializeField]
		private GameObject compass;

		[SerializeField]
		private GameObject missionPanel;

		[SerializeField]
		private GameObject statusInfo;

		[SerializeField]
		private GameObject shortcutPanel;

		[SerializeField]
		private GameObject shortcutPanel2;

		[SerializeField]
		private GameObject interactPanel;

		[SerializeField]
		private GameObject heartBeatEffect;

		[SerializeField]
		private GameObject manualPanel;

		[SerializeField]
		private AudioMixer audioMixer;

		[SerializeField]
		private Transform missionPanelSizeTarget;

		public void Initialize(GameObject faceCameraObject, GameObject bodyCameraObject)
		{
			// Recovered from GameAssembly.dll RVA 0x4A8700
			EventManager.AddListener<OptionChangeEvent>(OnChange, false);
			faceCamera = faceCameraObject;
			bodyCamera = bodyCameraObject;
			OnChange(null);
		}

		private void OnDestroy()
		{
            // Original RVA 0x4A8890: pair with Initialize's OptionChangeEvent registration.
            EventManager.RemoveListener<OptionChangeEvent>(OnChange);
		}

		private void OnChange(OptionChangeEvent evt)
		{
            // Original RVA 0x4A8950; original option and field order is preserved.
            faceCamera.SetActive(GameState.OptionData.FaceCamera);
            faceCameraPanel.SetActive(GameState.OptionData.FaceCamera);
            bodyCamera.SetActive(GameState.OptionData.BodyCamera);
            bodyCameraPanel.SetActive(GameState.OptionData.BodyCamera);
            playerInfoPanel.SetActive(GameState.OptionData.PlayerInfo);
            heartRatePanel.SetActive(GameState.OptionData.PlayerInfo);
            ecstasyGauge.SetActive(GameState.OptionData.EcstasyGauge);
            compass.SetActive(GameState.OptionData.Compass);
            statusInfo.SetActive(GameState.OptionData.RightDownPanel);
            shortcutPanel.SetActive(GameState.OptionData.RightDownPanel);
            shortcutPanel2.SetActive(GameState.OptionData.RightDownPanel);
            interactPanel.SetActive(GameState.OptionData.InteractPanel);
            heartBeatEffect.SetActive(GameState.OptionData.HeartBeatEffect);
            manualPanel.SetActive(GameState.OptionData.Manual);
            // Original does not SetActive(missionPanel); it changes this target's local scale.
            missionPanelSizeTarget.localScale = Vector3.one * GameState.OptionData.MissionPanelSize100 * 0.01f;
            // Native log10f helper RVA 0x3A0800, original mixer names from Cpp2IL ISIL.
            audioMixer.SetFloat("Master", Mathf.Clamp(Mathf.Log10(GameState.OptionData.SoundMaster100 / 100f) * 20f + 6f, -80f, 20f));
            audioMixer.SetFloat("Environment", Mathf.Clamp(Mathf.Log10(GameState.OptionData.SoundSe100 / 100f) * 20f + 6f, -80f, 20f));
            audioMixer.SetFloat("SystemSe", Mathf.Clamp(Mathf.Log10(GameState.OptionData.SoundUi100 / 100f) * 20f + 6f, -80f, 20f));
            audioMixer.SetFloat("Bgm", Mathf.Clamp(Mathf.Log10(GameState.OptionData.SoundBgm100 / 100f) * 20f + 6f, -80f, 20f));
		}
	}
}
