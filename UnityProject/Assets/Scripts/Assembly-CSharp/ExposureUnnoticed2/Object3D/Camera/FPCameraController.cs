using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Camera
{
	public class FPCameraController : MonoBehaviour, ILateUpdater
	{
		private const float JogZOffset = 0.03f;

		[SerializeField]
		private Vector3 targetOffset;

		[SerializeField]
		private Vector3 neckOffset;

		[SerializeField]
		private Transform child;

		private PlayerController player;

		private Vector3 targetDashOffset;

		private SmoothFloat currentZOffset = new SmoothFloat(0.2f, 0.03f, 0f, false, false, -1f);

		private BaseCameraController baseCamera;

		private int skipFrameCameraCount;

		private Vector3 initLocalPosition;

		public Transform Child => child;

		public float Pitch { get; private set; }

		private float Yaw { get; set; }

		public void Initialize(PlayerController player, BaseCameraController baseCamera)
        {
            this.player = player;
            this.baseCamera = baseCamera;
        }

		public void OnLateUpdate()
		{
            float target = 0f;
            if (InputManager.IsDown(InputManager.InputType.Dash) && InputManager.GetVector3(InputManager.InputType.Move).magnitude >= 0.5f)
                target = 0.03f;
            currentZOffset.UpdateTo(target);
            Vector2 rawCameraMove = InputManager.GetVector2(InputManager.InputType.CameraMove);
            float moveX = GameStateFacade.GetCameraMoveX();
            float moveY = GameStateFacade.GetCameraMoveY();
            if (CursorController.Instance.IsVisible || baseCamera.IsLockCamera)
            {
                skipFrameCameraCount = 4;
                moveX = 0f;
                moveY = 0f;
            }
            if (skipFrameCameraCount > 0 && rawCameraMove.y * rawCameraMove.y + rawCameraMove.x * rawCameraMove.x > 0f)
            {
                skipFrameCameraCount--;
                moveX = 0f;
                moveY = 0f;
            }
            float yaw = moveX + Yaw;
            if (yaw < -80f) yaw = -80f;
            else if (yaw > 80f) yaw = 80f;
            float pitch = Pitch - moveY;
            Yaw = yaw;
            if (pitch < -60f) pitch = -60f;
            else if (pitch > 60f) pitch = 60f;
            Pitch = pitch;
            var action = player.Pca.PlayerState.CurrentAction;
            if ((int)action.Type == -1 || action.CanMove) Yaw = 0f;
            float heightScale = (float)GameState.PlayerCustomData.CustomizeHeight / 160f;
            Vector3 worldPosition = player.transform.position + (player.transform.rotation * targetOffset) * heightScale;
            worldPosition = worldPosition + ((player.transform.rotation * Quaternion.Euler(Pitch * 0.5f + 45f, Yaw, 0f)) * neckOffset) * heightScale;
            float negativeAvatarOffset = -PlayerController.Instance.Pca.PlayerAvatarOffsetManager.OffsetY;
            Vector3 dashOffset = player.transform.rotation * ((Vector3.forward * currentZOffset.Current) * heightScale);
            float positiveAvatarOffset = PlayerController.Instance.Pca.PlayerAvatarOffsetManager.OffsetY;
            worldPosition = worldPosition + dashOffset;
            worldPosition = worldPosition + Vector3.up * positiveAvatarOffset;
            worldPosition = worldPosition + Vector3.up * negativeAvatarOffset;
            transform.position = worldPosition;
            Quaternion lookBasis = player.transform.rotation * Quaternion.Euler(Pitch, Yaw, 0f);
            Vector3 forward = lookBasis * Vector3.forward;
            Transform targetChild = child;
            Quaternion look = Quaternion.LookRotation(forward, Vector3.up);
            targetChild.rotation = look;
            initLocalPosition = transform.localPosition;
            Vector3 neckUp = player.Pca.PlayerAvatarObjectReferencer.Neck.up;
            float neckDelta = (float)(GameState.PlayerCustomData.CustomizeNeck - 50);
            Vector3 spineUp = player.Pca.PlayerAvatarObjectReferencer.Spine.up;
            float spineScale;
            var custom = GameState.PlayerCustomData;
            if (custom.CustomizeUpperBodyLength < 50)
                spineScale = (float)(GameState.PlayerCustomData.CustomizeUpperBodyLength - 50) * 0.0004f;
            else spineScale = (float)(custom.CustomizeUpperBodyLength - 50) * 0.0008f;
            Vector3 avatarOffset = Vector3.zero;
            if (!player.Pca.PlayerState.CurrentAction.IsIgnoreYOffset)
                avatarOffset = Vector3.up * player.Pca.PlayerAvatarOffsetManager.OffsetY;
            Transform destination = transform;
            Vector3 capturedInitial = initLocalPosition;
            Transform vectorTransform = transform;
            Vector3 localOffset = spineUp * spineScale + (neckUp * neckDelta) * 0.0005f;
            Vector3 convertedOffset = vectorTransform.InverseTransformDirection(localOffset);
            Vector3 final = capturedInitial + convertedOffset;
            final = final + avatarOffset;
            Vector3 kneeOffset = (Vector3.up * player.Pca.PlayerKneeAdjuster.KneeOffsetY) * -1f;
            destination.localPosition = final + kneeOffset;
		}

		public void OnChangeFirstPersonView()
        {
            Pitch = 0f;
            Yaw = 0f;
        }
	}
}
