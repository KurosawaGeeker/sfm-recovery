using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.InventoryPanel.CosplaySerieseSelectPanel
{
	public class CosplaySeriesChipView : MonoBehaviour
	{
		[SerializeField]
		private Color activeColor;

		[SerializeField]
		private Color deActiveColor;

		private SmoothFloat activeBlend;

		private float oldBlend;

		private Image image;

		private void Awake()
		{
            // Complete original RVA 0x4F5F00.
            image = GetComponent<Image>();
        }

		private void Update()
		{
		}

		public void SetActive(bool isActive)
		{
            // Complete original RVA 0x4F6130; original writes SmoothFloat.Target.
            activeBlend.Target = isActive ? 1f : 0f;
        }
		// Complete original constructor RVA 0x4F6160.
		public CosplaySeriesChipView()
		{
            activeBlend = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.1f, 1f, 0f, false, false, -1f);
            oldBlend = -999f;
		}

	}
}
