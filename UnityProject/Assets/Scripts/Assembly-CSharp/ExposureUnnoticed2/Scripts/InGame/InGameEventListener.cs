using ExposureUnnoticed2.Scripts.Base;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class InGameEventListener
	{
		private GameStateApplier gameStateApplier;

		public void Initialize(GameStateApplier gameStateApplier)
		{
// Complete original native body; retain original callback and failure order.
this.gameStateApplier = gameStateApplier;
EventManager.AddListener<CompleteTransitionStageEvent>(OnCompleteTransitionStageEvent, false);
EventManager.AddListener<ExitInGameEvent>(OnExitInGameEvent, false);
EventManager.AddListener<FoundStrangerEvent>(OnFoundExposureEvent, false);
}

		public void Dispose()
		{
// Complete original native body; retain original callback and failure order.
EventManager.RemoveListener<CompleteTransitionStageEvent>(OnCompleteTransitionStageEvent);
EventManager.RemoveListener<ExitInGameEvent>(OnExitInGameEvent);
EventManager.RemoveListener<FoundStrangerEvent>(OnFoundExposureEvent);
}

		private void OnCompleteTransitionStageEvent(CompleteTransitionStageEvent completeTransitionStageEvent)
		{
// Complete original native body; retain original callback and failure order.
GameState.GameStateData.CurrentStageType = completeTransitionStageEvent.StageController.StageType;
var spawnPoint = completeTransitionStageEvent.StageController.GetSpawnPoint(InGameManager.Instance.TempInGameState.BeforeStageType);
if (spawnPoint)
{
    var player = InGameManager.Instance.InGameReferencer.Player;
    var position = spawnPoint.position;
    var rotation = spawnPoint.rotation;
    player.Warp(position, rotation);
    gameStateApplier.OnLateUpdate(false);
}
global::ExposureUnnoticed2.ObjectUI.StageNamePanel.StageNamePanelView.Open(global::ExposureUnnoticed2.Master.Stage.MStage.Get(completeTransitionStageEvent.StageController.StageType));
InGameManager.Instance.InGameReferencer.NpcManager.OnLoadStage();
InGameManager.Instance.DroppedItemManager.ApplyReserveDropIfNeed();
InGameManager.Instance.DroppedItemManager.ActiveDroppedItemIfNeed();
if (DroneManager.Instance.IsActiveMission)
{
    Scheduler.DelayCondition(() => global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("ドローンミッション中はオートセーブされません"), global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType.Log),
        () => !global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.IsOpenFade, 10f);
}
if (GameState.GameStateData.CurrentStageType == global::ExposureUnnoticed2.Master.Stage.StageType.Apart)
    InGameManager.Instance.InGameReferencer.Player.Pca.Mental.RecoveryMental();
}

		private void OnExitInGameEvent(ExitInGameEvent exitInGameEvent)
		{
// Complete original native body; retain original callback and failure order.
InGameManager.Instance.OnExitInGameEvent();
}

		private void OnFoundExposureEvent(FoundStrangerEvent foundStrangerEvent)
		{
// Complete original native body; retain original callback and failure order.
InGameManager.Instance.InGameReferencer.Player.OnFoundStrangerEvent(foundStrangerEvent.FoundNpc);
}
	}
}
