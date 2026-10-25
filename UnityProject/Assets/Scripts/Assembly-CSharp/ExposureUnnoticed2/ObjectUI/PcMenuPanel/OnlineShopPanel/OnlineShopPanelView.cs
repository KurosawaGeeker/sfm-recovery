using System.Collections.Generic;
using Common.CommonUI.CommonScrollView;
using Common.Scripts.UI;
using ExposureUnnoticed2.Master.BodyPaint;
using ExposureUnnoticed2.Master.Clothes;
using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.Master.Costume;
using ExposureUnnoticed2.Master.Hair;
using ExposureUnnoticed2.ObjectUI.CommonButton;
using ExposureUnnoticed2.ObjectUI.CommonUI.CommonTab;
using ExposureUnnoticed2.ObjectUI.InventoryPanel;
using ExposureUnnoticed2.ObjectUI.InventoryPanel.CosplaySerieseSelectPanel;
using ExposureUnnoticed2.ObjectUI.InventoryPanel.PartsOptionSetupPanel;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.PcMenuPanel.OnlineShopPanel
{
	public class OnlineShopPanelView : BasePanelView
	{
		[SerializeField]
		private Transform itemParent;

		[SerializeField]
		private Transform cosplayItemParent;

		[SerializeField]
		private GameObject itemPrefab;

		[SerializeField]
		private CommonTabView tabView;

		[SerializeField]
		private ButtonView purchaseButton;

		[SerializeField]
		private GameObject basicContentParent;

		[SerializeField]
		private GameObject cosplayContentParent;

		[SerializeField]
		private GameObject hairContentParent;

		[SerializeField]
		private ButtonGroupManager basicButtonGroupManager;

		[SerializeField]
		private ButtonGroupManager cosplayButtonGroupManager;

		[SerializeField]
		private GameObject descriptionPanel;

		[SerializeField]
		private TextMeshProUGUI descriptionPanelName;

		[SerializeField]
		private TextMeshProUGUI descriptionPanelDescription;

		[SerializeField]
		private Image descriptionPanelIMage;

		[SerializeField]
		private Transform hairItemParent;

		[SerializeField]
		private GameObject hairItemPrefab;

		[SerializeField]
		private ButtonGroupManager hairButtonGroupManager;

		[SerializeField]
		private CosplaySeriesSelectPanelView cosplaySeriesSelectPanelView;

		[SerializeField]
		private PartsOptionSetupPanelView partsOptionSetupPanelView;

		[SerializeField]
		private InventoryEffectPanelView inventoryEffectPanelView;

		[SerializeField]
		private RectTransform basicScrollViewRect;

		[SerializeField]
		private CommonScrollRectView basicCommonScrollRectView;

		[SerializeField]
		private CommonScrollRectView cosplayCommonScrollRectView;

		[SerializeField]
		private CommonScrollRectView hairCommonScrollRectView;

		public bool IsOpenChild;

		private MHairRecord currentSelectHair;

		private MHairRecord oldHair;

		private MClothesRecord oldClothes;

		private bool isBring;

		private int currentTabIndex;

		private CosplayType currentSeries;

		private int currentSeriesIndex;

		private Dictionary<CostumeType, bool> playerCostumeDictCache;

		private Dictionary<BodyPaintType, bool> playerBodyPaintDictCache;

		private Dictionary<int, bool> playerCosplayPartsDictCache;

		private int currentSelectIndex;

		private List<OnlineShopItemView> itemList = new List<OnlineShopItemView>();

		private List<BarberItemButtonView> hairItemList = new List<BarberItemButtonView>();

		private bool justOnPurchase;

		public static OnlineShopPanelView Instance { get; private set; }

		public bool IsHideClothesCostume { get; private set; }

		public IOnlineShopItemRecord CurrentRecord { get; private set; }

		private void Initialize()
		{
            Instance = this;
            global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAnimationManager.SetPreviewNakedArmPose(true);
            tabView.Initialize(tabIndex => OnClickTab(tabIndex, false), () => IsOpenChild);
            global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.BaseCameraController.SetInventoryCamera(true);
            global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.SetVisibleRightDownInfo(false);
            oldHair = MHair.Get(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CurrentHair);
            oldClothes = MClothes.Get(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothes);
            partsOptionSetupPanelView.Initialize();
            playerCostumeDictCache = new Dictionary<CostumeType, bool>(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCostumeDict);
            playerCosplayPartsDictCache = new Dictionary<int, bool>(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCosplayPartsDict);
            playerBodyPaintDictCache = new Dictionary<BodyPaintType, bool>(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.BodyPaintTypeDict);
            OnClickTab(0);
            onClose = () =>
            {
                Instance = null;
                global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)50, -1f, false);
                global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCostumeDict = new Dictionary<CostumeType, bool>(playerCostumeDictCache);
                global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCosplayPartsDict = new Dictionary<int, bool>(playerCosplayPartsDictCache);
                global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.BodyPaintTypeDict = new Dictionary<BodyPaintType, bool>(playerBodyPaintDictCache);
                global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.BaseCameraController.SetInventoryCamera(false);
                global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.SetVisibleRightDownInfo(true);
                global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerCostumeController.SetupCostumeFromGameState();
                global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerCosplayManager.SetupCostumeFromGameState();
                global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerBodyMaterialController.SetupBodyPaintFromGameState();
                global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.ClothesParent.SetActive(true);
                global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAnimationManager.SetPreviewNakedArmPose(false);
                global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.ChangeHair(oldHair);
                global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.ChangeClothes(oldClothes);
            };
            cosplaySeriesSelectPanelView.Initialize(index => ChangeSeries(index), true);
            global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() => ToPadModeIfNeed(true), 1);
		}

		public void ToPadModeIfNeed(bool isForce = false)
		{
            if (!IsActivePanel() && !isForce) return;
            if (!global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad) return;
            var group = GetCurrentActiveButtonGroupManager();
            if (group.ButtonCount <= currentSelectIndex)
                currentSelectIndex = Mathf.Max(0, unchecked(group.ButtonCount - 1));
            group.ForceSelect(group.ButtonCount != 0 ? group.Get(currentSelectIndex) : null);
		}

		private ButtonGroupManager GetCurrentActiveButtonGroupManager()
		{
            switch (currentTabIndex)
            {
                case 0: case 1: case 4: return basicButtonGroupManager;
                case 2: return cosplayButtonGroupManager;
                case 3: return hairButtonGroupManager;
                default: return null;
            }
		}

		private void Update()
		{
            if (IsOpenChild) return;
            CommonCheckClose();
            if (IsActivePanel() && global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad)
            {
                if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsJustToGamePad) ToPadModeIfNeed(false);
                int move = 0;
                if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1001)
                    || global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownLongRepeat((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1001, 20, 5)) move = -1;
                if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1002)
                    || global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownLongRepeat((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1002, 20, 5)) move = unchecked(move + 1);
                if (currentTabIndex == 0 || currentTabIndex == 1 || currentTabIndex == 4)
                {
                    if (itemList.Count != 0)
                    {
                        if (move != 0)
                        {
                            itemList[currentSelectIndex].ForceSetEmit(0);
                            currentSelectIndex = unchecked(basicButtonGroupManager.ButtonCount + currentSelectIndex + move) % basicButtonGroupManager.ButtonCount;
                            itemList[currentSelectIndex].ForceSetEmit(1);
                            basicButtonGroupManager.InvokeOnClick(currentSelectIndex);
                            var scroll = basicCommonScrollRectView;
                            scroll.ScrollToCentering(itemList[currentSelectIndex].gameObject);
                        }
                        if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1000)) itemList[currentSelectIndex].OnClickPurchase();
                    }
                }
                else if (currentTabIndex == 2)
                {
                    if (move != 0)
                    {
                        itemList[currentSelectIndex].ForceSetEmit(0);
                        currentSelectIndex = unchecked(cosplayButtonGroupManager.ButtonCount + currentSelectIndex + move) % cosplayButtonGroupManager.ButtonCount;
                        itemList[currentSelectIndex].ForceSetEmit(1);
                        cosplayButtonGroupManager.InvokeOnClick(currentSelectIndex);
                        var scroll = cosplayCommonScrollRectView;
                        scroll.ScrollToCentering(itemList[currentSelectIndex].gameObject);
                    }
                    if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1000)) itemList[currentSelectIndex].OnClickPurchase();
                    if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1007))
                    {
                        global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49);
                        itemList[currentSelectIndex].OnClickOption();
                    }
                }
                else if (currentTabIndex == 3)
                {
                    if (move != 0)
                    {
                        hairItemList[currentSelectIndex].ForceSetEmit(0);
                        currentSelectIndex = unchecked(hairButtonGroupManager.ButtonCount + currentSelectIndex + move) % hairButtonGroupManager.ButtonCount;
                        hairItemList[currentSelectIndex].ForceSetEmit(1);
                        hairButtonGroupManager.InvokeOnClick(currentSelectIndex);
                        var scroll = hairCommonScrollRectView;
                        scroll.ScrollToCentering(hairItemList[currentSelectIndex].gameObject);
                    }
                    if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1000)) hairItemList[currentSelectIndex].OnClickPurchase();
                }
            }
            if (currentTabIndex == 2) cosplaySeriesSelectPanelView.OnLateUpdate();
		}

		private void OnClickTab(int index, bool isOnPurchase = false)
		{
            int oldIndex = currentTabIndex;
            currentTabIndex = index;
            global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DestroyChildren(itemParent);
            basicContentParent.SetActive(false);
            cosplayContentParent.SetActive(false);
            hairContentParent.SetActive(false);
            CurrentRecord = null;
            global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.ChangeHair(oldHair);
            global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerCostumeController.DeactivateAll();
            global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerCosplayManager.DeactivateAll();
            global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.BodyPaintTypeDict = new Dictionary<BodyPaintType, bool>(playerBodyPaintDictCache);
            global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerBodyMaterialController.SetupBodyPaintFromGameState();
            itemList.Clear();
            hairItemList.Clear();
            if (!isOnPurchase) currentSelectIndex = 0;
            if (index == 1)
            {
                global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.ClothesParent.SetActive(true);
                IsHideClothesCostume = false;
                descriptionPanel.SetActive(false);
                basicContentParent.SetActive(true);
                foreach (var pair in global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoods.GetAll())
                {
                    var record = pair.Value;
                    if (global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.FutanariDisable && (int)record.Type == 8) continue;
                    if (((int)record.Type == 1 || (int)record.Type == 7 || (int)record.Type == 6) && !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)111)) continue;
                    if (((int)record.Type == 8 || (int)record.Type == 9 || (int)record.Type == 10) && !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)114)) continue;
                    if (((int)record.Type == 3 || (int)record.Type == 5 || (int)record.Type == 4) && !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)110)) continue;
                    if ((int)record.Type == 2 && !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)113)) continue;
                    var view = Object.Instantiate(itemPrefab, itemParent).GetComponent<OnlineShopItemView>();
                    itemList.Add(view);
                    bool bring = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringAdultGoods(record.Type);
                    view.Initialize(record, bring, OnClickItem, OnClickPurchase);
                }
                basicButtonGroupManager.ReSearchButton();
                inventoryEffectPanelView.gameObject.SetActive(true);
                global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.SetHeight(basicScrollViewRect, 705f);
                inventoryEffectPanelView.Refresh(null);
                if (oldIndex != currentTabIndex) basicCommonScrollRectView.ResetScroll();
            }
            else if (index == 2)
            {
                inventoryEffectPanelView.Refresh(null);
                global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.ClothesParent.SetActive(false);
                IsHideClothesCostume = true;
                descriptionPanel.SetActive(false);
                cosplayContentParent.SetActive(true);
                inventoryEffectPanelView.gameObject.SetActive(true);
                global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.SetHeight(basicScrollViewRect, 705f);
                cosplaySeriesSelectPanelView.OnClickSeriesChange(0);
                if (oldIndex != currentTabIndex) cosplayCommonScrollRectView.ResetScroll();
                return;
            }
            else if (index == 3)
            {
                inventoryEffectPanelView.gameObject.SetActive(false);
                descriptionPanel.SetActive(false);
                hairContentParent.SetActive(true);
                global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.ClothesParent.SetActive(true);
                IsHideClothesCostume = false;
                global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DestroyChildren(hairItemParent);
                foreach (var pair in MHair.GetAll())
                {
                    var record = pair.Value;
                    if (global::ExposureUnnoticed2.Scripts.Base.SystemManager.IsTrial && record.Price >= 21) continue;
                    if ((int)record.Type == 0) continue;
                    var view = Object.Instantiate(hairItemPrefab, hairItemParent).GetComponent<BarberItemButtonView>();
                    hairItemList.Add(view);
                    view.Initialize(record, OnClickItem, global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.HairTypeList.Contains(record.Type), OnClickPurchase);
                }
                global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.SetHeight(basicScrollViewRect, 815f);
                hairButtonGroupManager.ReSearchButton();
                if (oldIndex != currentTabIndex) hairCommonScrollRectView.ResetScroll();
            }
            else if (index == -1)
            {
                inventoryEffectPanelView.gameObject.SetActive(false);
                global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.ClothesParent.SetActive(true);
                IsHideClothesCostume = false;
                descriptionPanel.SetActive(false);
                basicContentParent.SetActive(true);
                foreach (var pair in MClothes.GetAll())
                {
                    var record = pair.Value;
                    if ((int)record.ClothesType == 0 || (int)record.ClothesType == -1) continue;
                    var view = Object.Instantiate(itemPrefab, itemParent).GetComponent<OnlineShopItemView>();
                    bool bring = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringClothes(record);
                    view.Initialize(record, bring, OnClickItem, OnClickPurchase);
                }
                global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.SetHeight(basicScrollViewRect, 815f);
                basicButtonGroupManager.ReSearchButton();
            }
            else if (index == 0)
            {
                inventoryEffectPanelView.gameObject.SetActive(false);
                global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.ClothesParent.SetActive(true);
                IsHideClothesCostume = false;
                descriptionPanel.SetActive(false);
                basicContentParent.SetActive(true);
                foreach (var pair in global::ExposureUnnoticed2.Master.Item.MItem.GetAll())
                {
                    var record = pair.Value;
                    if (global::ExposureUnnoticed2.Scripts.Base.SystemManager.IsTrial) break;
                    if (!record.CanByOnlineShop) continue;
                    if ((int)record.Type == 1 && !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)112)) continue;
                    if ((int)record.Type == 10002 && !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)106)) continue;
                    if (((int)record.Type == 5 || (int)record.Type == 8 || (int)record.Type == 9) && !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)109)) continue;
                    if (((int)record.Type == 3 || (int)record.Type == 4) && !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)107)) continue;
                    if (((int)record.Type == 2 || (int)record.Type == 6 || (int)record.Type == 7) && !global::ExposureUnnoticed2.Master.Achievement.MAchievement.IsRelease((global::ExposureUnnoticed2.Master.Item.ItemType)2)) continue;
                    if (global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.FutanariDisable && ((int)record.Type == 3 || (int)record.Type == 4)) continue;
                    bool bring = record.IsReusable && global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringItem(record.Type);
                    var view = Object.Instantiate(itemPrefab, itemParent).GetComponent<OnlineShopItemView>();
                    itemList.Add(view);
                    view.Initialize(record, bring, OnClickItem, OnClickPurchase);
                }
                global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.SetHeight(basicScrollViewRect, 815f);
                basicButtonGroupManager.ReSearchButton();
                if (oldIndex != currentTabIndex) basicCommonScrollRectView.ResetScroll();
            }
            else if (index == 4)
            {
                inventoryEffectPanelView.gameObject.SetActive(false);
                global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.ClothesParent.SetActive(false);
                IsHideClothesCostume = true;
                descriptionPanel.SetActive(false);
                basicContentParent.SetActive(true);
                foreach (var pair in global::ExposureUnnoticed2.Master.BodyPaint.MBodyPaint.GetAll())
                {
                    var record = pair.Value;
                    var view = Object.Instantiate(itemPrefab, itemParent).GetComponent<OnlineShopItemView>();
                    itemList.Add(view);
                    bool bring = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringBodyPaint(record.Type);
                    view.Initialize(record, bring, OnClickItem, OnClickPurchase);
                    if (global::ExposureUnnoticed2.Scripts.Base.SystemManager.IsTrial) break;
                }
                global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.SetHeight(basicScrollViewRect, 815f);
                basicButtonGroupManager.ReSearchButton();
                if (oldIndex != currentTabIndex) basicCommonScrollRectView.ResetScroll();
            }
            global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() => ToPadModeIfNeed(false), 1);
		}

		private IOnlineShopItemRecord ChangeSeries(int changeAmount)
		{
            inventoryEffectPanelView.Refresh(null);
            currentSeriesIndex = unchecked(currentSeriesIndex + changeAmount);
            int seriesCount = global::ExposureUnnoticed2.Scripts.Base.SystemManager.IsTrial ? 2 : MCosplay.CosplayNum;
            currentSeriesIndex = unchecked(currentSeriesIndex + seriesCount) % seriesCount;
            var series = MCosplay.GetByIndex(currentSeriesIndex);
            currentSeries = series.Type;
            itemList.Clear();
            if (!justOnPurchase) currentSelectIndex = 0;
            global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DestroyChildren(cosplayItemParent);
            bool isResetScroll;
            OnlineShopItemView firstView = null;
            IOnlineShopItemRecord firstRecord = null;
            foreach (var recordPartsRecord in series.GetAll())
            {
                if (global::ExposureUnnoticed2.Scripts.Base.SystemManager.IsTrial && recordPartsRecord.ReleaseRank > 0) continue;
                if (recordPartsRecord.IsNotSell) continue;
                var view = Object.Instantiate(itemPrefab, cosplayItemParent).GetComponent<OnlineShopItemView>();
                itemList.Add(view);
                bool bring = playerCosplayPartsDictCache.ContainsKey(unchecked((int)recordPartsRecord.ParentCosplayType * 10000 + recordPartsRecord.Id));
                view.Initialize(recordPartsRecord, bring, OnClickItem, OnClickPurchase, record =>
                {
                    IsOpenChild = true;
                    partsOptionSetupPanelView.Open(recordPartsRecord);
                });
                if (firstRecord == null)
                {
                    firstView = view;
                    firstRecord = recordPartsRecord;
                }
            }
            if (CurrentRecord is RCosplayParts) CurrentRecord.SwitchVisible(-1);
            CurrentRecord = null;
            cosplayButtonGroupManager.ReSearchButton();
            isResetScroll = !justOnPurchase;
            global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() =>
            {
                ToPadModeIfNeed(false);
                if (isResetScroll) cosplayCommonScrollRectView.ResetScroll();
                if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad && firstView) firstView.OnClick();
            }, 1);
            return firstRecord;
		}

		private void OnClickHairItem(MHairRecord record)
		{
            if (currentSelectHair == record)
            {
                global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.ChangeHair(oldHair);
                currentSelectHair = null;
            }
            else
            {
                global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.ChangeHair(record);
                currentSelectHair = record;
            }
		}

		private void OnClickItem(IOnlineShopItemRecord record, bool isBring)
		{
            if (object.ReferenceEquals(CurrentRecord, record)) CurrentRecord = null;
            else
            {
                if (CurrentRecord is not null) CurrentRecord.SwitchVisible(-1);
                CurrentRecord = record;
            }
            this.isBring = isBring;
            int defaultType;
            if (currentTabIndex == 3) defaultType = (int)oldHair.Type;
            else if (currentTabIndex == 4) defaultType = (int)oldClothes.ClothesType;
            else defaultType = -1;
            record.SwitchVisible(defaultType);
            if (record is RCosplayParts || record is global::ExposureUnnoticed2.Master.Accessory.MAdultGoodsRecord)
                inventoryEffectPanelView.Refresh(record as global::ExposureUnnoticed2.ObjectUI.InventoryPanel.IEffectiveEquipRecord, true);
            RefreshRight(CurrentRecord);
		}

		private void RefreshRight(IOnlineShopItemRecord record)
		{
            if (record is global::ExposureUnnoticed2.Master.Accessory.MAdultGoodsRecord item)
            {
                descriptionPanel.SetActive(false);
                var title = descriptionPanelName;
                string translatedName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(record.NameKey);
                title.text = translatedName;
                var descriptionLabel = descriptionPanelDescription;
                string translatedDescription = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(item.DescriptionKey);
                descriptionLabel.text = translatedDescription;
                return;
            }
            descriptionPanel.SetActive(false);
		}

		public void OnClickPurchase()
		{
            // Recovered complete original native body; presentation callees remain original calls.
            if (isBring) return;
if (!global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEnoughRp(CurrentRecord.Price))
{
    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_common_not_enough_rp"), global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType.Caution);
    return;
}
var itemForLimit = CurrentRecord as global::ExposureUnnoticed2.Master.Item.MItemRecord;
if (itemForLimit is not null && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ItemDict.ContainsKey(itemForLimit.Type)
    && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ItemDict[itemForLimit.Type] > 98)
{
    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_shop_over_item"), global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType.Caution);
    return;
}
global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.ConsumeRp(CurrentRecord.Price);
global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)47, -1f, false);
global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCostumeDict = new Dictionary<CostumeType, bool>(playerCostumeDictCache);
global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCosplayPartsDict = new Dictionary<int, bool>(playerCosplayPartsDictCache);
global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.BodyPaintTypeDict = new Dictionary<BodyPaintType, bool>(playerBodyPaintDictCache);
var boughtRecord = CurrentRecord;
if (boughtRecord is global::ExposureUnnoticed2.Master.Accessory.MAdultGoodsRecord adult)
{
    global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.AddAdultGoods(adult.Type);
    if ((int)adult.MotionType == 1 && !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringItem((global::ExposureUnnoticed2.Master.Item.ItemType)10001))
    {
        global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.ChangeItemNum((global::ExposureUnnoticed2.Master.Item.ItemType)10001, 1);
        global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.Open((global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.TutorialOpenKind)7);
    }
    if ((int)adult.Type == 4 && !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringItem((global::ExposureUnnoticed2.Master.Item.ItemType)10000))
        global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.ChangeItemNum((global::ExposureUnnoticed2.Master.Item.ItemType)10000, 1);
    if ((int)adult.MotionType == 3)
        global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.Open((global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.TutorialOpenKind)9);
    if ((int)adult.Type == 3 || (int)adult.Type == 4 || (int)adult.Type == 5)
        global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.Open((global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.TutorialOpenKind)6);
}
else if (boughtRecord is RCostume costume)
    global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCostumeDict.Add(costume.Type, false);
