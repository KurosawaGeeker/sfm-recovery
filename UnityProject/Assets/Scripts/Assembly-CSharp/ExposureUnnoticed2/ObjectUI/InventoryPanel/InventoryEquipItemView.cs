using System;
using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.Scripts.Base;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InventoryPanel
{
	public class InventoryEquipItemView : MonoBehaviour
	{
		[SerializeField]
		private new TextMeshProUGUI name;

		[SerializeField]
		private ButtonView buttonView;

		[SerializeField]
		private GameObject optionButtonObject;

		[SerializeField]
		private GameObject HidePartsIcon;

		[SerializeField]
		private GameObject infoIcon;

		[SerializeField]
		private GameObject descriptionObject;

		[SerializeField]
		private TextMeshProUGUI descriptionText;

		[SerializeField]
		private CanvasGroup emitCanvasGroup;

		private Action<IEquipableMaster> onClickEquip;

		private Action<IEffectiveEquipRecord> onClickBody;

		private IEquipableMaster master;

		private Action<RCosplayParts> onClickOption;

		private bool isViewDescription;

		private bool isExistDescription;

		private ButtonGroupManager.IButtonView selfButtonView;

		public void Initialize(IEquipableMaster master, Action<IEquipableMaster> onClickEquip, Action<IEffectiveEquipRecord> onClickBody, Action<RCosplayParts> onClickOption)
		{
            // Recovered complete original native body.
            this.master = master;
            selfButtonView = GetComponent<ButtonGroupManager.IButtonView>();
            name.text = Localizer.Trans(master.NameKey);
            this.onClickEquip = onClickEquip;
            this.onClickBody = onClickBody;
            EventManager.AddListener<ChangeSomeEquipEvent>(OnChangeSomeEquipEvent, false);
            if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipped(master)) buttonView.Emit();
            optionButtonObject.SetActive(false);
            HidePartsIcon.SetActive(false);
            descriptionObject.SetActive(false);
            if (master is RCosplayParts part)
            {
                optionButtonObject.SetActive(part.IsShowCustomizeButton);
                this.onClickOption = onClickOption;
                HidePartsIcon.SetActive((int)part.HiddenPartsType != 0);
            }
            if (string.IsNullOrEmpty(master.DescriptionKey))
            {
                isExistDescription = false;
                infoIcon.SetActive(false);
            }
            else
            {
                isExistDescription = true;
                infoIcon.SetActive(true);
                descriptionText.text = Localizer.Trans(master.DescriptionKey);
            }
        }

		public void ForceSetEmit(int alpha)
		{
            // Original shared native4E97D0, distinct original Equip method identity.
            emitCanvasGroup.alpha = alpha;
        }

		private void Update()
		{
			if (!isExistDescription) return;
			if (isViewDescription == selfButtonView.IsEmitting) return;
			isViewDescription = selfButtonView.IsEmitting;
			descriptionObject.SetActive(isViewDescription);
		}

		public void RefreshEmit()
		{
			if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipped(master)) buttonView.Emit();
			else buttonView.Extinction();
		}

		public void OnClickOption()
		{
            // Recovered complete original native body.
            if (optionButtonObject.activeInHierarchy)
{
    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49, -1f, false);
    onClickOption?.Invoke(master as RCosplayParts);
}
        }

		public void OnClickEquip()
		{
            // Recovered complete original native body.
            if (global::ExposureUnnoticed2.Scripts.InGame.SlowAssistManager.Instance.currentState == global::ExposureUnnoticed2.Scripts.InGame.SlowAssistManager.State.Slow)
{
    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_cant_equip_in_slow"), global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType.Caution);
    return;
}
onClickEquip?.Invoke(master);
        }

		public void OnClickBody()
		{
            // Recovered complete original native body.
            onClickBody?.Invoke((IEffectiveEquipRecord)master);
        }

		private void OnDestroy()
		{
			EventManager.RemoveListener<ChangeSomeEquipEvent>(OnChangeSomeEquipEvent);
		}

		private void OnChangeSomeEquipEvent(ChangeSomeEquipEvent evt)
		{
			if (buttonView.gameObject != null) RefreshEmit();
		}
	}
}
