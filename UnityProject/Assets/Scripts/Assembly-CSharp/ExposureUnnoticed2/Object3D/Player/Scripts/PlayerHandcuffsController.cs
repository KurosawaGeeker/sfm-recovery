namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerHandcuffsController
	{
		private PlayerClassAccessor pca;

		private bool interactButtonLock;

		private float currentProbably;

		private float Progress2Goal;

		private bool initHandcuff;

		private bool initHandcuffMap;

		private int forceUnlockPhase;

		public float UnlockProgress { get; private set; }

		public float UnlockProgress2 { get; private set; }

		public bool IsConditionMission()
		{
if (!global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsAttachSomeHandcuffs()) return false;
int type = (int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.AttachedHandcuffType;
if (type == 3) return true;
if (type == 4) return false;
if (type == 5) return global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerHandcuffTimerRemainTime > 0f;
return true;
}

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x64E490
			this.pca = pca;
			pca.PlayerAnimationManager.SetHandcuffs(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerIsAttachHandcuffs);
			if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerIsAttachHandcuffs) initHandcuff = true;
			if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerIsAttachMapHandcuffs) initHandcuffMap = true;
		}

		public void OnUpdate(InputData inputData)
		{
			// SOURCE DRAFT: original64E5B0; caller643A10 inlines this exact dispatch.
			if (!global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsGameOver)
			{
			    if (!initHandcuff && !initHandcuffMap)
			    {
			        var data = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
			        if (data.PlayerIsAttachHandcuffs || data.PlayerIsAttachMapHandcuffs)
			        {
			            OnAttachProcess(inputData);
			            return;
			        }
			    }
			    OnNoAttachProcess();
			    return;
			}
			pca.PlayerSoundManager.SetPickingSeSoundIfNeed(false);
		}

		public void OnEmergencyEscape()
		{
if (!initHandcuff && !initHandcuffMap && global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsAttachSomeHandcuffs()) forceUnlockPhase = 1;
}

		private void OnAttachProcess(InputData inputData)
		{
if (forceUnlockPhase == 0 && inputData.Interact && (int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.AttachedHandcuffType == 4 && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ItemDict[(global::ExposureUnnoticed2.Master.Item.ItemType)10000] == 0)
{
    if (!interactButtonLock && (int)global::ExposureUnnoticed2.Scripts.Base.InteractManager.GetHead().Type != 15) global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("log_not_have_handcuff_key"));
    interactButtonLock = true;
    return;
}
if ((int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.AttachedHandcuffType == 5)
{
    var timerData = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
    float oldTime = timerData.PlayerHandcuffTimerRemainTime;
    float delta = global::UnityEngine.Time.deltaTime;
    timerData.PlayerHandcuffTimerRemainTime = oldTime - delta;
    if (oldTime > 0f && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerHandcuffTimerRemainTime <= 0f)
    {
        pca.PlayerSoundManager.PlayPickingSe((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)21);
        string message = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("item_handcuff_timer_end");
        if (global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.Message)
        {
            // Original static AddMessage inlined here; preserve its option/receiver prefix.
            global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(message, (global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType)1);
        }
    }
    if (forceUnlockPhase == 0 && inputData.Interact && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerHandcuffTimerRemainTime > 0f && !interactButtonLock)
    {
        int minutes = unchecked((int)(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerHandcuffTimerRemainTime / 60f) + 1);
        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("item_handcuff_timer_remain", minutes.ToString()));
        interactButtonLock = true;
        return;
    }
}
if (forceUnlockPhase < 1 && !(inputData.Interact && !interactButtonLock && !pca.PlayerState.IsEcstasyMotion && pca.PlayerState.CurrentAction.CanUnlock && pca.PlayerState.VibeBikunMotionRemainTime <= 0f))
{
    if (!pca.PlayerState.IsEcstasyMotion)
    {
        TransFromUnlockAction();
        pca.PlayerSoundManager.SetPickingSeSoundIfNeed(false);
        UnlockProgress = 0f;
    }
    else
    {
        pca.PlayerSoundManager.SetPickingSeSoundIfNeed(false);
        interactButtonLock = true;
        UnlockProgress = 0f;
    }
    goto Tail;
}
TransToUnlockAction();
float oldProgress = UnlockProgress;
float step = global::UnityEngine.Time.deltaTime;
var common = global::ExposureUnnoticed2.Master.Common.MCommon.Instance;
if (common is null) throw new System.NullReferenceException();
var access = pca;
if (access is null) throw new System.NullReferenceException();
var speedState = access.PlayerState;
float next = oldProgress + (step * common.HandcuffsSpeed) * (1f + speedState.ReinforceEffectData.UnlockSpeed100 * 0.01f);
if (next < 0f) next = 0f;
else if (next > 1f) next = 1f;
UnlockProgress = next;
if (next >= 1f || forceUnlockPhase >= 2)
{
    pca.PlayerSoundManager.SetPickingSeSoundIfNeed(false);
    float random = global::UnityEngine.Random.value;
    float newProgress2 = UnlockProgress2 + (0.05f + (random * random) * 0.7f);
    UnlockProgress2 = newProgress2;
    if (!(newProgress2 >= Progress2Goal) && forceUnlockPhase < 2)
    {
        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("log_handcuff_failed_unlock"), (global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType)1);
        float previousProbability = currentProbably;
        var probabilityCommon = global::ExposureUnnoticed2.Master.Common.MCommon.Instance;
        if (probabilityCommon is null) throw new System.NullReferenceException();
        currentProbably = previousProbability + probabilityCommon.HandcuffsAddProbabilityByFailure;
        TransFromUnlockAction();
    }
    else
    {
        UnlockProgress2 = 1f;
        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("log_handcuff_success_unlock"), (global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType)1);
        pca.PlayerAnimationManager.SetHandcuffs(false);
        pca.PlayerSoundManager.PlayPickingSe((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)21);
        if (!global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerIsAttachMapHandcuffs) TransFromUnlockAction();
        else pca.PlayerAction.ChangeAction((global::ExposureUnnoticed2.Master.Action.ActionType)(-1));
        var clearedData = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
        clearedData.PlayerIsAttachHandcuffs = false;
        clearedData.PlayerIsAttachMapHandcuffs = false;
        var adultGoods = pca.PlayerAdultGoodsController;
        var type = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.AttachedHandcuffType;
        bool active = !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipAdultGoods(type);
        if (adultGoods is null) throw new System.NullReferenceException();
        adultGoods.ChangeAdultGoods(type, active);
        forceUnlockPhase = 0;
    }
    UnlockProgress = 0f;
    interactButtonLock = true;
}
if (forceUnlockPhase >= 1) forceUnlockPhase = unchecked(forceUnlockPhase + 1);
Tail:
if (!inputData.Interact && !pca.PlayerState.IsEcstasyMotion) interactButtonLock = false;
}

		private void TransToUnlockAction()
		{
bool attached = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerIsAttachHandcuffs;
var state = pca.PlayerState;
if (!attached)
{
    if ((int)state.CurrentAction.Type != 10022) pca.PlayerAction.ChangeAction((global::ExposureUnnoticed2.Master.Action.ActionType)10022);
    return;
}
if ((int)state.CurrentAction.Type == 10019) return;
var menu = global::ExposureUnnoticed2.ObjectUI.InteractMenuPanel.InteractMenuPanelView.Instance;
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::UnityEngine.Object).TypeHandle);
if (menu != null) return;
pca.PlayerAction.ChangeAction((global::ExposureUnnoticed2.Master.Action.ActionType)10019);
}

		private void TransFromUnlockAction()
		{
bool attached = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerIsAttachHandcuffs;
var state = pca.PlayerState;
if (!attached)
{
    if ((int)state.CurrentAction.Type == 10022) pca.PlayerAction.ChangeAction((global::ExposureUnnoticed2.Master.Action.ActionType)10021);
    return;
}
if ((int)state.CurrentAction.Type == 10019) pca.PlayerAction.ChangeAction((global::ExposureUnnoticed2.Master.Action.ActionType)(-1));
}

		private void OnNoAttachProcess()
		{
if (!initHandcuff && !initHandcuffMap)
{
    if (pca.PlayerState.IsEcstasyMotion)
    {
        pca.PlayerState.CompleteAttachHandcuffsTime = float.MaxValue;
        pca.PlayerSoundManager.SetPickingSeSoundIfNeed(false);
        return;
    }
}
bool normalAttach = initHandcuff;
if (!normalAttach && (int)pca.PlayerState.CurrentAction.Type == 10019)
{
    float completeTime = pca.PlayerState.CompleteAttachHandcuffsTime;
    float now = global::UnityEngine.Time.time;
    normalAttach = now > completeTime;
}
if (normalAttach)
{
    if (!initHandcuff)
    {
        var capturedData = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
        var state = pca.PlayerState;
        if (capturedData is null) throw new System.NullReferenceException();
        capturedData.AttachedHandcuffType = state.LastAttachedHandcuffType;
    }
    initHandcuff = false;
    global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerIsAttachHandcuffs = true;
    pca.PlayerAnimationManager.SetHandcuffs(true);
    UnlockProgress = 0f;
    UnlockProgress2 = 0f;
    pca.PlayerAction.ChangeAction((global::ExposureUnnoticed2.Master.Action.ActionType)(-1));
    pca.PlayerSoundManager.SetPickingSeSoundIfNeed(false);
    currentProbably = global::ExposureUnnoticed2.Master.Common.MCommon.Instance.HandcuffsProbability;
    Progress2Goal = 1f;
    int type = (int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.AttachedHandcuffType;
    if (type == 4 || type == 5) { currentProbably = 1f; Progress2Goal = 0.01f; }
    var accessor = pca;
    if (accessor is null) throw new System.NullReferenceException();
    var data = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
    if (data is null) throw new System.NullReferenceException();
    accessor.PlayerAdultGoodsController.ChangeAdultGoods(data.AttachedHandcuffType, true);
    global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeSomeEquipEvent);
}
bool mapAttach = initHandcuffMap;
if (!mapAttach && (int)pca.PlayerState.CurrentAction.Type == 10020)
{
    float completeTime = pca.PlayerState.CompleteAttachHandcuffsTime;
    float now = global::UnityEngine.Time.time;
    mapAttach = now >= completeTime;
}
if (mapAttach)
{
    if (!initHandcuffMap)
    {
        var facade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
        if (facade is null) throw new System.NullReferenceException();
        facade.SmoothRotateY(180f);
        var capturedData = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
        var state = pca.PlayerState;
        if (capturedData is null) throw new System.NullReferenceException();
        capturedData.AttachedHandcuffType = state.LastAttachedHandcuffType;
    }
    initHandcuffMap = false;
    global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerIsAttachMapHandcuffs = true;
    UnlockProgress = 0f;
    UnlockProgress2 = 0f;
    pca.PlayerAction.ChangeAction((global::ExposureUnnoticed2.Master.Action.ActionType)10021);
    pca.PlayerSoundManager.SetPickingSeSoundIfNeed(false);
    currentProbably = global::ExposureUnnoticed2.Master.Common.MCommon.Instance.HandcuffsProbability;
    Progress2Goal = 1f;
    int type = (int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.AttachedHandcuffType;
    if (type == 4 || type == 5) { currentProbably = 1f; Progress2Goal = 0.01f; }
    var accessor = pca;
    if (accessor is null) throw new System.NullReferenceException();
    var data = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
    if (data is null) throw new System.NullReferenceException();
    accessor.PlayerAdultGoodsController.ChangeAdultGoods(data.AttachedHandcuffType, true);
    global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeSomeEquipEvent);
}
if (global::ExposureUnnoticed2.Scripts.InGame.DebugController.IsHandcuffOnePunch) { currentProbably = 1f; Progress2Goal = 0.01f; }
}
	}
}
