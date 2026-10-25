using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.Scripts.Base
{
	public class AspectRatioApplyer : MonoBehaviour
	{
		private CanvasScaler canvasScaler;

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x4B5750
			canvasScaler = GetComponent<CanvasScaler>();
			EventManager.AddListener<ChangeWindowModeEvent>(OnChange);
			OnChange(null);
		}

		private void OnDestroy()
		{
			// Recovered from GameAssembly.dll RVA 0x4B5990
			EventManager.RemoveListener<ChangeWindowModeEvent>(OnChange);
		}

		private void OnChange(ChangeWindowModeEvent evt)
		{
			// Recovered from GameAssembly.dll RVA 0x4B5A50
			int aspect = global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.AspectRatioIndex;
			int width = aspect switch
			{
			    0 => 1920,
			    1 => 2560,
			    _ => throw new System.Runtime.CompilerServices.SwitchExpressionException(aspect)
			};
			canvasScaler.referenceResolution = new Vector2(width, canvasScaler.referenceResolution.y);
		}
	}
}
