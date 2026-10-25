using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.InGame.TempUnlockPanel
{
	public class TempUnlockPanelView : MonoBehaviour
	{
		[SerializeField]
		private GameObject contents;

		[SerializeField]
		private Image circleGauge;

		[SerializeField]
		private Image circle2Gauge;

		[SerializeField]
		private CanvasGroup progressCanvasGroup;

		private PlayerHandcuffsController handcuffsController;

		private SmoothFloat progressAlpha = new SmoothFloat(0.2f, 1f, 0f, false, false, -1f);

		private float lastActiveTime;

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x4FD2F0
			contents.SetActive(false);
			progressCanvasGroup.alpha = 0f;
			handcuffsController = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.PlayerHandcuffsController;
		}

		public void OnLateUpdate()
		{
			// Recovered from GameAssembly.dll RVA 0x4FD460
			var firstHandcuffs = handcuffsController;
			if (firstHandcuffs is null) throw new global::System.NullReferenceException();
			var firstGauge = circle2Gauge;
			if (firstGauge is null) throw new global::System.NullReferenceException();
			float secondProgress = firstHandcuffs.UnlockProgress2;
			firstGauge.fillAmount = secondProgress;
			progressAlpha.UpdateToTarget();
			var displayedAlphaModel = progressAlpha;
			if (displayedAlphaModel is null) throw new global::System.NullReferenceException();
			var capturedCanvas = progressCanvasGroup;
			float displayedAlpha = displayedAlphaModel.Current;
			capturedCanvas.alpha = displayedAlpha;
			if (handcuffsController.UnlockProgress2 >= 1f)
			{
			    var completedAlpha = progressAlpha;
			    completedAlpha.Target = 0f;
			    completedAlpha.Current = 0f;
			}
			if (handcuffsController.UnlockProgress <= 0f)
			{
			    contents.SetActive(false);
			    float capturedLastActiveTime = lastActiveTime;
			    float currentTime = Time.unscaledTime;
			    if (currentTime > capturedLastActiveTime + 0.5f)
			        progressAlpha.Target = 0f;
			    return;
			}
			progressAlpha.Target = 1f;
			float nextActiveTime = Time.unscaledTime;
			var capturedContents = contents;
			lastActiveTime = nextActiveTime;
			capturedContents.SetActive(true);
			var finalHandcuffs = handcuffsController;
			if (finalHandcuffs is null) throw new global::System.NullReferenceException();
			var finalGauge = circleGauge;
			if (finalGauge is null) throw new global::System.NullReferenceException();
			float firstProgress = finalHandcuffs.UnlockProgress;
			finalGauge.fillAmount = firstProgress;
		}
	}
}
