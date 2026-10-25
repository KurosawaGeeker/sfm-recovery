using Common.CommonUI.CommonGauge;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.Sound;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class SlowAssistManager : MonoBehaviour, IUpdater
	{
		public enum State
		{
			Normal = 0,
			Slow = 1,
			GameOver = 2
		}

		private const float SlowScale = 0.1f;

		private const float RiseTimePerSecond = 0.3f;

		public static SlowAssistManager Instance;

		[SerializeField]
		private GaugeView remainTimeGauge;

		[SerializeField]
		private Animator viewAnimator;

		[SerializeField]
		private AudioSource slowInAudioSource;

		[SerializeField]
		private AudioSource slowOutAudioSource;

		[SerializeField]
		private AudioSource gogogoAudioSource;

		private SmoothFloat currentTimeScale = new SmoothFloat(0.001f);

		private SmoothFloat currentGogogoVolume = new SmoothFloat(0.5f);

		private float maxSlowRemainTime;

		private bool isCancelSlow;

		private bool inSlowByAutoSkill;

		private bool inSlowByAutoBaretaSkill;

		private bool isCancelAutoBaretaSlowSkill;

		private float disableAutoSlowRemainTime;

		private static readonly int IsDisplayMain = Animator.StringToHash("IsDisplayMain");

		private static readonly int IsDisplayGauge = Animator.StringToHash("IsDisplayGauge");

		public State currentState { get; private set; }

		public void Initialize()
		{
            Instance = this;
            EventManager.AddListener<SkillUpEvent>(OnSkillUp, false);
            EventManager.AddListener<StartTransitionStageEvent>(OnTransitionStageEvent, false);
            EventManager.AddListener<ChangeTimeEvent>(OnChangeTime, false);
            currentTimeScale.Target = 1f;
            currentTimeScale.Current = 1f;
            currentGogogoVolume.Target = 0f;
            currentGogogoVolume.Current = 0f;
            remainTimeGauge.SetLengthScale(40f);
            var data = GameState.GameStateData;
            float max = global::ExposureUnnoticed2.Master.Skill.SkillSlow.MSkillSlow.Get(GameState.GameStateData.SkillLevel[(global::ExposureUnnoticed2.Master.Skill.SkillType)6]).MaxTimeTank;
            maxSlowRemainTime = max;
            data.SlowRemainTime = max;
		}

		public void Dispose()
		{
            Instance = null;
            EventManager.RemoveListener<SkillUpEvent>(OnSkillUp);
            EventManager.RemoveListener<StartTransitionStageEvent>(OnTransitionStageEvent);
            EventManager.RemoveListener<ChangeTimeEvent>(OnChangeTime);
		}

		public void OnUpdate()
		{
		}

		private void PlaySe(InGameSoundManager.SeType seType)
		{
		}

		private void NormalUpdate()
		{
		}

		private void SlowUpdate()
		{
		}

		private void GameOverUpdate()
		{
		}

		private void OnSkillUp(SkillUpEvent evt)
		{
            if (evt.Type != (global::ExposureUnnoticed2.Master.Skill.SkillType)6) return;
            var data = GameState.GameStateData;
            float max = global::ExposureUnnoticed2.Master.Skill.SkillSlow.MSkillSlow.Get(GameState.GameStateData.SkillLevel[(global::ExposureUnnoticed2.Master.Skill.SkillType)6]).MaxTimeTank;
            maxSlowRemainTime = max;
            data.SlowRemainTime = max;
		}

		public void CancelSlow()
		{
            isCancelSlow = true;
		}

		private void OnTransitionStageEvent(StartTransitionStageEvent startTransitionStageEvent)
		{
            isCancelSlow = true;
            Scheduler.DelayFlame(() => isCancelSlow = false, 1);
		}

		private void OnChangeTime(ChangeTimeEvent evt)
		{
            isCancelSlow = true;
		}
	}
}
