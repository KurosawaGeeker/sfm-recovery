using ExposureUnnoticed2.Scripts.Base;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerKneeAdjuster
	{
		private PlayerClassAccessor pca;

		private SmoothFloat kneeSlide;

		public float KneeOffsetY { get; private set; }

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x426C30
			this.pca = pca;
		}

		public void OnLateUpdate()
		{
            // Original RVA 0x6506F0: apply animated knee slide in world coordinates.
            var bones = pca.PlayerAvatarObjectReferencer;
            global::UnityEngine.Transform left = bones.LowerLegL;
            global::UnityEngine.Transform right = bones.LowerLegR;
            kneeSlide.UpdateTo(pca.PlayerAnimationParameter.KneeSlide);
            global::UnityEngine.Vector3 leftOffset = left.forward * -0.05f * kneeSlide.Current;
            global::UnityEngine.Vector3 rightOffset = right.forward * -0.05f * kneeSlide.Current;
            left.position += leftOffset;
            right.position += rightOffset;
            KneeOffsetY = global::UnityEngine.Mathf.Max(
                global::UnityEngine.Vector3.Dot(global::UnityEngine.Vector3.down, leftOffset), 0f);
		}
	// Recovered original member RVA 0x650A80: .ctor
		public PlayerKneeAdjuster()
        {
            kneeSlide = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
        }
	}
}
