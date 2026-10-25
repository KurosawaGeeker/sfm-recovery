using System;
using System.Collections.Generic;
using ExposureUnnoticed2.Scripts.Base;
using HadakaCoat.ObjectsUi.Common.Button;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.CommonUI.CommonTab
{
	public class CommonTabView : MonoBehaviour
	{
		[SerializeField]
		private Transform itemParent;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		[SerializeField]
		private bool isSecondaryKey;

		private Action<int> onClick;

		private int buttonNum;

		private List<ButtonView> buttonList;

		private Func<bool> isStopUpdate;

		public int CurrentSelect { get; private set; }

		private InputManager.InputType TabLeft => (InputManager.InputType)(isSecondaryKey ? 24 : 20);

		private InputManager.InputType TabRight => (InputManager.InputType)(isSecondaryKey ? 25 : 21);

		public void Initialize(Action<int> onClick, Func<bool> isStopUpdate = null)
		{
            // Recovered complete original RVA 0x577190.
            this.onClick = onClick;
            this.isStopUpdate = isStopUpdate;
            buttonNum = unchecked(transform.childCount - 2);
            foreach (Transform child in transform)
            {
                if (!child.gameObject.activeInHierarchy) buttonNum = unchecked(buttonNum - 1);
            }
            foreach (Transform child in itemParent)
            {
                ButtonView button = child.GetComponent<ButtonView>();
                if (button != null && child.gameObject.activeInHierarchy) buttonList.Add(button);
            }
            buttonGroupManager.ForceSelect(0);
        }

		public void OnClickTab(int index)
		{
            // Recovered complete original RVA 0x577950.
            CurrentSelect = index;
            onClick?.Invoke(index);
            buttonGroupManager.ForceSelect(index);
            global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)48, -1f, false);
        }

		private void Update()
		{
            // Recovered complete original five-record linked-unwind family RVA 0x577A10.
            if (isStopUpdate != null && isStopUpdate()) return;
            int oldIndex = CurrentSelect;
            if (InputManager.IsDownFrame(TabLeft)) CurrentSelect = unchecked(CurrentSelect - 1);
            if (InputManager.IsDownFrame(TabRight)) CurrentSelect = unchecked(CurrentSelect + 1);
            CurrentSelect = Mathf.Clamp(CurrentSelect, 0, unchecked(buttonNum - 1));
            if (oldIndex == CurrentSelect) return;
            OnClickTab(CurrentSelect);
            buttonGroupManager.ForceSelect(buttonList[CurrentSelect]);
        }
		// Recovered original constructor RVA 0x577B70.
		public CommonTabView()
		{
			buttonList = new List<ButtonView>();
		}

	}
}
