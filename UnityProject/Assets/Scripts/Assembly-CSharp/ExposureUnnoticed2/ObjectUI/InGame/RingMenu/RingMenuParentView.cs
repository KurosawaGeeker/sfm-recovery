using System.Collections.Generic;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InGame.RingMenu
{
	public class RingMenuParentView : MonoBehaviour
	{
		public enum RingMenuType
		{
			Common = 0,
			Skill = 1,
			Skill3 = 2,
			Skill4 = 3,
			Skill5 = 4,
			Skill6 = 5,
			Skill7 = 6,
			Skill8 = 7
		}

		public enum RingMenuOverrideType
		{
			None = 0,
			DildoFloorPussy = 1,
			DildoFloorAnal = 2,
			DildoFloorFella = 3,
			DildoWallPussy = 4,
			DildoWallAnal = 5,
			DildoWallFella = 6,
			SittingChair = 7
		}

		public enum RingItemType
		{
			None = 0,
			Pee = 1,
			TakeOffBra = 2,
			TakeOffPants = 3,
			TestSwitchVibrator = 4,
			Drop = 5,
			DropHandcuffKey = 6,
			DropVibeRemocon = 7,
			AutoRun = 8,
			AutoChase = 9,
			TimeStop = 10,
			PistonSwitch = 11,
			Dildo = 12,
			Handcuff = 13,
			EyeMask = 14,
			FirstPoseSkill = 10000
		}

		[SerializeField]
		private GameObject ringMenuPrefab;

		[SerializeField]
		private Transform menuParent;

		[SerializeField]
		private CanvasGroup labelCanvasGroup;

		[SerializeField]
		private CanvasGroup descriptionCanvasGroup;

		[SerializeField]
		private ShortcutItemView[] shortcutItemViews;

		[SerializeField]
		private GameObject shortcutItemParent;

		private bool lockOpen;

		private int currentSelectRingIndex;

		private RingMenuType currentOpenType;

		private List<RingMenuView> openRingList = new List<RingMenuView>();

		private RingMenuItemModel lastInvokeModelCommon;

		private RingMenuItemModel lastInvokeModelSkill;

		private RingMenuItemModel lastInvokeModelSkill3;

		private RingMenuItemModel lastInvokeModelSkill4;

		private RingMenuItemModel lastInvokeModelSkill5;

		private RingMenuItemModel lastInvokeModelSkill6;

		private RingMenuItemModel lastInvokeModelSkill7;

		private RingMenuItemModel lastInvokeModelSkill8;

		private SmoothFloat labelAlpha = new SmoothFloat(0.2f);

		private SmoothFloat descriptionAlpha = new SmoothFloat(0.2f);

		private int maxRingIndexCommon;

		private int maxRingIndexSkill;

		public bool IsClickToAction;

		private RingMenuItemModel[] commonRingMenuModel = new RingMenuItemModel[]
        {
            new RingMenuItemModel { Index = 0, RingItemType = RingItemType.AutoRun, NameKey = "ui_ring_menu_common_auto_run" },
            new RingMenuItemModel { Index = 1, RingItemType = RingItemType.AutoChase, NameKey = "ui_ring_menu_common_chase" },
            new RingMenuItemModel { Index = 2, RingItemType = RingItemType.Dildo, NameKey = "m_item_dildo", ReleaseCondition = () =>
            {
                var release = global::ExposureUnnoticed2.Master.RankRelease.MRankRelease.Get((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)112);
                var rank = global::ExposureUnnoticed2.Master.Rank.MRank.GetByRank(release.ReleaseRank);
                if (rank is null) throw new System.NullReferenceException();
                var state = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
                if (state is null) throw new System.NullReferenceException();
                int experience = state.ExposeExp;
                return rank.NeedRp <= experience;
            } },
            new RingMenuItemModel { Index = 3, RingItemType = RingItemType.TestSwitchVibrator, NameKey = "m_item_vibe_switch", ReleaseCondition = () => global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringItem((global::ExposureUnnoticed2.Master.Item.ItemType)10001) },
            new RingMenuItemModel { Index = 4, RingItemType = RingItemType.Handcuff, NameKey = "m_adult_goods_handcuff", ReleaseCondition = () => global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringAdultGoods((global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoodsType)3) },
            new RingMenuItemModel { Index = 5, RingItemType = RingItemType.PistonSwitch, NameKey = "ui_ring_menu_common_piston_switch", ReleaseCondition = () => global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringAdultGoods((global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoodsType)8) || global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringAdultGoods((global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoodsType)9) || global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringAdultGoods((global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoodsType)10) },
            new RingMenuItemModel { Index = 6, RingItemType = RingItemType.TimeStop, NameKey = "m_skill_time_stop", ReleaseCondition = () => global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)17) },
            new RingMenuItemModel { Index = 7, RingItemType = RingItemType.Pee, NameKey = "ui_tutorial_pee", ReleaseCondition = () => !global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.PeeDisable }
        };

		private RingMenuItemModel[] poseRingMenuModel;

		private RingMenuItemModel[] dildoFloorPussyRingMenuModel;

		private RingMenuItemModel[] dildoWallPussyRingMenuModel;

		private RingMenuItemModel[] sittingRingMenuModel;

		private RingMenuOverrideType currentOverrideType;

		public static RingMenuParentView Instance { get; private set; }

		public int CurrentRingIndexCommon { get; private set; }

		public int CurrentRingIndexSkill { get; private set; }

		public int CurrentRingIndexSkill3 { get; private set; }

		public int CurrentRingIndexSkill4 { get; private set; }

		public int CurrentRingIndexSkill5 { get; private set; }

		public int CurrentRingIndexSkill6 { get; private set; }

		public int CurrentRingIndexSkill7 { get; private set; }

		public int CurrentRingIndexSkill8 { get; private set; }

		public bool IsOpenRing { get; private set; }

		public void SetOverride(RingMenuOverrideType type)
		{
            if (currentOverrideType != type)
            {
                var options = global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData;
                if (options is null) throw new System.NullReferenceException();
                if (options.RingMenuMessage)
                    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(
                        Localizer.Trans("ui_ring_menu_change_1"),
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType.Log);
                var shortcuts = shortcutItemViews;
                if (shortcuts is null) throw new System.NullReferenceException();
                var shortcut = shortcuts[0];
                if (shortcut is null) throw new System.NullReferenceException();
                if (type == RingMenuOverrideType.None)
                {
                    shortcut.Initialize(RingMenuType.Common, commonRingMenuModel);
                    var state = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
                    if (state is null) throw new System.NullReferenceException();
                    if (state.RingShortcutModelIndexCommon == -1)
                        lastInvokeModelCommon = null;
                    else
                    {
                        var models = commonRingMenuModel;
                        if (models is null) throw new System.NullReferenceException();
                        lastInvokeModelCommon = models[state.RingShortcutModelIndexCommon];
                    }
                }
                else
                    shortcut.SetName(null);
            }
            currentOverrideType = type;
        }

		public void Initialize()
		{
            // Complete original native 4FDDF0..50046E; original generated callbacks retained.
            Instance = this;
            ExternalMethodManager.DestroyChildren(menuParent);
            labelAlpha.Current = 0f;
            labelCanvasGroup.alpha = 0f;
            descriptionAlpha.Current = 0f;
            descriptionCanvasGroup.alpha = 0f;
            var pose = new List<RingMenuItemModel>();
            for (int i = 0; i < 23; i++)
            {
                var record = global::ExposureUnnoticed2.Master.Skill.MSkill.GetByIndex(i);
                global::ExposureUnnoticed2.Master.Skill.SkillType type = record.Type;
                var model = new RingMenuItemModel
                {
                    Index = i + 8,
                    RingItemType = (RingItemType)(i + 10000),
                    NameKey = Localizer.Trans(record.NameKey),
                    ReleaseCondition = () => global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetSkillLevel(type) > 0
                };
                pose.Add(model);
            }
            var all = global::System.Linq.Enumerable.ToList(commonRingMenuModel);
            foreach (var model in pose) all.Add(model);
            commonRingMenuModel = all.ToArray();
            maxRingIndexSkill = (commonRingMenuModel.Length - 1) / 8;
            dildoFloorPussyRingMenuModel = new RingMenuItemModel[]
            {
                new RingMenuItemModel { NameKey = Localizer.Trans("動きを止める"), OnSelect = () => { } },
                new RingMenuItemModel { NameKey = Localizer.Trans("膣アクション" + (1).ToString()), OnSelect = () => { } },
                new RingMenuItemModel { NameKey = Localizer.Trans("膣アクション" + (2).ToString()), OnSelect = () => { } },
                new RingMenuItemModel { NameKey = Localizer.Trans("膣アクション" + (3).ToString()), OnSelect = () => { } },
                new RingMenuItemModel { NameKey = Localizer.Trans("膣アクション" + (4).ToString()), OnSelect = () => { } },
                new RingMenuItemModel { NameKey = Localizer.Trans("膣アクション" + (5).ToString()), OnSelect = () => { } }
            };
            dildoWallPussyRingMenuModel = new RingMenuItemModel[]
            {
                new RingMenuItemModel { NameKey = Localizer.Trans("動きを止める"), OnSelect = () =>
                {
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransActionIfNewNext((global::ExposureUnnoticed2.Master.Action.ActionType)10063);
                } },
                new RingMenuItemModel { NameKey = Localizer.Trans("膣アクション" + (1).ToString()), OnSelect = () =>
                {
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransActionIfNewNext((global::ExposureUnnoticed2.Master.Action.ActionType)10045);
                } },
                new RingMenuItemModel { NameKey = Localizer.Trans("膣アクション" + (2).ToString()), OnSelect = () => { } },
                new RingMenuItemModel { NameKey = Localizer.Trans("膣アクション" + (3).ToString()), OnSelect = () => { } },
                new RingMenuItemModel { NameKey = Localizer.Trans("膣アクション" + (4).ToString()), OnSelect = () => { } },
                new RingMenuItemModel { NameKey = Localizer.Trans("膣アクション" + (5).ToString()), OnSelect = () => { } }
            };
            sittingRingMenuModel = new RingMenuItemModel[]
            {
                new RingMenuItemModel { NameKey = Localizer.Trans("ui_ring_sit_dildo_onani"), OnSelect = () =>
                {
                    if (!global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.IsSit) return;
                    if ((int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.CurrentAction.Type == 10101)
                    {
                        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransActionIfNewNext((global::ExposureUnnoticed2.Master.Action.ActionType)10026);
                    }
                    else
                    {
                        if ((int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.CurrentAction.Type != 10026)
                        {
                            global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_cant_use_hand_by_other"));
                            return;
                        }
                        if ((int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.LastChairSitController.CurrentDildoState == 0)
                        {
                            global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("ui_ring_sit_not_put_dildo"));
                            return;
                        }
                        if (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.IsEcstasyMotion)
                        {
                            global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_cant_use_hand_by_other"));
                            return;
                        }
                        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransActionIfNewNext((global::ExposureUnnoticed2.Master.Action.ActionType)10101);
                    }
                } },
                new RingMenuItemModel { NameKey = Localizer.Trans("ui_ring_sit_put_dildo"), OnSelect = () =>
                {
                    if (!global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.IsSit) return;
                    if (!global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringItem((global::ExposureUnnoticed2.Master.Item.ItemType)1))
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("ui_ring_sit_dildo_dont_bring"));
                        return;
                    }
                    if ((int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.LastChairSitController.CurrentDildoState != 0)
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("ui_ring_sit_already_put_dildo"));
                        return;
                    }
                    if ((int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.CurrentAction.Type == 10101)
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("ui_ring_sit_cant_doing_onani"));
                        return;
                    }
                    if (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.IsEcstasyMotion)
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_cant_use_hand_by_other"));
                        return;
                    }
                    if ((int)global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.Instance.TransNonUseItemId((global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.DropItemType)10000) == -1)
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("ui_dildo_limit"));
                        return;
                    }
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransActionIfNewNext((global::ExposureUnnoticed2.Master.Action.ActionType)10102);
                } },
                new RingMenuItemModel { NameKey = Localizer.Trans("ui_ring_sit_take_dildo"), OnSelect = () =>
                {
                    if (!global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.IsSit) return;
                    if ((int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.LastChairSitController.CurrentDildoState == 0)
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("ui_ring_sit_not_put_dildo"));
                        return;
                    }
                    if ((int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.CurrentAction.Type == 10101)
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("ui_ring_sit_cant_doing_onani"));
                        return;
                    }
                    if (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.IsEcstasyMotion)
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_cant_use_hand_by_other"));
                        return;
                    }
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransActionIfNewNext((global::ExposureUnnoticed2.Master.Action.ActionType)10103);
                } },
                new RingMenuItemModel { NameKey = Localizer.Trans("ui_ring_sit_move_dildo_pussy"), OnSelect = () =>
                {
                    if (!global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.IsSit) return;
                    if ((int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.LastChairSitController.CurrentDildoState == 0)
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("ui_ring_sit_not_put_dildo"));
                        return;
                    }
                    if ((int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.LastChairSitController.CurrentDildoState == 1)
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("ui_ring_sit_already_move_dildo"));
                        return;
                    }
                    if ((int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.CurrentAction.Type == 10101)
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("ui_ring_sit_cant_doing_onani"));
                        return;
                    }
                    if (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.IsEcstasyMotion)
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_cant_use_hand_by_other"));
                        return;
                    }
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransActionIfNewNext((global::ExposureUnnoticed2.Master.Action.ActionType)10105);
                } },
                new RingMenuItemModel { NameKey = Localizer.Trans("ui_ring_sit_move_dildo_anal"), OnSelect = () =>
                {
                    if (!global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.IsSit) return;
                    if ((int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.LastChairSitController.CurrentDildoState == 0)
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("ui_ring_sit_not_put_dildo"));
                        return;
                    }
                    if ((int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.LastChairSitController.CurrentDildoState == 2)
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("ui_ring_sit_already_move_dildo"));
                        return;
                    }
                    if ((int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.CurrentAction.Type == 10101)
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("ui_ring_sit_cant_doing_onani"));
                        return;
                    }
                    if (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.PlayerState.IsEcstasyMotion)
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_cant_use_hand_by_other"));
                        return;
                    }
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransActionIfNewNext((global::ExposureUnnoticed2.Master.Action.ActionType)10104);
                } }
            };
            shortcutItemViews[0].Initialize((RingMenuType)0, commonRingMenuModel);
            shortcutItemViews[1].Initialize((RingMenuType)1, commonRingMenuModel);
            shortcutItemViews[2].Initialize((RingMenuType)2, commonRingMenuModel);
            shortcutItemViews[3].Initialize((RingMenuType)3, commonRingMenuModel);
            shortcutItemViews[4].Initialize((RingMenuType)4, commonRingMenuModel);
            shortcutItemViews[5].Initialize((RingMenuType)5, commonRingMenuModel);
            shortcutItemViews[6].Initialize((RingMenuType)6, commonRingMenuModel);
            shortcutItemViews[7].Initialize((RingMenuType)7, commonRingMenuModel);
            if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexCommon != -1)
                lastInvokeModelCommon = commonRingMenuModel[global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexCommon];
            if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill != -1)
                lastInvokeModelSkill = commonRingMenuModel[global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill];
            if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill3 != -1)
                lastInvokeModelSkill3 = commonRingMenuModel[global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill3];
            if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill4 != -1)
                lastInvokeModelSkill4 = commonRingMenuModel[global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill4];
            if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill5 != -1)
                lastInvokeModelSkill5 = commonRingMenuModel[global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill5];
            if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill6 != -1)
                lastInvokeModelSkill6 = commonRingMenuModel[global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill6];
            if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill7 != -1)
                lastInvokeModelSkill7 = commonRingMenuModel[global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill7];
            if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill8 != -1)
                lastInvokeModelSkill8 = commonRingMenuModel[global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill8];
        
}

		public int CurrentRingIndex()
		{
            if (currentOpenType == RingMenuType.Common) return CurrentRingIndexCommon;
            if (currentOpenType == RingMenuType.Skill) return CurrentRingIndexSkill;
            if (currentOpenType == RingMenuType.Skill3) return CurrentRingIndexSkill3;
            if (currentOpenType == RingMenuType.Skill4) return CurrentRingIndexSkill4;
            if (currentOpenType == RingMenuType.Skill5) return CurrentRingIndexSkill5;
            if (currentOpenType == RingMenuType.Skill6) return CurrentRingIndexSkill6;
            if (currentOpenType == RingMenuType.Skill7) return CurrentRingIndexSkill7;
            if (currentOpenType == RingMenuType.Skill8) return CurrentRingIndexSkill8;
            return -1;
        }

		public int CurrentShortcutIndex()
		{
            int value;
            if (currentOpenType == RingMenuType.Common) value = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexCommon;
            else if (currentOpenType == RingMenuType.Skill) value = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill;
            else if (currentOpenType == RingMenuType.Skill3) value = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill3;
            else if (currentOpenType == RingMenuType.Skill4) value = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill4;
            else if (currentOpenType == RingMenuType.Skill5) value = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill5;
            else if (currentOpenType == RingMenuType.Skill6) value = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill6;
            else if (currentOpenType == RingMenuType.Skill7) value = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill7;
            else if (currentOpenType == RingMenuType.Skill8) value = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill8;
            else return 0;
            return value < 0 ? 0 : value;
        }

		private void LateUpdate()
		{
            if (!IsOpenRing)
            {
                descriptionAlpha.UpdateTo(0f);
                labelAlpha.UpdateTo(0f);
                if (!global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.IsOpenSomeUi
                    && !global::ExposureUnnoticed2.Scripts.InGame.SexManager.Instance.IsSexMode
                    && !lockOpen && !IsClickToAction
                    && (GetLongDown((InputManager.InputType)14) || GetLongDown((InputManager.InputType)23) || GetLongDown((InputManager.InputType)27) || GetLongDown((InputManager.InputType)28) || GetLongDown((InputManager.InputType)1018) || GetLongDown((InputManager.InputType)1019) || GetLongDown((InputManager.InputType)1020) || GetLongDown((InputManager.InputType)1021)))
                {
                    global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.BaseCameraController.IsLockCamera = true;
                    IsOpenRing = true;
                    shortcutItemParent.SetActive(true);
                    RingMenuType type = GetLongDown((InputManager.InputType)14) ? (RingMenuType)0 : GetLongDown((InputManager.InputType)23) ? (RingMenuType)1 : GetLongDown((InputManager.InputType)27) ? (RingMenuType)2 : GetLongDown((InputManager.InputType)28) ? (RingMenuType)3 : GetLongDown((InputManager.InputType)1018) ? (RingMenuType)4 : GetLongDown((InputManager.InputType)1019) ? (RingMenuType)5 : GetLongDown((InputManager.InputType)1020) ? (RingMenuType)6 : RingMenuType.Skill8;
                    OpenRingMenu(type);
                }
                if (!lockOpen && GetShortUp((InputManager.InputType)14) && lastInvokeModelCommon != null && lastInvokeModelCommon.OnSelect == null)
                {
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.InputResolver.ShiftRing = false;
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.InputResolver.RingShortcutInput = lastInvokeModelCommon.RingItemType;
                }
                if (!lockOpen && GetShortUp((InputManager.InputType)14) && lastInvokeModelCommon != null && lastInvokeModelCommon.OnSelect != null)
                    lastInvokeModelCommon.OnSelect();
                if (!lockOpen && GetShortUp((InputManager.InputType)23) && lastInvokeModelSkill != null && lastInvokeModelSkill.OnSelect == null)
                {
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.InputResolver.ShiftRing = false;
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.InputResolver.RingShortcutInput = lastInvokeModelSkill.RingItemType;
                }
                if (!lockOpen && GetShortUp((InputManager.InputType)27) && lastInvokeModelSkill3 != null && lastInvokeModelSkill3.OnSelect == null)
                {
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.InputResolver.ShiftRing = false;
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.InputResolver.RingShortcutInput = lastInvokeModelSkill3.RingItemType;
                }
                if (!lockOpen && GetShortUp((InputManager.InputType)28) && lastInvokeModelSkill4 != null && lastInvokeModelSkill4.OnSelect == null)
                {
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.InputResolver.ShiftRing = false;
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.InputResolver.RingShortcutInput = lastInvokeModelSkill4.RingItemType;
                }
                if (!lockOpen && GetShortUp((InputManager.InputType)1018) && lastInvokeModelSkill5 != null && lastInvokeModelSkill5.OnSelect == null)
                {
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.InputResolver.ShiftRing = true;
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.InputResolver.RingShortcutInput = lastInvokeModelSkill5.RingItemType;
                }
                if (!lockOpen && GetShortUp((InputManager.InputType)1019) && lastInvokeModelSkill6 != null && lastInvokeModelSkill6.OnSelect == null)
                {
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.InputResolver.ShiftRing = true;
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.InputResolver.RingShortcutInput = lastInvokeModelSkill6.RingItemType;
                }
                if (!lockOpen && GetShortUp((InputManager.InputType)1020) && lastInvokeModelSkill7 != null && lastInvokeModelSkill7.OnSelect == null)
                {
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.InputResolver.ShiftRing = true;
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.InputResolver.RingShortcutInput = lastInvokeModelSkill7.RingItemType;
                }
                if (!lockOpen && GetShortUp((InputManager.InputType)1021) && lastInvokeModelSkill8 != null && lastInvokeModelSkill8.OnSelect == null)
                {
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.InputResolver.ShiftRing = true;
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.InputResolver.RingShortcutInput = lastInvokeModelSkill8.RingItemType;
                }
                if (IsClickToAction && !InputManager.IsDown((InputManager.InputType)14) && !InputManager.IsDown((InputManager.InputType)23) && !InputManager.IsDown((InputManager.InputType)27) && !InputManager.IsDown((InputManager.InputType)28) && !InputManager.IsDown((InputManager.InputType)1018) && !InputManager.IsDown((InputManager.InputType)1019) && !InputManager.IsDown((InputManager.InputType)1020) && !InputManager.IsDown((InputManager.InputType)1021))
                    IsClickToAction = false;
            }
            else
            {
                descriptionAlpha.UpdateTo(InputManager.IsGamePad ? 0f : 1f);
                if ((currentOpenType == (RingMenuType)0 && !GetLongDown((InputManager.InputType)14)) ||
                    (currentOpenType == (RingMenuType)1 && !GetLongDown((InputManager.InputType)23)) ||
                    (currentOpenType == (RingMenuType)2 && !GetLongDown((InputManager.InputType)27)) ||
                    (currentOpenType == (RingMenuType)3 && !GetLongDown((InputManager.InputType)28)) ||
                    (currentOpenType == (RingMenuType)4 && !GetLongDown((InputManager.InputType)1018)) ||
                    (currentOpenType == (RingMenuType)5 && !GetLongDown((InputManager.InputType)1019)) ||
                    (currentOpenType == (RingMenuType)6 && !GetLongDown((InputManager.InputType)1020)) ||
                    (currentOpenType == (RingMenuType)7 && !GetLongDown((InputManager.InputType)1021))
                    || IsClickToAction
                    || global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.IsOpenSomeUi
                    || global::ExposureUnnoticed2.Scripts.InGame.SexManager.Instance.IsSexMode)
                {
                    global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.BaseCameraController.IsLockCamera = false;
                    IsOpenRing = false;
                    lockOpen = true;
                    shortcutItemParent.SetActive(global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.RightDownPanel);
                    foreach (var view in openRingList) view.SetShow(false);
                    Scheduler.DelayFlame(() =>
                    {
                        foreach (var view in openRingList) UnityEngine.Object.Destroy(view.gameObject);
                        openRingList.Clear();
                        lockOpen = false;
                    }, 18);
                }
                int ringCount = unchecked((currentOpenType == RingMenuType.Common ? maxRingIndexCommon : maxRingIndexSkill) + 1);
                if (currentOpenType == (RingMenuType)0)
                {
                    labelAlpha.UpdateTo(maxRingIndexCommon >= 1 ? 1f : 0f);
                    if (InputManager.IsDownFrame((InputManager.InputType)1010)) CurrentRingIndexCommon = unchecked(CurrentRingIndexCommon - 1);
                    if (InputManager.IsDownFrame((InputManager.InputType)1011)) CurrentRingIndexCommon = unchecked(CurrentRingIndexCommon + 1);
                    CurrentRingIndexCommon = unchecked(CurrentRingIndexCommon + ringCount) % ringCount;
                }
                if (currentOpenType == (RingMenuType)1)
                {
                    labelAlpha.UpdateTo(maxRingIndexSkill >= 1 ? 1f : 0f);
                    if (InputManager.IsDownFrame((InputManager.InputType)1010)) CurrentRingIndexSkill = unchecked(CurrentRingIndexSkill - 1);
                    if (InputManager.IsDownFrame((InputManager.InputType)1011)) CurrentRingIndexSkill = unchecked(CurrentRingIndexSkill + 1);
                    CurrentRingIndexSkill = unchecked(CurrentRingIndexSkill + ringCount) % ringCount;
                }
                if (currentOpenType == (RingMenuType)2)
                {
                    labelAlpha.UpdateTo(maxRingIndexSkill >= 1 ? 1f : 0f);
                    if (InputManager.IsDownFrame((InputManager.InputType)1010)) CurrentRingIndexSkill3 = unchecked(CurrentRingIndexSkill3 - 1);
                    if (InputManager.IsDownFrame((InputManager.InputType)1011)) CurrentRingIndexSkill3 = unchecked(CurrentRingIndexSkill3 + 1);
                    CurrentRingIndexSkill3 = unchecked(CurrentRingIndexSkill3 + ringCount) % ringCount;
                }
                if (currentOpenType == (RingMenuType)3)
                {
                    labelAlpha.UpdateTo(maxRingIndexSkill >= 1 ? 1f : 0f);
                    if (InputManager.IsDownFrame((InputManager.InputType)1010)) CurrentRingIndexSkill4 = unchecked(CurrentRingIndexSkill4 - 1);
                    if (InputManager.IsDownFrame((InputManager.InputType)1011)) CurrentRingIndexSkill4 = unchecked(CurrentRingIndexSkill4 + 1);
                    CurrentRingIndexSkill4 = unchecked(CurrentRingIndexSkill4 + ringCount) % ringCount;
                }
                if (currentOpenType == (RingMenuType)4)
                {
                    labelAlpha.UpdateTo(maxRingIndexSkill >= 1 ? 1f : 0f);
                    if (InputManager.IsDownFrame((InputManager.InputType)1010)) CurrentRingIndexSkill5 = unchecked(CurrentRingIndexSkill5 - 1);
                    if (InputManager.IsDownFrame((InputManager.InputType)1011)) CurrentRingIndexSkill5 = unchecked(CurrentRingIndexSkill5 + 1);
                    CurrentRingIndexSkill5 = unchecked(CurrentRingIndexSkill5 + ringCount) % ringCount;
                }
                if (currentOpenType == (RingMenuType)5)
                {
                    labelAlpha.UpdateTo(maxRingIndexSkill >= 1 ? 1f : 0f);
                    if (InputManager.IsDownFrame((InputManager.InputType)1010)) CurrentRingIndexSkill6 = unchecked(CurrentRingIndexSkill6 - 1);
                    if (InputManager.IsDownFrame((InputManager.InputType)1011)) CurrentRingIndexSkill6 = unchecked(CurrentRingIndexSkill6 + 1);
                    CurrentRingIndexSkill6 = unchecked(CurrentRingIndexSkill6 + ringCount) % ringCount;
                }
                if (currentOpenType == (RingMenuType)6)
                {
                    labelAlpha.UpdateTo(maxRingIndexSkill >= 1 ? 1f : 0f);
                    if (InputManager.IsDownFrame((InputManager.InputType)1010)) CurrentRingIndexSkill7 = unchecked(CurrentRingIndexSkill7 - 1);
                    if (InputManager.IsDownFrame((InputManager.InputType)1011)) CurrentRingIndexSkill7 = unchecked(CurrentRingIndexSkill7 + 1);
                    CurrentRingIndexSkill7 = unchecked(CurrentRingIndexSkill7 + ringCount) % ringCount;
                }
                if (currentOpenType == (RingMenuType)7)
                {
                    labelAlpha.UpdateTo(maxRingIndexSkill >= 1 ? 1f : 0f);
                    if (InputManager.IsDownFrame((InputManager.InputType)1010)) CurrentRingIndexSkill8 = unchecked(CurrentRingIndexSkill8 - 1);
                    if (InputManager.IsDownFrame((InputManager.InputType)1011)) CurrentRingIndexSkill8 = unchecked(CurrentRingIndexSkill8 + 1);
                    CurrentRingIndexSkill8 = unchecked(CurrentRingIndexSkill8 + ringCount) % ringCount;
                }
            }
            labelCanvasGroup.alpha = labelAlpha.Current;
            descriptionCanvasGroup.alpha = descriptionAlpha.Current;
            shortcutItemViews[0].OnLateUpdate();
            shortcutItemViews[1].OnLateUpdate();
            shortcutItemViews[2].OnLateUpdate();
            shortcutItemViews[3].OnLateUpdate();
            shortcutItemViews[4].OnLateUpdate();
            shortcutItemViews[5].OnLateUpdate();
            shortcutItemViews[6].OnLateUpdate();
            shortcutItemViews[7].OnLateUpdate();
        }

		private bool GetLongDown(InputManager.InputType type)
		{
            if (!InputManager.IsGamePad || ((uint)((int)type - 1018) > 2u && (int)type != 1021))
                return InputManager.GetLongDown(type, 10);
            if (!InputManager.IsDown((InputManager.InputType)34)) return false;
            InputManager.InputType mapped;
            if ((int)type == 1018) mapped = (InputManager.InputType)8;
            else if ((int)type == 1019) mapped = (InputManager.InputType)7;
            else if ((int)type == 1020) mapped = (InputManager.InputType)3;
            else mapped = (InputManager.InputType)6;
            return InputManager.GetLongDown(mapped, 10);
        }

		private bool GetShortUp(InputManager.InputType type)
		{
            if (!InputManager.IsGamePad || ((uint)((int)type - 1018) > 2u && (int)type != 1021))
                return InputManager.GetShortUp(type, 20);
            if (!InputManager.IsDown((InputManager.InputType)34)) return false;
            InputManager.InputType mapped;
            if ((int)type == 1018) mapped = (InputManager.InputType)8;
            else if ((int)type == 1019) mapped = (InputManager.InputType)7;
            else if ((int)type == 1020) mapped = (InputManager.InputType)3;
            else mapped = (InputManager.InputType)6;
            return InputManager.GetShortUp(mapped, 20);
        }

		private void OpenRingMenu(RingMenuType type)
		{
            currentOpenType = type;
            if (type == RingMenuType.Common)
            {
                RingMenuItemModel[] models = currentOverrideType switch
                {
                    RingMenuOverrideType.None => commonRingMenuModel,
                    RingMenuOverrideType.DildoFloorPussy => dildoFloorPussyRingMenuModel,
                    RingMenuOverrideType.DildoFloorAnal => commonRingMenuModel,
                    RingMenuOverrideType.DildoFloorFella => commonRingMenuModel,
                    RingMenuOverrideType.DildoWallPussy => dildoWallPussyRingMenuModel,
                    RingMenuOverrideType.DildoWallAnal => commonRingMenuModel,
                    RingMenuOverrideType.DildoWallFella => commonRingMenuModel,
                    RingMenuOverrideType.SittingChair => sittingRingMenuModel
                };
                maxRingIndexCommon = (models.Length - 1) / 8;
                for (int i = 0; i <= maxRingIndexCommon; i++)
                {
                    int index = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexCommon / 8;
                    CurrentRingIndexCommon = index < 0 ? 0 : index;
                    var view = UnityEngine.Object.Instantiate(ringMenuPrefab, menuParent).GetComponent<RingMenuView>();
                    view.Initialize(this, models, model =>
                    {
                        shortcutItemViews[0].SetName(model);
                        lastInvokeModelCommon = model;
                        if (model != null) global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexCommon = model.Index;
                    }, i);
                    openRingList.Add(view);
                }
            }
            else
            {
                for (int i = 0; i <= maxRingIndexSkill; i++)
                {
                    switch (type)
                    {
                        case RingMenuType.Skill:
                        {
                            int index = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill / 8;
                            CurrentRingIndexSkill = index < 0 ? 0 : index;
                            break;
                        }
                        case RingMenuType.Skill3:
                        {
                            int index = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill3 / 8;
                            CurrentRingIndexSkill3 = index < 0 ? 0 : index;
                            break;
                        }
                        case RingMenuType.Skill4:
                        {
                            int index = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill4 / 8;
                            CurrentRingIndexSkill4 = index < 0 ? 0 : index;
                            break;
                        }
                        case RingMenuType.Skill5:
                        {
                            int index = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill5 / 8;
                            CurrentRingIndexSkill5 = index < 0 ? 0 : index;
                            break;
                        }
                        case RingMenuType.Skill6:
                        {
                            int index = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill6 / 8;
                            CurrentRingIndexSkill6 = index < 0 ? 0 : index;
                            break;
                        }
                        case RingMenuType.Skill7:
                        {
                            int index = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill7 / 8;
                            CurrentRingIndexSkill7 = index < 0 ? 0 : index;
                            break;
                        }
                        case RingMenuType.Skill8:
                        {
                            int index = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill8 / 8;
                            CurrentRingIndexSkill8 = index < 0 ? 0 : index;
                            break;
                        }
                    }
                    var view = UnityEngine.Object.Instantiate(ringMenuPrefab, menuParent).GetComponent<RingMenuView>();
                    view.Initialize(this, commonRingMenuModel, model =>
                    {
                        int slot = type switch
                        {
                            RingMenuType.Skill => 1,
                            RingMenuType.Skill3 => 2,
                            RingMenuType.Skill4 => 3,
                            RingMenuType.Skill5 => 4,
                            RingMenuType.Skill6 => 5,
                            RingMenuType.Skill7 => 6,
                            RingMenuType.Skill8 => 7
                        };
                        shortcutItemViews[slot].SetName(model);
                        switch (type)
                        {
                            case RingMenuType.Skill:
                                lastInvokeModelSkill = model;
                                if (model != null) global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill = model.Index;
                                break;
                            case RingMenuType.Skill3:
                                lastInvokeModelSkill3 = model;
                                if (model != null) global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill3 = model.Index;
                                break;
                            case RingMenuType.Skill4:
                                lastInvokeModelSkill4 = model;
                                if (model != null) global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill4 = model.Index;
                                break;
                            case RingMenuType.Skill5:
                                lastInvokeModelSkill5 = model;
                                if (model != null) global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill5 = model.Index;
                                break;
                            case RingMenuType.Skill6:
                                lastInvokeModelSkill6 = model;
                                if (model != null) global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill6 = model.Index;
                                break;
                            case RingMenuType.Skill7:
                                lastInvokeModelSkill7 = model;
                                if (model != null) global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill7 = model.Index;
                                break;
                            case RingMenuType.Skill8:
                                lastInvokeModelSkill8 = model;
                                if (model != null) global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill8 = model.Index;
                                break;
                        }
                    }, i);
                    openRingList.Add(view);
                }
            }
        }

		private void OnInvokeCommandCommon(RingMenuItemModel model)
		{
		}
	}
}
