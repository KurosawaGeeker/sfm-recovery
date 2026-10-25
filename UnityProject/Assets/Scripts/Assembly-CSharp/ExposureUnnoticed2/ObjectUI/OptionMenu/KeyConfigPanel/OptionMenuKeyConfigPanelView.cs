using System;
using ExposureUnnoticed2.Scripts.Base;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.OptionMenu.KeyConfigPanel
{
	public class OptionMenuKeyConfigPanelView : MonoBehaviour, IOptionMiddlePanel
	{
		[SerializeField]
		private KeyConfigItemView[] keyConfigItemViews;

		[SerializeField]
		private GameObject blockKeyPanel;

		[SerializeField]
		private TextMeshProUGUI cancelText;

		private Action onClose;

		private Animator animator;

		private Action onClickCloseCansel;

		private bool isOpenChild;

		public static OptionMenuKeyConfigPanelView Instance { get; private set; }

		public bool IsChangeSomething { get; private set; }

		public void Initialize(Action onClose, Action onClickCloseCansel)
		{
		}

		private void OnStartBindChange(InputManager.InputType type)
		{
		}

		private void OnEndBindChange()
		{
		}

		private void OnEndBindChangeComplete()
		{
		}

		public void OnClickSave()
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
