using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InteractMenuPanel
{
	public class InteractMenuItemView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI text;

		public void Initialize(string nameKey)
		{
			// Recovered from GameAssembly.dll RVA 0x4F6FB0
			var capturedText = text;
			string translated = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(nameKey);
			capturedText.text = translated;
		}
	}
}
