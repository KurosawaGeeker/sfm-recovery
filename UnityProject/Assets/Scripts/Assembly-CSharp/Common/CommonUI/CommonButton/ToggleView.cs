using System;
using HadakaCoat.ObjectsUi.Common.Button;
using UnityEngine;
using UnityEngine.UI;

namespace Common.CommonUI.CommonButton
{
	public class ToggleView : MonoBehaviour, ButtonGroupManager.IButtonView
	{
		private Animator animator;

		private bool isInitialized;

		private Action<ToggleView> sendOnClickNotify;

		private static readonly int IsEmit;

		private Toggle toggle;

		private Action onClick;

		public bool IsEmitting { get; set; }

		private void Awake()
		{
		}

		public GameObject GameObject()
		{
			return null;
		}

		public void Initialize()
		{
		}

		private void OnClick(bool val)
		{
		}

		public void InitializeFromButtonGroupManager(Action<ButtonGroupManager.IButtonView> sendOnClickNotify)
		{
		}

		public void Emit()
		{
		}

		public void Extinction()
		{
		}

		public void InvokeOnClick()
		{
		}
	}
}
