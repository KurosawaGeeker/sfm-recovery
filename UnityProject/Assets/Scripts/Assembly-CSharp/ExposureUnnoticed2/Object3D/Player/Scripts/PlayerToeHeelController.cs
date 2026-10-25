using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerToeHeelController
	{
		private const float TiptoeStandOffset = 0.05f;

		private const float MaxToeRotateX = 90f;

		public const float DefaultHeelBias = -0.035f;

		private PlayerClassAccessor pca;

		private SmoothFloat toeRotateX;

		private SmoothFloat toeActiveFloat;

		private float initToeRotateXL;

		private float initToeRotateXR;

		private Vector3 toeLUp;

		private Vector3 toeRUp;

		public float HeelOffset { get; private set; }

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x3FAB20
			this.pca = pca;
			initToeRotateXL = pca.PlayerAvatarObjectReferencer.ToeL.localRotation.eulerAngles.x;
			initToeRotateXR = pca.PlayerAvatarObjectReferencer.ToeR.localRotation.eulerAngles.x;
			toeLUp = pca.PlayerAvatarObjectReferencer.ToeL.up;
			toeRUp = pca.PlayerAvatarObjectReferencer.ToeR.up;
		}

		public void OnLateUpdate()
		{
            // Original RVA 0x3FAD30: update animated toes before calculating the shoe offset.
            Transform toeL = pca.PlayerAvatarObjectReferencer.ToeL;
            Transform toeR = pca.PlayerAvatarObjectReferencer.ToeR;
            toeRotateX.UpdateTo(pca.PlayerAnimationParameter.ToeDegree);
            HeelOffset = 0f;
            int heelToeShape = 0;
            float leftX = Mathf.Lerp(initToeRotateXL, initToeRotateXL + MaxToeRotateX, toeRotateX.Current);
            toeL.localEulerAngles = new Vector3(leftX,
                toeL.localRotation.eulerAngles.y, toeL.localRotation.eulerAngles.z);
            float rightX = Mathf.Lerp(initToeRotateXR, initToeRotateXR + MaxToeRotateX, toeRotateX.Current);
            toeR.localEulerAngles = new Vector3(rightX,
                toeR.localRotation.eulerAngles.y, toeR.localRotation.eulerAngles.z);
            if (pca.PlayerState.CurrentShoesRecord != null)
            {
                Vector3 pivot = Vector3.up * 0.03f;
                toeL.localPosition += Quaternion.Euler(leftX, 0f, 0f) * pivot - pivot;
                toeR.localPosition += Quaternion.Euler(rightX, 0f, 0f) * pivot - pivot;
                if (!pca.PlayerState.CurrentShoesRecord.IsNotHeelToe)
                    heelToeShape = (int)(pca.PlayerState.CurrentShoesRecord.HeelToeShapeScale * 100f);
                toeActiveFloat.UpdateTo(pca.PlayerAnimationParameter.ToeDegree > 0f ? 1f : 0f);
                HeelOffset = Mathf.Lerp(pca.PlayerState.CurrentShoesRecord.HeelSize,
                    TiptoeStandOffset, toeActiveFloat.Current);
                if (pca.PlayerState.IsCrouch)
                    HeelOffset = 0f;
            }
            HeelOffset += DefaultHeelBias;
            pca.PlayerAvatarObjectReferencer.bodyMeshRenderer.SetBlendShapeWeight(1, heelToeShape);
		}
	// Recovered original member RVA 0x3FB940: .ctor
		public PlayerToeHeelController()
        {
            toeRotateX = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
            toeActiveFloat = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
        }
	}
}
