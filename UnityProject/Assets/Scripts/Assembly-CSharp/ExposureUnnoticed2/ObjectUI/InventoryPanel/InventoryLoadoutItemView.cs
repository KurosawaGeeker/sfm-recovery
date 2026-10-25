using System;
using HadakaCoat.ObjectsUi.Common.Button;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.InventoryPanel
{
	public class InventoryLoadoutItemView : MonoBehaviour
	{
		[SerializeField]
		private new Text name;

		[SerializeField]
		private CanvasGroup emitCanvasGroup;

		private Action<int> onClickItem;

		private Action<int> onClickEquip;

		private Action<int> onClickRegister;

		private int index;

		public ButtonView ButtonView { get; private set; }

		private string[] TargetNameData => global::ExposureUnnoticed2.ObjectUI.BarberMenu.ClosetMenuView.Instance != null ? global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CustomizeMySetNames : global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CosplayMySetNames;

		public void Initialize(int index, Action<int> onClickItem, Action<int> onClickEquip, Action<int> onClickRegister)
		{
            // Complete original RVA 0x4EA660; text receiver is captured before the original name-array getter.
            var text = name;
            this.index = index;
            text.text = TargetNameData[index];
            this.onClickItem = onClickItem;
            this.onClickEquip = onClickEquip;
            this.onClickRegister = onClickRegister;
            ButtonView = GetComponent<ButtonView>();
        }

		public void ForceSetEmit(int alpha)
		{
            // Complete original4EA870.
            emitCanvasGroup.alpha = alpha;
        }

		public void OnClickItem()
		{
            onClickItem?.Invoke(index);
        }

		public void OnClickEquip()
		{
            onClickEquip?.Invoke(index);
        }

		public void OnClickRegister()
		{
            onClickRegister?.Invoke(index);
        }

		public void OnClickRename()
		{
            // Original4EA940 and original callback4EAB60; captured Text precedes name-array lookup.
            global::ExposureUnnoticed2.ObjectUI.NameEditPanel.NameEditPanelView.Open(name.text, newName =>
            {
                var text = name;
                TargetNameData[index] = newName;
                text.text = newName;
            });
        }
	}
}
