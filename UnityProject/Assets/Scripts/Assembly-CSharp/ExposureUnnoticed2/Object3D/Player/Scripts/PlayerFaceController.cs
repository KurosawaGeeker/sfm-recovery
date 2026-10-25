using ExposureUnnoticed2.Scripts.Base;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerFaceController
	{
		private enum State
		{
			Idle = 0,
			Iku = 1,
			IkuAfter = 2
		}

		private const int ShapeEyeBig = 199;

		private const int ShapeEyeSmall = 200;

		private const int ShapeEyeUp = 205;

		private const int ShapeEyeYori = 209;

		private const int ShapeMouseA = 242;

		private const int ShapeMouseO = 252;

		private const int ShapeMouthSmile = 261;

		private const int ShapeMouseTangA = 291;

		private const int ShapeMouseTangA2 = 292;

		private const int ShapeMouseTangLong = 301;

		private const int ShapeToothTopX = 318;

		private PlayerClassAccessor pca;

		private SmoothFloat tang;

		private SmoothFloat blink;

		private SmoothFloat fellaMouth01;

		private static readonly int FaceDokidoki = UnityEngine.Animator.StringToHash("FaceDokidoki");

		private static readonly int FaceEcstasy = UnityEngine.Animator.StringToHash("FaceEcstasy");

		private int randCount;

		private float blinkTime;

		private bool isEcstasy;

		private int targetFaceId;

		private static readonly int FaceId = UnityEngine.Animator.StringToHash("FaceId");

		private static readonly int AddEcstasy = UnityEngine.Animator.StringToHash("AddEcstasy");

		private static readonly int IsNoFaceFear = UnityEngine.Animator.StringToHash("IsNoFaceFear");

		private State currentState;

		private float ikuAfterRemainTime;

		public int OverrideFaceId { get; set; }

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x649AC0
			this.pca = pca;
			global::ExposureUnnoticed2.Scripts.Base.EventManager.AddListener<global::ExposureUnnoticed2.Scripts.Base.ChangeBodyEvent>(OnChangeBody, false);
		}

		public void Dispose()
		{
			// Recovered from GameAssembly.dll RVA 0x649BD0
			global::ExposureUnnoticed2.Scripts.Base.EventManager.RemoveListener<global::ExposureUnnoticed2.Scripts.Base.ChangeBodyEvent>(OnChangeBody);
		}

		public void OnUpdate()
		{
		}

		private void UpdateBlink()
		{
		}

		public void OnTransEcstasy()
		{
			// Recovered from GameAssembly.dll RVA 0x64A290
			isEcstasy = true;
		}

		public void OnEndEcstasy()
		{
			// Recovered from GameAssembly.dll RVA 0x64A2A0
			isEcstasy = false;
		}

		public void OnLateUpdate()
		{
		}

		private void OnChangeBody(ChangeBodyEvent evt)
		{
            // Original GameAssembly RVA 0x64B380. Apply the original FaceMesh indices and order.
            var custom = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData;
            var face = pca.PlayerAvatarObjectReferencer.FaceMesh;
            if (!custom.CustomizeAdjustHeight)
            {
                face.SetBlendShapeWeight(401, UnityEngine.Mathf.Clamp((50 - custom.CustomizeLoliFace) * 2, 0, 100));
                face.SetBlendShapeWeight(400, UnityEngine.Mathf.Clamp((custom.CustomizeLoliFace - 50) * 2, 0, 100));
            }
            else
            {
                float autoFace = (1f - UnityEngine.Mathf.Min((custom.CustomizeHeight - 120) / 40f, 1f)) * 100f;
                face.SetBlendShapeWeight(401, UnityEngine.Mathf.Clamp(autoFace * -2f + 100f, 0f, 100f));
                face.SetBlendShapeWeight(400, UnityEngine.Mathf.Clamp((autoFace - 50f) * 2f, 0f, 100f));
            }
            face.SetBlendShapeWeight(379, UnityEngine.Mathf.Clamp((custom.CustomizeEyeSize - 50) * 2, 0, 100));
            face.SetBlendShapeWeight(380, UnityEngine.Mathf.Clamp((50 - custom.CustomizeEyeSize) * 2, 0, 100));
            face.SetBlendShapeWeight(55, UnityEngine.Mathf.Clamp((custom.CustomizeTareme - 50) * 2, 0, 100));
            face.SetBlendShapeWeight(64, UnityEngine.Mathf.Clamp((50 - custom.CustomizeTareme) * 2, 0, 100));
            face.SetBlendShapeWeight(375, UnityEngine.Mathf.Clamp((custom.CustomizeEyeDist - 50) * 2, 0, 100));
            face.SetBlendShapeWeight(376, UnityEngine.Mathf.Clamp((50 - custom.CustomizeEyeDist) * 2, 0, 100));
            face.SetBlendShapeWeight(373, UnityEngine.Mathf.Clamp((custom.CustomizeEyeHeight - 50) * 2, 0, 100));
            face.SetBlendShapeWeight(374, UnityEngine.Mathf.Clamp((50 - custom.CustomizeEyeHeight) * 2, 0, 100));
            int thick = UnityEngine.Mathf.Clamp((custom.CustomizeMatsugeThick - 50) * 2, 0, 100);
            int thin = UnityEngine.Mathf.Clamp((50 - custom.CustomizeMatsugeThick) * 2, 0, 100);
            face.SetBlendShapeWeight(147, thick);
            face.SetBlendShapeWeight(148, thin);
            face.SetBlendShapeWeight(149, thick);
            face.SetBlendShapeWeight(150, thin);
            int longer = UnityEngine.Mathf.Clamp((custom.CustomizeMatsugeLength - 50) * 2, 0, 100);
            int shorter = UnityEngine.Mathf.Clamp((50 - custom.CustomizeMatsugeLength) * 2, 0, 100);
            face.SetBlendShapeWeight(145, longer);
            face.SetBlendShapeWeight(146, shorter);
            face.SetBlendShapeWeight(158, longer);
            face.SetBlendShapeWeight(159, longer);
            face.SetBlendShapeWeight(160, longer);
            face.SetBlendShapeWeight(161, longer);
            face.SetBlendShapeWeight(162, shorter);
            face.SetBlendShapeWeight(163, shorter);
            face.SetBlendShapeWeight(164, shorter);
            face.SetBlendShapeWeight(165, shorter);
            face.SetBlendShapeWeight(166, longer);
            face.SetBlendShapeWeight(167, longer);
            face.SetBlendShapeWeight(168, longer);
            face.SetBlendShapeWeight(169, shorter);
            face.SetBlendShapeWeight(170, shorter);
            // Original 0x64C3B9 writes index 161 again, with the opposite value. Preserve it.
            face.SetBlendShapeWeight(161, shorter);
            face.SetBlendShapeWeight(414, custom.CustomizeEyeDark ? 101 : 0);
            face.SetBlendShapeWeight(363, UnityEngine.Mathf.Clamp((custom.CustomizeMayuThick - 50) * 2, 0, 100));
            face.SetBlendShapeWeight(364, UnityEngine.Mathf.Clamp((50 - custom.CustomizeMayuThick) * 2, 0, 100));
            face.SetBlendShapeWeight(365, custom.CustomizeMayuDog);
            face.SetBlendShapeWeight(393, UnityEngine.Mathf.Clamp((custom.CustomizeCheekFat - 50) * 8, 0, 400));
            face.SetBlendShapeWeight(392, UnityEngine.Mathf.Clamp((50 - custom.CustomizeCheekFat) * 2, 0, 100));
            face.SetBlendShapeWeight(394, UnityEngine.Mathf.Clamp((custom.CustomizeChinLength - 50) * 2, 0, 100));
            face.SetBlendShapeWeight(395, UnityEngine.Mathf.Clamp((50 - custom.CustomizeChinLength) * 2, 0, 100));
            face.SetBlendShapeWeight(388, custom.CustomizeEarElf);
            face.SetBlendShapeWeight(312, custom.CustomizeYaeba ? 100 : 0);
            face.SetBlendShapeWeight(313, custom.CustomizeYaeba ? 100 : 0);
		}
	// Recovered original member RVA 0x64CE40: .ctor
		public PlayerFaceController()
        {
            tang = new SmoothFloat(0.3f, 100f, 0f, false, false, -1f);
            blink = new SmoothFloat(0.12f, 180f, 0f, false, false, -1f);
            fellaMouth01 = new SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
            OverrideFaceId = -1;
        }
	// Recovered original member RVA 0x64D060: .cctor
		// Recovered original .cctor RVA 0x64D060: ordered field initializers preserve BeforeFieldInit.
	}
}
