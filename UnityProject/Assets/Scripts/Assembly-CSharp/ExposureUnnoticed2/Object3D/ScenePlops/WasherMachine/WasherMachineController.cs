using ExposureUnnoticed2.Object3D.InteractArea;
using ExposureUnnoticed2.Object3D.ScenePlops.Toilet;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.Sound;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.WasherMachine
{
	public class WasherMachineController : MonoBehaviour
	{
		public enum State
		{
			Idle = 0,
			Washing = 1,
			FinishWash = 2
		}

		private enum SoundState
		{
			None = 0,
			Start = 1,
			Loop = 2,
			LoopEnd = 3,
			Beep = 4
		}

		[SerializeField]
		private InteractAreaController interactAreaController;

		[SerializeField]
		private Transform dropClothesPoint;

		[SerializeField]
		private LoopSoundPlayer loopBgmController;

		[SerializeField]
		private AudioSource audioSource;

		[SerializeField]
		private InSidePlayerChecker inSidePlayerChecker;

		private SoundState currentSoundState;

		private Animator animator;

		private float initStartVolume;

		private float animatorSpeedCache;

		private SmoothFloat insideVolumeScale;

		private SmoothFloat startVolumeScale;

		private float remainWashTime;

		private static readonly int Use;

		private static readonly int Take;

		private int useTime;

		private static readonly int ByNpc;

		public Transform InteractPoint => null;

		public bool IsUseByNpc { get; private set; }

		public bool IsUseByPlayer { get; private set; }

		public State CurrentState { get; private set; }

		private void Start()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnChangeTime(ChangeTimeEvent evt)
		{
		}

		public void OnLoadUsing()
		{
		}

		private void Update()
		{
		}

		public void StartWash(int useTime, bool byNpc = false)
		{
		}

		public void TakeClothes(bool isByNpc = false)
		{
		}

		private void OnInteract()
		{
		}

		private void OnPlaySe(InGameSoundManager.SeType type)
		{
		}

		public void UseByNpc()
		{
		}

		public void ReleaseByNpc()
		{
		}

		private void OnAnimEndTake()
		{
		}

		private void OnTimeStopEvent(TimeStopEvent evt)
		{
		}
	}
}
