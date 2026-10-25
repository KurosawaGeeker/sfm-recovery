using ExposureUnnoticed2.Scripts.Base;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerMiscProcess
	{
		private PlayerStateModel playerState;

		private PlayerClassAccessor pca;

		private bool isStrafeCache;

		private SmoothFloat hipDBWeight;

		public float nextUpdateTimeHiddenBodyPartsByCostume;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x6555E0
			this.pca = pca;
			playerState = pca.PlayerState;
		}

		public void OnUpdate(InputData inputData)
		{
// Complete original method; raw/typed/differential verification required before sealing.
CalcDokidoki();
CalcHiddenBodyPartsByCostume();
var aftermath = pca.PlayerState;
if (!aftermath.IsEcstasyMotion)
{
    float before = aftermath.EcstasyRemainAftermathTime;
    aftermath.EcstasyRemainAftermathTime = before - UnityEngine.Time.deltaTime;
}
else aftermath.EcstasyRemainAftermathTime = 0.3f;
if (inputData.JustInteract)
{
    var head = InteractManager.GetHead();
    if (head != null && (int)head.Type != 0 && !playerState.IsEcstasyMotion &&
        ((int)playerState.CurrentAction.Type == -1 || head.Controller.CanInteractWithAction))
    {
        var evt = Events.InteractEvent;
        var again = InteractManager.GetHead();
        evt.Model = again;
        EventManager.Broadcast(Events.InteractEvent);
    }
}
var vibrationReceiver = pca.PlayerState;
var mode = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerVibrationModeType;
float vibrationBonus;
switch ((int)mode)
{
    case 0: vibrationBonus = 0f; break;
    case 1: vibrationBonus = 1f; break;
    case 2: vibrationBonus = 2f; break;
    case 3:
        vibrationBonus = global::ExposureUnnoticed2.Object3D.AdultGoods.CommonVibratorController.Leader
            ? ((int)global::ExposureUnnoticed2.Object3D.AdultGoods.CommonVibratorController.VibrationStrength == 2 ? 2f : 1f) : 0f;
        break;
    default: throw new System.Runtime.CompilerServices.SwitchExpressionException(mode);
}
vibrationReceiver.VibratorRpBonusScale = vibrationBonus;
var pistonPca = pca;
int pistonMode = pistonPca.PistonMachineController.CurrentSpeedType;
float pistonBonus = pistonMode switch
{
    0 => 0f, 1 => 0.6f, 2 => 0.8f, 3 => 1f,
    _ => throw new System.Runtime.CompilerServices.SwitchExpressionException(pistonMode)
};
pistonPca.PlayerState.PistonRpBonusScale = pistonBonus;
if (isStrafeCache != pca.PlayerState.IsStrafe)
{
    isStrafeCache = pca.PlayerState.IsStrafe;
    if (!pca.PlayerState.IsStrafe) pca.LookAtController.DisableLookAt(LookAtController.LookAtLayer.Strafe);
    else
    {
        var lookPca = pca;
        var target = lookPca.PlayerAvatarObjectReferencer.FPCameraTarget;
        lookPca.LookAtController.EnableLookAt(LookAtController.LookAtLayer.Strafe, target, true);
    }
}
var bikun = pca.PlayerState;
if (!bikun.IsGamanBaibu) bikun.VibeBikunMotionRemainTime = -1f;
else if (bikun.VibeBikunMotionRemainTime < 0f) bikun.VibeBikunMotionRemainTime = 0.4f;
else
{
    float before = bikun.VibeBikunMotionRemainTime;
    float next = before - UnityEngine.Time.deltaTime;
    bikun.VibeBikunMotionRemainTime = 0f > next ? 0f : next;
}
if (playerState.TimeStopStrangeMaxNpcListClearRemainTime > 0f)
{
    var temp = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState;
    var timer = playerState;
    if (!temp.IsTimeStop)
    {
        float before = timer.TimeStopStrangeMaxNpcListClearRemainTime;
        timer.TimeStopStrangeMaxNpcListClearRemainTime = before - UnityEngine.Time.deltaTime;
    }
    else timer.TimeStopStrangeMaxNpcListClearRemainTime = 0f;
    if (playerState.TimeStopStrangeMaxNpcListClearRemainTime <= 0f) playerState.timeStopStrangeMaxNpcList.Clear();
}
float subBlend = pca.PlayerAvatarObjectReferencer.CoatController.coatSubBlend.Current;
SmoothFloat hip;
bool hipEnabled = false;
if (!(subBlend >= 100f))
{
    var clothes = pca.PlayerState.CurrentClothes;
    int type = (int)clothes.ClothesType;
    hip = hipDBWeight;
    if (type == -1 && !pca.PlayerState.IsOffHipDBFromCosplay) hipEnabled = true;
}
else
{
    var state = pca.PlayerState;
    hip = hipDBWeight;
    if (state is null) throw new System.NullReferenceException();
    if (!state.IsOffHipDBFromCosplay) hipEnabled = true;
}
hip.UpdateTo(hipEnabled ? 1f : 0f);
var avatar = pca.PlayerAvatarObjectReferencer;
var capturedHip = hipDBWeight;
if (capturedHip is null) throw new System.NullReferenceException();
avatar.HipDB.m_BlendWeight = capturedHip.Current;
if ((int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType == 0) return;
if (!global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)17) &&
    !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)22) &&
    !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)21))
{
    var data = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
    if (!data.IsActiveInvisibleReusable && !data.IsActiveBodyPaintReusable) return;
}
if (!global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsNoAchieveMissionMode)
    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_noAchieveMissionMode"), (global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType)0);
