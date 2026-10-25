using ExposureUnnoticed2.Object3D.NPC.Script;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerStrangenessController
	{
		public const float NightBareyasusa = 0.4f;

		public const float DaytimeBareyasusa = 0.9f;

		public const float MaxBareyasusa = 1f;

		public const float MaxBareyasusaIncludeBoost = 1.5f;

		private PlayerClassAccessor pca;

		private bool isBlockByCostume;

		private bool cacheIsBlockByCostumeFront;

		private bool cacheIsBlockByCostumeFrontBoob;

		private bool cacheIsBlockByCostumeFrontFuta;

		private float cacheMaxDiscomfortFront;

		private float cacheMaxDiscomfortSide;

		private float cacheMaxDiscomfortBack;

		private float cacheStrangeFront;

		private float cacheStrangeFrontBoob;

		private float cacheStrangeFrontFuta;

		private float cacheStrangeSide;

		private float cacheStrangeBack;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x3CAB80
			this.pca = pca;
		}

		public void OnUpdate()
		{
            // SOURCE DRAFT: complete original PlayerStrangeness family, awaiting full semantic proof.
            float baseScale = pca.PlayerState.ReinforceEffectData.Bareyasusa100 / 100f + pca.PlayerState.AddBareyasusaByCosplay01 + 1f;
pca.PlayerState.BareyasusaFromStreetLight = 0f;
foreach (var light in pca.PlayerState.TouchedLightList)
{
    var state = pca.PlayerState;
    float current = state.BareyasusaFromStreetLight;
    state.BareyasusaFromStreetLight = light.CalcBareyasusa(pca.GameObject) + current;
}
float illumination;
if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime) illumination = 1f;
else
{
    float fromLight = pca.PlayerState.BareyasusaFromStreetLight;
    var stage = global::ExposureUnnoticed2.Master.Stage.MStage.Get(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType);
    illumination = Mathf.Max(fromLight, stage.DefaultLightBareyasusaScale100 * 0.01f);
}
illumination = Mathf.Clamp01(illumination);
pca.PlayerState.BaseBareyasusa = Mathf.Clamp(baseScale, 0.1f, 3f);
pca.PlayerState.Bareyasusa = illumination * pca.PlayerState.BaseBareyasusa;
CalcCacheMaxDiscomfort();

        }

		public StrangeData CalcStrangerDegreeFrom(Transform npcTransform, Transform head, NpcComponentAccessor nca)
		{
            // SOURCE DRAFT: complete original PlayerStrangeness family, awaiting full semantic proof.
            StrangeData result = default;
float expose = CalcExposeStrange(npcTransform.position);
float actionScale = CalcActionStrangeScale();
float actionStrange = pca.PlayerState.CurrentAction.Strange;
float sightScale = CalcSightScale(npcTransform, head, nca);
result.StrangerDegree = (actionScale * expose + actionStrange) * pca.PlayerState.Bareyasusa * sightScale;
result.MaxDiscomfort = CalcMaxDiscomfort(npcTransform.position);
result.AddDiscomfort = result.MaxDiscomfort * pca.PlayerState.Bareyasusa * sightScale;
result.Noisy = CalcNoisy(npcTransform.position);
result.OnlySightScale = sightScale;
if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveInvisible)
{
    result.AddDiscomfort = 0f;
    result.MaxDiscomfort = 0f;
    result.StrangerDegree = 0f;
}
pca.PlayerState.IsLookedFlashing = pca.PlayerState.IsLookedFlashing || pca.PlayerState.IsLookedUpper || pca.PlayerState.IsLookedHip;
return result;

        }

		private float CalcActionStrangeScale()
		{
            // SOURCE DRAFT: complete original PlayerStrangeness family, awaiting full semantic proof.
            if ((int)pca.PlayerState.CurrentAction.Type == -1) return 1f;
return pca.PlayerState.CurrentAction.StrangenessScale;

        }

		private float CalcNoisy(Vector3 npcPosition)
		{
            // SOURCE DRAFT: complete original PlayerStrangeness family, awaiting full semantic proof.
            float distance = pca.GameObject.DistSqrt(npcPosition);
float attenuation = (global::ExposureUnnoticed2.Master.Common.MCommon.Instance.ListenNoisyDist - distance + 1f) / global::ExposureUnnoticed2.Master.Common.MCommon.Instance.ListenNoisyDist;
attenuation = Mathf.Clamp01(attenuation);
float noisy = 0f;
if (global::ExposureUnnoticed2.Object3D.AdultGoods.CommonVibratorController.Leader)
{
    if ((int)global::ExposureUnnoticed2.Object3D.AdultGoods.CommonVibratorController.VibrationStrength == 2)
        noisy = global::ExposureUnnoticed2.Master.Common.MCommon.Instance.StrangeVibratorHigh;
    if ((int)global::ExposureUnnoticed2.Object3D.AdultGoods.CommonVibratorController.VibrationStrength == 1)
        noisy = global::ExposureUnnoticed2.Master.Common.MCommon.Instance.StrangeVibratorLow;
}
noisy = noisy + 0f;
if (pca.PlayerState.IsPeeing) noisy = noisy + 0.03f;
return attenuation * noisy;

        }

		private float CalcSightScale(Transform npcTransform, Transform head, NpcComponentAccessor nca)
		{
            // SOURCE DRAFT: complete original PlayerStrangeness family, awaiting full semantic proof.
            float distance = (pca.Transform.position - npcTransform.position).magnitude;
// Original accesses GameStateData even though no field participates in this distance term.
var capturedGameState = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
if (capturedGameState is null) throw new global::System.NullReferenceException();
float distanceScale = Mathf.Clamp01(1f - (distance - 4f) * 0.06666667f);
Vector3 forward = head.forward;
forward.y = 0f;
forward = forward.normalized;
Vector3 direction = pca.PlayerAvatarObjectReferencer.Head.position - head.position;
direction.y = 0f;
direction = direction.normalized;
float look = Mathf.Clamp01(Vector3.Dot(direction, forward));
float proximity = 0f;
if ((int)nca.StateModel.CurrentState != 0)
    proximity = Mathf.Clamp01(1f - (distance - 0f) * 0.5f);
float combined = Mathf.Clamp01(proximity * proximity + look);
float distanceFactor = distanceScale * distanceScale * distanceScale;
return combined * combined * combined * combined * combined * distanceFactor;

        }

		private float CalcMaxDiscomfort(Vector3 npcPosition)
		{
            // SOURCE DRAFT: complete original PlayerStrangeness family, awaiting full semantic proof.
            float result = 0f;
Vector3 direction = npcPosition - pca.PlayerState.CacheForNpcPosition;
direction.y = 0f;
direction = direction.normalized;
float dot = Vector3.Dot(direction, pca.PlayerState.CacheForNpcForward);
if (dot >= 0.7f) result = cacheMaxDiscomfortFront;
if (dot <= -0.7f) result = cacheMaxDiscomfortBack;
if (dot > -0.7f && dot < 0.7f) result = cacheMaxDiscomfortSide;
return result;

        }

		private void CalcCacheMaxDiscomfort()
		{
            // SOURCE DRAFT: complete original PlayerStrangeness family, awaiting full semantic proof.
            cacheMaxDiscomfortSide = 0f;
cacheMaxDiscomfortBack = 0f;
cacheMaxDiscomfortFront = 0f;
cacheMaxDiscomfortFront = Mathf.Max(0f, pca.PlayerState.CurrentAction.Discomfort);
cacheMaxDiscomfortSide = Mathf.Max(0f, pca.PlayerState.CurrentAction.Discomfort);
cacheMaxDiscomfortBack = Mathf.Max(0f, pca.PlayerState.CurrentAction.Discomfort);
foreach (var pair in global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.MAdultGoodsDict)
{
    if (!pair.Value) continue;
    var record = global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoods.Get(pair.Key);
    if (IsInDirect(record.DiscomfortDirect, true, false, false)) cacheMaxDiscomfortFront = Mathf.Max(cacheMaxDiscomfortFront, record.Discomfort);
    if (IsInDirect(record.DiscomfortDirect, false, true, false)) cacheMaxDiscomfortBack = Mathf.Max(cacheMaxDiscomfortBack, record.Discomfort);
    if (IsInDirect(record.DiscomfortDirect, false, false, true)) cacheMaxDiscomfortSide = Mathf.Max(cacheMaxDiscomfortSide, record.Discomfort);
}
if (!global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)24))
{
    foreach (var pair in global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCosplayPartsDict)
    {
        if (!pair.Value) continue;
        var record = global::ExposureUnnoticed2.Master.Cosplay.MCosplay.GetParts(pair.Key);
        if (IsInDirect(record.DiscomfortDirect, true, false, false)) cacheMaxDiscomfortFront = Mathf.Max(cacheMaxDiscomfortFront, record.Discomfort);
        if (IsInDirect(record.DiscomfortDirect, false, true, false)) cacheMaxDiscomfortBack = Mathf.Max(cacheMaxDiscomfortBack, record.Discomfort);
        if (IsInDirect(record.DiscomfortDirect, false, false, true)) cacheMaxDiscomfortSide = Mathf.Max(cacheMaxDiscomfortSide, record.Discomfort);
    }
}
if (pca.PlayerState.CurrentShoesRecord is null)
{
    cacheMaxDiscomfortFront = Mathf.Max(cacheMaxDiscomfortFront, 0.25f);
    cacheMaxDiscomfortSide = Mathf.Max(cacheMaxDiscomfortSide, 0.25f);
    cacheMaxDiscomfortBack = Mathf.Max(cacheMaxDiscomfortBack, 0.25f);
}
if (isBlockByCostume)
{
    cacheMaxDiscomfortFront = Mathf.Max(cacheMaxDiscomfortFront, pca.PlayerState.MaxLookHiddenPartMaxStrange);
    cacheMaxDiscomfortSide = Mathf.Max(cacheMaxDiscomfortSide, pca.PlayerState.MaxLookHiddenPartMaxStrange);
    cacheMaxDiscomfortBack = Mathf.Max(cacheMaxDiscomfortBack, pca.PlayerState.MaxLookHiddenPartMaxStrange);
}
if (pca.PlayerState.IsCrouch)
{
    cacheMaxDiscomfortFront = Mathf.Max(cacheMaxDiscomfortFront, 0.4f);
    cacheMaxDiscomfortSide = Mathf.Max(cacheMaxDiscomfortSide, 0.4f);
    cacheMaxDiscomfortBack = Mathf.Max(cacheMaxDiscomfortBack, 0.4f);
}
if (pca.PlayerState.IsGamanBaibu)
{
    cacheMaxDiscomfortFront = Mathf.Max(cacheMaxDiscomfortFront, 0.35f);
    cacheMaxDiscomfortSide = Mathf.Max(cacheMaxDiscomfortSide, 0.35f);
    cacheMaxDiscomfortBack = Mathf.Max(cacheMaxDiscomfortBack, 0.35f);
}
if (pca.PlayerState.IsEcstasyMotion)
{
    cacheMaxDiscomfortFront = Mathf.Max(cacheMaxDiscomfortFront, 0.4f);
    cacheMaxDiscomfortSide = Mathf.Max(cacheMaxDiscomfortSide, 0.4f);
    cacheMaxDiscomfortBack = Mathf.Max(cacheMaxDiscomfortBack, 0.4f);
}
if (pca.PlayerState.EscalatorPushVThisFrame != Vector3.zero)
{
    Vector3 move = pca.PlayerState.MoveV;
    move.y = 0f;
    if (move != Vector3.zero)
    {
        Vector3 push = pca.PlayerState.EscalatorPushVThisFrame;
        Vector3 actualMove = pca.PlayerState.MoveV;
        actualMove.y = 0f;
        if (Vector3.Dot(push, actualMove) < 0f)
        {
            cacheMaxDiscomfortFront = Mathf.Max(cacheMaxDiscomfortFront, 0.35f);
            cacheMaxDiscomfortSide = Mathf.Max(cacheMaxDiscomfortSide, 0.35f);
            cacheMaxDiscomfortBack = Mathf.Max(cacheMaxDiscomfortBack, 0.35f);
        }
    }
}
if (global::ExposureUnnoticed2.Scripts.InGame.StrangeAreaManager.Instance)
{
    cacheMaxDiscomfortFront = Mathf.Max(cacheMaxDiscomfortFront, global::ExposureUnnoticed2.Scripts.InGame.StrangeAreaManager.Instance.GetStrange());
    cacheMaxDiscomfortSide = Mathf.Max(cacheMaxDiscomfortSide, global::ExposureUnnoticed2.Scripts.InGame.StrangeAreaManager.Instance.GetStrange());
    cacheMaxDiscomfortBack = Mathf.Max(cacheMaxDiscomfortBack, global::ExposureUnnoticed2.Scripts.InGame.StrangeAreaManager.Instance.GetStrange());
}
if (pca.PlayerState.Dokidoki >= 0.5f) cacheMaxDiscomfortFront = Mathf.Max(cacheMaxDiscomfortFront, 0.25f);

        }

		private bool IsInDirect(Constant.DiscomfortDirect direct, bool isFromFront, bool isFromBack, bool isFromSide)
		{
            // SOURCE DRAFT: complete original PlayerStrangeness family, awaiting full semantic proof.
            if (isFromFront && ((int)direct & 1) != 0) return true;
if (isFromBack && ((int)direct & 2) != 0) return true;
if (!isFromSide) return false;
if (((int)direct & 8) != 0) return true;
return (((int)direct >> 2) & 1) != 0;

        }

		private float CalcExposeStrange(Vector3 targetPosition)
		{
            // SOURCE DRAFT: complete original PlayerStrangeness family, awaiting full semantic proof.
            float result = 0f;
int hidden = (int)pca.PlayerState.HiddenBodyPartsByCostume;
isBlockByCostume = false;
int breastSize = pca.PlayerBodyCustomizeAdjuster.BigBreastSize;
Vector3 direction = targetPosition - pca.Transform.position;
direction.y = 0f;
direction = direction.normalized;
float dot = Vector3.Dot(direction, pca.Transform.forward);
bool fromFront = dot >= 0.5f;
bool hideFuta = false;
if ((hidden & 4) != 0)
    hideFuta = !global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveFutanari || pca.PlayerState.IsHideFutaByCosplay;
bool breastVisibleFromSide = false;
if (breastSize > 29)
{
    float threshold = Mathf.Clamp(0.7f - Mathf.Clamp((float)breastSize, 0f, 200f) / 200f * 0.7f, 0f, 0.7f);
    Vector3 breastDirection = targetPosition - pca.Transform.position;
    breastDirection.y = 0f;
    breastDirection = breastDirection.normalized;
    breastVisibleFromSide = Vector3.Dot(breastDirection, pca.Transform.forward) >= threshold;
}
bool futaVisibleFromSide = false;
if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveFutanari)
{
    float threshold = Mathf.Clamp(0.7f - Mathf.Clamp((float)global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariSaoSize, 0f, 50f) / 50f * 0.7f, 0f, 0.7f);
    Vector3 futaDirection = targetPosition - pca.Transform.position;
    futaDirection.y = 0f;
    futaDirection = futaDirection.normalized;
    futaVisibleFromSide = Vector3.Dot(futaDirection, pca.Transform.forward) >= threshold;
}
int exposed = (int)pca.PlayerClothes.GetExposedBodyParts();
int actionExposed = 0;
if (pca.PlayerState.CurrentAction.ExposedBodyParts is not null)
{
    var actionParts = pca.PlayerState.CurrentAction.ExposedBodyParts;
    for (int i = 0; i < actionParts.Length; ++i) actionExposed |= (int)actionParts[i];
}
if (pca.PlayerState.IsCrouch)
{
    hidden &= ~2;
    actionExposed |= 4;
}
exposed |= actionExposed;
if ((exposed & 2) != 0 && (fromFront || breastVisibleFromSide || futaVisibleFromSide))
{
    if (((fromFront || breastVisibleFromSide) && (hidden & 1) == 0) || ((fromFront || futaVisibleFromSide) && !hideFuta))
        result = global::ExposureUnnoticed2.Master.Common.MCommon.Instance.StrangeExposeFront + result;
    else isBlockByCostume = true;
}
if ((exposed & 8) != 0)
{
    if (fromFront)
    {
        if ((hidden & 1) != 0 && hideFuta) isBlockByCostume = true;
        else
        {
            result += global::ExposureUnnoticed2.Master.Common.MCommon.Instance.StrangeExposeUpperFromFront;
            pca.PlayerState.IsLookedUpper = true;
        }
    }
    else
    {
        if ((hidden & 8) != 0) isBlockByCostume = true;
        else
        {
            result += global::ExposureUnnoticed2.Master.Common.MCommon.Instance.StrangeExposeUpperFromBack;
            pca.PlayerState.IsLookedUpper = true;
        }
    }
}
if ((exposed & 4) != 0 && dot <= -0.7f)
{
    if ((hidden & 2) != 0 || (hidden & 16) != 0) isBlockByCostume = true;
    else
    {
        result += global::ExposureUnnoticed2.Master.Common.MCommon.Instance.StrangeExposeHip;
        pca.PlayerState.IsLookedHip = true;
    }
}
if ((exposed & 32) != 0)
{
    bool blocked;
    if (fromFront) blocked = (hidden & 1) != 0 && hideFuta;
    else if (dot < 0f) blocked = (hidden & 2) != 0 || (hidden & 16) != 0;
    else blocked = (hidden & 8) != 0;
    if (blocked) isBlockByCostume = true;
    else
    {
        result = global::ExposureUnnoticed2.Master.Common.MCommon.Instance.StrangeExposeAll;
        pca.PlayerState.IsLookedFlashing = true;
    }
    if (dot < 0f && (hidden & 2) == 0 && (hidden & 16) == 0) pca.PlayerState.IsLookedHip = true;
}
if (pca.PlayerState.IsPeeing) result += 0.03f;
return result;

        }
	}
}
