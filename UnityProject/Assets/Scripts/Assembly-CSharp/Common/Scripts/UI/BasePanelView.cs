using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Scripts.UI
{
	public abstract class BasePanelView : MonoBehaviour
	{
		protected enum ForceActiveButtonType
		{
			None = 0,
			Active = 1,
			NonActive = 2
		}

		[SerializeField]
		private bool cancelButtonSearch;

		private Action onCloseBase;

		protected Action onClose;

		protected Animator animator;

		private static readonly int Close1 = Animator.StringToHash("Close");

		private bool isAlreadyClose;

		private bool oldIsActivePanel;

		private Button[] buttonss;

		private bool isActive;

		protected ForceActiveButtonType ForceActiveButtonMode;

		private void Awake()
		{
			// Recovered from GameAssembly.dll RVA 0x57EA30
			animator = GetComponent<Animator>();
			animator.updateMode = AnimatorUpdateMode.UnscaledTime;
			if (!cancelButtonSearch) buttonss = GetComponentsInChildren<Button>(true);
			CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
			if (canvasGroup != null) canvasGroup.alpha = 0f;
		}

		public void InitializeFromManager(Action onClose)
		{
			// Recovered from GameAssembly.dll RVA 0x4690C0
			onCloseBase = onClose;
		}

		protected void Close()
		{
			// Recovered from GameAssembly.dll RVA 0x57ECB0
			if (isAlreadyClose) return;
			isAlreadyClose = true;
			// Original delayed callback RVA 0x57F630.
			global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() =>
			{
			    onCloseBase?.Invoke();
			    onClose?.Invoke();
			}, 1);
			animator.SetTrigger(Close1);
		}

		protected void OnUpdate()
		{
			// Recovered from GameAssembly.dll RVA 0x57EDD0
			if (cancelButtonSearch) return;
			if (!isActive)
			{
			    if ((IsActivePanel() && ForceActiveButtonMode != ForceActiveButtonType.NonActive)
			        || ForceActiveButtonMode == ForceActiveButtonType.Active)
			    {
			        isActive = true;
			        foreach (Button button in buttonss) if (button != null) button.enabled = true;
			    }
			}
			else
			{
			    if ((!IsActivePanel() && ForceActiveButtonMode != ForceActiveButtonType.Active)
			        || ForceActiveButtonMode == ForceActiveButtonType.NonActive)
			    {
			        isActive = false;
			        foreach (Button button in buttonss) if (button != null) button.enabled = false;
			    }
			}
		}

		protected void DelayImmediately()
		{
			// Recovered from GameAssembly.dll RVA 0x57F020
			global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() =>
			{
			    onCloseBase?.Invoke();
			    onClose?.Invoke();
			    UnityEngine.Object.Destroy(gameObject);
			}, 1);
		}

		protected bool CommonCheckClose()
		{
			// Recovered from GameAssembly.dll RVA 0x57F0A0
			if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.Cancel) && IsActivePanel())
			{
			    Close();
			    return true;
			}
			return false;
		}

		protected bool IsTransActiveFrame()
		{
			// Recovered from GameAssembly.dll RVA 0x57F120
			bool current = IsActivePanel();
			bool previous = oldIsActivePanel;
			oldIsActivePanel = current;
			return current && !previous;
		}

		public virtual void OnAnimCloseEnd()
		{
			// Recovered from GameAssembly.dll RVA 0x57F150
			UnityEngine.Object.Destroy(gameObject);
		}

		protected bool IsActivePanelPadMode()
		{
			// Recovered from GameAssembly.dll RVA 0x57F1F0
			return IsActivePanel() && global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad;
		}

		public bool IsActivePanel()
		{
			// Recovered from GameAssembly.dll RVA 0x57F250
			if (global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance == null) return true;
			BasePanelView current = global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.GetCurrentBasePanelView();
			return !global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.IsForceDeActive && current != null && current == this;
		}

		public virtual void OnReturnActive()
		{
		}
	}
}
