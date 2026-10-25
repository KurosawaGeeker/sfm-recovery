using System;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.OptionMenu
{
	public class OptionControlMenuView : MonoBehaviour, IOptionMiddlePanel
	{
		[SerializeField]
		private OptionItemCheckView mouseInverseX;

		[SerializeField]
		private OptionItemCheckView mouseInverseY;

		[SerializeField]
		private OptionItemCheckView padInverseX;

		[SerializeField]
		private OptionItemCheckView padInverseY;

		[SerializeField]
		private OptionItemCheckView wheelInverse;

		[SerializeField]
		private OptionItemBarView mouseSensitive;

		[SerializeField]
		private OptionItemBarView wheelSensitive;

		[SerializeField]
		private OptionItemBarView padSensitive;

		[SerializeField]
		private OptionItemCheckView padUpDownInverse;

		[SerializeField]
		private OptionItemBarView longPressTimeScale;

		private Animator animator;

		private Action onClose;

		private Action<OptionItemType> onClickItem;

		public void Initialize(Action onClose, Action<OptionItemType> onClickItem)
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
