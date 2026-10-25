using System;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Camera
{
	public class BaseCameraController : MonoBehaviour, ILateUpdater
	{
		private const int TpsAngle = 85;

		private const int FpsAngle = 90;

		private TPCameraController thirdPersonCamera;

		private FPCameraController firstPersonCamera;

		private InventoryCameraController inventoryCameraController;

		public Action OnChangeFirstPersonView;

		private int tpsCameraMask;

		private int fpsCameraMask;

		private UnityEngine.Camera camera;

		private PlayerStateModel playerStateModel;

		private bool isStrafeCache;

		private bool IsActiveInventoryCamera;

		public UnityEngine.Camera Camera { get; private set; }

		public bool IsLockCamera { get; set; }

		public void Initialize(PlayerController player, PlayerStateModel playerStateModel, TPCameraController thirdPersonCamera, FPCameraController firstPersonCamera, InventoryCameraController inventoryCameraController)
        {
            this.playerStateModel = playerStateModel;
            this.thirdPersonCamera = thirdPersonCamera;
            this.firstPersonCamera = firstPersonCamera;
            this.inventoryCameraController = inventoryCameraController;
            Camera = GetComponent<UnityEngine.Camera>();
            thirdPersonCamera.Initialize(player, this);
            firstPersonCamera.Initialize(player, this);
            inventoryCameraController.Initialize(player);
            camera = GetComponent<UnityEngine.Camera>();
            tpsCameraMask = camera.cullingMask;
            fpsCameraMask = tpsCameraMask & ~(1 << LayerMask.NameToLayer("PlayerFace"));
        }

		private bool IsStrafe()
        {
            return playerStateModel.IsStrafe;
        }

		public void OnLateUpdate()
		{
            int status = (int)InGameManager.Instance.InGameReferencer.GameOverController.currentState;
            if (status != 0 && status != 1) return;
            if (isStrafeCache != playerStateModel.IsStrafe)
            {
                isStrafeCache = playerStateModel.IsStrafe;
                if (playerStateModel.IsStrafe) TransStrafe();
                else TransNotStrafe();
            }
            if (!IsActiveInventoryCamera)
            {
                if (!playerStateModel.IsStrafe)
                    transform.SetPositionAndRotation(thirdPersonCamera.transform.position, thirdPersonCamera.transform.rotation);
                else
                    transform.SetPositionAndRotation(firstPersonCamera.Child.position, firstPersonCamera.Child.rotation);
            }
            else
            {
                inventoryCameraController.OnLateUpdate();
                transform.SetPositionAndRotation(inventoryCameraController.transform.position, inventoryCameraController.transform.rotation);
            }
		}

		public void SetDirectCameraFOV(int fov)
		{
			// Recovered from GameAssembly.dll RVA 0x45B970
			camera.fieldOfView = fov;
		}

		public void SetTpsCullingMask()
        {
            camera.cullingMask = tpsCameraMask;
        }

		private void TransStrafe()
		{
            OnChangeFirstPersonView?.Invoke();
            firstPersonCamera.OnChangeFirstPersonView();
            firstPersonCamera.OnLateUpdate();
            camera.cullingMask = fpsCameraMask;
            var options = GameState.OptionData;
            if (options is null) throw new global::System.NullReferenceException();
            var targetCamera = camera;
            float fov = options.CameraAngle1st;
            targetCamera.fieldOfView = fov;
		}

		private void TransNotStrafe()
        {
            thirdPersonCamera.OnChangeThirdPersonView();
            if (thirdPersonCamera is null) throw new global::System.NullReferenceException();
            SetTpsCullingMask();
            var options = GameState.OptionData;
            if (options is null) throw new global::System.NullReferenceException();
            var targetCamera = camera;
            float fov = options.CameraAngle3rd;
            targetCamera.fieldOfView = fov;
        }

		public void SetInventoryCamera(bool isActive, bool isCloset = false)
		{
            IsActiveInventoryCamera = isActive;
            var options = GameState.OptionData;
            if (options is null) throw new global::System.NullReferenceException();
            var targetCamera = camera;
            float fov = options.CameraAngle3rd;
            targetCamera.fieldOfView = fov;
            if (!IsActiveInventoryCamera)
            {
                if (playerStateModel.IsStrafe) TransStrafe();
                InGameManager.Instance.InGameReferencer.GlobalVolume.SetPaniniProjection(false);
                return;
            }
            if (playerStateModel.IsStrafe)
            {
                thirdPersonCamera.OnChangeThirdPersonView();
                thirdPersonCamera.Yaw += 180f;
                if (thirdPersonCamera is null) throw new global::System.NullReferenceException();
                SetTpsCullingMask();
            }
            if (!isCloset) inventoryCameraController.SetTransform(thirdPersonCamera);
            else inventoryCameraController.SetClosetMode(1f, 90f);
            InGameManager.Instance.InGameReferencer.GlobalVolume.SetPaniniProjection(true);
		}
	}
}
