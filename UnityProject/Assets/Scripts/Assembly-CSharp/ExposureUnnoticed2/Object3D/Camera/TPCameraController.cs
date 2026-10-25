using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Camera
{
	public class TPCameraController : MonoBehaviour, IStarter
	{
		private Transform cameraTarget;

		[SerializeField]
		private float cameraMaxDistance;

		private float currentVerticalValue = 1f;

		private PlayerController player;

		private BaseCameraController baseCamera;

		private int ignoreLayerMask;

		private int skipFrameCameraCount;

		public float Yaw { get; set; }

		public float Pitch { get; private set; }

		public void Initialize(PlayerController player, BaseCameraController baseCamera)
        {
            this.player = player;
            this.baseCamera = baseCamera;
            ignoreLayerMask = (1 << LayerMask.NameToLayer("Player"))
                | (1 << LayerMask.NameToLayer("Npc"))
                | (1 << LayerAccessor.SpermCollider)
                | (1 << LayerMask.NameToLayer("PlayerBlocker"))
                | (1 << LayerMask.NameToLayer("PlayerFace"))
                | (1 << LayerMask.NameToLayer("ForeignObject"))
                | (1 << LayerAccessor.XRayLevel)
                | (1 << LayerAccessor.NpcSightBlocker)
                | (1 << LayerAccessor.IgnorePeeParticle);
            cameraTarget = this.player.Pca.PlayerAvatarObjectReferencer.CameraTarget;
        }

		public void OnStart()
		{
            EventManager.AddListener<CompleteTransitionStageEvent>(OnCompleteTransitionStageEvent, false);
		}

		private void OnDestroy()
		{
            EventManager.RemoveListener<CompleteTransitionStageEvent>(OnCompleteTransitionStageEvent);
		}

		public void OnFixedUpdate()
		{
		}

		public void OnLateUpdate()
		{
            Vector2 cameraMove = InputManager.GetVector2(InputManager.InputType.CameraMove);
            float x = GameStateFacade.GetCameraMoveX();
            float y = GameStateFacade.GetCameraMoveY();
            float wheel = Input.mouseScrollDelta.y;
            float vertical = 0f;
            if (!InputManager.IsGamePad)
            {
                if (InputManager.IsDown(InputManager.InputType.MiddleClick))
                {
                    x = 0f;
                    vertical = -y;
                    y = 0f;
                    vertical *= 0.02f;
                }
            }
            else if (InputManager.IsDown(InputManager.InputType.PadShift))
            {
                int direction = InputManager.IsDown(InputManager.InputType.OpenClothesA) ? 1 : 0;
                if (InputManager.IsDown(InputManager.InputType.OpenClothesB)) direction = -1;
                if (GameState.OptionData.PadUpDownInverse) direction = -direction;
                vertical = direction * 0.02f;
            }
            bool checkInputLock = true;
            if (SexManager.Instance.IsSexMode && !InputManager.IsGamePad)
            {
                if (InputManager.IsDown(InputManager.InputType.MiddleClick)
                    || InputManager.IsDown(InputManager.InputType.RightClick)
                    || !CursorController.Instance.IsVisible)
                    checkInputLock = false;
            }
            if (checkInputLock && (CursorController.Instance.IsVisible || baseCamera.IsLockCamera
                || global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.isOpenSomeAllowInputUi
                || global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.IsOpenSomeCancelInputUi))
            {
                y = 0f;
                x = 0f;
                if (!SexManager.Instance.IsSexMode) wheel = 0f;
                cameraMove = Vector2.zero;
                skipFrameCameraCount = 4;
                vertical = 0f;
            }
            if (PlayerController.Instance.Pca.PlayerState.IsDownMoveForwardOneHand) wheel = 0f;
            if (skipFrameCameraCount > 0 && cameraMove.sqrMagnitude > 0f)
            {
                --skipFrameCameraCount;
                vertical = 0f;
                wheel = 0f;
                y = 0f;
                x = 0f;
            }
            currentVerticalValue = Mathf.Clamp(currentVerticalValue + vertical, 0.3f, 1.1f);
            if (!InputManager.IsDown(InputManager.InputType.PadShift))
            {
                Yaw = x + Yaw;
                Pitch = Mathf.Clamp(Pitch - y, -80f, 80f);
            }
            if (!player.Pca.PlayerState.IsStrafe)
            {
                if (!InputManager.IsGamePad)
                {
                    float sensitivity = global::ExposureUnnoticed2.Master.Common.MCommon.CameraScrollSensitive;
                    float wheelScale = GameStateFacade.GetOptionWheelScale();
                    float maxDistance = cameraMaxDistance;
                    Vector3 difference = transform.position - cameraTarget.position;
                    cameraMaxDistance = maxDistance - difference.magnitude * wheelScale * sensitivity * 3f * wheel;
                }
                else if (InputManager.IsDown(InputManager.InputType.PadShift))
                {
                    cameraMaxDistance = cameraMove.y * -0.05f + cameraMaxDistance;
                }
                cameraMaxDistance = Mathf.Clamp(cameraMaxDistance, 0.2f, 1.5f);
            }
            float height = (cameraTarget.position.y - player.Pca.Transform.position.y) * currentVerticalValue;
            height = height < 0.3f ? 0.3f : height;
            height += player.Pca.Transform.position.y;
            Vector3 targetPosition = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.GetSetY(cameraTarget.position, height);
            float distance = CalcCameraDistance(targetPosition);
            Quaternion rotation = Quaternion.Euler(Pitch, Yaw, 0f);
            transform.position = targetPosition + rotation * Vector3.forward * (distance * -1f);
            transform.rotation = Quaternion.LookRotation((targetPosition - transform.position).normalized, Vector3.up);
		}

		private float CalcCameraDistance(Vector3 targetPosition)
		{
            Quaternion rotation = Quaternion.Euler(Pitch, Yaw, 0f);
            Vector3 direction = rotation * Vector3.forward * -1f;
            float maxDistance = cameraMaxDistance;
            int mask = ignoreLayerMask;
            float distance = float.MaxValue;
            RaycastHit hit;
            if (Physics.SphereCast(targetPosition, 0.05f, direction, out hit, maxDistance, ~mask, QueryTriggerInteraction.Ignore))
                distance = hit.distance;
            float currentMaxDistance = cameraMaxDistance;
            return currentMaxDistance > distance ? distance : currentMaxDistance;
		}

		public void OnChangeThirdPersonView()
        {
            Yaw = player.transform.rotation.eulerAngles.y;
            Pitch = 0f;
        }

		private void OnCompleteTransitionStageEvent(CompleteTransitionStageEvent completeTransitionStageEvent)
		{
            ResetToPlayerDirect();
		}

		public void ResetToPlayerDirect()
		{
            Scheduler.DelayFlame(() => { Yaw = player.transform.rotation.eulerAngles.y; }, 1);
		}
	}
}
