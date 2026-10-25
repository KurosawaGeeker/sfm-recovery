using System;
using ExposureUnnoticed2.Master.Item;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InventoryPanel
{
	public class InventoryItemView : MonoBehaviour
	{
		[SerializeField]
		private new TextMeshProUGUI name;

		[SerializeField]
		private TextMeshProUGUI num;

		[SerializeField]
		private GameObject useButtonObject;

		[SerializeField]
		private GameObject dropButtonObject;

		[SerializeField]
		private GameObject descriptionPanel;

		[SerializeField]
		private TextMeshProUGUI description;

		[SerializeField]
		private GameObject infoIconObject;

		[SerializeField]
		private CanvasGroup emitCanvasGroup;

		private Action<MItemRecord> onClick;

		private Action<MItemRecord> onClickDrop;

		private MItemRecord master;

		private ButtonGroupManager.IButtonView selfButtonView;

		private bool isViewDescription;

		private bool isExistDescription;

		public void Initialize(MItemRecord master, int num, Action<MItemRecord> onClick, Action<MItemRecord> onClickDrop)
		{
            // Recovered complete original RVA 0x4E9DD0.
            this.master = master;
            selfButtonView = GetComponent<ButtonGroupManager.IButtonView>();
            name.text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(master.NameKey);
            this.num.text = "x" + num.ToString();
            this.onClick = onClick;
            this.onClickDrop = onClickDrop;
            useButtonObject.SetActive(!master.IsValuables);
            dropButtonObject.SetActive(master.CanDrop);
            this.num.gameObject.SetActive(!master.IsValuables && !master.IsReusable);
            isExistDescription = !string.IsNullOrEmpty(master.DescriptionKey);
            descriptionPanel.SetActive(false);
            if (!isExistDescription) infoIconObject.SetActive(false);
            else
            {
                description.text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(master.DescriptionKey);
                infoIconObject.SetActive(true);
            }
        }

		public void ForceSetEmit(int alpha)
		{
            // Recovered complete original RVA 0x4E97D0.
            emitCanvasGroup.alpha = alpha;
        }

		private void Update()
		{
            // Recovered complete original linked-unwind family RVA 0x4EA2C0.
            if (!isExistDescription) return;
            if (isViewDescription == selfButtonView.IsEmitting) return;
            isViewDescription = selfButtonView.IsEmitting;
            descriptionPanel.SetActive(isViewDescription);
        }

		public void CommonOnClickByPad()
		{
            // Recovered complete original native body.
            if (useButtonObject.activeInHierarchy)
{
    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49, -1f, false);
    onClick?.Invoke(master);
}
else if (dropButtonObject.activeInHierarchy)
{
    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49, -1f, false);
    onClickDrop?.Invoke(master);
}
        }

		public void OnClickUse()
		{
            // Recovered complete original native body.
            onClick?.Invoke(master);
        }

		public void OnClickDrop()
		{
            // Recovered complete original native body.
            onClickDrop?.Invoke(master);
        }
	}
}
