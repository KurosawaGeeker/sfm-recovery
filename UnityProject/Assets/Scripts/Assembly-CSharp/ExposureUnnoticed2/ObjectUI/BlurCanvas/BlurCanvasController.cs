using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.BlurCanvas
{
	public class BlurCanvasController : MonoBehaviour
	{
		private static bool isActive;

		private SmoothFloat alpha;

		private CanvasGroup canvasGroup;

		private float lastApplyAlpha;

		public static void SetBlur(bool isActive)
		{
			// Recovered from GameAssembly.dll RVA 0x545610
			BlurCanvasController.isActive = isActive;
		}

		private void Awake()
		{
		}

		private void Update()
		{
		}
	}
}