else if (boughtRecord is RCosplayParts part)
{
    global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCosplayPartsDict.Add(unchecked((int)part.ParentCosplayType * 10000 + part.Id), false);
    if ((int)part.HiddenPartsType != 0)
        global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.Open((global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.TutorialOpenKind)13);
}
else if (boughtRecord is MClothesRecord clothes)
    global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.AddClothes(clothes);
else if (boughtRecord is MHairRecord hair)
    global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.HairTypeList.Add(hair.Type);
else if (boughtRecord is global::ExposureUnnoticed2.Master.Item.MItemRecord item)
{
    global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.ChangeItemNum(item.Type, 1);
    if ((int)item.Type == 1)
        global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.Open((global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.TutorialOpenKind)5);
    if ((int)item.Type == 5 || (int)item.Type == 8)
        global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.Open((global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.TutorialOpenKind)10);
    if ((int)item.Type == 2 || (int)item.Type == 7)
        global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.Open((global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.TutorialOpenKind)11);
}
else if (boughtRecord is RBodyPaint bodyPaint)
    global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.SetActiveBodyPaint(bodyPaint.Type, false);
playerCostumeDictCache = new Dictionary<CostumeType, bool>(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCostumeDict);
playerBodyPaintDictCache = new Dictionary<BodyPaintType, bool>(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.BodyPaintTypeDict);
playerCosplayPartsDictCache = new Dictionary<int, bool>(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCosplayPartsDict);
global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerCostumeController.SetupCostumeFromGameState();
global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerCosplayManager.SetupCostumeFromGameState();
global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerBodyMaterialController.SetupBodyPaintFromGameState();
var purchased = CurrentRecord;
justOnPurchase = true;
OnClickTab(tabView.CurrentSelect, true);
justOnPurchase = false;
CurrentRecord = null;
RefreshRight(purchased);
global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_shop_purchased", global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(purchased.NameKey)), global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType.Log);

        }

		public static void Open()
		{
            var opened = global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.Open(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.OnlineShopPanel, false) as OnlineShopPanelView;
            opened.Initialize();
		}
	}
}
