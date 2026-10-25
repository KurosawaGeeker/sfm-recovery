using ExposureUnnoticed2.Scripts.InGame;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Master.Mission;
using ExposureUnnoticed2.ObjectUI.InGame.MissionPanel;

namespace ExposureUnnoticed2.Scripts.Mission
{
	public abstract class MissionBase : IMissionItemPanelModel
	{
		private const float DestroyTime = 1f;

		private const float InitRpPerSecScale = 0.5f;

		private const float InitRpPerSecScaleChangeRemainTime = 50f;

		private float rpPerSecScale = 0.5f;

		private float rpPerSecScaleChangeRemainTime = 50f;

		private float remainStartPerSecTime = 1f;

		public MMissionOverrideRecord RMission;

		protected float satisfy;

		private float lastAddSatisfyTime = -999f;

		private float lastNonAddSatisfyTime = -999f;

		protected bool isCancelByHideCosplay;

		private bool isDoneOneCompleteProcessContinue;

		private float perSecScale = 0.5f;

		public bool IsOverDestroyTime
		{
			get
			{
                if (!(global::UnityEngine.Time.time - lastAddSatisfyTime >= 1f)) return false;
                if (!RandomMissionManager.Instance.IsActiveMission) return true;
                return RandomMissionManager.Instance.CurrentMission != this;
			}
		}

		public bool IsCreatePanel
		{
			get
			{
                if (global::UnityEngine.Time.time - lastNonAddSatisfyTime >= 0.3f && !IsOverDestroyTime && IsVisiblePanel()) return true;
                if (!RandomMissionManager.Instance.IsActiveMission) return false;
                return RandomMissionManager.Instance.CurrentMission == this;
			}
		}

		public bool IsComplete => satisfy >= 1f;

		public string MissionName => RMission.GetMissionName();

		public MissionType MissionType => RMission.OriginalRecord.MissionType;

		public int RewardRp => RMission.RewardRankPoint;

		public float Satisfy => satisfy;

		public bool IsPerSec => RMission.OriginalRecord.IsAfterPerSec;

		public float RpPerSec { get; private set; }

		public float ContinueSatisfy { get; private set; }

		public int UniqueMissionId => 0;

		public bool IsDoneOneCompleteProcess { get; private set; }

		public abstract bool IsInCondition();

		protected virtual float GetAddSatisfy()
		{
			return 0f;
		}

		public virtual void Initialize(MMissionOverrideRecord record)
		{
RMission = record;
satisfy = GameStateFacade.GetMissionSatisfy(RMission.Id);
GameStateFacade.GetMissionEarnRp(RMission.Id);
}

		public virtual void Dispose()
		{
		}

		private bool IsVisiblePanel()
		{
            if (RMission.OriginalRecord is null) throw new global::System.NullReferenceException();
            if (satisfy >= 1f)
            {
                if (!RMission.OriginalRecord.IsAfterPerSec) return true;
                if (remainStartPerSecTime > 0f) return true;
            }
            if (RMission.OriginalRecord.IsHideEyeMask && InGameManager.Instance.VisibilityFogController.IsBadVisibility) return false;
            if (RMission.OriginalRecord.IsHideDroneMission && DroneManager.Instance.IsActiveMission) return false;
            if (RMission.OriginalRecord.IsShowInSightVisibleNpc
                && !global::ExposureUnnoticed2.Object3D.NPC.Script.NpcManager.Instance.IsSomeOnePlayerFoundAndVisibleInCameraSomeOne) return false;
            if (RMission.OriginalRecord.IsShowInSightVisibleRiseStrangenessNpc
                && !global::ExposureUnnoticed2.Object3D.NPC.Script.NpcManager.Instance.IsRiseStrangeInCameraSomeOne) return false;
            return true;
		}

