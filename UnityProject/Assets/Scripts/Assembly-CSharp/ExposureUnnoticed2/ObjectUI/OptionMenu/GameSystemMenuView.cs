using System;
using Common.CommonUI.CommonSettingItem;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.OptionMenu
{
	public class GameSystemMenuView : MonoBehaviour, IOptionMiddlePanel
	{
		[SerializeField]
		private OptionItemCheckView futanariDisable;

		[SerializeField]
		private OptionItemCheckView peeDisable;

		[SerializeField]
		private OptionItemCheckView sexDisable;

		[SerializeField]
		private OptionItemCheckView futanariFemaleClimaxDisable;

		[SerializeField]
		private OptionItemCheckView interactEffectInvisible;

		[SerializeField]
		private OptionItemCheckView redWallEffectInvisible;

		[SerializeField]
		private OptionItemBarView maleRate;

		[SerializeField]
		private OptionItemBarView oldPeopleRate;

		[SerializeField]
		private OptionItemCheckView blackCensor;

		[SerializeField]
		private OptionItemBarView blackCensorSize;

		[SerializeField]
		private OptionItemCheckView ekitaiDisable;

		[SerializeField]
		private OptionItemCheckView vibeStateColor;

		[SerializeField]
		private OptionItemBarView mallDayTimeNpcNumScale;

		[SerializeField]
		private TextMeshProUGUI mallDayTimeNpcNumScaleText;

		[SerializeField]
		private SettingSelectItemView hairType;

		[SerializeField]
		private OptionItemBarView faceRankBias;

		[SerializeField]
		private OptionItemCheckView faceRange;

		private Animator animator;

		private Action onClose;

		private Action<OptionItemType> onClickItem;

		public void Initialize(Action onClose, Action<OptionItemType> onClickItem)
		{
		}

		public void AllReset()
		{
		}

		private void Update()
		{
		}

		public void Close()
		{
		}

		public void OnAnimCloseEnd()
		{
		}
	}
}
