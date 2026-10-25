using System;
using ExposureUnnoticed2.Scripts.Base;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ExposureUnnoticed2.ObjectUI.OptionMenu.KeyConfigPanel
{
	public class KeyConfigItemView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI keyName;

		[SerializeField]
		private InputManager.InputType targetInput;

		[SerializeField]
		private int bindIndexOffset;

		private InputActionRebindingExtensions.RebindingOperation rebindOperation;

		private InputAction action;

		private Action<InputManager.InputType> onStartBindChange;

		private Action onEndBindChange;

		private Action onEndBindChangeComplete;

		private int targetBindIndex;

		public void Initialize(Action<InputManager.InputType> onStartBindChange, Action onEndBindChange, Action onEndBindChangeComplete)
		{
		}

		private void OnDestroy()
		{
		}

		private void DisposeRebind()
		{
		}

		private void OnFinishedRebind()
		{
		}

		public void Reset()
		{
		}

		public void OnClickReset()
		{
		}

		public void OnClickChange()
		{
		}

		private void RefreshView()
		{
		}
	}
}
