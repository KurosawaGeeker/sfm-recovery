using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Other
{
	public class FutanariController
	{
		private PlayerClassAccessor pca;

		private SmoothFloat activeScale;

		private PlayerAvatarObjectReferencer referencer;

		private Animator animator;

		private SmoothFloat openBlend;

		private SmoothFloat dynamicBlend;

		private SmoothFloat lookAtBlend;

		private SmoothFloat thumbBlend;

		private SmoothFloat scaleSmooth;

		private DynamicBone dynamicBone;

		private Quaternion initLookAtRotation;

		private static readonly int OpenBlend = Animator.StringToHash("OpenBlend");

		private Vector3 peeLocalPosition;

		private Quaternion peeLocalRotation;

		private Vector3 peeLocalScale;

		private static readonly int IsStandPee = Animator.StringToHash("IsStandPee");

		private static readonly int IsKaikyakuPee = Animator.StringToHash("IsKaikyakuPee");

		private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

		private static readonly int StShadeColor = Shader.PropertyToID("_1st_ShadeColor");

		private float lookAtDelayRemainTime;

		private static readonly int IsCrouchUp = Animator.StringToHash("IsCrouchUp");

		private static readonly int MatCapColor = Shader.PropertyToID("_MatCapColor");

		private static readonly int IsGamanOffest = Animator.StringToHash("IsGamanOffset");

		private static readonly int IsCrouchOffset = Animator.StringToHash("IsCrouchOffset");

		private static readonly int FirstShadeColor = Shader.PropertyToID("_1st_ShadeColor");

		private bool currentPeeParentIsFuta;

		private float saoCustomizeScale;

		public Material TamaClothesPaintMaterial { get; private set; }

		public Material SaoClothesPaintMaterial { get; private set; }

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x3FEF90
			this.pca = pca;
			global::ExposureUnnoticed2.Scripts.Base.EventManager.AddListener<global::ExposureUnnoticed2.Scripts.Base.ChangeBodyEvent>(OnChangeBody, false);
			referencer = pca.PlayerAvatarObjectReferencer;
			dynamicBone = referencer.FutanariSaoParent.GetComponentInChildren<DynamicBone>();
			animator = referencer.FutanariObject.GetComponent<Animator>();
			peeLocalPosition = referencer.PeeParticle.transform.localPosition;
			peeLocalRotation = referencer.PeeParticle.transform.localRotation;
			peeLocalScale = referencer.PeeParticle.transform.localScale;
			float active = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveFutanari ? 1f : 0f;
			activeScale.Current = active;
			activeScale.Target = active;
			referencer.PeeParticle.transform.parent = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveFutanari ? referencer.PeeParent : referencer.Hip;
			referencer.PeeParticle.transform.localPosition = peeLocalPosition;
			referencer.PeeParticle.transform.localRotation = peeLocalRotation;
			referencer.PeeParticle.transform.localScale = peeLocalScale;
			Material[] sao = referencer.FutanariMeshSao.materials;
			Material[] tama = referencer.FutanariMeshTama.materials;
			referencer.FutanariMeshSao.materials = sao;
			referencer.FutanariMeshTama.materials = tama;
			initLookAtRotation = referencer.FutanariLockAtDirect.localRotation;
		}

		public void Dispose()
		{
			// Recovered from GameAssembly.dll RVA 0x3FFA40
			global::ExposureUnnoticed2.Scripts.Base.EventManager.AddListener<global::ExposureUnnoticed2.Scripts.Base.ChangeBodyEvent>(OnChangeBody, false);
		}

		public void ChangeActive(bool isActive)
		{
		}

		private void MovePeeParticle(bool isFuta)
		{
		}

		public void OnUpdate()
		{
		}

		public void OnLateUpdate()
		{
		}

		private void OnChangeBody(ChangeBodyEvent evt)
		{
		}

		private Color Calc1stShadeColor(Color color)
		{
			// Recovered from GameAssembly.dll RVA 0x400D50
			Color.RGBToHSV(color, out float hue, out float saturation, out float value);
			if (saturation > 0f) saturation = Mathf.Clamp01(saturation + (saturation < 0.1f ? saturation : 0.1f));
			value = Mathf.Clamp01(value * 0.9f);
			return Color.HSVToRGB(hue, saturation, value, true);
		}

		public void OnChangeCosplay()
		{
			// Recovered from GameAssembly.dll RVA 0x400EA0
			var sao = pca.PlayerAvatarObjectReferencer.FutanariMeshSao.gameObject;
			sao.SetActive(!pca.PlayerState.IsHideFutaByCosplay);
			var tama = pca.PlayerAvatarObjectReferencer.FutanariMeshTama.gameObject;
			tama.SetActive(!pca.PlayerState.IsHideFutaByCosplay);
		}

		public void FireSperm()
		{
		}

		public void SetupPaint()
		{
			// Recovered from GameAssembly.dll RVA 0x401350
			Color primary = global::ExposureUnnoticed2.Master.Common.MCustomizeColor.Instance.SkinPattern[global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColor].BodyPrimary;
			Color secondary = global::ExposureUnnoticed2.Master.Common.MCustomizeColor.Instance.SkinPattern[global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColor].BodySecondary;
			var custom = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData;
			if (custom.CustomIsSkinColorFree)
			{
			    primary = new Color(custom.CustomizeSkinColorR / 255f, custom.CustomizeSkinColorG / 255f, custom.CustomizeSkinColorB / 255f, 1f);
			    secondary = Calc1stShadeColor(primary);
			}
			var sao = pca.PlayerAvatarObjectReferencer.FutanariMeshSao;
			var saoMaterials = new System.Collections.Generic.List<Material>();
			if (!global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipCosplayParts(10027))
			{
			    saoMaterials.Add(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.FutanariMaterial);
			    saoMaterials[0].SetColor(MatCapColor, global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipBodyPaint((global::ExposureUnnoticed2.Master.BodyPaint.BodyPaintType)8) ? Color.white : Color.black);
			    saoMaterials[0].SetColor(BaseColor, primary);
			    saoMaterials[0].SetColor(StShadeColor, secondary);
			}
			else saoMaterials.Add(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.FutanariRubberMaterial);
			if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerRemainBodyPaintTime > 0f && !pca.PlayerAvatarObjectReferencer.RubberSuitDummy.activeInHierarchy)
			    saoMaterials.Add(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.FutanariPaint);
			sao.materials = saoMaterials.ToArray();
			SaoClothesPaintMaterial = saoMaterials[saoMaterials.Count - 1];
			var tama = pca.PlayerAvatarObjectReferencer.FutanariMeshTama;
			var tamaMaterials = new System.Collections.Generic.List<Material>();
			if (!global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipCosplayParts(10027))
			{
			    tamaMaterials.Add(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.FutanariMaterial);
			    tamaMaterials[0].SetColor(MatCapColor, global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipBodyPaint((global::ExposureUnnoticed2.Master.BodyPaint.BodyPaintType)8) ? Color.white : Color.black);
			    tamaMaterials[0].SetColor(BaseColor, primary);
			    tamaMaterials[0].SetColor(StShadeColor, secondary);
			}
			else tamaMaterials.Add(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.FutanariRubberMaterial);
			tamaMaterials.Add(tama.materials[1]);
			if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerRemainBodyPaintTime > 0f && !pca.PlayerAvatarObjectReferencer.RubberSuitDummy.activeInHierarchy)
			    tamaMaterials.Add(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.FutanariPaint);
			tama.materials = tamaMaterials.ToArray();
			TamaClothesPaintMaterial = tamaMaterials[tamaMaterials.Count - 1];
		}

		public void SetupCosplayOption()
		{
			// Recovered from GameAssembly.dll RVA 0x402090
			if (!pca.PlayerAvatarObjectReferencer.RubberSuitDummy.activeInHierarchy) return;
			SetupPaint();
			var saoMaterial = pca.PlayerAvatarObjectReferencer.FutanariMeshSao.materials[0];
			var option = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetCosplayPartsOptionData(10027);
			saoMaterial.SetColor(MatCapColor, option.boolOption1 ? Color.black : Color.white);
			float factor = option.materialIndex == 0 ? 0f : 0.8f;
			Color color = new Color((option.r / 255f) * factor, (option.g / 255f) * factor, (option.b / 255f) * factor, 1f);
			Color.RGBToHSV(color, out float hue, out float saturation, out float value);
			if (saturation > 0f) saturation = Mathf.Clamp01(saturation + (saturation < 0.1f ? saturation : 0.1f));
			value = Mathf.Clamp01(value * 0.9f);
			Color alternate = Color.HSVToRGB(option.shadeColor < 0 ? 0f : 0.627451f, 1f, value, true);
			int magnitude = option.shadeColor < 0 ? unchecked(-option.shadeColor) : option.shadeColor;
			float amount = magnitude / 100f;
			Color shadeBase = Color.HSVToRGB(hue, saturation, value, true);
			amount = Mathf.Clamp01(amount);
			Color shade = Color.LerpUnclamped(shadeBase, alternate, amount);
			saoMaterial.SetColor(BaseColor, color);
			saoMaterial.SetColor(FirstShadeColor, shade);
			var tamaMaterial = pca.PlayerAvatarObjectReferencer.FutanariMeshTama.materials[0];
			tamaMaterial.SetColor(MatCapColor, option.boolOption1 ? Color.black : Color.white);
			tamaMaterial.SetColor(BaseColor, color);
			tamaMaterial.SetColor(FirstShadeColor, shade);
		}
	// Recovered original member RVA 0x4028F0: .ctor
		public FutanariController()
        {
            activeScale = new SmoothFloat(0.5f, 1f, 0f, false, false, -1f);
            openBlend = new SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
            dynamicBlend = new SmoothFloat(0.5f, 1f, 0f, false, false, -1f);
            lookAtBlend = new SmoothFloat(0.5f, 1f, 0f, false, false, -1f);
            thumbBlend = new SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
            scaleSmooth = new SmoothFloat(0.1f, 1f, 0f, false, false, -1f);
            saoCustomizeScale = 1f;
        }
	// Recovered original member RVA 0x402CE0: .cctor
		// Recovered original .cctor RVA 0x402CE0: ordered field initializers preserve BeforeFieldInit.
	}
}
