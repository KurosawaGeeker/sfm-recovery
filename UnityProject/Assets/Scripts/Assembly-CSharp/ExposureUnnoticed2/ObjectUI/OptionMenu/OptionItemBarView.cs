using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.OptionMenu
{
	public class OptionItemBarView : MonoBehaviour, IOptionItemType
	{
		[SerializeField]
		private TextMeshProUGUI valueText;

		[SerializeField]
		private OptionItemType optionItemType;

		private Slider slider;

		private Action<int> onChange;

		private Action<OptionItemType> onClick;

		private bool inInitialize;

		public int Current => 0;

		public OptionItemType OptionItemType => OptionItemType.NoDescription;

		public void Initialize(int initValue, int max, Action<int> onChange, Action<OptionItemType> onClick = null)
		{
		}

		public void SetValueText(string text)
		{
		}

		public void OnChangeSlider()
		{
		}

		public void OnClick()
		{
		}
	}
}
