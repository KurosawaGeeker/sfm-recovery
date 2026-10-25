using System.Collections.Generic;
using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerBodyCustomizeAdjuster
	{
		public const int DefaultHeight = 160;

		public const int MinHeight = 120;

		public const int MaxHeight = 200;

		private PlayerClassAccessor pca;

		private float defaultHeadScale;

		private float defaultBaseScale;

		private Transform avatarTransform;

		private SkinnedMeshRenderer bodyMesh;

		private Material bodyMaterial;

		private Material faceMaterial;

		private Material fixedColorAreaMaterial;

		private Material eyeRightMaterial;

		private Material eyeLeftMaterial;

		private Material eyeLashMaterial;

		private Material eyeBlowMaterial;

		private Material faceOptionMaterial;

		private SmoothFloat pussyShape01;

		private SmoothFloat analShape01;

		private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

		private static readonly int StShadeColor = Shader.PropertyToID("_1st_ShadeColor");

		private static readonly int MainTex = Shader.PropertyToID("_MainTex");

		private static readonly int EmissiveTex = Shader.PropertyToID("_Emissive_Tex");

		public Dictionary<ShrinkType, bool> shrinkCacheDict;

		private List<ShrinkType> keyList;

		private Quaternion initBreastRotateL;

		private Quaternion initBreastRotateR;

		private static readonly int EmissiveColor = Shader.PropertyToID("_Emissive_Color");

		public float AvatarYOffset { get; private set; }

		public float AvatarCrouchYOffset { get; private set; }

		public float AvatarCrouchYOffsetB { get; private set; }

		public int BigBreastSize { get; private set; }

		public int SmallBreastSize { get; private set; }

		public int IntegratedBreastSize { get; private set; }

		public float HeightScale { get; private set; }

		public float LowerLegLength { get; private set; }

		public float ThighsLength { get; private set; }

		public float UpperLength { get; private set; }

		public float UpperLength2 { get; private set; }

		public float MoveSpeed { get; private set; }

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x631C30
			this.pca = pca;
			global::ExposureUnnoticed2.Scripts.Base.EventManager.AddListener<global::ExposureUnnoticed2.Scripts.Base.ChangeBodyEvent>(OnChangeBody, false);
			bodyMesh = pca.PlayerAvatarObjectReferencer.bodyMeshRenderer;
			avatarTransform = pca.PlayerAnimationReceiver.transform;
			defaultBaseScale = avatarTransform.localScale.x;
			defaultHeadScale = pca.PlayerAvatarObjectReferencer.Head.localScale.x;
			Renderer body = pca.PlayerAvatarObjectReferencer.bodyMeshRenderer.GetComponent<Renderer>();
			for (int i = 0; i < body.materials.Length; ++i)
			{
			    body.materials[i] = UnityEngine.Object.Instantiate(body.materials[i]);
			    if (i == 1) bodyMaterial = body.materials[i];
			}
			Renderer face = pca.PlayerAvatarObjectReferencer.FaceMesh.GetComponent<Renderer>();
			for (int i = 0; i < face.materials.Length; ++i)
			{
			    face.materials[i] = UnityEngine.Object.Instantiate(face.materials[i]);
			    if (i == 0) fixedColorAreaMaterial = face.materials[i];
			    else if (i == 1) eyeLashMaterial = face.materials[i];
			    else if (i == 2) faceMaterial = face.materials[i];
			    else if (i == 3) eyeBlowMaterial = face.materials[i];
			    else if (i == 4) eyeLeftMaterial = face.materials[i];
			    else if (i == 5) eyeRightMaterial = face.materials[i];
			    else if (i == 6) faceOptionMaterial = face.materials[i];
			}
			global::ExposureUnnoticed2.Scripts.Base.EventManager.AddListener<global::ExposureUnnoticed2.Scripts.Base.ChangeCostumeCosplayEvent>(OnChangeCostumeCosplayEvent, false);
			shrinkCacheDict = System.Linq.Enumerable.ToDictionary(System.Linq.Enumerable.Cast<ShrinkType>(System.Enum.GetValues(typeof(ShrinkType))), k => k, v => false);
			keyList = new List<ShrinkType>(shrinkCacheDict.Keys);
			initBreastRotateL = pca.PlayerAvatarObjectReferencer.BreastRotateL.localRotation;
			initBreastRotateR = pca.PlayerAvatarObjectReferencer.BreastRotateR.localRotation;
		}

		public void Dispose()
		{
			// Recovered from GameAssembly.dll RVA 0x632C60
			global::ExposureUnnoticed2.Scripts.Base.EventManager.RemoveListener<global::ExposureUnnoticed2.Scripts.Base.ChangeBodyEvent>(OnChangeBody);
			global::ExposureUnnoticed2.Scripts.Base.EventManager.RemoveListener<global::ExposureUnnoticed2.Scripts.Base.ChangeCostumeCosplayEvent>(OnChangeCostumeCosplayEvent);
		}

		private Vector3 CalcFatVector(float scale)
		{
            // Recovered complete original native body.
            return new Vector3(scale, 1f, scale);
        }

		private Vector3 ScaleVectorY(Vector3 v, float scale)
		{
            // Recovered complete original native body.
            return new Vector3(v.x, scale * v.y, v.z);
        }

		private void OnChangeBody(ChangeBodyEvent evt)
		{
            // Recovered complete original native body.
            AvatarYOffset = 0f;
HeightScale = (float)global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeHeight / 160f;
avatarTransform.localScale = Vector3.one * defaultBaseScale * HeightScale;
float headScale;
if (!global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeAdjustHeight)
    headScale = ((float)global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeHead / 50f - 1f) * 0.2f + 1f;
else
{
    float height = (float)unchecked(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeHeight - 120) / 40f;
    if (1.25f <= height) height = 1.25f;
    headScale = (height - 1f) * -0.1f + 1f;
}
pca.PlayerAvatarObjectReferencer.Head.localScale = Vector3.one * defaultHeadScale * headScale;
int value = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeNeckThick;
float fat = value < 50 ? 1f - ((float)unchecked(50 - value) / 50f) * 0.2f : ((float)unchecked(value - 50) / 50f) * 0.4f + 1f;
pca.PlayerAvatarObjectReferencer.Neck.localScale = new Vector3(fat, 1f, fat);
int integrated = unchecked(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeBoobs +
    (global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSuperBoobs + global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeHyperBoobs * 2) * 2);
IntegratedBreastSize = integrated;
int big = unchecked(integrated * 2 - 100);
if (big < 0) big = 0; else if (2000 < big) big = 2000;
BigBreastSize = big;
int small = unchecked(integrated * 2);
if (small < 0) small = 0; else if (100 < small) small = 100;
SmallBreastSize = small;
bodyMesh.SetBlendShapeWeight(4, SmallBreastSize);
bodyMesh.SetBlendShapeWeight(3, BigBreastSize);
Material material = pca.PlayerAvatarObjectReferencer.bodyMeshRenderer.materials[1];
Texture texture = integrated < 20 ? global::ExposureUnnoticed2.Master.Common.MCustomizeColor.Instance.HinnyuSkinTexture : global::ExposureUnnoticed2.Master.Common.MCustomizeColor.Instance.NormalSkinTexture;
material.SetTexture(MainTex, texture);
int angleA = unchecked(integrated - 200), angleB = unchecked(integrated - 350);
if (angleA < 0) angleA = 0; else if (350 < angleA) angleA = 350;
if (angleB < 0) angleB = 0; else if (350 < angleB) angleB = 350;
float angle = (float)angleB * 0.05f + (float)angleA * -0.1f;
pca.PlayerAvatarObjectReferencer.BreastRotateL.localRotation = Quaternion.Euler(0f, angle, 0f) * initBreastRotateL;
// Both original products use the left initial rotation at +0xC8.
pca.PlayerAvatarObjectReferencer.BreastRotateR.localRotation = Quaternion.Euler(0f, -angle, 0f) * initBreastRotateL;
pca.PlayerTitController.OnChangeCustomizeBar();
Vector3 butt;
value = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeButt;
if (value < 50)
    butt = Vector3.one * (1f - ((float)unchecked(50 - value) / 50f) * 0.5f);
else
{
    float factor = (float)unchecked(value - 50) / 10f;
    if (factor < 0f) factor = 0f; else if (1f < factor) factor = 1f;
    float scale = ((float)unchecked(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeButt - 50) / 50f) * 3f + 1f;
    Vector3 one = Vector3.one;
    butt = new Vector3(scale * ((1f - one.x) * factor + one.x), scale * ((0.75f - one.y) * factor + one.y), scale * ((1f - one.z) * factor + one.z));
}
pca.PlayerAvatarObjectReferencer.ButtL.localScale = butt;
pca.PlayerAvatarObjectReferencer.ButtR.localScale = butt;
fat = ((float)global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeWaist / 100f) * 0.5f + 1f;
pca.PlayerAvatarObjectReferencer.Hip.localScale = new Vector3(fat, 1f, fat);
value = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeThighs;
fat = value < 50 ? 1f - ((float)unchecked(50 - value) / 50f) * 0.2f : ((float)unchecked(value - 50) / 50f) * 0.4f + 1f;
value = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeThighsLength;
ThighsLength = value < 50 ? 1f - ((float)unchecked(50 - value) / 50f) * 0.2f : ((float)unchecked(value - 50) / 50f) * 0.4f + 1f;
pca.PlayerAvatarObjectReferencer.LegL.localScale = new Vector3(fat, ThighsLength, fat);
pca.PlayerAvatarObjectReferencer.LegR.localScale = new Vector3(fat, ThighsLength, fat);
float delta = (float)global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeThighsLength / 50f - 1f;
delta *= global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeThighsLength < 50 ? 0.07f : 0.14f;
AvatarYOffset = AvatarYOffset + delta;
value = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeLowerLeg;
fat = value < 50 ? 1f - ((float)unchecked(50 - value) / 50f) * 0.2f : ((float)unchecked(value - 50) / 50f) * 0.6f + 1f;
value = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeLowerLegLength;
LowerLegLength = value < 50 ? 1f - ((float)unchecked(50 - value) / 50f) * 0.2f : ((float)unchecked(value - 50) / 50f) * 0.4f + 1f;
pca.PlayerAvatarObjectReferencer.LowerLegL.localScale = new Vector3(fat, LowerLegLength, fat);
pca.PlayerAvatarObjectReferencer.LowerLegR.localScale = new Vector3(fat, LowerLegLength, fat);
delta = (float)global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeLowerLegLength / 50f - 1f;
delta *= global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeLowerLegLength < 50 ? 0.07f : 0.14f;
AvatarYOffset = AvatarYOffset + delta;
value = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeUpperBody;
fat = value < 50 ? 1f - ((float)unchecked(50 - value) / 50f) * 0.2f : ((float)unchecked(value - 50) / 50f) * 0.4f + 1f;
value = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeUpperBodyLength;
UpperLength = value < 50 ? 1f - ((float)unchecked(50 - value) / 50f) * 0.2f : ((float)unchecked(value - 50) / 50f) * 0.4f + 1f;
pca.PlayerAvatarObjectReferencer.Spine.localScale = new Vector3(fat, UpperLength, fat);
value = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeUpperBody2;
fat = value < 50 ? 1f - ((float)unchecked(50 - value) / 50f) * 0.2f : ((float)unchecked(value - 50) / 50f) * 0.4f + 1f;
value = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeUpperBodyLength2;
UpperLength2 = value < 50 ? 1f - ((float)unchecked(50 - value) / 50f) * 0.2f : ((float)unchecked(value - 50) / 50f) * 0.4f + 1f;
pca.PlayerAvatarObjectReferencer.Chest.localScale = new Vector3(fat, UpperLength2, fat);
value = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeArm;
fat = value < 50 ? 1f - ((float)unchecked(50 - value) / 50f) * 0.2f : ((float)unchecked(value - 50) / 50f) * 0.6f + 1f;
pca.PlayerAvatarObjectReferencer.UpperArmL.localScale = new Vector3(fat, 1f, fat);
pca.PlayerAvatarObjectReferencer.UpperArmR.localScale = new Vector3(fat, 1f, fat);
pca.PlayerAvatarObjectReferencer.LowerArmL.localScale = new Vector3(fat, 1f, fat);
pca.PlayerAvatarObjectReferencer.LowerArmR.localScale = new Vector3(fat, 1f, fat);
Color primary = global::ExposureUnnoticed2.Master.Common.MCustomizeColor.Instance.SkinPattern[global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColor].BodyPrimary;
Color secondary = global::ExposureUnnoticed2.Master.Common.MCustomizeColor.Instance.SkinPattern[global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColor].BodySecondary;
if (global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomIsSkinColorFree)
{
    primary = new Color((float)global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColorR / 255f, (float)global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColorG / 255f, (float)global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColorB / 255f, 1f);
    secondary = Calc1stShadeColor(primary);
}
bodyMaterial.SetColor(BaseColor, primary);
bodyMaterial.SetColor(StShadeColor, secondary);
pca.PlayerBodyMaterialController.SweatBodyMaterial.SetColor(BaseColor, primary);
pca.PlayerBodyMaterialController.SweatBodyMaterial.SetColor(StShadeColor, secondary);
faceMaterial.SetColor(BaseColor, primary);
faceMaterial.SetColor(StShadeColor, secondary);
faceOptionMaterial.SetColor(BaseColor, primary);
faceOptionMaterial.SetColor(StShadeColor, secondary);
eyeRightMaterial.SetTexture(MainTex, global::ExposureUnnoticed2.Master.Common.MCustomizeColor.Instance.EyeTextures[global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyePattern]);
eyeRightMaterial.SetColor(EmissiveColor, new Color(101f / 255f, 101f / 255f, 101f / 255f, 1f));
eyeRightMaterial.SetTexture(EmissiveTex, global::ExposureUnnoticed2.Master.Common.MCustomizeColor.Instance.EyeEmitTextures[global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyePattern]);
int eyeIndex = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeOddEye ? global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLeftPattern : global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyePattern;
eyeLeftMaterial.SetTexture(MainTex, global::ExposureUnnoticed2.Master.Common.MCustomizeColor.Instance.EyeTextures[eyeIndex]);
eyeLeftMaterial.SetColor(EmissiveColor, new Color(101f / 255f, 101f / 255f, 101f / 255f, 1f));
eyeLeftMaterial.SetTexture(EmissiveTex, global::ExposureUnnoticed2.Master.Common.MCustomizeColor.Instance.EyeEmitTextures[eyeIndex]);
Color brow = new Color((float)global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeBlowColorR / 255f, (float)global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeBlowColorG / 255f, (float)global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeBlowColorB / 255f, 1f);
eyeBlowMaterial.SetColor(BaseColor, brow);
eyeBlowMaterial.SetColor(StShadeColor, Calc1stShadeColor(brow));
Color lash = new Color((float)global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLashColorR / 255f, (float)global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLashColorG / 255f, (float)global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLashColorB / 255f, 1f);
eyeLashMaterial.SetColor(BaseColor, lash);
eyeLashMaterial.SetColor(StShadeColor, Calc1stShadeColor(lash));
float speedDelta = HeightScale - 1f;
speedDelta *= HeightScale <= 1f ? 0.6f : 0.8f;
MoveSpeed = 1f / (speedDelta + 1f);

        }

		private Color Calc1stShadeColor(Color color)
		{
            // Recovered complete original native body.
            Color.RGBToHSV(color, out float h, out float s, out float v);
if (0f < s)
{
    float add = s;
    if (0.1f <= s) add = 0.1f;
    s = add + s;
    if (s < 0f) s = 0f; else if (1f < s) s = 1f;
}
v = v * 0.9f;
if (v < 0f) v = 0f; else if (1f < v) v = 1f;
return Color.HSVToRGB(h, s, v, true);

        }

		public void AdjustMoveSpeed()
		{
			// Recovered from GameAssembly.dll RVA 0x635470
			pca.PlayerAnimationManager.SetMoveSpeed(MoveSpeed);
		}

		public void OnLateUpdate()
		{
            // Recovered complete original native body.
            Transform target = pca.PlayerAvatarObjectReferencer.Neck;
Vector3 position = target.position;
Vector3 axis = pca.PlayerAvatarObjectReferencer.Neck.up;
float amount = (float)unchecked(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeNeck - 50);
target.position = new Vector3((amount * axis.x) * 0.0005f + position.x, (amount * axis.y) * 0.0005f + position.y, (amount * axis.z) * 0.0005f + position.z);
target = pca.PlayerAvatarObjectReferencer.ShoulderL;
position = target.position;
axis = pca.PlayerAvatarObjectReferencer.ShoulderL.up;
amount = (float)unchecked(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeShoulder - 50);
target.position = new Vector3((amount * axis.x) * 0.001f + position.x, (amount * axis.y) * 0.001f + position.y, (amount * axis.z) * 0.001f + position.z);
target = pca.PlayerAvatarObjectReferencer.ShoulderR;
position = target.position;
axis = pca.PlayerAvatarObjectReferencer.ShoulderR.up;
amount = (float)unchecked(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeShoulder - 50);
target.position = new Vector3((axis.x * amount) * 0.001f + position.x, (axis.y * amount) * 0.001f + position.y, (axis.z * amount) * 0.001f + position.z);
target = pca.PlayerAvatarObjectReferencer.LegL;
position = target.position;
axis = pca.PlayerAvatarObjectReferencer.LegL.right;
amount = (float)unchecked(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeLegInterval - 50);
target.position = new Vector3((axis.x * amount) * -0.0004f + position.x, (axis.y * amount) * -0.0004f + position.y, (axis.z * amount) * -0.0004f + position.z);
target = pca.PlayerAvatarObjectReferencer.LegR;
position = target.position;
axis = pca.PlayerAvatarObjectReferencer.LegR.right;
amount = (float)unchecked(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeLegInterval - 50);
target.position = new Vector3((axis.x * amount) * 0.0004f + position.x, (axis.y * amount) * 0.0004f + position.y, (axis.z * amount) * 0.0004f + position.z);
target = pca.PlayerAvatarObjectReferencer.LowerLegL;
position = target.localPosition;
amount = ThighsLength;
target.localPosition = new Vector3(position.x * amount, position.y * amount, position.z * amount);
target = pca.PlayerAvatarObjectReferencer.LowerLegR;
position = target.localPosition;
amount = ThighsLength;
target.localPosition = new Vector3(position.x * amount, position.y * amount, position.z * amount);
target = pca.PlayerAvatarObjectReferencer.FootL;
position = target.localPosition;
amount = LowerLegLength;
target.localPosition = new Vector3(position.x * amount, position.y * amount, amount * position.z);
target = pca.PlayerAvatarObjectReferencer.FootR;
position = target.localPosition;
amount = LowerLegLength;
target.localPosition = new Vector3(amount * position.x, amount * position.y, amount * position.z);
target = pca.PlayerAvatarObjectReferencer.Chest;
position = target.localPosition;
amount = UpperLength;
target.localPosition = new Vector3(amount * position.x, amount * position.y, amount * position.z);
bool open = pca.PlayerState.CurrentAction.IsUsePussy || pca.PlayerAdultGoodsController.IsSlideManko || pca.PlayerCosplayManager.IsSlideManko;
pussyShape01.UpdateTo(open ? 1f : 0f);
pca.PlayerAvatarObjectReferencer.bodyMeshRenderer.SetBlendShapeWeight(34, (int)(pussyShape01.Current * 100f));
open = pca.PlayerState.CurrentAction.IsUseAnal || pca.PlayerAdultGoodsController.IsSlideAnal || pca.PlayerCosplayManager.IsSlideAnal;
analShape01.UpdateTo(open ? 1f : 0f);
pca.PlayerAvatarObjectReferencer.bodyMeshRenderer.SetBlendShapeWeight(35, (int)(analShape01.Current * 200f));

        }

		public void ForceShrink(bool isShrink)
		{
		}

		public void OnChangeCostumeCosplayEvent(ChangeCostumeCosplayEvent evt)
		{
			foreach (ShrinkType key in keyList)
				shrinkCacheDict[key] = false;

			int minBreastSize = 0;
			bool isStocking = false;
			bool isCorset = false;
			bool hideHair = false;
			foreach (KeyValuePair<int, bool> equipped in global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCosplayPartsDict)
			{
				if (!equipped.Value) continue;
				RCosplayParts parts = MCosplay.GetParts(equipped.Key);
				ShrinkType[] shrinkTypes = parts.ShrinkTypes;
				for (int i = 0; i < shrinkTypes.Length; ++i)
					shrinkCacheDict[shrinkTypes[i]] = true;
				if (minBreastSize <= parts.MinBreastSize)
					minBreastSize = parts.MinBreastSize;
				isStocking = isStocking | parts.IsStocking;
				isCorset = isCorset | parts.IsCorset;
				bool useHairHide = true;
				if (parts.IsHideHair)
					useHairHide = !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetCosplayPartsOptionData(parts.UniqueId).boolOption1;
				hideHair = hideHair | (parts.IsHideHair & useHairHide);
			}

			foreach (KeyValuePair<ShrinkType, bool> shrink in shrinkCacheDict)
				bodyMesh.SetBlendShapeWeight(unchecked((int)shrink.Key + 13), shrink.Value ? 100f : 0f);

			int breastSize = unchecked(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeBoobs +
				(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSuperBoobs +
				global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeHyperBoobs * 2) * 2);
			if (breastSize < minBreastSize)
			{
				string message = Localizer.Trans("log_adjust_breast_size");
				global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(message,
					global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType.Log);
				global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeBoobs = unchecked(
					global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeBoobs - breastSize + minBreastSize);
				EventManager.Broadcast(Events.ChangeBodyEvent);
			}

			bodyMesh.SetBlendShapeWeight(0, isStocking ? 100f : 0f);
			bodyMesh.SetBlendShapeWeight(2, isCorset ? 100f : 0f);
			pca.PlayerAvatarObjectReferencer.HairParent.SetActive(!hideHair);
			EventManager.Broadcast(Events.UpdateBodyShrinkEvent);
		}
	// Recovered original member RVA 0x637080: .ctor
		public PlayerBodyCustomizeAdjuster()
        {
            pussyShape01 = new SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
            analShape01 = new SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
            shrinkCacheDict = new Dictionary<ShrinkType, bool>();
        }
	// Recovered original member RVA 0x6372A0: .cctor
		// Recovered original .cctor RVA 0x6372A0: ordered field initializers preserve BeforeFieldInit.
	}
}
