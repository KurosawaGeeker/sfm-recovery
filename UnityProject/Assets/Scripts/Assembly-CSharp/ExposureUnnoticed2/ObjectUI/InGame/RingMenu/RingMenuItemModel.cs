using System;
using ExposureUnnoticed2.Scripts.Base;

namespace ExposureUnnoticed2.ObjectUI.InGame.RingMenu
{
	public class RingMenuItemModel
	{
		public InputManager.InputType InputTypae { get; set; }

		public RingMenuParentView.RingItemType RingItemType { get; set; }

		public Action OnSelect { get; set; }

		public string NameKey { get; set; }

		public Func<bool> ReleaseCondition { get; set; }

		public int Index { get; set; }
	}
}
