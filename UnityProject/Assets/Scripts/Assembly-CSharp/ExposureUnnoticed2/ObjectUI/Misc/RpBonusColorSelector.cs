using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.Misc
{
	public class RpBonusColorSelector
	{
		public static Color GetTextColor(float rp)
		{
			// Recovered from GameAssembly.dll RVA 0x4E2C40
			int index;
			if (rp >= 4f) index = 6;
			else if (rp >= 3.5f) index = 5;
			else if (rp >= 3f) index = 4;
			else if (rp >= 2.5f) index = 3;
			else if (rp >= 2f) index = 2;
			else if (rp >= 1.5f) index = 1;
			else index = 0;
			return global::ExposureUnnoticed2.Master.Common.MCommon.Instance.RpBonusColor[index];
		}

		public static Color GetTextColorSecondary(float rp)
		{
			// Recovered from GameAssembly.dll RVA 0x4E2E10
			int index;
			if (rp >= 4f) index = 6;
			else if (rp >= 3.5f) index = 5;
			else if (rp >= 3f) index = 4;
			else if (rp >= 2.5f) index = 3;
			else if (rp >= 2f) index = 2;
			else if (rp >= 1.5f) index = 1;
			else index = 0;
			return global::ExposureUnnoticed2.Master.Common.MCommon.Instance.RpBonusColorSecondary[index];
		}
	}
}
