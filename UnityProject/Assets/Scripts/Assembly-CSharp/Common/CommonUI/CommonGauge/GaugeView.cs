using UnityEngine;
using UnityEngine.UI;

namespace Common.CommonUI.CommonGauge
{
	public class GaugeView : MonoBehaviour
	{
		[SerializeField]
		private RectTransform GaugeFront;

		[SerializeField]
		private RectTransform GaugeFrame;

		[SerializeField]
		private Image GaugeFrontImage;

		private float initLength;

		private float length;

		private float lengthScale;

		private float reciprocalMax;

		private float max;

		private bool isUseAlpha;

		public float FrontLength => GaugeFront.sizeDelta.x;

		public void UseAlphaInitialize()
		{
			// Recovered from GameAssembly.dll RVA 0x5824E0
			initLength = GaugeFront.sizeDelta.x;
			isUseAlpha = true;
		}

		public void SetLengthScale(float scale)
		{
			// Recovered from GameAssembly.dll RVA 0x45DBB0
			lengthScale = scale;
		}

		public void SetMax(float max)
		{
			// Recovered from GameAssembly.dll RVA 0x582570
			if (!(System.Math.Abs(this.max - max) < 0.0001f))
			{
			    var capturedFrame = GaugeFrame;
			    float capturedScale = lengthScale;
			    this.max = max;
			    global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.SetWidth(capturedFrame, capturedScale * max);
			    var capturedFront = GaugeFront;
			    reciprocalMax = 1f / max;
			    length = max * lengthScale - 2f;
			    global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.SetWidth(capturedFront, length);
			    GaugeFrontImage.fillAmount = 1f;
			}
		}

		public void Set(float val)
		{
			// Recovered from GameAssembly.dll RVA 0x582690
			bool capturedUseAlpha = isUseAlpha;
			var capturedFront = GaugeFront;
			float width;
			if (!capturedUseAlpha)
			{
			    float capturedReciprocalMax = reciprocalMax;
			    float capturedLength = length;
			    width = capturedReciprocalMax * val * capturedLength;
			}
			else
			{
			    float capturedInitialLength = initLength;
			    width = capturedInitialLength * val;
			}
			global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.SetWidth(capturedFront, width);
		}
	// Recovered original member RVA 0x582750: .ctor
		public GaugeView()
        {
            lengthScale = 1f;
            reciprocalMax = 1f;
        }
	}
}
