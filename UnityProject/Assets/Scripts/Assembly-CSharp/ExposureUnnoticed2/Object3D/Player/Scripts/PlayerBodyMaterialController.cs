using ExposureUnnoticed2.Master.BodyPaint;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerBodyMaterialController
	{
		public const int SweatBodyIndex = 5;

		private const float MaxSweatLevel = 0.49f;

		private static readonly int ClippingLevel = Shader.PropertyToID("_Clipping_Level");

		private PlayerClassAccessor pca;

		private SmoothFloat sweatLevel;

		private static readonly int TweakTransparency = Shader.PropertyToID("_Tweak_transparency");

		private static readonly int MatCapColor = Shader.PropertyToID("_MatCapColor");

		private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

		private static readonly int FirstShadeColor = Shader.PropertyToID("_1st_ShadeColor");

		private static readonly int MainTex = Shader.PropertyToID("_MainTex");

		public Material SweatBodyMaterial { get; private set; }

		public Material ClothesPaintMaterial { get; private set; }

		public Material RubberSuitMaterial { get; private set; }

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x637590
			this.pca = pca;
			global::ExposureUnnoticed2.Scripts.Base.EventManager.AddListener<global::ExposureUnnoticed2.Scripts.Base.ChangeBodyEvent>(OnChangeBody, false);
			global::ExposureUnnoticed2.Scripts.Base.EventManager.AddListener<global::ExposureUnnoticed2.Scripts.Base.ChangeCosplayPartsOptionEvent>(OnChangeEvent, false);
			SetupBodyPaintFromGameState();
			ApplyFromGameState();
		}

		public void Dispose()
		{
			// Recovered from GameAssembly.dll RVA 0x637710
			global::ExposureUnnoticed2.Scripts.Base.EventManager.RemoveListener<global::ExposureUnnoticed2.Scripts.Base.ChangeBodyEvent>(OnChangeBody);
			global::ExposureUnnoticed2.Scripts.Base.EventManager.RemoveListener<global::ExposureUnnoticed2.Scripts.Base.ChangeCosplayPartsOptionEvent>(OnChangeEvent);
		}

		public void OnUpdate()
		{
			// SOURCE DRAFT: original637830; SDK Material.SetFloat remains a real call.
			float dokidoki = pca.PlayerState.Dokidoki;
			if (dokidoki < 0f) dokidoki = 0f;
			else if (0.5f < dokidoki) dokidoki = 0.5f;
			sweatLevel.UpdateTo((dokidoki + dokidoki) - 1f);
			var capturedMaterial = SweatBodyMaterial;
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(PlayerBodyMaterialController).TypeHandle);
			var capturedLevel = sweatLevel;
			if (capturedLevel is null || capturedMaterial is null) throw new System.NullReferenceException();
			float current = capturedLevel.Current;
			int property = TweakTransparency;
			capturedMaterial.SetFloat(property, current);
		}

		private Material TransToMaterialAsset(BodyPaintType type)
		{
			// Recovered from GameAssembly.dll RVA 0x637960
			switch ((int)type)
			{
			    case 0: return global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.Inmon;
			    case 1: return global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.Rakugaki;
			    case 2: return global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.TattooDark;
			    case 3: return global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.TattooFlower1;
			    case 4: return global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.TattooFlower2;
			    case 5: return global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.TattooLotus;
			    case 6: return global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.TattooSatanism;
			    case 7: return global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.TattooSlave;
			    case 9: return global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.RubberSuit;
			    default: throw new System.ArgumentOutOfRangeException("type", type, null);
			}
		}

		public void SetupBodyPaintFromGameState()
		{
			// Recovered from GameAssembly.dll RVA 0x637BC0
			var renderer = pca.PlayerAvatarObjectReferencer.bodyMeshRenderer;
			var materials = new System.Collections.Generic.List<Material>();
			materials.Add(renderer.materials[0]);
			materials.Add(renderer.materials[1]);
			foreach (BodyPaintType type in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<BodyPaintType>(System.Enum.GetValues(typeof(BodyPaintType)))))
			{
			    bool active = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipBodyPaint(type);
			    if ((int)type == 8) materials[1].SetColor(MatCapColor, active ? Color.white : Color.black);
			    else if (active) materials.Add(TransToMaterialAsset(type));
			}
			if (pca.PlayerAvatarObjectReferencer.RubberSuitDummy.activeInHierarchy) materials.Add(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.RubberSuit);
			if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerRemainBodyPaintTime > 0f) materials.Add(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.ClothesPaint);
			materials.Add(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.SweatBody);
			renderer.materials = materials.ToArray();
			var array = renderer.materials;
			SweatBodyMaterial = array[array.Length - 1];
			if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerRemainBodyPaintTime > 0f)
			{
			    array = renderer.materials;
			    ClothesPaintMaterial = array[array.Length - 2];
			}
			else ClothesPaintMaterial = null;
			RubberSuitMaterial = null;
			if (pca.PlayerAvatarObjectReferencer.RubberSuitDummy.activeInHierarchy)
			{
			    int offset = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerRemainBodyPaintTime > 0f ? 3 : 2;
			    array = renderer.materials;
			    RubberSuitMaterial = array[array.Length - offset];
			}
			pca.FutanariController.SetupPaint();
			ApplyFromGameState();
		}

		private void OnChangeBody(ChangeBodyEvent evt)
		{
			// Recovered from GameAssembly.dll RVA 0x638640
			SetupBodyPaintFromGameState();
		}

		private void OnChangeEvent(ChangeCosplayPartsOptionEvent evt)
		{
			// Recovered from GameAssembly.dll RVA 0x638650
			if (evt.uniqueId == 10027) ApplyFromGameState();
		}

		private void ApplyFromGameState()
		{
			// Recovered from GameAssembly.dll RVA 0x638680
			if (RubberSuitMaterial == null) return;
			var option = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetCosplayPartsOptionData(10027);
			RubberSuitMaterial.SetTexture(MainTex, option.materialIndex == 0 ? global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.RubberSuitBlack : global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.RubberSuitWhite);
			RubberSuitMaterial.SetColor(MatCapColor, option.boolOption1 ? Color.black : Color.white);
			Color color = new Color(option.r / 255f, option.g / 255f, option.b / 255f, 1f);
			Color.RGBToHSV(color, out float hue, out float saturation, out float value);
			if (saturation > 0f) saturation = Mathf.Clamp01(saturation + (saturation < 0.1f ? saturation : 0.1f));
			value = Mathf.Clamp01(value * 0.9f);
			Color alternate = Color.HSVToRGB(option.shadeColor < 0 ? 0f : 0.627451f, 1f, value, true);
			int shadeMagnitude = option.shadeColor < 0 ? unchecked(-option.shadeColor) : option.shadeColor;
			float amount = shadeMagnitude / 100f;
			Color shadeBase = Color.HSVToRGB(hue, saturation, value, true);
			amount = Mathf.Clamp01(amount);
			RubberSuitMaterial.SetColor(BaseColor, color);
			RubberSuitMaterial.SetColor(FirstShadeColor, Color.LerpUnclamped(shadeBase, alternate, amount));
			pca.FutanariController.SetupCosplayOption();
		}
	// Recovered original member RVA 0x638CE0: .ctor
		public PlayerBodyMaterialController()
        {
            sweatLevel = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(2f, 1f, 0f, false, false, -1f);
        }
	// Recovered original member RVA 0x638DB0: .cctor
		// Recovered original .cctor RVA 0x638DB0: ordered field initializers preserve BeforeFieldInit.
	}
}
