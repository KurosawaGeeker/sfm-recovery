using System;
using ExposureUnnoticed2.Scripts.InGame;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.Mission;
using ExposureUnnoticed2.Object3D.IngameManager;
using ExposureUnnoticed2.Master.RankRelease;
using ExposureUnnoticed2.Master.Mission;
using ExposureUnnoticed2.Master.Stage;
using ExposureUnnoticed2.ObjectUI.MissionMenuPanel;
using Common.CommonUI.CommonBarText;
using ExposureUnnoticed2.ObjectUI.TutorialPanel;
using System.Collections.Generic;
using Common.Scripts.UI;
using ExposureUnnoticed2.Master.Rank;
using ExposureUnnoticed2.Master.Stage;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.ResultPanel
{
	public class ResultPanelView : BasePanelView
	{
		private enum Phase
		{
			Content1 = 0,
			Content2 = 1
		}

		[SerializeField]
		private GameObject newRecord;

		[SerializeField]
		private TextMeshProUGUI earnRp;

		[SerializeField]
		private TextMeshProUGUI totalPerSecRpText;

		[SerializeField]
		private TextMeshProUGUI maxEarnRp;

		[SerializeField]
		private TextMeshProUGUI oldExp;

		[SerializeField]
		private TextMeshProUGUI newExp;

		[SerializeField]
		private TextMeshProUGUI oldBringRpText;

		[SerializeField]
		private TextMeshProUGUI newBringRpText;

		[SerializeField]
		private GameObject completedMissionItemPrefab;

		[SerializeField]
		private Transform completedMissionItemParent;

		[SerializeField]
		private TextMeshProUGUI oldRankText;

		[SerializeField]
		private TextMeshProUGUI newRankText;

		[SerializeField]
		private GameObject rankReleaseItemPrefab;

		[SerializeField]
		private Transform rankReleaseItemParent;

		[SerializeField]
		private GameObject barTextPrefab;

		private Phase currentPhase;

		private bool onCompleteAnimContent1;

		private bool onCompleteAnimContent2;

		private bool isRankUp;

		private bool isNewRecord;

		private MRankRecord oldRank;

		private MRankRecord newRank;

		private HashSet<StageType> existBarTextSet = new HashSet<StageType>();

		private void Initialize()
		{
// Recovered from complete original4C6540..4C82F0; controlled proof accompanies this candidate.
if (InGameManager.Instance.TempInGameState.IsNoAchieveMissionMode)
{
    GameState.GameStateData.CurrentEarnThisTimeRp = 0d;
    GameState.GameStateData.DroneMissionEarnRp = 0f;
    InGameManager.Instance.TempInGameState.MissionCompleteThisGameSet.Clear();
    InGameManager.Instance.TempInGameState.IsNoAchieveMissionMode = false;
}
oldRank = MRank.CalcCurrentRank((double)GameState.GameStateData.ExposeExp);
oldExp.text = GameState.GameStateData.ExposeExp.ToString();
oldBringRpText.text = unchecked((int)GameState.GameStateData.CurrentRp).ToString();
var data = GameState.GameStateData;
data.ExposeExp = unchecked(data.ExposeExp + (int)data.CurrentEarnThisTimeRp);
data = GameState.GameStateData;
data.CurrentRp = (double)unchecked((int)data.CurrentEarnThisTimeRp) + data.CurrentRp;
if (SystemManager.IsTrial)
{
    data = GameState.GameStateData;
    data.ExposeExp = Math.Min(data.ExposeExp, 190);
    data = GameState.GameStateData;
    data.CurrentRp = Math.Min(data.CurrentRp, 190d);
}
newRank = MRank.CalcCurrentRank((double)GameState.GameStateData.ExposeExp);
if (newRank != oldRank) isRankUp = true;
InGameManager.Instance.InGameReferencer.Player.Pca.PlayerState.CurrentRank = newRank;
data = GameState.GameStateData;
data.ReleasedInfoRank = Math.Max(data.ReleasedInfoRank, newRank.Rank);
data = GameState.GameStateData;
if (data.StatistisMaxEarnRp < unchecked((int)data.CurrentEarnThisTimeRp))
{
    isNewRecord = true;
    data = GameState.GameStateData;
    data.StatistisMaxEarnRp = unchecked((int)data.CurrentEarnThisTimeRp);
}
newRecord.SetActive(isNewRecord);
earnRp.text = unchecked((int)GameState.GameStateData.CurrentEarnThisTimeRp) + "RP";
maxEarnRp.text = GameState.GameStateData.StatistisMaxEarnRp + "RP";
newExp.text = GameState.GameStateData.ExposeExp + "Exp";
oldRankText.text = Localizer.Trans(oldRank.NameKey);
newRankText.text = Localizer.Trans(newRank.NameKey);
newBringRpText.text = unchecked((int)GameState.GameStateData.CurrentRp) + "RP";
ExternalMethodManager.DestroyChildren(rankReleaseItemParent);
foreach (var release in MRankRelease.GetByRank(newRank.Rank))
    UnityEngine.Object.Instantiate(rankReleaseItemPrefab, rankReleaseItemParent).GetComponent<RankConfirmReleaseItemView>().Initialize(release, true);
ExternalMethodManager.DestroyChildren(completedMissionItemParent);
foreach (var mission in MissionManager.Instance.MissionList)
{
    if (InGameManager.Instance.TempInGameState.MissionCompleteThisGameSet.Contains(mission.RMission.Id))
    {
        var stage = mission.RMission.StageType;
        if (!existBarTextSet.Contains(stage))
        {
            existBarTextSet.Add(stage);
            UnityEngine.Object.Instantiate(barTextPrefab, completedMissionItemParent).GetComponent<CommonBarTextView>().Initialize(Localizer.Trans(MStage.Get(stage).StageNameKey));
        }
        string missionName = mission.RMission.GetMissionName();
        string missionRp = unchecked((int)GameState.GameStateData.MissionEarnRpDict[mission.RMission.Id]) + "RP";
        string bonus = string.Format("x{0:0.00}", (float)InGameManager.Instance.TempInGameState.MissionMaxRpBonus100ThisGameDict[mission.RMission.Id] * 0.01f);
        int oldBonus = GameState.GameStateData.MissionMaxRpBonus100Dict[mission.RMission.Id];
        int newBonus = InGameManager.Instance.TempInGameState.MissionMaxRpBonus100ThisGameDict[mission.RMission.Id];
        UnityEngine.Object.Instantiate(completedMissionItemPrefab, completedMissionItemParent).GetComponent<CompletedMissionItem>().Initialize(missionName, missionRp, bonus, oldBonus < newBonus);
        GameStateFacade.AchieveMission(mission.RMission.Id);
        data = GameState.GameStateData;
        data.StatistisMissionAchiveCount = unchecked(data.StatistisMissionAchiveCount + 1);
    }
}
if (GameState.GameStateData.DroneMissionEarnRp > 0f)
{
    string missionName = Localizer.Trans("ui_common_drone_mission");
    string missionRp = unchecked((int)GameState.GameStateData.DroneMissionEarnRp) + "RP";
    UnityEngine.Object.Instantiate(completedMissionItemPrefab, completedMissionItemParent).GetComponent<CompletedMissionItem>().Initialize(missionName, missionRp, "", false);
}
MissionManager.Instance.ResetMission();
GameState.GameStateData.CurrentEarnThisTimeRp = 0d;
InGameManager.Instance.TempInGameState.CurrentEarnThisTimePerSecRp = 0f;
GameState.GameStateData.DroneMissionEarnRp = 0f;
InGameManager.Instance.TempInGameState.MissionCompleteThisGameSet.Clear();
InGameManager.Instance.TempInGameState.DroneMissionCompleteThisGame = false;
foreach (var pair in InGameManager.Instance.TempInGameState.MissionMaxRpBonus100ThisGameDict)
{
    var dict = GameState.GameStateData.MissionMaxRpBonus100Dict;
    dict[pair.Key] = Math.Max(dict[pair.Key], pair.Value);
}
InGameManager.Instance.TempInGameState.MissionMaxRpBonus100ThisGameDict.Clear();
InGameManager.Instance.TempInGameState.AchieveMissionCountByStageThisGameDict.Clear();
onClose = () =>
{
    if (SystemManager.IsTrial)
    {
        foreach (var pair in MMission.Get(StageType.Residence).GetAll())
            if (pair.Value.Trial && GameStateFacade.GetMissionAchieveCount(pair.Value.Id) == 0) return;
        Scheduler.DelayFlame(() => TutorialPanelView.Open(TutorialPanelView.TutorialOpenKind.EndTrial), 1);
    }
};
}

		private void LateUpdate()
		{
            // Complete original0x4C82F0. The content2 flag also gates nonzero/unknown phases.
            if (currentPhase == Phase.Content1 && onCompleteAnimContent1)
            {
                if (isRankUp)
                {
                    if (InputManager.IsDownFrame(InputManager.InputType.Cancel) || InputManager.IsDownFrame((InputManager.InputType)1000))
                    {
                        if (animator is null) throw new System.NullReferenceException();
                        animator.SetTrigger("PlayContent2");
                        currentPhase = Phase.Content2;
                    }
                    return;
                }
            }
            else if (!onCompleteAnimContent2) return;
            if (InputManager.IsDownFrame((InputManager.InputType)1000))
            {
                var sound = global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance;
                if (sound is null) throw new System.NullReferenceException();
                sound.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)50, -1f, false);
                Close();
            }
            else if (CommonCheckClose())
            {
                var sound = global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance;
                if (sound is null) throw new System.NullReferenceException();
                sound.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)50, -1f, false);
            }
        }

		public void OnClickOkContent1()
		{
            if (isRankUp)
            {
                if (animator is null) throw new System.NullReferenceException();
                animator.SetTrigger("PlayContent2");
                currentPhase = Phase.Content2;
                return;
            }
            var sound = global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance;
            if (sound is null) throw new System.NullReferenceException();
            sound.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)50, -1f, false);
            Close();
        }

		public void OnClickOkContent2()
		{
            if (!onCompleteAnimContent2) return;
            var sound = global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance;
            if (sound is null) throw new System.NullReferenceException();
            sound.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)50, -1f, false);
            Close();
        }

		public void OnEndAminContent1()
		{
            SaveManager.Save((global::ExposureUnnoticed2.Scripts.Base.SaveRapper.SaveDataType)3, -1, false);
            global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("ui_save_complete"), (global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType)1);
            onCompleteAnimContent1 = true;
        }

		public void OnEndAminContent2()
		{
            onCompleteAnimContent2 = true;
        }

		public void PlaySeRankUp()
		{
            var sound = global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance;
            if (sound is null) throw new System.NullReferenceException();
            sound.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)23, -1f, false);
        }

		public static void OpenIfNeed()
		{
if (InGameManager.Instance.TempInGameState.MissionCompleteThisGameSet.Count == 0 && !InGameManager.Instance.TempInGameState.DroneMissionCompleteThisGame)
{
    InGameManager.Instance.TempInGameState.IsNoAchieveMissionMode = false;
    return;
}
(InGameUiManager.Instance.Open(AssetReferencer.Instance.ResultPanel, false) as ResultPanelView).Initialize();
}
	}
}
