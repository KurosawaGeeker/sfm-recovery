using ExposureUnnoticed2.Object3D.Player.Scripts;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Camera
{
	public class InventoryCameraController : MonoBehaviour
	{
		private const float CameraMaxDistance = 1.6f;

		private Transform cameraTarget;

		private int layerMask;

		private bool isClosetMode;

		private float currentVerticalValue;

		private float currentFarValue;

		private float currentCameraDistance;

		private int isFirstUpdate;

		private float Yaw { get; set; }

		private float Pitch { get; set; }

		public void Initialize(PlayerController player)
        {
            layerMask = (1 << LayerMask.NameToLayer("Player"))
                | (1 << LayerMask.NameToLayer("Npc"))
                | (1 << LayerMask.NameToLayer("PlayerBlocker"))
                | (1 << LayerMask.NameToLayer("PlayerFace"));
            cameraTarget = player.Pca.PlayerAvatarObjectReferencer.CameraTarget;
        }

		public void OnLateUpdate()
		{
            if (global::ExposureUnnoticed2.ObjectUI.DroneMissionPanel.DroneMissionPanelView.Instance != null) return;
            int width = Screen.width;
            float scale = 1920f / (float)width;
            float mouseX = Input.mousePosition.x * scale;
            float mouseY = Input.mousePosition.y * scale;
            bool uiArea = !global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad && mouseX >= 0f && mouseX < 940f && mouseY >= 0f && mouseY < 1080f;
            float moveX = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetCameraMoveX();
            float moveY = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetCameraMoveY();
            if (isFirstUpdate > 0)
            {
                if ((moveX != 0f || moveY != 0f) && !global::ExposureUnnoticed2.Scripts.Base.CursorController.Instance.IsVisible)
                    isFirstUpdate--;
                moveY = 0f;
                moveX = 0f;
            }
            if (global::ExposureUnnoticed2.Scripts.Base.CursorController.Instance.IsVisible) isFirstUpdate = 2;
            float scroll = Input.mouseScrollDelta.y;
            if (uiArea)
            {
                scroll = 0f;
                moveY = 0f;
                moveX = 0f;
            }
            else
            {
                if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDown(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.MiddleClick))
                {
                    float vertical = moveY * 0.02f + currentVerticalValue;
                    if (vertical < -1f) vertical = -1f;
                    else if (vertical > 1f) vertical = 1f;
                    currentVerticalValue = vertical;
                    moveX = 0f;
                    moveY = 0f;
                }
                if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad)
                {
                    Vector3 move = global::ExposureUnnoticed2.Scripts.Base.InputManager.GetVector3(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.Move);
                    if (move.x < 0.5f && move.x > -0.5f)
                    {
                        float vertical = move.z * -0.03f + currentVerticalValue;
                        if (vertical < -1f) vertical = -1f;
                        else if (vertical > 1f) vertical = 1f;
                        currentVerticalValue = vertical;
                    }
                    if (move.z < 0.5f && move.z > -0.5f) scroll = move.x * 5f;
                }
                if (!global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad)
                {
                    if (!global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDown(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.MiddleClick) &&
                        !global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDown(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.RightClick))
                    {
                        moveY = 0f;
                        moveX = 0f;
                        global::ExposureUnnoticed2.Scripts.Base.CursorController.SetVisible(true);
                    }
                    else global::ExposureUnnoticed2.Scripts.Base.CursorController.SetVisible(false);
                }
            }
            float far = currentFarValue;
            int wheelSensitivity = global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.WheelSensitive100;
            float wheelScale = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetOptionWheelScale();
            far = (((wheelScale * (float)wheelSensitivity) * scroll) * 0.01f) * 0.15f + far;
            if (far < 0f) far = 0f;
            else if (far > 1f) far = 1f;
            currentFarValue = far;
            currentCameraDistance = 1.6f - far * 1.3f;
            if (isClosetMode) { moveY = 0f; moveX = 0f; }
            float pitch = Pitch - moveY;
            Yaw = moveX + Yaw;
            if (pitch < -80f) pitch = -80f;
            else if (pitch > 80f) pitch = 80f;
            Pitch = pitch;
            Vector3 target = cameraTarget.position - (currentVerticalValue * Vector3.up) * 0.7f;
            float distance = CalcCameraDistance();
            Vector3 forward = Quaternion.Euler(Pitch, Yaw, 0f) * Vector3.forward;
            Transform positionTarget = transform;
            positionTarget.position = forward * (distance * -1f) + target;
            Transform rotationTarget = transform;
            Transform positionReader = transform;
            Vector3 actualPosition = positionReader.position;
            Vector3 delta = target - actualPosition;
            Vector3 normalized = delta.normalized;
            Quaternion look = Quaternion.LookRotation(normalized, Vector3.up);
            rotationTarget.rotation = look;
            transform.Rotate(new Vector3(0f, -27f, 0f), Space.Self);
		}

		public void SetTransform(TPCameraController tpCamera)
        {
            Yaw = tpCamera.Yaw;
            Pitch = tpCamera.Pitch;
            currentVerticalValue = 0f;
            currentFarValue = 0f;
            isClosetMode = false;
            isFirstUpdate = 1;
        }

		public void SetClosetMode(float pitch, float yaw)
        {
            Pitch = pitch;
            currentVerticalValue = 0f;
            currentFarValue = 0f;
            Yaw = yaw;
            isClosetMode = true;
            isFirstUpdate = 1;
        }

		private void OnActiveCamera()
        {
            isFirstUpdate = 1;
            currentVerticalValue = 0f;
            currentFarValue = 0f;
        }

		private float CalcCameraDistance()
		{
            Vector3 forward = Quaternion.Euler(Pitch, Yaw, 0f) * Vector3.forward;
            Vector3 origin = cameraTarget.position;
            Vector3 secondPosition = cameraTarget.position;
            global::System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
            origin = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.GetSetY(origin, secondPosition.y);
            float maxDistance = currentCameraDistance;
            int mask = layerMask;
            Vector3 direction = forward * -1f;
            bool found = Physics.SphereCast(origin, 0.05f, direction, out RaycastHit hit, maxDistance, ~mask, QueryTriggerInteraction.Ignore);
            float result = float.MaxValue;
            if (found) result = hit.distance;
            if (!(currentCameraDistance > result)) result = currentCameraDistance;
            return result;
		}
	}
}