global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsNoAchieveMissionMode = true;

}

		private void CalcDokidoki()
		{
// Complete original method; raw/typed/differential verification required before sealing.
var exposed = PlayerFacade.Instance.GetExposedBodyPartsByClothesAndCostume();
float clothes = 0f;
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ExternalMethodManager).TypeHandle);
if (((uint)exposed & 2u) != 0u) clothes = global::ExposureUnnoticed2.Master.Common.MCommon.Instance.DokidokiOpenFront + 0f;
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ExternalMethodManager).TypeHandle);
if (((uint)exposed & 8u) != 0u) clothes = clothes + global::ExposureUnnoticed2.Master.Common.MCommon.Instance.DokidokiOpenUpper;
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ExternalMethodManager).TypeHandle);
if (((uint)exposed & 4u) != 0u) clothes = clothes + global::ExposureUnnoticed2.Master.Common.MCommon.Instance.DokidokiOpenHip;
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ExternalMethodManager).TypeHandle);
if (((uint)exposed & 32u) != 0u) clothes = clothes + global::ExposureUnnoticed2.Master.Common.MCommon.Instance.DokidokiDropClothes;
bool exposedPositive = clothes > 0f;
float value = clothes + pca.PlayerState.CurrentAction.AddDokidoki100 * 0.01f;
if (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.VisibilityFogController.IsBadVisibility) value = value + 0.1f;
if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsAttachSomeHandcuffs()) value = value + 0.1f;
if (pca.PlayerState.IsPeeing) value = value + 0.1f;
if (pca.PlayerState.EcstasyRiseByVibratorPerSecond > 0f)
{
    float add;
    if ((int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerVibrationModeType == 3) add = 0.2f;
    else add = (int)global::ExposureUnnoticed2.Object3D.AdultGoods.CommonVibratorController.VibrationStrength switch
    {
        0 => 0f, 1 => 0.1f, 2 => 0.2f,
        _ => throw new System.Runtime.CompilerServices.SwitchExpressionException(global::ExposureUnnoticed2.Object3D.AdultGoods.CommonVibratorController.VibrationStrength)
    };
    value = value + add;
}
var state = pca.PlayerState;
if (state.EcstasyRiseByPistonMachinePerSecond > 0f)
{
    float add = 0.2f;
    if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PistonMachineModeType != 4)
    {
        int mode = pca.PistonMachineController.CurrentSpeedType;
        add = mode switch { 0 => 0f, 1 => 0.1f, 2 => 0.15f, 3 => 0.2f, _ => throw new System.Runtime.CompilerServices.SwitchExpressionException(mode) };
    }
    value = value + add;
}
if (exposedPositive)
{
    float light = 0.1f;
    if (!global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime)
    {
        float currentLight = pca.PlayerState.BareyasusaFromStreetLight;
        int defaultLight = global::ExposureUnnoticed2.Master.Stage.MStage.Get(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType).DefaultLightBareyasusaScale100;
        float minimum = (float)defaultLight * 0.01f;
        currentLight = currentLight > minimum ? currentLight : minimum;
        if (currentLight < 0f) currentLight = 0f; else if (currentLight > 1f) currentLight = 1f;
        light = currentLight * 0.1f + 0f;
    }
    value = value + light;
    state = pca.PlayerState;
    float near = (4f - state.NearNpcDist) * 0.25f;
    if (near < 0f) near = 0f; else if (near > 1f) near = 1f;
    value = value + near * 0.4f;
}
else state = pca.PlayerState;
value = value * state.CurrentRank.DokidokiBoostScale;
if (value < 0f) value = 0f; else if (value > 1f) value = 1f;
playerState.Dokidoki = value;

}

		private void CalcHiddenBodyPartsByCostume()
		{
// Complete original method; raw/typed/differential verification required before sealing.
float before = nextUpdateTimeHiddenBodyPartsByCostume;
if (UnityEngine.Time.time < before) return;
nextUpdateTimeHiddenBodyPartsByCostume = UnityEngine.Time.time + 1f;
uint hidden = 0u;
float max = 0f;
foreach (var pair in global::ExposureUnnoticed2.Master.Cosplay.MCosplay.GetAll())
{
    var records = pair.Value.GetAll();
    for (int i = 0; i < records.Length; i++)
    {
        var record = records[i];
        if (!global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipCosplayParts(record.UniqueId)) continue;
        hidden |= (uint)record.HiddenPartsType;
        float candidate = record.LookHiddenPartMaxStrange;
        if (!(max > candidate)) max = candidate;
    }
}
pca.PlayerState.HiddenBodyPartsByCostume = (PlayerStateModel.HiddenBodyPartsByCostumeType)hidden;
pca.PlayerState.MaxLookHiddenPartMaxStrange = max;

}

		public void ReturnInteractTokenIfNeed()
		{
            // Original RVA 0x656860 inlines the existing public ReturnToken method.
            var token = PlayerController.Instance.Pca.PlayerState.LastInteractTokenPublisher;
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(UnityEngine.Object).TypeHandle);
            if (token != null)
            {
                PlayerController.Instance.Pca.PlayerState.LastInteractTokenPublisher.ReturnToken();
                PlayerController.Instance.Pca.PlayerState.LastInteractTokenPublisher = null;
            }
        }
	// Recovered original member RVA 0x656A60: .ctor
		public PlayerMiscProcess()
        {
            hipDBWeight = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.5f, 1f, 0f, false, false, -1f);
        }
	}
}
