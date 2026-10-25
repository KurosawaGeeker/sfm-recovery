using ExposureUnnoticed2.Scripts.Base;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InGame.MotionSpeedPanel
{
	public class MotionSpeedPanelView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI scaleText;

		[SerializeField]
		private RectTransform currentSelectObject;

		[SerializeField]
		private CanvasGroup canvasGroup;

		private int currentViewIndex;

		private float currentAlpha;

		private SmoothFloat alpha = new SmoothFloat(0.1f, 1f, 0f, false, false, -1f);

		private float remainShowTime;

		private bool alreadyHideStart;

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x507C40
			currentViewIndex = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.MotionSpeedScaleIndex;
			alpha.Target = 0f;
			currentAlpha = 0f;
			canvasGroup.alpha = 0f;
			AdjustSpeed();
			RefreshView();
		}

		public void OnLateUpdate()
		{
            bool changed = false;
            if ((!global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.isOpenSomeAllowInputUi
                 && !global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.IsOpenSomeCancelInputUi)
                || global::ExposureUnnoticed2.Scripts.InGame.SexManager.Instance.IsSexMode)
            {
                int next = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.MotionSpeedScaleIndex;
                if (!InputManager.IsGamePad)
                {
                    if (InputManager.IsDownFrame((InputManager.InputType)29)) next = unchecked(next + 1);
                    if (InputManager.IsDownFrame((InputManager.InputType)30)) next = unchecked(next - 1);
                }
                else if (InputManager.IsDown((InputManager.InputType)34))
                {
                    if (InputManager.IsDownFrame((InputManager.InputType)28)) next = unchecked(next + 1);
                    if (InputManager.IsDownFrame((InputManager.InputType)23)) next = unchecked(next - 1);
                }
                changed = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.MotionSpeedScaleIndex != next;
                global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.MotionSpeedScaleIndex = Mathf.Clamp(next, -5, 5);
            }
            remainShowTime -= Time.deltaTime;
            if (currentViewIndex != global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.MotionSpeedScaleIndex)
            {
                currentViewIndex = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.MotionSpeedScaleIndex;
                RefreshView();
                AdjustSpeed();
                if (global::ExposureUnnoticed2.Scripts.InGame.SexManager.Instance.IsSexMode)
                    global::ExposureUnnoticed2.Scripts.InGame.SexManager.Instance.SetNpcMotionSpeed();
            }
            if (changed)
            {
                alpha.Target = 1f;
                alpha.SetSecond(0.1f);
                remainShowTime = 1f;
                alreadyHideStart = false;
            }
            if (remainShowTime < 0f && !alreadyHideStart)
            {
                alpha.SetSecond(0.5f);
                alpha.Target = 0f;
                alreadyHideStart = true;
            }
            alpha.UpdateTo(alpha.Target);
            if (currentAlpha != alpha.Current)
            {
                currentAlpha = alpha.Current;
                canvasGroup.alpha = currentAlpha;
            }
		}

		private void RefreshView()
		{
			// Recovered from GameAssembly.dll RVA 0x508120
			Vector3 oldPosition = currentSelectObject.localPosition;
			currentSelectObject.localPosition = new Vector3(unchecked(currentViewIndex * 28), oldPosition.y, oldPosition.z);
		}

		private void AdjustSpeed()
		{
			// Recovered from GameAssembly.dll RVA 0x508260
			float scale = currentViewIndex < 0 ? currentViewIndex * 0.1f : currentViewIndex * 0.2f;
			scale += 1f;
			global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAnimationManager.SetMotionSpeedScale(scale);
			scaleText.SetText("x{0:1}", scale);
			global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.CurrentMotionSpeed = scale;
		}
	}
}
