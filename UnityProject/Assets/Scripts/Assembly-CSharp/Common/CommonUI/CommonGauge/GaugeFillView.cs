using UnityEngine;
using UnityEngine.UI;

namespace Common.CommonUI.CommonGauge
{
	public class GaugeFillView : MonoBehaviour
	{
		[SerializeField]
		private RectTransform GaugeFront;

		private Image GaugeFrontImage;

		private void Awake()
		{
			// Recovered from GameAssembly.dll RVA 0x582280
			GaugeFrontImage = GaugeFront.gameObject.GetComponent<Image>();
		}

		public void Set(float val)
		{
			// Recovered from GameAssembly.dll RVA 0x582370
			if (GaugeFrontImage != null) GaugeFrontImage.fillAmount = val;
		}
	}
}
