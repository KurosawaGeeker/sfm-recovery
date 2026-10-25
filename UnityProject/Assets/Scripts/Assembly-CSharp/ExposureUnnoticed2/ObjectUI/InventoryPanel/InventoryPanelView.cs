using System;
using System.Collections.Generic;
using Common.CommonUI.CommonScrollView;
using Common.Scripts.UI;
using ExposureUnnoticed2.Master.AdultGoods;
using ExposureUnnoticed2.ObjectUI.CommonUI.CommonTab;
using ExposureUnnoticed2.Scripts.Base;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InventoryPanel
{
	public class InventoryPanelView : BasePanelView
	{
		public static InventoryPanelView Instance;

		[SerializeField]
		private Transform equipmentItemParent;

		[SerializeField]
		private Transform itemItemParent;

		[SerializeField]
		private Transform adultGoodsItemParent;

		[SerializeField]
		private GameObject equipmentItemPrefab;

		[SerializeField]
		private GameObject itemItemPrefab;

		[SerializeField]
		private GameObject equipmentObject;

		[SerializeField]
		private GameObject itemObject;

		[SerializeField]
		private GameObject adultGoodsObject;

		[SerializeField]
		private CommonTabView tabView;

		[SerializeField]
		private InventoryPanelCosplayView cosplayView;

		[SerializeField]
		private InventoryPanelLoadoutView loadoutView;

		[SerializeField]
		private InventoryEffectPanelView effectPanelView;

		[SerializeField]
		private ButtonGroupManager equipButtonGroupManager;

		[SerializeField]
		private ButtonGroupManager adultGoodsButtonGroupManager;

		[SerializeField]
		private ButtonGroupManager itemButtonGroupManager;

		[SerializeField]
		private ButtonGroupManager loadoutButtonGroupManager;

		[SerializeField]
		private ButtonGroupManager cosplayButtonGroupManager;

		[SerializeField]
		private TextMeshProUGUI equipCountText;

		[SerializeField]
		private CommonScrollRectView itemCommonScrollRectView;

		[SerializeField]
		private CommonScrollRectView adultGoodsCommonScrollRectView;

		[SerializeField]
		private CommonScrollRectView cosplayCommonScrollRectView;

		[SerializeField]
		private CommonScrollRectView loadoutCommonScrollRectView;

		public bool IsOpenChild;

		public ItemCategory currentCategory;

		public int currentSelectIndex;

		private List<InventoryItemView> itemViewList;

		private List<InventoryEquipItemView> adultGoodsViewList;

		private int directOpenTab;

		private void Initialize(Action onClose, int directOpenTab)
		{
            // Recovered complete original RVA 0x4EFC50.
            this.directOpenTab = directOpenTab;
            Instance = this;
            this.onClose += onClose;
            EventManager.AddListener<ChangeSomeEquipEvent>(OnChangeSomeEquipEvent, false);
            if (!global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.IsStrafe)
                global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.BaseCameraController.SetInventoryCamera(true, false);
            tabView.Initialize(OnClickCategory, () => IsOpenChild || !IsActivePanel());
            cosplayView.Initialize(effectPanelView, this);
            cosplayView.Hide();
            loadoutView.Initialize(this);
            loadoutView.Hide();
            RefreshEquipCount();
            if (directOpenTab < 0) OnClickCategory(0);
            else tabView.OnClickTab(directOpenTab);
            Scheduler.DelayFlame(() => ToPadModeIfNeed(true), 1);
        }

		public void ToPadModeIfNeed(bool isForce = false)
		{
            // Recovered complete original RVA 0x4F00B0. IsActivePanel is evaluated first.
            if (!IsActivePanel() && !isForce) return;
            if (GetCurrentActiveButtonGroupManager().ButtonCount <= currentSelectIndex)
                currentSelectIndex = unchecked(GetCurrentActiveButtonGroupManager().ButtonCount - 1);
            if (InputManager.IsGamePad) GetCurrentActiveButtonGroupManager().ForceSelect(currentSelectIndex);
            else GetCurrentActiveButtonGroupManager().ForceSelect((ButtonGroupManager.IButtonView)null);
        }

		public override void OnReturnActive()
		{
            // Recovered complete original RVA 0x4F02C0; original has no index clamp here.
            if (InputManager.IsGamePad) GetCurrentActiveButtonGroupManager().ForceSelect(currentSelectIndex);
            else GetCurrentActiveButtonGroupManager().ForceSelect((ButtonGroupManager.IButtonView)null);
        }

		private ButtonGroupManager GetCurrentActiveButtonGroupManager()
		{
            // Recovered complete original RVA 0x4F0420; category4 intentionally has no group.
            switch ((int)currentCategory)
            {
                case 0: return itemButtonGroupManager;
                case 1: return adultGoodsButtonGroupManager;
                case 2: return cosplayButtonGroupManager;
                case 3: return loadoutButtonGroupManager;
                default: return null;
            }
        }

		private void OnDestroy()
		{
            // Recovered complete original RVA 0x4F0460.
            EventManager.RemoveListener<ChangeSomeEquipEvent>(OnChangeSomeEquipEvent);
        }

		private void OnChangeSomeEquipEvent(ChangeSomeEquipEvent evt)
		{
            // Recovered complete original RVA 0x4F0520.
            RefreshEquipCount();
        }

		public void RefreshEquipCount()
		{
            // Recovered complete original RVA 0x4F0530, including all linked unwind records.
            int count = CalcEquipCount();
            int level = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetSkillLevel((global::ExposureUnnoticed2.Master.Skill.SkillType)11);
            var record = global::ExposureUnnoticed2.Master.Skill.SkillMaxAccessoryNum.MSkillMaxAccessoryNum.Get(level);
            int max = record.MaxNum;
            equipCountText.text = Localizer.Trans("ui_inventory_equip_num") + count.ToString() + "/" + max.ToString();
        }

		public void OnClickBack()
		{
            // Recovered complete original RVA 0x4F0720; camera, static clear and Close order retained.
            global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.BaseCameraController.SetInventoryCamera(false, false);
            Instance = null;
            Close();
        }

		private void LateUpdate()
		{
            // Complete original4F0820. IsOpenChild gates the entire frame, including the final cosplay tick.
            if (IsOpenChild) return;
            if (IsActivePanel() && InputManager.IsGamePad)
            {
                if (InputManager.IsJustToGamePad) ToPadModeIfNeed(false);
                int move = 0;
                if (InputManager.IsDownFrame((InputManager.InputType)1001) || InputManager.IsDownLongRepeat((InputManager.InputType)1001,20,5)) move = -1;
                if (InputManager.IsDownFrame((InputManager.InputType)1002) || InputManager.IsDownLongRepeat((InputManager.InputType)1002,20,5)) ++move;
                switch ((int)currentCategory)
                {
                    case 0:
                        if (move != 0)
                        {
                            itemViewList[currentSelectIndex].ForceSetEmit(0);
                            currentSelectIndex = unchecked(itemButtonGroupManager.ButtonCount + currentSelectIndex + move) % itemButtonGroupManager.ButtonCount;
                            itemViewList[currentSelectIndex].ForceSetEmit(1);
                            itemButtonGroupManager.InvokeOnClick(currentSelectIndex);
                            itemCommonScrollRectView.ScrollToCentering(itemViewList[currentSelectIndex].gameObject);
                        }
                        if (InputManager.IsDownFrame((InputManager.InputType)1000)) itemViewList[currentSelectIndex].CommonOnClickByPad();
                        break;
                    case 1:
                        if (move != 0)
                        {
                            adultGoodsViewList[currentSelectIndex].ForceSetEmit(0);
                            currentSelectIndex = unchecked(adultGoodsButtonGroupManager.ButtonCount + currentSelectIndex + move) % adultGoodsButtonGroupManager.ButtonCount;
                            adultGoodsViewList[currentSelectIndex].ForceSetEmit(1);
                            adultGoodsButtonGroupManager.InvokeOnClick(currentSelectIndex);
                            adultGoodsCommonScrollRectView.ScrollToCentering(adultGoodsViewList[currentSelectIndex].gameObject);
                        }
                        if (InputManager.IsDownFrame((InputManager.InputType)1000)) adultGoodsViewList[currentSelectIndex].OnClickEquip();
                        break;
                    case 2:
                        if (move != 0)
                        {
                            cosplayView.ItemList[currentSelectIndex].ForceSetEmit(0);
                            currentSelectIndex = unchecked(cosplayButtonGroupManager.ButtonCount + currentSelectIndex + move) % cosplayButtonGroupManager.ButtonCount;
                            cosplayView.ItemList[currentSelectIndex].ForceSetEmit(1);
                            cosplayButtonGroupManager.InvokeOnClick(currentSelectIndex);
                            cosplayCommonScrollRectView.ScrollToCentering(cosplayView.ItemList[currentSelectIndex].gameObject);
                        }
                        if (InputManager.IsDownFrame((InputManager.InputType)1000)) cosplayView.ItemList[currentSelectIndex].OnClickEquip();
                        if (InputManager.IsDownFrame((InputManager.InputType)1007)) cosplayView.ItemList[currentSelectIndex].OnClickOption();
                        break;
                    case 3:
                        if (move != 0)
                        {
                            loadoutView.ItemList[currentSelectIndex].ForceSetEmit(0);
                            currentSelectIndex = unchecked(loadoutButtonGroupManager.ButtonCount + currentSelectIndex + move) % loadoutButtonGroupManager.ButtonCount;
                            loadoutView.ItemList[currentSelectIndex].ForceSetEmit(1);
                            loadoutButtonGroupManager.InvokeOnClick(currentSelectIndex);
                            loadoutCommonScrollRectView.ScrollToCentering(loadoutView.ItemList[currentSelectIndex].gameObject);
                        }
                        if (InputManager.IsDownFrame((InputManager.InputType)1000)) loadoutView.ItemList[currentSelectIndex].OnClickEquip();
                        if (InputManager.IsDownFrame((InputManager.InputType)1007))
                        {
                            global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49);
                            loadoutView.ItemList[currentSelectIndex].OnClickRegister();
                        }
                        if (InputManager.IsDownFrame((InputManager.InputType)1017))
                        {
                            global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49);
                            loadoutView.ItemList[currentSelectIndex].OnClickRename();
                        }
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
            if (IsActivePanel())
            {
                if (InputManager.IsDownFrame((InputManager.InputType)10)
                    || ((int)currentCategory == 0 && InputManager.IsDownFrame((InputManager.InputType)1008))
                    || ((int)currentCategory == 1 && InputManager.IsDownFrame((InputManager.InputType)1015))
                    || ((int)currentCategory == 2 && InputManager.IsDownFrame((InputManager.InputType)1013))
                    || ((int)currentCategory == 3 && InputManager.IsDownFrame((InputManager.InputType)1014)))
                {
                    OnClickBack();
                    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)50);
                }
                else if (InputManager.IsDownFrame((InputManager.InputType)1008)) tabView.OnClickTab(0);
                else if (InputManager.IsDownFrame((InputManager.InputType)1015)) tabView.OnClickTab(1);
                else if (InputManager.IsDownFrame((InputManager.InputType)1013)) tabView.OnClickTab(2);
                else if (InputManager.IsDownFrame((InputManager.InputType)1014)) tabView.OnClickTab(3);
            }
            cosplayView.OnLateUpdate();
        }

		public void OnClickCategory(int index)
		{
            // Recovered complete original RVA 0x4F1740, preserving complete original Show/Hide callees.
            currentCategory = (ItemCategory)index;
            equipmentObject.SetActive(false);
            itemObject.SetActive(false);
            adultGoodsObject.SetActive(false);
            effectPanelView.gameObject.SetActive(false);
            cosplayView.Hide();
            loadoutView.Hide();
            equipCountText.gameObject.SetActive(false);
            currentSelectIndex = 0;
            switch (index)
            {
                case 0:
                    itemObject.SetActive(true);
                    RefreshItemCategory();
                    return;
                case 1:
                    adultGoodsObject.SetActive(true);
                    RefreshAdultGoodsCategory();
                    effectPanelView.gameObject.SetActive(true);
                    effectPanelView.Refresh(null, false);
                    return;
                case 2:
                    cosplayView.Show();
                    effectPanelView.gameObject.SetActive(true);
                    equipCountText.gameObject.SetActive(true);
                    return;
                case 3:
                    loadoutView.Show();
                    return;
                case 4:
                    equipmentObject.SetActive(true);
                    effectPanelView.gameObject.SetActive(true);
                    effectPanelView.Refresh(null, false);
                    OnClickEquipmentCategory(0);
                    return;
                default:
                    return;
            }
        }

		public void RefreshItemCategory()
		{
            // Complete original 0x4F1D60 and original local Process/use/drop callback family.
            void Process(global::ExposureUnnoticed2.Master.Item.ItemType type, int num)
            {
                if (num == 0) return;
                var item = UnityEngine.Object.Instantiate(itemItemPrefab, itemItemParent).GetComponent<InventoryItemView>();
                itemViewList.Add(item);
                item.Initialize(global::ExposureUnnoticed2.Master.Item.MItem.Get(type), num, mItem =>
                {
                    void UseProcess()
                    {
                        if (!mItem.IsReusable && !mItem.IsDelayConsume)
                            global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.ChangeItemNum(mItem.Type, -1);
                        global::ExposureUnnoticed2.Master.Item.MItem.UseItemProcess(mItem);
                        RefreshItemCategory();
                    }
                    if ((int)mItem.Type == 10002 && global::ExposureUnnoticed2.Scripts.InGame.DroneManager.Instance.IsActiveMission)
                    {
                        if (!global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.CanUseHand(true, true)) return;
                    }
                    else if (!CheckPossibleAndAddMessageIfNeed()) return;
                    if ((int)mItem.Type == 8 || (int)mItem.Type == 7)
                    {
                        string title = Localizer.Trans("ui_common_confirm");
                        string description = Localizer.Trans("ui_caution_NoAchieveMissionMode");
                        var left = new global::ExposureUnnoticed2.ObjectUI.CommonPopup.CommonButtonData
                        {
                            Text = Localizer.Trans("ui_common_no"),
                            OnClick = () => { }
                        };
                        var right = new global::ExposureUnnoticed2.ObjectUI.CommonPopup.CommonButtonData
                        {
                            Text = Localizer.Trans("ui_common_yes"),
                            OnClick = () => UseProcess()
                        };
                        global::ExposureUnnoticed2.ObjectUI.CommonPopup.CommonPopupView.Open(title, description, left, right, false);
                    }
                    else UseProcess();
                }, mItem =>
                {
                    if ((int)mItem.Type == 10000)
                    {
                        if (!global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.CanUseHand(true, true)) return;
                        if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ItemDict[(global::ExposureUnnoticed2.Master.Item.ItemType)10000] == 0)
                        {
                            global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_not_have_handcuff_key"));
                            return;
                        }
                        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.LastDropItemType = (global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.DropItemType)5;
                    }
                    else if ((int)mItem.Type == 10001)
                    {
                        if (!CheckPossibleAndAddMessageIfNeed()) return;
                        if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ItemDict[(global::ExposureUnnoticed2.Master.Item.ItemType)10001] == 0)
                        {
                            global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_not_have_vibe_remocon"));
                            return;
                        }
                        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.LastDropItemType = (global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.DropItemType)6;
                    }
                    else return;
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAction.ChangeAction((global::ExposureUnnoticed2.Master.Action.ActionType)10002);
                });
            }
            ExternalMethodManager.DestroyChildren(itemItemParent);
            itemViewList.Clear();
            foreach (var pair in global::ExposureUnnoticed2.Master.Item.MItem.GetAll())
            {
                if (!global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ItemDict.ContainsKey(pair.Key)) continue;
                if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ItemDict[pair.Key] > 0)
                    Process(pair.Key, global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ItemDict[pair.Key]);
            }
            itemButtonGroupManager.ReSearchButton(false, false);
            Scheduler.DelayFlame(() => ToPadModeIfNeed(false), 1);
        }

		private void RefreshAdultGoodsCategory()
		{
            // Recovered complete original native body.
            adultGoodsViewList.Clear();
            ExternalMethodManager.DestroyChildren(adultGoodsItemParent);
            foreach (var pair in global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoods.GetAll())
            {
                if (!global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringAdultGoods(pair.Key)) continue;
                var item = UnityEngine.Object.Instantiate(equipmentItemPrefab, adultGoodsItemParent).GetComponent<InventoryEquipItemView>();
                adultGoodsViewList.Add(item);
                var record = global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoods.Get(pair.Key);
                item.Initialize(record, master =>
                {
                    var accessory = (global::ExposureUnnoticed2.Master.Accessory.MAdultGoodsRecord)master;
                    if ((int)accessory.Type == 8 && !global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveFutanari)
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_cant_equip_by_not_futanari"));
                        return;
                    }
                    if ((int)accessory.Type == 1 && global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipAdultGoods(accessory.Type))
                    {
                        var mission = global::ExposureUnnoticed2.Scripts.InGame.DroneManager.Instance.GetActiveMission();
                        if (mission != null && mission.Record.IsVibe)
                        {
                            global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_cantTakeOffViewInDrone"));
                            return;
                        }
                    }
                    if (CheckPossibleAndAddMessageIfNeed())
                    {
                        global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoods.UseItemProcess(accessory);
                        RefreshAdultGoodsCategory();
                        RefreshEquipCount();
                    }
                }, master => effectPanelView.Refresh(master, false), null);
            }
            adultGoodsButtonGroupManager.ReSearchButton(false, false);
            Scheduler.DelayFlame(() => ToPadModeIfNeed(false), 1);
        }

		public void OnClickEquipmentCategory(int index)
		{
			global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DestroyChildren(equipmentItemParent);
			foreach (var pair in global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCostumeDict)
			{
			    var masterRecord = global::ExposureUnnoticed2.Master.Costume.MCostume.Get(pair.Key);
			    var view = global::UnityEngine.Object.Instantiate(equipmentItemPrefab, equipmentItemParent).GetComponent<InventoryEquipItemView>();
			    view.Initialize(masterRecord, master =>
			    {
			        if (!CheckPossibleAndAddMessageIfNeed()) return;
			        var costume = (global::ExposureUnnoticed2.Master.Costume.RCostume)master;
			        if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipCostume(costume) || CanEquipByMaxCount())
			        {
			            global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.ChangeAccessoryIfAble(master);
			            RefreshEquipCount();
			        }
			    }, record => effectPanelView.Refresh(record, false), null);
			}
			equipButtonGroupManager.ReSearchButton(false, false);
		}

		private bool CheckPossibleAndAddMessageIfNeed()
		{
            // Recovered complete original native body.
            return global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.CanUseHand(true);
        }

		public bool CanEquipByMaxCount()
		{
            // Recovered complete original native body.
            int count = CalcEquipCount();
            int level = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetSkillLevel((global::ExposureUnnoticed2.Master.Skill.SkillType)11);
            var record = global::ExposureUnnoticed2.Master.Skill.SkillMaxAccessoryNum.MSkillMaxAccessoryNum.Get(level);
            if (count < record.MaxNum) return true;
            global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(
                Localizer.Trans("ui_inventory_over_equip_num"));
            return false;
        }

		private int CalcEquipCount()
		{
            // Recovered complete original native body.
            int costume = global::System.Linq.Enumerable.Count(
                global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCostumeDict, w => w.Value);
            int cosplay = global::System.Linq.Enumerable.Count(
                global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCosplayPartsDict, w => w.Value);
            return unchecked(costume + cosplay);
        }

		public static void Open(Action onClose, int directOpenTab = -1)
		{
            // Recovered complete original RVA 0x4F3070; wrong returned type remains a null Init fault.
            var panel = global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.Open(
                global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.InventoryPanel, false) as InventoryPanelView;
            panel.Initialize(onClose, directOpenTab);
        }

		private AccessoryCategory ToEquipmentCategory(int index)
		{
            // Recovered complete original 3-byte identity enum cast RVA 0x4B1F00.
            return (AccessoryCategory)index;
        }
		// Recovered original constructor RVA 0x4F3190.
		public InventoryPanelView()
		{
			itemViewList = new List<InventoryItemView>();
			adultGoodsViewList = new List<InventoryEquipItemView>();
		}

	}
}