		public void Update()
		{
// Original native 0x478B40. Preserve per-call reads and float operation order.
if (InGameManager.Instance.TempInGameState.IsGameOver) return;
if (GameState.GameStateData.CurrentStageType == 0) return;
if (InGameManager.Instance.TempInGameState.IsPlayingSomePerformance) return;
if (global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.IsOpenFade) return;
if (global::ExposureUnnoticed2.ObjectUI.SleepBlackPanel.SleepBlackPanelView.IsOpen) return;
var rankRecord = RMission;
if (rankRecord is null) throw new System.NullReferenceException();
if (PlayerController.Instance.Pca.PlayerState.CurrentRank.Rank < rankRecord.ReleaseRank) return;
if (RandomMissionManager.Instance.IsActiveMission && RandomMissionManager.Instance.CurrentMission != this && !(satisfy >= 1f)) return;
if (SystemManager.IsTrial)
{
    if (RMission is null) throw new System.NullReferenceException();
    if (!RMission.Trial) return;
}
bool stopped = InGameManager.Instance.TempInGameState.IsTimeStop;
var currentState = GameState.GameStateData;
if (currentState is null) throw new System.NullReferenceException();
var currentRecord = RMission;
if (currentRecord is null) throw new System.NullReferenceException();
if (currentState.CurrentStageType == currentRecord.StageType && IsInCondition() && !stopped)
{
    float rpBonus = InGameManager.Instance.InGameReferencer.Player.Pca.PlayerState.RpBonus;
    if (RMission is null) throw new System.NullReferenceException();
    float scale;
    if (RMission.AddSatisfyScalePassiveContinue > 0f && GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)5))
        scale = RMission.AddSatisfyScalePassiveContinue;
    else
        scale = RMission.AddSatisfyScale;
    float addScale = scale > 1f ? 0.05f : global::ExposureUnnoticed2.Master.Common.MCommon.Instance.AddMissionSatisfyPerSecond;
    addScale = addScale * scale;
    float add;
    if (RMission.OriginalRecord.IsOverrideAddSatisfy)
    {
        add = GetAddSatisfy();
        addScale = RMission.AddSatisfyScale;
    }
    else add = UnityEngine.Time.deltaTime;
    add = (addScale * add) * DebugController.MissionSatisfyScale;
    if (!RMission.OriginalRecord.IsOverrideAddSatisfy)
    {
        int gauge = unchecked(PlayerController.Instance.Pca.PlayerState.ReinforceEffectData.MissionGauge100 + 100);
        int boundedGauge = gauge < 0 ? 0 : gauge;
        add = ((float)boundedGauge * 0.01f) * add;
    }
    if (satisfy >= 1f)
    {
        if (!IsDoneOneCompleteProcess)
        {
            IsDoneOneCompleteProcess = true;
            var sound = global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance;
            if (sound is null) throw new System.NullReferenceException();
            sound.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)97, -1f, false);
            if (RMission.OriginalRecord.MissionType == MissionType.Condition)
            {
                RiseRp((float)RMission.RewardRankPoint * rpBonus);
                var temp = InGameManager.Instance.TempInGameState;
                if (RMission is null) throw new System.NullReferenceException();
                temp.MissionMaxRpBonus100ThisGameDict.TryAdd(RMission.Id, 0);
                temp = InGameManager.Instance.TempInGameState;
                var record = RMission;
                if (record is null) throw new System.NullReferenceException();
                var playerState = PlayerController.Instance.Pca.PlayerState;
                if (playerState is null) throw new System.NullReferenceException();
                temp.MissionMaxRpBonus100ThisGameDict[record.Id] = unchecked((int)playerState.RpBonusInt);
                temp = InGameManager.Instance.TempInGameState;
                if (RMission is null) throw new System.NullReferenceException();
                temp.MissionCompleteThisGameSet.Add(RMission.Id);
                temp = InGameManager.Instance.TempInGameState;
                if (RMission is null) throw new System.NullReferenceException();
                temp.AchieveMissionCountByStageThisGameDict.TryAdd(RMission.StageType, 0);
                temp = InGameManager.Instance.TempInGameState;
                record = RMission;
                if (record is null) throw new System.NullReferenceException();
                var dictionary = temp.AchieveMissionCountByStageThisGameDict;
                if (dictionary is null) throw new System.NullReferenceException();
                var stage = record.StageType;
                dictionary[stage] = unchecked(dictionary[stage] + 1);
            }
            lastAddSatisfyTime = lastAddSatisfyTime + 1f;
        }
        if (RMission.OriginalRecord.IsAfterPerSec && IsRpPerSecCondition())
        {
            float previousRemain = remainStartPerSecTime;
            float elapsed = UnityEngine.Time.unscaledDeltaTime;
            remainStartPerSecTime = previousRemain - elapsed;
            lastAddSatisfyTime = UnityEngine.Time.time;
            var record = RMission;
            if (record is null) throw new System.NullReferenceException();
            int reward = record.RewardRankPoint;
            float oldRpScale = rpPerSecScale;
            float recordScale = record.RpPerSecScale;
            float virtualScale = IsRpPerSecScale();
            float perSec = virtualScale * ((((float)reward * 0.04f) * oldRpScale) * recordScale);
            float capturedPerSec = 0.1f > perSec ? 0.1f : perSec;
            RpPerSec = capturedPerSec;
            float amount = (UnityEngine.Time.deltaTime * capturedPerSec) * rpBonus;
            RiseRp(amount);
            var temp = InGameManager.Instance.TempInGameState;
            temp.CurrentEarnThisTimePerSecRp = amount + temp.CurrentEarnThisTimePerSecRp;
            float previousScaleRemain = rpPerSecScaleChangeRemainTime;
            float scaledElapsed = UnityEngine.Time.deltaTime;
            record = RMission;
            if (record is null) throw new System.NullReferenceException();
            float decayScale = record.RpPerSecScale;
            rpPerSecScaleChangeRemainTime = previousScaleRemain - (decayScale * scaledElapsed);
            if (rpPerSecScaleChangeRemainTime <= 0f && rpPerSecScale > 0f)
            {
                rpPerSecScale = rpPerSecScale - 0.05f;
                if (rpPerSecScale <= 0.01f) rpPerSecScale = 0f;
                if (rpPerSecScale > 0f)
                    rpPerSecScaleChangeRemainTime = 1f / (rpPerSecScale * 0.04f);
            }
        }
    }
    else
    {
        if (RMission.OriginalRecord.MissionType == MissionType.Time)
        {
            int reward = RMission.RewardRankPoint;
            float amount = (UnityEngine.Time.deltaTime * (float)reward) * rpBonus;
            RiseRp(amount);
        }
        satisfy = add + satisfy;
        lastAddSatisfyTime = UnityEngine.Time.time;
    }
    float value = satisfy;
    if (value < 0f) value = 0f;
    else if (value > 1f) value = 1f;
    satisfy = value;
    var data = GameState.GameStateData;
    if (data is null) throw new System.NullReferenceException();
    if (RMission is null) throw new System.NullReferenceException();
    data.MissionSatisfyDict[RMission.Id] = value;
}
else
{
    if (GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)5) && 1f > satisfy)
        satisfy = 0f;
    if (RMission.OriginalRecord.MissionType == MissionType.Time && satisfy >= 1f) return;
    if (RMission.OriginalRecord.MissionType == MissionType.Condition && satisfy >= 1f && ContinueSatisfy >= 1f) return;
    lastNonAddSatisfyTime = UnityEngine.Time.time;
}

}

		private void RiseRp(float riseRp)
		{
GameStateFacade.RiseEarnRp(riseRp);
var data = GameState.GameStateData;
if (data is null) throw new System.NullReferenceException();
var record = RMission;
if (record is null) throw new System.NullReferenceException();
var dictionary = data.MissionEarnRpDict;
if (dictionary is null) throw new System.NullReferenceException();
int id = record.Id;
dictionary[id] = dictionary[id] + riseRp;
}

		protected virtual bool IsRpPerSecCondition()
		{
return true;
}

		protected virtual float IsRpPerSecScale()
		{
return 1f;
}

		public virtual void Reset()
		{
isCancelByHideCosplay = false;
satisfy = 0f;
remainStartPerSecTime = 1f;
ContinueSatisfy = 0f;
rpPerSecScale = 0.5f;
rpPerSecScaleChangeRemainTime = 50f;
var data = GameState.GameStateData;
if (data is null) throw new System.NullReferenceException();
var record = RMission;
if (record is null) throw new System.NullReferenceException();
data.MissionSatisfyDict[record.Id] = 0f;
data = GameState.GameStateData;
if (data is null) throw new System.NullReferenceException();
record = RMission;
if (record is null) throw new System.NullReferenceException();
data.MissionEarnRpDict[record.Id] = 0f;
lastAddSatisfyTime = -999f;
lastNonAddSatisfyTime = -999f;
IsDoneOneCompleteProcess = false;
}
	}
}
