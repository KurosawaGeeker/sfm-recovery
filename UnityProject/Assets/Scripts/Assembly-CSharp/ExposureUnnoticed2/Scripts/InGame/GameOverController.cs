using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class GameOverController : MonoBehaviour
	{
		public enum GameOverStagingState
		{
			Before = 0,
			MentalBlink = 1,
			CameraUp = 2,
			SadCharacter = 3,
			RunAndShowPanel = 4
		}

		[SerializeField]
		private float blend1;

		[SerializeField]
		private Transform initCameraTransform;

		[SerializeField]
		private Transform cameraUpTransform;

		[SerializeField]
		private Transform playerLookAtTransform;

		[SerializeField]
		private Animator mentalRedLightAnimator;

		public GameOverStagingState currentState;

		private Animator animator;

		public bool IsOnCameraStaging => currentState != GameOverStagingState.Before && currentState != GameOverStagingState.MentalBlink;

		public void Initialize()
		{
// Complete original RVA 0x4847A0; PE/typed identity is audited in this owned stage.
EventManager.AddListener<GameOverEvent>(OnGameOverEvent, false);
animator = GetComponent<Animator>();
}

		public void Dispose()
		{
// Complete original RVA 0x4848C0; PE/typed identity is audited in this owned stage.
EventManager.RemoveListener<GameOverEvent>(OnGameOverEvent);
}

		public void OnLateUpdate()
		{
// Complete original RVA 0x484980; PE/typed identity is audited in this owned stage.
if (currentState != GameOverStagingState.CameraUp) return;
InGameManager.Instance.InGameReferencer.BaseCameraController.transform.position = Vector3.Lerp(initCameraTransform.position, cameraUpTransform.position, blend1);
InGameManager.Instance.InGameReferencer.BaseCameraController.transform.rotation = Quaternion.Slerp(initCameraTransform.rotation, cameraUpTransform.rotation, blend1);
}

		private void OnGameOverEvent(GameOverEvent gameOverEvent)
		{
// Complete original RVA 0x484E00; PE/typed identity is audited in this owned stage.
if ((int)GameState.GameStateData.DifficultType != 0)
{
    var beforeRank = global::ExposureUnnoticed2.Master.Rank.MRank.CalcCurrentRank((double)GameState.GameStateData.ExposeExp);
    var managerForLost = InGameManager.Instance;
    if (managerForLost is null) throw new global::System.NullReferenceException();
    var stateForLost = GameState.GameStateData;
    if (stateForLost is null) throw new global::System.NullReferenceException();
    var tempForLost = managerForLost.TempInGameState;
    if (tempForLost is null) throw new global::System.NullReferenceException();
    tempForLost.LostRp = unchecked((int)stateForLost.CurrentEarnThisTimeRp);
    GameState.GameStateData.StatistisTotalLostRp = GameState.GameStateData.CurrentEarnThisTimeRp + GameState.GameStateData.StatistisTotalLostRp;
    var statisticsState = GameState.GameStateData;
    if (statisticsState is null) throw new global::System.NullReferenceException();
    statisticsState.StatistisTotalLostExp = (double)InGameManager.Instance.TempInGameState.LostExp + statisticsState.StatistisTotalLostExp;
    var afterRank = global::ExposureUnnoticed2.Master.Rank.MRank.CalcCurrentRank((double)GameState.GameStateData.ExposeExp);
    InGameManager.Instance.InGameReferencer.Player.Pca.PlayerState.CurrentRank = afterRank;
    var rankManager = InGameManager.Instance;
    if (rankManager is null || beforeRank is null || afterRank is null) throw new global::System.NullReferenceException();
    rankManager.TempInGameState.IsDownRank = beforeRank.Rank != afterRank.Rank;
}
var clothesState = InGameManager.Instance.InGameReferencer.Player.Pca.PlayerState;
var defaultClothes = global::ExposureUnnoticed2.Master.Clothes.MClothes.Get((global::ExposureUnnoticed2.Master.Clothes.ClothesType)0);
if (clothesState is null) throw new global::System.NullReferenceException();
clothesState.CurrentClothes = defaultClothes;
GameState.GameStateData.PlayerCurrentClothes = (global::ExposureUnnoticed2.Master.Clothes.ClothesType)0;
var dropped = DroppedItemManager.Instance;
if (dropped is null) throw new global::System.NullReferenceException();
dropped.Collect((DroppedItemManager.DropItemType)0);
var drone = DroneManager.Instance;
if (drone is null) throw new global::System.NullReferenceException();
drone.InterruptMission();
var random = RandomMissionManager.Instance;
if (random is null) throw new global::System.NullReferenceException();
random.InterruptMission();
GameState.GameStateData.CurrentEarnThisTimeRp = 0d;
var missions = global::ExposureUnnoticed2.Scripts.Mission.MissionManager.Instance;
if (missions is null) throw new global::System.NullReferenceException();
missions.ResetMission();
GameState.GameStateData.PlayerVibrationModeType = (global::ExposureUnnoticed2.Master.AdultGoods.VibrationModeType)0;
GameState.GameStateData.PistonMachineModeType = 0;
GameState.GameStateData.PlayerMoisture = 0f;
GameState.GameStateData.PlayerAddMoisture = 0f;
GameState.GameStateData.PlayerLife = 1;
GameState.GameStateData.CurrentStageType = (global::ExposureUnnoticed2.Master.Stage.StageType)0;
GameState.GameStateData.PlayerPosition = new Vector3(0f, 0.1f, 0f);
GameState.GameStateData.PlayerFear = 0f;
unchecked { ++GameState.GameStateData.StatistisGameOverCount; }
bool mapHandcuffs = GameState.GameStateData.PlayerIsAttachMapHandcuffs;
if (mapHandcuffs)
{
    GameState.GameStateData.PlayerIsAttachMapHandcuffs = false;
    var type = GameState.GameStateData.AttachedHandcuffType;
    GameStateFacade.UpdateAdultGoodsState(type, !GameStateFacade.IsEquipAdultGoods(GameState.GameStateData.AttachedHandcuffType));
}
if (GameState.GameStateData.PlayerIsAttachHandcuffs)
{
    GameState.GameStateData.PlayerIsAttachHandcuffs = false;
    var type = GameState.GameStateData.AttachedHandcuffType;
    GameStateFacade.UpdateAdultGoodsState(type, !GameStateFacade.IsEquipAdultGoods(GameState.GameStateData.AttachedHandcuffType));
}
if (!DebugController.SkipGameOverSave) SaveManager.Save((SaveRapper.SaveDataType)3, -1, false);
InGameManager.Instance.TempInGameState.IsStopGameStateApplier = true;
InGameManager.Instance.InGameReferencer.Player.OnGameOverEvent();
InGameManager.Instance.InGameReferencer.NpcManager.OnGameOverEvent();
var initialCamera = initCameraTransform;
Vector3 cameraPosition = InGameManager.Instance.InGameReferencer.BaseCameraController.transform.position;
Quaternion cameraRotation = InGameManager.Instance.InGameReferencer.BaseCameraController.transform.rotation;
initialCamera.SetPositionAndRotation(cameraPosition, cameraRotation);
float height = mapHandcuffs ? 0.6f : global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerBodyCustomizeAdjuster.HeightScale * 1.2f;
int playerLayer = LayerMask.NameToLayer("Player");
int npcLayer = LayerMask.NameToLayer("Npc");
int blockerLayer = LayerMask.NameToLayer("PlayerBlocker");
int faceLayer = LayerMask.NameToLayer("PlayerFace");
var player = InGameManager.Instance.InGameReferencer.Player;
var lookAt = playerLookAtTransform;
Vector3 playerPosition = player.transform.position;
float y = player.transform.position.y;
if (lookAt is null) throw new global::System.NullReferenceException();
lookAt.position = new Vector3(playerPosition.x, y + height, playerPosition.z);
Vector3 origin = playerLookAtTransform.position;
Vector3 direction = player.transform.forward;
RaycastHit hit;
float distance = Physics.SphereCast(origin, 0.025f, direction, out hit, 0.8f,
    ~((1 << playerLayer) | (1 << npcLayer) | (1 << blockerLayer) | (1 << faceLayer)), QueryTriggerInteraction.Ignore) ? hit.distance : 0.8f;
var cameraUp = cameraUpTransform;
cameraUp.position = player.transform.position + player.transform.forward * distance + Vector3.up * height;
cameraUpTransform.LookAt(playerLookAtTransform);
animator.SetTrigger("Start");
}

		public void StartMentalRedLight()
		{
		}

		public void ChangeTpCameraView()
		{
// Complete original RVA 0x4862E0; PE/typed identity is audited in this owned stage.
InGameManager.Instance.InGameReferencer.BaseCameraController.SetTpsCullingMask();
}

		public void PlayJingle()
		{
// Complete original RVA 0x4863A0; PE/typed identity is audited in this owned stage.
global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)19, -1f, true);
}

		public void StartSadCharacter()
		{
// Complete original RVA 0x486410; PE/typed identity is audited in this owned stage.
global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.SetFaceLayerWeight(0f);
global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.SetAnimationMoveSpeed(0f);
global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.SetAnimatorSpeed(1f);
global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransAction((global::ExposureUnnoticed2.Master.Action.ActionType)10018);
InGameManager.Instance.InGameReferencer.Player.Pca.PlayerAnimationManager.SetStrafe(false, false, 0f, 0f);
InGameManager.Instance.InGameReferencer.Player.Pca.LookAtController.DisableAll();
}

		public void StartCameraUp()
		{
// Complete original RVA 0x486640; PE/typed identity is audited in this owned stage.
var initialCamera = initCameraTransform;
Vector3 position = InGameManager.Instance.InGameReferencer.BaseCameraController.transform.position;
Quaternion rotation = InGameManager.Instance.InGameReferencer.BaseCameraController.transform.rotation;
initialCamera.SetPositionAndRotation(position, rotation);
currentState = GameOverStagingState.CameraUp;
}

		public void OpenFailedPanel()
		{
// Complete original RVA 0x4868C0; PE/typed identity is audited in this owned stage.
global::ExposureUnnoticed2.ObjectUI.MissionFailedPanel.MissionFailedPanelView.Open();
}
	}
}
