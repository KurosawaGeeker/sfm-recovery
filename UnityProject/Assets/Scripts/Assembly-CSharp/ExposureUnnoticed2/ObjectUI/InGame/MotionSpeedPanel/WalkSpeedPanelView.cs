using ExposureUnnoticed2.Scripts.Base;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InGame.MotionSpeedPanel
{
	public class WalkSpeedPanelView : MonoBehaviour
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
			// Recovered from GameAssembly.dll RVA 0x508560
			currentViewIndex = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.WalkSpeedScaleIndex + 2;
			alpha.Target = 0f;
			currentAlpha = 0f;
			canvasGroup.alpha = 0f;
			AdjustSpeed();
			RefreshView();
		}

		public void OnLateUpdate()
		{
            bool changed = false;
            if (!global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.isOpenSomeAllowInputUi
                && !global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.IsOpenSomeCancelInputUi
                && !global::ExposureUnnoticed2.ObjectUI.InGame.RingMenu.RingMenuParentView.Instance.IsOpenRing)
            {
                int next = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.WalkSpeedScaleIndex;
                if (!InputManager.IsGamePad)
                {
                    if (InputManager.IsDownFrame((InputManager.InputType)31)) next = unchecked(next + 1);
                    if (InputManager.IsDownFrame((InputManager.InputType)32)) next = unchecked(next - 1);
                }
                else if (InputManager.IsDown((InputManager.InputType)34))
                {
                    if (InputManager.IsDownFrame((InputManager.InputType)14)) next = unchecked(next + 1);
                    if (InputManager.IsDownFrame((InputManager.InputType)27)) next = unchecked(next - 1);
                }
                if (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.IsDownMoveForwardOneHand)
                {
                    Vector2 wheel = Input.mouseScrollDelta;
                    float delta = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetOptionWheelScale() * wheel.y;
                    if (delta > 0f) next = unchecked(next + 1);
                    else if (delta < 0f) next = unchecked(next - 1);
                }
                changed = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.WalkSpeedScaleIndex != next;
                global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.WalkSpeedScaleIndex = Mathf.Clamp(next, -7, 3);
            }
            remainShowTime -= Time.deltaTime;
            if (currentViewIndex != unchecked(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.WalkSpeedScaleIndex + 2))
            {
                currentViewIndex = unchecked(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.WalkSpeedScaleIndex + 2);
                RefreshView();
                AdjustSpeed();
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
			// Recovered from GameAssembly.dll RVA 0x508AC0
			Vector3 oldPosition = currentSelectObject.localPosition;
			currentSelectObject.localPosition = new Vector3(unchecked(currentViewIndex * 28), oldPosition.y, oldPosition.z);
		}

		private void AdjustSpeed()
		{
			// Recovered from GameAssembly.dll RVA 0x508C00
			scaleText.SetText("x{0:2}", global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.WalkSpeedScaleIndex * 0.05f + 1f);
		}
	}
}
