using System;
using ExposureUnnoticed2.Master.Stage;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class StageTransController
	{
		public int LastPortalId { get; private set; }

		public bool IsAbleTransStage()
		{
            if (FashionShopManager.IsWearTryClothes())
            {
                global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("k試着中は店から出られません"), (global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType)0);
                return false;
            }
            if (GameState.GameStateData.PlayerCurrentHadObject == global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerStateModel.HadObjectType.Basket)
            {
                global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("kカゴ所持中は店から出られません"), (global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType)0);
                return false;
            }
            return true;
		}

		public void TransStageIfInCondition(StageType from, StageType stageType, int id, Action onTrans = null)
		{
            void TransStageProcess()
            {
                DroppedItemManager.Instance.DisposeAllPutDildo();
                InGameManager.Instance.StageTransController.TransStage(from, stageType);
                onTrans?.Invoke();
            }
            if (!IsAbleTransStage()) return;
            LastPortalId = id;
            if (DroneManager.Instance.IsActiveMission && stageType == StageType.Apart)
            {
                global::ExposureUnnoticed2.ObjectUI.CommonPopup.CommonPopupView.Open(
                    global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(""),
                    global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("家に入るとドローンミッションが中断されます。\nよろしいですか？\nドローンによって配置されたアイテムは全て回収されます。"),
                    new global::ExposureUnnoticed2.ObjectUI.CommonPopup.CommonButtonData { Text = "いいえ", OnClick = () => { } },
                    new global::ExposureUnnoticed2.ObjectUI.CommonPopup.CommonButtonData { Text = "はい", OnClick = () =>
                    {
                        TransStageProcess();
                        DroneManager.Instance.InterruptMission();
                    } });
            }
            else TransStageProcess();
		}

		public void TransStage(StageType from, StageType to, Action onStartFadeIn = null, float waitTimeAfterSceneLoad = 0f, Action onPureBlack = null)
		{
		
            // Original TransStage 0x4A7190 and original closures 0x4A7DD0/0x4A7620/0x4A7B50.
            InGameManager.Instance.TempInGameState.BeforeStageType = from;
            global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.OpenFadeInTransStage(
                () =>
                {
                    InGameManager.Instance.TempInGameState.EntryStageInitPosition = null;
                    global::UnityEngine.SceneManagement.SceneManager.LoadScene(MStage.Get(to).SceneName);
                    onPureBlack?.Invoke();
                }, onStartFadeIn, waitTimeAfterSceneLoad,
                () =>
                {
                    var temp = InGameManager.Instance.TempInGameState;
                    temp.EntryStageInitPosition = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.Transform.position;
                    if (GameState.GameStateData.CurrentStageType != StageType.Apart)
                    {
                        global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.Open(global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.TutorialOpenKind.FirstOutside);
                        return;
                    }
                    if (GameState.GameStateData.PlayerHandcuffTimerRemainTime > 0f)
                    {
                        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerSoundManager.PlayPickingSe(global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType.Unlock);
                        GameState.GameStateData.PlayerHandcuffTimerRemainTime = 0f;
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("item_handcuff_timer_end"), (global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType)1);
                    }
                    global::ExposureUnnoticed2.ObjectUI.ResultPanel.ResultPanelView.OpenIfNeed();
                    global::ExposureUnnoticed2.Scripts.Mission.MissionManager.Instance.ResetMission();
                    global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayUnscaledTime(() =>
                    {
                        if (InGameManager.Instance != null)
                            SaveManager.Save((global::ExposureUnnoticed2.Scripts.Base.SaveRapper.SaveDataType)3, -1, true);
                    }, 5f);
                });
            InGameManager.Instance.InGameReferencer.Player.ChangeLighting();
            global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.Instance.RefreshState();
            global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.StartTransitionStageEvent);
		}
	}
}
