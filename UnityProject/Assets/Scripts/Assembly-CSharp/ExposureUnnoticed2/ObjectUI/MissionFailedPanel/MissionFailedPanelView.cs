using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.MissionFailedPanel
{
	public class MissionFailedPanelView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI downRankText;

		[SerializeField]
		private TextMeshProUGUI lostExpText;

		[SerializeField]
		private TextMeshProUGUI baretaText;

		[SerializeField]
		private TextMeshProUGUI mentalText;

		[SerializeField]
		private TextMeshProUGUI lostRpText;

		private bool isOpenChild;

		private bool showCursor;

		private void Initialize()
		{
// Complete original RVA 0x4E1800; PE/typed identity is audited in this owned stage.
bool mentalFailed = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerMental <= 0f;
baretaText.gameObject.SetActive(!mentalFailed);
mentalText.gameObject.SetActive(mentalFailed);
if (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.LostExp == 0)
{
    downRankText.gameObject.SetActive(false);
    lostExpText.gameObject.SetActive(false);
}
else
{
    downRankText.gameObject.SetActive(global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsDownRank);
    var rankLabel = downRankText;
    string rank = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.PlayerState.CurrentRank.NameKey);
    string rankMessage = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_mission_failed_rank_down", rank);
    rankLabel.text = rankMessage;
    var experienceLabel = lostExpText;
    string lostExperience = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.LostExp.ToString() ?? "";
    string experienceMessage = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_mission_failed_lost_exp", lostExperience);
    experienceLabel.text = experienceMessage;
}
var moneyLabel = lostRpText;
if (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.LostRp == 0)
    moneyLabel.gameObject.SetActive(false);
else
{
    string lostMoney = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.LostRp.ToString() ?? "";
    string moneyMessage = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_mission_failed_earn_rp", lostMoney);
    moneyLabel.text = moneyMessage;
}
}

		public void OnClickLoad()
		{
// Complete original RVA 0x4E1EB0; PE/typed identity is audited in this owned stage.
global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.FadeStop(0.5f);
global::ExposureUnnoticed2.ObjectUI.FadeOutPanel.FadePanelView.Open(new global::ExposureUnnoticed2.ObjectUI.FadeOutPanel.WaitSceneLoadTwiceStarter(), () =>
{
    global::ExposureUnnoticed2.Scripts.InGame.TemporaryInGameState.IsCollectDropItem = global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.Instance.CollectAll();
    global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState = (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerStateModel.ClothesState)0;
    global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesBState = (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerStateModel.ClothesState)10;
    global::ExposureUnnoticed2.Scripts.InGame.SaveManager.Save((global::ExposureUnnoticed2.Scripts.Base.SaveRapper.SaveDataType)3, -1, false);
    global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsStopGameStateApplier = true;
    global::ExposureUnnoticed2.Scripts.InGame.SaveManager.Load((global::ExposureUnnoticed2.Scripts.Base.SaveRapper.SaveDataType)3, global::ExposureUnnoticed2.Scripts.InGame.SaveManager.CurrentSaveSlot);
    global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ExitInGameEvent);
    global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() => global::UnityEngine.SceneManagement.SceneManager.LoadScene("SpringboardLoadEmpty", global::UnityEngine.SceneManagement.LoadSceneMode.Single), 1);
}, null, () =>
{
    if (global::ExposureUnnoticed2.Scripts.InGame.TemporaryInGameState.IsCollectDropItem)
        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_mission_failed_correct_drop_item"), (global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType)1);
    global::ExposureUnnoticed2.Scripts.InGame.TemporaryInGameState.IsCollectDropItem = false;
}, true);
}

		private void Update()
		{
// Complete original RVA 0x4E21E0; PE/typed identity is audited in this owned stage.
if (!showCursor) return;
if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1000) ||
    global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)10)) OnClickLoad();
}

		public void OnClickGoToTitle()
		{
// Complete original RVA 0x4E2270; PE/typed identity is audited in this owned stage.
global::ExposureUnnoticed2.ObjectUI.FadeOutPanel.FadePanelView.Open(new global::ExposureUnnoticed2.ObjectUI.FadeOutPanel.WaitSceneLoadStarter(), () =>
{
    global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ExitInGameEvent);
    global::UnityEngine.SceneManagement.SceneManager.LoadScene("Title", global::UnityEngine.SceneManagement.LoadSceneMode.Single);
}, null, null, true);
}

		public void ShowCursor()
		{
// Complete original RVA 0x4E2420; PE/typed identity is audited in this owned stage.
showCursor = true;
global::ExposureUnnoticed2.Scripts.Base.CursorController.SetVisible(true);
}

		public static void Open()
		{
// Complete original RVA 0x4868C0; PE/typed identity is audited in this owned stage.
var prefab = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MissionFailedPanel;
var parent = global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.frontLayer;
var instance = global::UnityEngine.Object.Instantiate(prefab, parent);
if (instance is null) throw new global::System.NullReferenceException();
var panel = instance.GetComponent<MissionFailedPanelView>();
if (panel is null) throw new global::System.NullReferenceException();
panel.Initialize();
}
	}
}
