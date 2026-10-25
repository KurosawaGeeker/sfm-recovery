using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.Sound
{
	public class BgmController : MonoBehaviour
	{
		public enum BgmType
		{
			Title = 0,
			OutGame = 1
		}

		private const float FadeTime = 0.3f;

		[SerializeField]
		private InGameSoundManager.SeType daytimeBgm;

		[SerializeField]
		private InGameSoundManager.SeType nightBgm;

		[SerializeField]
		private float fadeTransBgmTimeRate;

		[SerializeField]
		private bool playOnAwake;

		private AudioSource[] bgmSources;

		private int currentAudioSourceIndex;

		private SmoothFloat bgmVolumeScale;

		private SmoothFloat bgmVolumeScaleByOverride;

		private SmoothFloat bgmVolumeScaleByTimeStop;

		private SmoothFloat elevatorOutsideVolumeScale;

		private float initVolume;

		private bool isPlay;

		public static BgmController Instance { get; private set; }

		public void Initialize()
        {
            // Original RVA 0x474350; original AudioData, source ordering and event callbacks.
            Instance = this;
            var data = InGameSoundManager.Instance.GetAudioData(GameState.GameStateData.IsDaytime ? daytimeBgm : nightBgm);
            bgmSources = GetComponentsInChildren<AudioSource>();
            var source = GetCurrentAudioSource();
            source.volume = data.Volume;
            source.clip = data.Clip;
            if (playOnAwake) { source.Play(); isPlay = true; }
            initVolume = source.volume;
            bgmVolumeScale = new SmoothFloat(0.3f, 1f, 1f, false, false, -1f);
            bgmVolumeScale.Target = 1f;
            bgmVolumeScaleByOverride.Target = bgmVolumeScaleByOverride.Current = 1f;
            bgmVolumeScaleByTimeStop.Target = bgmVolumeScaleByTimeStop.Current = 1f;
            EventManager.AddListener<StartTransitionStageEvent>(OnTransitionStageEvent, false);
            EventManager.AddListener<BgmControlEvent>(OnBgmControlEvent, false);
        }

		public void Play()
        {
            // Original RVA 0x474800.
            GetCurrentAudioSource().Play();
            isPlay = true;
            bgmVolumeScale.SetSecond(1f);
            bgmVolumeScale.Target = 1f;
        }

		public void Stop(float fadeTime)
        {
            // Original RVA 0x4748B0, shared with LoopSoundPlayer.Stop.
            bgmVolumeScale.SetSecond(fadeTime);
            bgmVolumeScale.Target = 0f;
        }

		public void ChangeBgm()
        {
            // Original RVA 0x4748F0. Reuse current source; no invented crossfade.
            var source = GetCurrentAudioSource();
            source.Stop();
            var data = InGameSoundManager.Instance.GetAudioData(GameState.GameStateData.IsDaytime ? daytimeBgm : nightBgm);
            isPlay = false;
            source.clip = data.Clip;
            if (playOnAwake) { source.Play(); isPlay = true; }
            initVolume = data.Volume;
        }

		private AudioSource GetCurrentAudioSource()
        {
            // Original RVA 0x473DE0, shared native body.
            return bgmSources[currentAudioSourceIndex];
        }

		private void Update()
        {
            // Original RVA 0x474AC0. Preserve the original four volume factors and source rollover.
            if (bgmVolumeScale == null || !isPlay) return;
            bgmVolumeScale.UpdateToTarget();
            bgmVolumeScaleByOverride.UpdateToTarget();
            bgmVolumeScaleByTimeStop.UpdateTo(global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsTimeStop ? 0f : 1f);
            bool inClosedElevator = false;
            var elevator = global::ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.Instance;
            if (elevator)
            {
                var state = global::ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.Instance.CurrentElevatorState;
                if (state != global::ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.ElevatorState.DoorClosing &&
                    state != global::ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.ElevatorState.DoorOpening &&
                    state != global::ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.ElevatorState.StopDoorOpen)
                    inClosedElevator = global::ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.Instance.PushBounds.Contains(global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.Transform.position);
            }
            elevatorOutsideVolumeScale.UpdateTo(inClosedElevator ? 0f : 1f);
            foreach (var source in bgmSources)
                source.volume = bgmVolumeScale.Current * initVolume * bgmVolumeScaleByOverride.Current * bgmVolumeScaleByTimeStop.Current * elevatorOutsideVolumeScale.Current;
            var current = GetCurrentAudioSource();
            if (current.time / current.clip.length >= fadeTransBgmTimeRate) ChangeAudioSource();
        }

		private void ChangeAudioSource()
        {
            // Original RVA 0x473C90, shared native body; old source is allowed to finish.
            var prior = GetCurrentAudioSource();
            currentAudioSourceIndex = (currentAudioSourceIndex + 1) % bgmSources.Length;
            var current = GetCurrentAudioSource();
            current.clip = prior.clip;
            current.Play();
        }

		private void OnDestroy()
        {
            // Original RVA 0x475140. Only the current singleton removes these listeners.
            if (Instance != this) return;
            EventManager.RemoveListener<StartTransitionStageEvent>(OnTransitionStageEvent);
            EventManager.RemoveListener<BgmControlEvent>(OnBgmControlEvent);
            Instance = null;
        }

		private void OnTransitionStageEvent(StartTransitionStageEvent startTransitionStageEvent)
        {
            // Original shared RVA 0x473E10.
            bgmVolumeScale.Target = 0f;
        }

		private void OnBgmControlEvent(BgmControlEvent bgmControlEvent)
        {
            // Original shared RVA 0x473E30.
            bgmVolumeScale.Target = bgmControlEvent.IsPlay ? 1f : 0f;
        }

		public void SetVolumeScaleByOverride(float target)
        {
            // Original RVA 0x475390.
            bgmVolumeScaleByOverride.Target = target;
        }
        // Original constructor RVA 0x4753B0; serialized fields may override these defaults.
        public BgmController()
        {
            fadeTransBgmTimeRate = 1f;
            playOnAwake = true;
            bgmVolumeScaleByOverride = new SmoothFloat(1f, 1f, 0f, false, false, -1f);
            bgmVolumeScaleByTimeStop = new SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
            elevatorOutsideVolumeScale = new SmoothFloat(1f, 1f, 1f, false, false, 0.2f);
        }

	}
}
