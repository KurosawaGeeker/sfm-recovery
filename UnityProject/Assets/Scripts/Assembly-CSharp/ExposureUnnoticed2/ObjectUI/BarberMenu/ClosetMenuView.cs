using System.Collections.Generic;
using Common.Scripts.UI;
using ExposureUnnoticed2.Master.Clothes;
using ExposureUnnoticed2.ObjectUI.ClosetMenu;
using ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu;
using ExposureUnnoticed2.ObjectUI.CommonUI.CommonTab;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.BarberMenu
{
	public class ClosetMenuView : BasePanelView
	{
		public static ClosetMenuView Instance;

		[SerializeField]
		private GameObject itemPrefab;

		[SerializeField]
		private Transform hairItemParent;

		[SerializeField]
		private GameObject clothesView;

		[SerializeField]
		private CustomizeMenuView customizeView;

		[SerializeField]
		private CommonTabView commonTabView;

		private List<ClosetMenuItemView> itemViewList;

		private bool isInitDisplayClothes;

		private bool isInitDisplayBra;

		private bool isInitDisplayPants;

		private bool isInitDisplayCostume;

		private int initRotateRemainFlame;

		public bool IsDisplayCostume { get; private set; }

		private void Initialize()
		{
// Complete original native body draft; native/CLR acceptance pending.

Instance = this;
global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAnimationManager.SetPreviewNakedArmPose(true);
global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DestroyChildren(hairItemParent);
global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.SetVisibleRightDownInfo(false);
foreach (var type in global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetPossessionClothes())
{
    var record = global::ExposureUnnoticed2.Master.Clothes.MClothes.Get(type);
    var view = UnityEngine.Object.Instantiate(itemPrefab, hairItemParent).GetComponent<ClosetMenuItemView>();
    view.Initialize(record, OnClickItem);
    itemViewList.Add(view);
}
var naked = UnityEngine.Object.Instantiate(itemPrefab, hairItemParent).GetComponent<ClosetMenuItemView>();
naked.InitializeNaked(OnClickNakedItem);
itemViewList.Add(naked);
customizeView.Initialize(null, this);
isInitDisplayClothes = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.ClothesParent.activeInHierarchy;
isInitDisplayBra = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.BraParent.activeInHierarchy;
isInitDisplayPants = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.PantsParent.activeInHierarchy;
isInitDisplayCostume = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.CosplayParent.activeInHierarchy;
global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.BaseCameraController.SetInventoryCamera(true, true);
RefreshClothesView();
global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.LookAtController.EnableLookAt((global::ExposureUnnoticed2.Object3D.Player.Scripts.LookAtController.LookAtLayer)6, global::ExposureUnnoticed2.Object3D.ScenePlops.CustomLookAtTarget.CustomLookAtTargetComponent.Instance.transform, true);
global::ExposureUnnoticed2.Object3D.ScenePlops.ClosetLight.ClosetLightController.Instance.SetClosetLight(true);
OnClickCategory(3);
global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49);
global::ExposureUnnoticed2.Scripts.InGame.SlowAssistManager.Instance.CancelSlow();

}

		private void RefreshClothesView()
		{
// Complete original native body draft; native/CLR acceptance pending.
foreach (var view in itemViewList) view.Refresh(global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.PlayerState.CurrentClothes);
}

		private void OnClickItem(MClothesRecord record)
		{
// Complete original native body draft; native/CLR acceptance pending.

if (record == global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.PlayerState.CurrentClothes) return;
global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShot(global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType.Equip);
global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.ChangeClothes(record);
RefreshClothesView();
var data = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DropDataDict[(global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.DropItemType)((int)record.ClothesType == 0 ? 0 : (int)record.ClothesType == 1 ? 1 : -1)];
if (data.IsDropped && (int)data.StageType == 0)
    global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.DroppedItemManager.Collect((global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.DropItemType)((int)record.ClothesType == 0 ? 0 : (int)record.ClothesType == 1 ? 1 : -1));

}

		private void OnClickNakedItem()
		{
// Complete original native body draft; native/CLR acceptance pending.

if ((int)global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.PlayerState.CurrentClothes.ClothesType == -1) return;
global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShot(global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType.Equip);
global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState = (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerStateModel.ClothesState)5;
global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.SkipDropClothes = true;
global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerClothes.DropClothes(null);
RefreshClothesView();

}

		private void LateUpdate()
		{
// Complete original native body draft; native/CLR acceptance pending.

if (initRotateRemainFlame >= 0)
{
    initRotateRemainFlame = unchecked(initRotateRemainFlame - 1);
    var receiver = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.Transform;
    var rotation = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.Transform.rotation;
    var angles = rotation.eulerAngles;
    angles.y = -90f;
    receiver.rotation = Quaternion.Euler(angles);
}
if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)10) && IsActivePanel()) OnClickBack();
float rotate = 0f;
if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDown((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)13))
{
    var move = global::ExposureUnnoticed2.Scripts.Base.InputManager.GetVector2((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1);
    float sensitivity = global::ExposureUnnoticed2.Master.Common.MCommon.BaseCameraSensitive;
    float option = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetOptionMouseXScale();
    rotate = option * (sensitivity * move.x);
}
global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.Transform.Rotate(Vector3.up, ((rotate * 0.35f) * Time.deltaTime) * -250f, Space.World);

}

		public void OnClickCategory(int index)
		{
// Complete original native body draft; native/CLR acceptance pending.

clothesView.SetActive(false);
customizeView.gameObject.SetActive(false);
if (index == 0) clothesView.SetActive(true);
if (index == 3) { customizeView.Show(); return; }
customizeView.customizeFacialPanelView.Hide();

}

		public void OnClickCostumeDisplay()
		{
// Complete original native body draft; native/CLR acceptance pending.
IsDisplayCostume = !IsDisplayCostume; RefreshEquipment();
}

		private void RefreshEquipment()
		{
// Complete original native body draft; native/CLR acceptance pending.

global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.ClothesParent.SetActive(IsDisplayCostume && isInitDisplayClothes);
global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.BraParent.SetActive(IsDisplayCostume && isInitDisplayBra);
global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.PantsParent.SetActive(IsDisplayCostume && isInitDisplayPants);
global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.CosplayParent.SetActive(IsDisplayCostume && isInitDisplayCostume);
if (!IsDisplayCostume) global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerBodyCustomizeAdjuster.ForceShrink(false);
else global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerBodyCustomizeAdjuster.OnChangeCostumeCosplayEvent(null);

}

		public void OnClickBack()
		{
// Complete original native body draft; native/CLR acceptance pending.

customizeView.OnClose();
global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() =>
{
    global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.SetVisibleRightDownInfo(true);
    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)50);
    global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.PlayerBodyCustomizeAdjuster.AdjustMoveSpeed();
    global::ExposureUnnoticed2.Object3D.ScenePlops.ClosetLight.ClosetLightController.Instance.SetClosetLight(false);
    global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.LookAtController.DisableLookAt((global::ExposureUnnoticed2.Object3D.Player.Scripts.LookAtController.LookAtLayer)6);
    global::ExposureUnnoticed2.Scripts.InGame.SaveManager.Save((global::ExposureUnnoticed2.Scripts.Base.SaveRapper.SaveDataType)3, -1, false);
    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_save_complete"), (global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType)1);
    global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.BaseCameraController.SetInventoryCamera(false, false);
    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerBoneScaleAdjustManager.ReAdjust();
    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerBodyCustomizeAdjuster.OnChangeCostumeCosplayEvent(null);
    IsDisplayCostume = true;
    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAnimationManager.SetPreviewNakedArmPose(false);
    Instance = null;
    RefreshEquipment();
    Close();
}, 5);

}

		public static void Open()
		{
            // Complete original52CBB0. Original Initialize remains a declared separate downstream family.
            var panel = global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.Open(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.ClosetMenu,false) as ClosetMenuView;
            panel.Initialize();
        }
        // Complete original constructor52CCB0; no original field or visibility is changed.
        public ClosetMenuView()
        {
            IsDisplayCostume = true;
            itemViewList = new List<ClosetMenuItemView>();
            initRotateRemainFlame = 5;
        }

	}
}
