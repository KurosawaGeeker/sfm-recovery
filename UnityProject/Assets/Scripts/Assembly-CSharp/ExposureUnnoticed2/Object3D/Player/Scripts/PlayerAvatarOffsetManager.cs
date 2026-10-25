using ExposureUnnoticed2.Master.Action;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;
using ExposureUnnoticed2.Scripts.Base;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerAvatarOffsetManager
	{
		private PlayerClassAccessor pca;

		private SmoothFloat offsetY;

		public float OffsetY { get; private set; }

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x426C30
			this.pca = pca;
		}

		public void OnUpdate()
		{
		}

		public void OnLateUpdate()
		{
            // Original RVA 0x630460: propagate the heel value through the original avatar/camera offset rules.
            OffsetY = pca.PlayerToeHeelController.HeelOffset + pca.PlayerBodyCustomizeAdjuster.AvatarYOffset
                + pca.PlayerKneeAdjuster.KneeOffsetY + SexManager.Instance.Offset.y;
            ActionType action = pca.PlayerState.CurrentAction.Type;
            if (action == ActionType.UseDildoFloorAnal1 || action == ActionType.UseDildoFloorPussy1)
            {
                if (GameState.PlayerCustomData.CustomizeThighsLength < 50)
                    OffsetY += (float)unchecked(GameState.PlayerCustomData.CustomizeThighsLength - 50) * -0.0015f;
                if (GameState.PlayerCustomData.CustomizeLowerLegLength < 50)
                    OffsetY += (float)unchecked(GameState.PlayerCustomData.CustomizeLowerLegLength - 50) * -0.0015f;
            }
            if (pca.PlayerState.CurrentAction.IsIgnoreYOffset)
                OffsetY = 0f;
            if (pca.PlayerState.IsCrouch)
                OffsetY = Mathf.Max(pca.PlayerBodyCustomizeAdjuster.AvatarCrouchYOffset,
                    pca.PlayerBodyCustomizeAdjuster.AvatarCrouchYOffsetB);
            float actionOffset = 0f;
            if (pca.PlayerState.CurrentAction.Type == ActionType.SexStandBack)
                actionOffset = (float)unchecked(GameState.PlayerCustomData.CustomizeThighsLength - 50) * 0.0002f
                    + (pca.PlayerBodyCustomizeAdjuster.HeightScale - 1f) * -0.9f
                    + (float)unchecked(GameState.PlayerCustomData.CustomizeLowerLegLength - 50) * 0.0002f;
            offsetY.UpdateTo(actionOffset + OffsetY);
            Transform avatar = pca.AvatorTransform;
            avatar.localPosition = avatar.TransformDirection(SexManager.Instance.Offset).GetSetY(offsetY.Current);
            Vector3 cameraParentPosition = pca.PlayerAvatarObjectReferencer.CameraTarget.parent.position;
            pca.PlayerAvatarObjectReferencer.CameraTarget.parent.position =
                new Vector3(cameraParentPosition.x, cameraParentPosition.y - offsetY.Current, cameraParentPosition.z);
		}
	// Recovered original member RVA 0x630AA0: .ctor
		public PlayerAvatarOffsetManager()
        {
            offsetY = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
        }
	}
}
