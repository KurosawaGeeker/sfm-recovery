using System.Linq;
using ExposureUnnoticed2.Master.Skill.SkillSlow;
using ExposureUnnoticed2.Master.Skill.SkillMaxAccessoryNum;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using ExposureUnnoticed2.Scripts.Sound;
using ExposureUnnoticed2.Object3D.IngameManager;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Object3D.UIPlayer;
using SeType = ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType;
using System;
using System.Collections.Generic;
using Common.CommonUI.CommonScrollView;
using Common.Scripts.UI;
using ExposureUnnoticed2.Master.Skill;
using ExposureUnnoticed2.ObjectUI.CommonUI.CommonTab;
using ExposureUnnoticed2.ObjectUI.SkillPanel;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.SaveDataSelectPanel
{
	public class SkillPanelView : BasePanelView
	{
		[SerializeField]
		private Transform itemParent;

		[SerializeField]
		private GameObject skillItemPrefab;

		[SerializeField]
		private CommonTabView tabView;

		[SerializeField]
		private TextMeshProUGUI currentSkillName;

		[SerializeField]
		private TextMeshProUGUI currentSkillDescription;

		[SerializeField]
		private GameObject uiPlayerPanelObject;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		[SerializeField]
		private CommonScrollRectView commonScrollRectView;

		private new Action onClose;

		private List<SkillItemView> childViewList = new List<SkillItemView>();

		private int currentSelectIndex;

		private void Initialize(Action onClose)
		{
            // Original RVA 0x4E6630.
            this.onClose = onClose;
            uiPlayerPanelObject.SetActive(false);
            tabView.Initialize(OnClickTab);
            OnClickTab(0);
		}

		private void ToPadModeIfNeed()
		{
            // Original RVA 0x4E6780.
            if (InputManager.IsGamePad) buttonGroupManager.ForceSelect(currentSelectIndex);
		}

		private void LateUpdate()
		{
            // Original RVA 0x4E6850.
            if (IsActivePanel() && InputManager.IsGamePad)
            {
                if (InputManager.IsJustToGamePad) ToPadModeIfNeed();
                int offset = 0;
                if (InputManager.IsDownFrame(InputManager.InputType.UIUp)
                    || InputManager.IsDownLongRepeat(InputManager.InputType.UIUp, 20, 5)) offset = -1;
                if (InputManager.IsDownFrame(InputManager.InputType.UIDown)
                    || InputManager.IsDownLongRepeat(InputManager.InputType.UIDown, 20, 5)) offset++;
                if (offset != 0)
                {
                    childViewList[currentSelectIndex].ForceSetEmit(0);
                    currentSelectIndex = unchecked(buttonGroupManager.ButtonCount + currentSelectIndex + offset) % buttonGroupManager.ButtonCount;
                    childViewList[currentSelectIndex].ForceSetEmit(1);
                    buttonGroupManager.InvokeOnClick(currentSelectIndex);
                    commonScrollRectView.ScrollToCentering(childViewList[currentSelectIndex].gameObject);
                }
                if (InputManager.IsDownFrame(InputManager.InputType.UIDecide)) childViewList[currentSelectIndex].OnClickReinforce();
                if (InputManager.IsDownFrame(InputManager.InputType.Cancel)) OnClickBack();
            }
            else if (InputManager.IsDownFrame(InputManager.InputType.Cancel) && IsActivePanel()) OnClickBack();
		}

		private void OnClickTab(int index)
		{
            // Original RVA 0x4E6C70.
            ExternalMethodManager.DestroyChildren(itemParent);
            childViewList.Clear();
            MSkillRecord first = null;
            foreach (var pair in MSkill.GetAll().OrderBy(o => PlayerController.Instance.Pca.PlayerState.CurrentRank.Rank < o.Value.ReleaseRank))
            {
                MSkillRecord record = pair.Value;
                if (SystemManager.IsTrial && record.ReleaseRank != 0) continue;
                if ((int)record.CategoryType != index) continue;
                if (GameState.OptionData.FutanariDisable && record.Type == (SkillType)10019) continue;
                if (GameState.OptionData.SexDisable && record.Type == (SkillType)22) continue;
                SkillItemView view = Instantiate(skillItemPrefab, itemParent).GetComponent<SkillItemView>();
                view.Initialize(record, OnClickItem, OnClickReinforce);
                childViewList.Add(view);
                if (first == null) first = record;
            }
            OnClickItem(first);
            buttonGroupManager.ReSearchButton();
            Scheduler.DelayFlame(() =>
            {
                currentSelectIndex = 0;
                buttonGroupManager.ForceSelect(0);
                ToPadModeIfNeed();
            }, 1);
		}

		private void OnClickReinforce(MSkillRecord record)
		{
            // Original RVA 0x4E7490.
            OnClickItem(record);
            foreach (SkillItemView child in childViewList) child.UpdateMask();
            if (record.Type == SkillType.NoReinforceEffect) PlayerController.Instance.Pca.PlayerCosplayManager.CalcCostumeEffect();
		}

		private void OnClickItem(MSkillRecord record)
		{
            // Original RVA 0x4E7660.
            currentSkillName.text = Localizer.Trans(record.NameKey);
            string passiveNote = record.IsPassiveToggle ? Localizer.Trans("ui_skill_passive_extra_description") : string.Empty;
            string rpNote = string.Empty;
            if (record.RpBonus > 0) rpNote = "\n\n" + Localizer.Trans("ui_common_rp_bonus_plus_percent", record.RpBonus.ToString());
            else if (record.RpBonus < 0) rpNote = "\n\n" + Localizer.Trans("ui_common_rp_bonus_percent", record.RpBonus.ToString());
            string description = Localizer.Trans(record.DescriptionKey);
            if (record.Type == SkillType.Stamina)
            {
                var next = MSkillStamina.Get(Mathf.Min(unchecked(GameStateFacade.GetSkillLevel(record.Type) + 1), record.MaxLevel));
                description = Localizer.Trans(record.DescriptionKey, ((next.MaxStamina - 1f) * 100f).ToString());
            }
            else if (record.Type == SkillType.MaxAccessoryNum)
            {
                var next = MSkillMaxAccessoryNum.Get(Mathf.Min(unchecked(GameStateFacade.GetSkillLevel(record.Type) + 1), record.MaxLevel));
                description = Localizer.Trans(record.DescriptionKey, next.MaxNum.ToString());
            }
            else if (record.Type == SkillType.Slow)
            {
                var next = MSkillSlow.Get(Mathf.Min(unchecked(GameStateFacade.GetSkillLevel(record.Type) + 1), record.MaxLevel));
                description = Localizer.Trans(record.DescriptionKey, next.MaxTimeTank.ToString());
            }
            currentSkillDescription.text = description + passiveNote + rpNote;
            if (record.CategoryType == SkillCategoryType.Pose)
            {
                UiPlayerController.Instance.ShowPose(record.Type);
                uiPlayerPanelObject.SetActive(true);
            }
            else
            {
                UiPlayerController.Instance.Hide();
                uiPlayerPanelObject.SetActive(false);
            }
		}

		public void OnClickBack()
		{
            // Original RVA 0x4E7B10.
            Scheduler.DelayFlame(() => onClose?.Invoke(), 1);
            foreach (SkillItemView child in childViewList) child.Dispose();
            UiSoundPlayer.Instance.PlayOneShotAudioData((SeType)50);
            Close();
		}

		public static void Open(Action onClose)
		{
            // Original RVA 0x4E7D10.
            InGameUiManager.Instance.Open(AssetReferencer.Instance.SkillPanel).GetComponentInChildren<SkillPanelView>().Initialize(onClose);
		}
	}
}
