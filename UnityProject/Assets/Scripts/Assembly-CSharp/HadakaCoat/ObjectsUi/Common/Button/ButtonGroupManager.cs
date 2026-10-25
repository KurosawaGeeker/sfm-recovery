using System;
using UnityEngine;

namespace HadakaCoat.ObjectsUi.Common.Button
{
	public class ButtonGroupManager : MonoBehaviour
	{
		public interface IButtonView
		{
			bool IsEmitting { get; set; }

			void InitializeFromButtonGroupManager(Action<IButtonView> sendOnClickNotify);

			void Emit();

			void Extinction();

			void InvokeOnClick();

			GameObject GameObject();
		}

		private enum ButtonGroupTintType
		{
			NoEmit = 0,
			OnlyOne = 1,
			Each = 2,
			OnlyOneNoExtinction = 3
		}

		[SerializeField]
		private ButtonGroupTintType buttonGroupTintType;

		[SerializeField]
		private ButtonView defaultSelected;

		[SerializeField]
		private bool isOnlyUpperLayerButton;

		private IButtonView[] buttons;

		public int ButtonCount => buttons.Length;

		private void Awake()
		{
			// Recovered from GameAssembly.dll RVA 0x57C9D0
			CollectButtonComponent();
			foreach (IButtonView view in buttons) view.InitializeFromButtonGroupManager(ReceiveOnClickNotify);
			if (defaultSelected != null) ReceiveOnClickNotify(defaultSelected);
		}

		public void ForceSelect(IButtonView notifiedView)
		{
			// Recovered from GameAssembly.dll RVA 0x57CC00
			ReceiveOnClickNotify(notifiedView);
		}

		public void ForceSelect(int index)
		{
			// Recovered from GameAssembly.dll RVA 0x57CC10
			ReceiveOnClickNotify(buttons[index]);
		}

		public void InvokeOnClick(int index)
		{
			// Recovered from GameAssembly.dll RVA 0x57CC50
			buttons[index].InvokeOnClick();
		}

		public IButtonView Get(int index)
		{
			// Recovered from GameAssembly.dll RVA 0x57CCC0
			return buttons[index];
		}

		private void CollectButtonComponent()
		{
			// Recovered from GameAssembly.dll RVA 0x57CD00
			if (!isOnlyUpperLayerButton)
			{
			    buttons = GetComponentsInChildren<IButtonView>();
			    return;
			}
			var list = new System.Collections.Generic.List<IButtonView>();
			foreach (Transform child in transform)
			{
			    IButtonView view = child.GetComponent<IButtonView>();
			    if (view != null) list.Add(view);
			}
			buttons = list.ToArray();
		}

		public void ReSearchButton(bool isNoSkipFrame = false, bool isSelectFirst = false)
		{
			// Recovered from GameAssembly.dll RVA 0x57D140
			// Original local callback RVA 0x57D4C0.
			Action process = () =>
			{
			    CollectButtonComponent();
			    foreach (IButtonView view in buttons) view.InitializeFromButtonGroupManager(ReceiveOnClickNotify);
			    if (isSelectFirst) ReceiveOnClickNotify(buttons[0]);
			};
			if (isNoSkipFrame) process();
			else global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(process, 1);
		}

		private void ReceiveOnClickNotify(IButtonView notifiedView)
		{
			// Recovered from GameAssembly.dll RVA 0x57D270
			switch (buttonGroupTintType)
			{
			    case ButtonGroupTintType.NoEmit:
			        return;
			    case ButtonGroupTintType.OnlyOne:
			        if (notifiedView != null && notifiedView.IsEmitting)
			        {
			            notifiedView.Extinction();
			            return;
			        }
			        foreach (IButtonView view in buttons)
			        {
			            if (view == notifiedView) view.Emit();
			            else view.Extinction();
			        }
			        return;
			    case ButtonGroupTintType.Each:
			        if (notifiedView.IsEmitting) notifiedView.Extinction();
			        else notifiedView.Emit();
			        return;
			    case ButtonGroupTintType.OnlyOneNoExtinction:
			        foreach (IButtonView view in buttons)
			        {
			            if (view == notifiedView) view.Emit();
			            else view.Extinction();
			        }
			        return;
			}
		}
	}
}
