using System;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.OptionMenu
{
	public class OptionItemCheckView : MonoBehaviour, IOptionItemType
	{
		[SerializeField]
		private OptionItemType optionItemType;

		private Action<OptionItemType> onClick;

		private Action<bool> onChange;

		private Toggle toggle;

		public OptionItemType OptionItemType => OptionItemType.NoDescription;

		public bool Current => false;

		public void Initialize(bool initValue, Action<bool> onChange, Action<OptionItemType> onClick = null)
		{
		}

		public void OnChangeToggle(int index)
		{
		}

		public void OnClick()
		{
		}
	}
}
