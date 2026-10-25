using ExposureUnnoticed2.Master.Skill.SkillSlow;
using ExposureUnnoticed2.ObjectUI.SelectDifficultyPanel;
using ExposureUnnoticed2.Scripts.Base;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class StaminaManager
	{
		private float RegenerationCoolTime;

		private RSkillStamina currentStaminaLevelRecord;

		private int currentReinforceEffectAmount;

		private bool isZero;

		private DifficultType currentDifficultType;

		public float Current { get; private set; }

		public float Max { get; private set; }

		public void Dispose()
		{
			// Recovered from GameAssembly.dll RVA 0x3FBC50
			EventManager.RemoveListener<SkillUpEvent>(OnSkillUp);
		}

		public bool IsEnough(float stamina)
		{
			// Original RVA 0x3FBD10 ignores the argument and tests current stamina strictly above zero.
			return Current > 0f;
		}

		public void ConsumePerSecond(float stamina)
		{
			// Draft from complete original 3FBD20..3FBEE8, including all five unwind regions.
			float capturedCurrent = Current;
			float delta = UnityEngine.Time.deltaTime;
			float remaining = capturedCurrent - (delta * stamina);
			if (remaining < 0f)
			{
			    remaining = 0f;
			}
			else
			{
			    float capturedMax = Max;
			    if (remaining > capturedMax) remaining = capturedMax;
			}
			RSkillStamina capturedRecord = currentStaminaLevelRecord;
			Current = remaining;
			if (capturedRecord is null) throw new System.NullReferenceException();
			bool reachedZero = remaining == 0f;
			RegenerationCoolTime = capturedRecord.RegenerationCoolTime;
			if (!reachedZero)
			{
			    isZero = false;
			}
			else
			{
			    if (!isZero)
			    {
			        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager).TypeHandle);
			        var capturedUi = global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance;
			        if (capturedUi is null) throw new System.NullReferenceException();
			        // Existing original 457D20 is already recovered; it owns the private Animator/RedBlink fields.
			        capturedUi.StaminaRedBlink();
			    }
			    isZero = true;
			}
		}

		public void OnUpdate()
		{
			// Candidate from complete original RVA 0x3FBEF0..0x3FC190.
			if (currentReinforceEffectAmount != PlayerController.Instance.Pca.PlayerState.ReinforceEffectData.Stamina100)
			{
			    currentReinforceEffectAmount = PlayerController.Instance.Pca.PlayerState.ReinforceEffectData.Stamina100;
			    UpdateMax();
			}
			else if (currentDifficultType != global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DifficultType)
			{
			    currentDifficultType = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DifficultType;
			    UpdateMax();
			}
			float capturedCool = RegenerationCoolTime;
			float firstDelta = UnityEngine.Time.deltaTime;
			float remainingCool = capturedCool - firstDelta;
			RegenerationCoolTime = remainingCool;
			if (remainingCool > 0f) return;
			float difficultyScale = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DifficultType == (DifficultType)2 ? 1.25f : 1.75f;
			RSkillStamina capturedRecord = currentStaminaLevelRecord;
			float capturedCurrent = Current;
			float capturedMax = Max;
			if (capturedRecord is null) throw new global::System.NullReferenceException();
			float capturedRegeneration = capturedRecord.Regeneration;
			float secondDelta = UnityEngine.Time.deltaTime;
			int regeneratePercent = PlayerController.Instance.Pca.PlayerState.ReinforceEffectData.StaminaRegenerate100;
			float baseRecovery = (capturedRegeneration * capturedMax) * secondDelta;
			float reinforceScale = (float)regeneratePercent * 0.01f + 1f;
			float nextCurrent = ((reinforceScale * baseRecovery) * difficultyScale) + capturedCurrent;
			if (nextCurrent < 0f)
			{
			    nextCurrent = 0f;
			}
			else
			{
			    float latestMax = Max;
			    if (nextCurrent > latestMax) nextCurrent = latestMax;
			}
			Current = nextCurrent;
		}

		private void OnSkillUp(SkillUpEvent evt)
		{
            // Original RVA 0x3FC190.
            if (evt.Type != global::ExposureUnnoticed2.Master.Skill.SkillType.Stamina) return;
            currentStaminaLevelRecord = MSkillStamina.Get(global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetSkillLevel(global::ExposureUnnoticed2.Master.Skill.SkillType.Stamina));
            UpdateMax();
		}

		private void UpdateMax()
		{
			// Recovered from GameAssembly.dll RVA 0x3FC220
			float deficit = Max - Current;
			Max = ((float)currentReinforceEffectAmount * 0.01f + 1f) * currentStaminaLevelRecord.MaxStamina;
			if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DifficultType != (global::ExposureUnnoticed2.ObjectUI.SelectDifficultyPanel.DifficultType)2)
			    Max = Max + 0.5f;
			// Original XORPS zero, MAXSS(zero, computed): preserves second-operand NaN and signed zero.
			Max = Max < 0f ? 0f : Max;
			Current = Max - deficit;
		}
	// Recovered original member RVA 0x3FBAE0: .ctor
		public StaminaManager()
        {
            Current = global::ExposureUnnoticed2.Master.Common.MCommon.PlayerMaxStamina;
            EventManager.AddListener<SkillUpEvent>(OnSkillUp, false);
            currentStaminaLevelRecord = MSkillStamina.Get(global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetSkillLevel((global::ExposureUnnoticed2.Master.Skill.SkillType)1));
            UpdateMax();
            currentDifficultType = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DifficultType;
        }
	}
}
