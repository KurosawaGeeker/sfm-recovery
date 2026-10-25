using System;
using ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.OptionMenu
{
	public class CustomizeBarItemView : MonoBehaviour, ICustomizeItemType
	{
		[SerializeField]
		private TextMeshProUGUI valueText;

		[SerializeField]
		private CustomizeItemType customizeItemType;

		[SerializeField]
		private GameObject adjustHeightMask;

		[SerializeField]
		private GameObject rankConditionMask;

		[SerializeField]
		private TextMeshProUGUI rankConditionMaskText;

		private Slider slider;

		private Action<int> onChange;

		private bool isInitializeSimple;

		private bool isAdjustHeight;

		private bool isSkipOnChange;

		private bool isPlayerCustom;

		public int Current => (int)slider.value;

		public CustomizeItemType CustomizeItemType => customizeItemType;

		public void Initialize(int initValue, Action<int> onChange, bool isAdjustHeight = false, bool isRankRelease = false)
		{
// Complete original body draft; native/CLR acceptance pending.

this.isAdjustHeight = isAdjustHeight;
isInitializeSimple = true;
this.onChange = onChange;
slider = GetComponentInChildren<Slider>();
slider.value = initValue;
OnChangeSlider();
bool locked = false;
if (isRankRelease)
{
    var rank = global::ExposureUnnoticed2.Master.Rank.MRank.GetByRank(global::ExposureUnnoticed2.Master.RankRelease.MRankRelease.Get((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)7).ReleaseRank);
    locked = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ExposeExp < rank.NeedRp;
}
rankConditionMask.SetActive(locked);
if (locked)
{
    var rank = global::ExposureUnnoticed2.Master.Rank.MRank.GetByRank(global::ExposureUnnoticed2.Master.RankRelease.MRankRelease.Get((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)7).ReleaseRank);
    var receiver = rankConditionMaskText;
    var caption = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_release_at_rank", global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetRankNameAutoHide(rank.Rank));
    receiver.text = caption;
}

}

		public void InitializeForPlayerCustom(int initValue, Action<int> onChange, bool isAdjustHeight = false, bool isRankRelease = false)
		{
// Complete original body draft; native/CLR acceptance pending.
isPlayerCustom = true; Initialize(initValue, onChange, isAdjustHeight, isRankRelease);
}

		public void SetMax(int max)
		{
// Complete original body draft; native/CLR acceptance pending.
slider = GetComponentInChildren<Slider>(); isSkipOnChange = true; slider.maxValue = max; isSkipOnChange = false;
}

		private void Update()
		{
// Complete original body draft; native/CLR acceptance pending.
adjustHeightMask.SetActive(isAdjustHeight && global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeAdjustHeight);
}

		public void SetValueText(string text)
		{
// Complete original body draft; native/CLR acceptance pending.
valueText.text = text;
}

		public void OnChangeSlider()
		{
// Complete original body draft; native/CLR acceptance pending.
if (isSkipOnChange) return; if (isInitializeSimple) valueText.text = ((int)slider.value).ToString(); onChange?.Invoke((int)slider.value); if (isPlayerCustom) global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeBodyEvent);
}
	}
}
