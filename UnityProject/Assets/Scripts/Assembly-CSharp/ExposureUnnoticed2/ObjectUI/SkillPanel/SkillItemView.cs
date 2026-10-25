using ExposureUnnoticed2.Master.Skill.SkillMental;
using ExposureUnnoticed2.Master.Skill.SkillSlow;
using ExposureUnnoticed2.Master.Skill.SkillMaxAccessoryNum;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using ExposureUnnoticed2.Scripts.Sound;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.ObjectUI.InGame.MessagePanel;
using ExposureUnnoticed2.ObjectUI.TutorialPanel;
using SeType = ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType;
using System;
using ExposureUnnoticed2.Master.Skill;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.SkillPanel
{
	public class SkillItemView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI nameText;

		[SerializeField]
		private TextMeshProUGUI levelText;

		[SerializeField]
		private TextMeshProUGUI necessaryRpText;

		[SerializeField]
		private TextMeshProUGUI buttonText;

		[SerializeField]
		private GameObject reinforceButtonObject;

		[SerializeField]
		private GameObject conditionMask;

		[SerializeField]
		private TextMeshProUGUI rankConditionText;

		[SerializeField]
		private TextMeshProUGUI orderConditionText;

		[SerializeField]
		private GameObject learnedText;

		[SerializeField]
		private GameObject enableCheckMark;

		[SerializeField]
		private CanvasGroup emitCanvasGroup;

		private int necessaryRp;

		private MSkillRecord skill;

		private Action<MSkillRecord> onClickReinforce;

		private Action<MSkillRecord> onClick;

		private bool isSomeMask;

		private bool isLearned;

		public void Initialize(MSkillRecord skill, Action<MSkillRecord> onClick, Action<MSkillRecord> onClickReinforce)
		{
            // Original RVA 0x4C3990.
            this.skill = skill;
            this.onClick = onClick;
            this.onClickReinforce = onClickReinforce;
            nameText.text = Localizer.Trans(skill.NameKey);
            Refresh();
            UpdateMask();
		}

		public void Dispose()
		{
		}

		public void ForceSetEmit(int alpha)
		{
            // Original RVA 0x4C3B00.
            emitCanvasGroup.alpha = alpha;
		}

		public void UpdateMask()
		{
            // Original RVA 0x4C3B70.
            bool rankLocked = PlayerController.Instance.Pca.PlayerState.CurrentRank.Rank < skill.ReleaseRank;
            bool orderLocked = skill.ReleaseOrderTarget != SkillType.None && GameStateFacade.GetSkillLevel(skill.ReleaseOrderTarget) < 1;
            isSomeMask = rankLocked || orderLocked;
            conditionMask.SetActive(isSomeMask);
            rankConditionText.gameObject.SetActive(rankLocked);
            orderConditionText.gameObject.SetActive(orderLocked);
            if (rankLocked)
                rankConditionText.text = Localizer.Trans("ui_release_at_rank", GameStateFacade.GetRankNameAutoHide(skill.ReleaseRank));
            if (orderLocked)
                orderConditionText.text = Localizer.Trans("ui_skill_release_by_learn", Localizer.Trans(MSkill.Get(skill.ReleaseOrderTarget).NameKey));
            rankConditionText.alignment = rankLocked && orderLocked ? TextAlignmentOptions.Top : TextAlignmentOptions.Center;
            orderConditionText.alignment = rankLocked && orderLocked ? TextAlignmentOptions.Bottom : TextAlignmentOptions.Center;
		}

		private void Refresh()
		{
            // Original RVA 0x4C4020.
            int level = GameStateFacade.GetSkillLevel(skill.Type);
            necessaryRp = unchecked(level * skill.IncrementRp + skill.InitRp);
            if (skill.MaxLevel != 1)
            {
                necessaryRp = skill.Type switch
                {
                    SkillType.Mental => MSkillMental.Get(level).NextLevelUpRp,
                    SkillType.Stamina => MSkillStamina.Get(level).NextLevelUpRp,
                    SkillType.Slow => MSkillSlow.Get(level).NextLevelUpRp,
                    SkillType.MaxAccessoryNum => MSkillMaxAccessoryNum.Get(level).NextLevelUpRp,
                };
            }
            necessaryRpText.text = necessaryRp.ToString();
            nameText.alignment = skill.MaxLevel == 1 ? TextAlignmentOptions.Left : TextAlignmentOptions.TopLeft;
            levelText.gameObject.SetActive(skill.MaxLevel != 1);
            buttonText.text = Localizer.Trans(level == 0 ? "ui_common_learn" : "ui_common_reinforce");
            if (level < skill.MaxLevel)
                levelText.text = "Lv." + unchecked(level + 1).ToString();
            else
            {
                reinforceButtonObject.SetActive(false);
                necessaryRpText.gameObject.SetActive(false);
                levelText.text = "Lv." + level.ToString();
            }
            isLearned = level >= skill.MaxLevel;
            learnedText.SetActive(isLearned);
            enableCheckMark.SetActive(skill.IsPassiveToggle && GameStateFacade.IsActivePassiveSkill(skill.Type));
		}

		public void OnClick()
		{
            // Original RVA 0x4C4710.
            onClick?.Invoke(skill);
		}

		public void OnClickReinforce()
		{
            // Original RVA 0x4C4740.
            if (isSomeMask || isLearned) return;
            if (GameStateFacade.IsEnoughRp(necessaryRp))
            {
                UiSoundPlayer.Instance.PlayOneShotAudioData((SeType)49);
                GameStateFacade.ConsumeRp(necessaryRp);
                GameStateFacade.RiseSkillLevel(skill.Type);
                Refresh();
                onClickReinforce?.Invoke(skill);
                Events.SkillUpEvent.Type = skill.Type;
                EventManager.Broadcast(Events.SkillUpEvent);
                if (skill.Type == SkillType.TimeStop)
                    TutorialPanelView.Open(TutorialPanelView.TutorialOpenKind.FirstTimeStop);
            }
            else MessagePanelView.AddMessage(Localizer.Trans("ui_common_not_enough_rp"));
		}
	}
}
