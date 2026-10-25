using ExposureUnnoticed2.Master.AdultGoods;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.Sound;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.AdultGoods
{
	public class CommonVibratorController : MonoBehaviour
	{
		[SerializeField]
		private InGameSoundManager.SeType HighSe;

		[SerializeField]
		private InGameSoundManager.SeType LowSe;

		public static CommonVibratorController Leader = null;

		public static VibrationModeType VibrationStrength;

		private static float changeRemainTime;

		private Animator animator;

		private AudioSource highAudioSource;

		private AudioSource lowAudioSource;

		private SmoothFloat highVolume = new SmoothFloat(0.8f);

		private SmoothFloat lowVolume = new SmoothFloat(0.8f);

		private float initVolumeHigh;

		private float initVolumeLow;

		private VibrationModeType currentMode;

		private static readonly int BaibuStrength = Animator.StringToHash("BaibuStrength");

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnEnable()
		{
// Complete original body; preserve original mode, event, sampler and effect order.
animator = GetComponent<Animator>();
var audioSources = GetComponents<AudioSource>();
highAudioSource = audioSources[0];
lowAudioSource = audioSources[1];
initVolumeHigh = highAudioSource.volume;
initVolumeLow = lowAudioSource.volume;
if (Leader == null) Leader = this;
EventManager.AddListener<ChangeVibratorLeaderEvent>(OnChangeLeader, false);
animator.SetInteger(BaibuStrength, 0);
}

		private void OnDisable()
		{
// Complete original body; preserve original mode, event, sampler and effect order.
EventManager.RemoveListener<ChangeVibratorLeaderEvent>(OnChangeLeader);
if (Leader == this) Leader = null;
EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeVibratorLeaderEvent);
}

		private void Update()
		{
// Complete original body; preserve original mode, event, sampler and effect order.
if (Leader == this)
{
    switch (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerVibrationModeType)
    {
        case VibrationModeType.Off: VibrationStrength = VibrationModeType.Off; break;
        case VibrationModeType.Low: VibrationStrength = VibrationModeType.Low; break;
        case VibrationModeType.High: VibrationStrength = VibrationModeType.High; break;
        case VibrationModeType.Random: UpdateRandomMode(); break;
    }
}
highVolume.UpdateToTarget();
lowVolume.UpdateToTarget();
if (lowVolume is null) throw new System.NullReferenceException();
var lowOption = global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData;
if (lowOption is null) throw new System.NullReferenceException();
var lowAudio = lowAudioSource;
if (lowAudio is null) throw new System.NullReferenceException();
lowAudio.volume = (float)lowOption.SoundVibe100 * lowVolume.Current * 0.01f;
if (highVolume is null) throw new System.NullReferenceException();
var highOption = global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData;
if (highOption is null) throw new System.NullReferenceException();
var highAudio = highAudioSource;
if (highAudio is null) throw new System.NullReferenceException();
highAudio.volume = (float)highOption.SoundVibe100 * highVolume.Current * 0.01f;
var capturedLowAudio = lowAudioSource;
var capturedHighAudio = highAudioSource;
bool mute = global::ExposureUnnoticed2.ObjectUI.SystemMenu.SystemMenuView.Instance == null;
if (capturedHighAudio is null) throw new System.NullReferenceException();
capturedHighAudio.mute = mute;
if (capturedLowAudio is null) throw new System.NullReferenceException();
capturedLowAudio.mute = mute;
if (currentMode != VibrationStrength)
{
    currentMode = VibrationStrength;
    switch (VibrationStrength)
    {
        case VibrationModeType.Off:
            animator.SetInteger(BaibuStrength, 0);
            highVolume.Target = 0f;
            lowVolume.Target = 0f;
            break;
        case VibrationModeType.Low:
            animator.SetInteger(BaibuStrength, 1);
            var lowData = InGameSoundManager.Instance.GetAudioData(LowSe);
            lowAudioSource.clip = lowData.Clip;
            lowVolume.Target = lowData.Volume;
            lowAudioSource.Play();
            highVolume.Target = 0f;
            break;
        case VibrationModeType.High:
            animator.SetInteger(BaibuStrength, 2);
            var highData = InGameSoundManager.Instance.GetAudioData(HighSe);
            highAudioSource.clip = highData.Clip;
            highVolume.Target = highData.Volume;
            highAudioSource.Play();
            lowVolume.Target = 0f;
            break;
    }
}
}

		private void UpdateRandomMode()
		{
// Complete original body; preserve original mode, event, sampler and effect order.
if (Leader != this) return;
if (global::ExposureUnnoticed2.Scripts.InGame.DebugController.IsManualChangeVibe &&
    InputManager.IsDownFrame(InputManager.InputType.Dash)) changeRemainTime = 0f;
float before = changeRemainTime;
float delta = Time.deltaTime;
changeRemainTime = before - delta;
if (changeRemainTime > 0f) return;
VibrationStrength = VibrationStrength switch
{
    VibrationModeType.Off => VibrationModeType.Low,
    VibrationModeType.Low => VibrationModeType.High,
    VibrationModeType.High => VibrationModeType.Low,
    _ => throw new System.Runtime.CompilerServices.SwitchExpressionException(VibrationStrength)
};
float first = UnityEngine.Random.value;
float second = UnityEngine.Random.value;
changeRemainTime = second * 3f + first * first * 27f + 1f;
}

		private void OnChangeLeader(ChangeVibratorLeaderEvent evt)
		{
// Complete original body; preserve original mode, event, sampler and effect order.
if (Leader == null) Leader = this;
}

		public static void OnChangeVibrationModeType()
		{
// Complete original body; preserve original mode, event, sampler and effect order.
var data = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
data.PlayerVibrationModeType = (VibrationModeType)unchecked(((int)data.PlayerVibrationModeType + 1) % (data.VibeRemoconLevel + 1));
AddMessage();
}

		public static void ForceSetVibrationMode(VibrationModeType type)
		{
// Complete original body; preserve original mode, event, sampler and effect order.
global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerVibrationModeType = type;
AddMessage();
}

		private static void AddMessage()
		{
// Complete original body; preserve original mode, event, sampler and effect order.
var mode = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerVibrationModeType;
string key = mode switch
{
    VibrationModeType.Off => "vibe_off",
    VibrationModeType.Low => "vibe_low",
    VibrationModeType.High => "vibe_high",
    VibrationModeType.Random => "vibe_random",
    _ => throw new System.Runtime.CompilerServices.SwitchExpressionException(mode)
};
global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans(key), global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType.Log);
}
	}
}
