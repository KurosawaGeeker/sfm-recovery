using System;
using TMPro;
using UnityEngine;

namespace Common.CommonUI.CommonSettingItem
{
	public class SettingSelectItemView : MonoBehaviour
	{
		[SerializeField]
		private new TextMeshProUGUI name;

		[SerializeField]
		private TextMeshProUGUI selectName;

		private int currentSelect;

		private string[] selectNameKeyData;

		private Action<int> onChange;

		private int oldSelect;

		public void Initialize(Action<int> onChange, string nameKey, string[] selectNameKeyData, int defaultSelect = 0)
		{
			this.onChange = onChange;
			if (name != null)
			{
			    name.text = ExposureUnnoticed2.Scripts.Base.Localizer.Trans(nameKey);
			}
			this.selectNameKeyData = selectNameKeyData;
			currentSelect = defaultSelect;
			oldSelect = defaultSelect;
			RefreshView();
		}

		public void ForceSelect(int index, bool forceOnChange = false)
		{
			oldSelect = currentSelect;
			currentSelect = index;
			if (forceOnChange)
			{
			    oldSelect = unchecked(index - 1);
			}
			OnChange();
		}

		private void RefreshView()
		{
			selectName.text = ExposureUnnoticed2.Scripts.Base.Localizer.Trans(selectNameKeyData[currentSelect]);
		}

		public void OnClickLeft()
		{
			oldSelect = currentSelect;
			currentSelect = Mathf.Clamp(unchecked(currentSelect - 1), 0, unchecked(selectNameKeyData.Length - 1));
			OnChange();
		}

		public void OnClickRight()
		{
			oldSelect = currentSelect;
			currentSelect = Mathf.Clamp(unchecked(currentSelect + 1), 0, unchecked(selectNameKeyData.Length - 1));
			OnChange();
		}

		private void OnChange()
		{
			if (oldSelect != currentSelect)
			{
			    RefreshView();
			    onChange?.Invoke(currentSelect);
			}
		}
	}
}
