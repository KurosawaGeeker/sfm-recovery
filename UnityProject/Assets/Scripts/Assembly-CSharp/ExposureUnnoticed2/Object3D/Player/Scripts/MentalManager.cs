using ExposureUnnoticed2.Master.Skill.SkillMental;
using ExposureUnnoticed2.Object3D.NPC.Script;
using ExposureUnnoticed2.Scripts.Base;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class MentalManager
	{
		private PlayerStateModel playerStateModel;

		private NpcManager npcManager;

		private RSkillMental currentMentalLevelRecord;

		private bool isPlayedAlert;

		public float MaxMental { get; private set; }

		public bool IsRedBlinkGauge { get; private set; }

		public void Initialize(PlayerStateModel playerStateModel)
		{
			// Recovered from GameAssembly.dll RVA 0x624640
			this.playerStateModel = playerStateModel;
			npcManager = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.NpcManager;
			EventManager.AddListener<SkillUpEvent>(OnSkillUp, false);
			currentMentalLevelRecord = MSkillMental.Get(global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetSkillLevel(global::ExposureUnnoticed2.Master.Skill.SkillType.Mental));
			MaxMental = currentMentalLevelRecord.MaxMental;
			UpdateMaxMental();
		}

		public void Dispose()
		{
			// Recovered from GameAssembly.dll RVA 0x624870
			EventManager.RemoveListener<SkillUpEvent>(OnSkillUp);
		}

		public void OnUpdate()
		{
			// Recovered from GameAssembly.dll RVA 0x624930
			if (playerStateModel.CurrentState == PlayerStateModel.PlayerState.Bareta) return;
			float consumption = (float)PlayerController.Instance.Pca.PlayerState.ReinforceEffectData.MentalDecreaseScale100 / 100f + 1f;
			consumption = consumption < 0.5f ? 0.5f : consumption;
			if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsActivePassiveSkill(global::ExposureUnnoticed2.Master.Skill.SkillType.MyPace)) consumption = consumption * 0f;
			if (!PlayerFacade.Instance.GetExposedBodyPartsByClothesAndCostume().OnlyHasFlag(global::ExposureUnnoticed2.Master.Mission.ExposedBodyParts.None))
			    ConsumePerSecond(consumption);
			if (!isPlayedAlert && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerMental / MaxMental <= 0.05f)
			{
			    IsRedBlinkGauge = true;
			    isPlayedAlert = true;
			    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_limit_mental"), global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType.Caution);
			    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData(global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType.MentalAlert, -1f, false);
			}
		}

		public void RecoveryMental()
		{
			// Recovered from GameAssembly.dll RVA 0x624C80
			global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerMental = MaxMental;
			isPlayedAlert = false;
			IsRedBlinkGauge = false;
		}

		private void OnSkillUp(SkillUpEvent evt)
		{
			// Recovered from GameAssembly.dll RVA 0x624CE0
			if (evt.Type != global::ExposureUnnoticed2.Master.Skill.SkillType.Mental) return;
			currentMentalLevelRecord = MSkillMental.Get(global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetSkillLevel(global::ExposureUnnoticed2.Master.Skill.SkillType.Mental));
			UpdateMaxMental();
		}

		private void UpdateMaxMental()
		{
			// Recovered from GameAssembly.dll RVA 0x624D70
			float deficit = MaxMental - global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerMental;
			MaxMental = currentMentalLevelRecord.MaxMental;
			global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerMental = MaxMental - deficit;
		}

		public void ConsumePerSecond(float mental)
		{
			// Recovered from GameAssembly.dll RVA 0x624E00
			if (global::ExposureUnnoticed2.ObjectUI.SleepBlackPanel.SleepBlackPanelView.IsOpen) return;
			if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType == (global::ExposureUnnoticed2.Master.Stage.StageType)0) return;
			// Capture original state and its current value before invoking the engine time getter.
			var state = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
			float previous = state.PlayerMental;
			state.PlayerMental = previous - UnityEngine.Time.deltaTime * mental;
			state = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
			float current = state.PlayerMental;
			if (current < 0f) current = 0f;
			else if (MaxMental < current) current = MaxMental;
			state.PlayerMental = current;
		}

		public bool IsEnough(float stamina)
		{
			// Recovered from GameAssembly.dll RVA 0x624F50
			return stamina < global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerMental;
		}

		public bool IsRemain()
		{
			// Recovered from GameAssembly.dll RVA 0x4797E0
			return true;
		}

		public void ReduceByFound()
		{
			// Recovered from GameAssembly.dll RVA 0x624FC0
			if (global::ExposureUnnoticed2.ObjectUI.SleepBlackPanel.SleepBlackPanelView.IsOpen) return;
			if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType == (global::ExposureUnnoticed2.Master.Stage.StageType)0) return;
			Scheduler.DelayUnscaledTime(() =>
			{
			    // Original non-capturing callback RVA 0x6251F0: no clamp and no second gate.
			    var state = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
			    float previous = state.PlayerMental;
			    state.PlayerMental = previous - global::ExposureUnnoticed2.Master.Common.MCommon.PulseMentalReduce;
			}, 0.8f);
		}
	}
}
