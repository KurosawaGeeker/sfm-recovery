using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.Util.UI
{
	[ExecuteAlways]
	public class FitByUpdateText : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI targetText;

		[SerializeField]
		private float bias;

		private string oldText;

		private float oldBias;

		private RectTransform rect;

		private void Initialize()
		{
		}

		private void Update()
		{
		}
	}
}
