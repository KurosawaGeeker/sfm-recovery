using ExposureUnnoticed2.Object3D.NPC.Script;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerBaretaState
	{
		private PlayerClassAccessor pca;

		private float enterTime;

		public NpcController FoundNpc { get; set; }

		public bool IsOnlyHeadReaction { get; private set; }

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x407380
			this.pca = pca;
		}

		public void OnUpdate()
		{
// Complete original native body; original gates, state order and failure prefixes retained.
if (pca.PlayerState.TransAction == global::ExposureUnnoticed2.Master.Action.ActionType.Sad)
{
    pca.PlayerAnimationManager.SetFaceLayerWeight(0f);
    pca.PlayerAnimationManager.SetBareta(false);
    pca.PlayerAnimationManager.SetBaretaFace(false);
    var action = pca.PlayerAction;
    if (!IsOnlyHeadReaction)
    {
        var next = pca.PlayerState.TransAction;
        if (action is null) throw new System.NullReferenceException();
        action.ChangeAction(next);
    }
    else
    {
        if (action is null) throw new System.NullReferenceException();
        action.ChangeAction(global::ExposureUnnoticed2.Master.Action.ActionType.SadHandcuffAtMap);
    }
    pca.LookAtController.DisableLookAt(LookAtController.LookAtLayer.Bareta);
    return;
}
if (global::UnityEngine.Time.time - enterTime >= 2f && !global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsGameOver)
    pca.Player.ChangeState(PlayerStateModel.PlayerState.Idle);
if (IsOnlyHeadReaction) return;
var rotation = global::UnityEngine.Quaternion.Euler(global::UnityEngine.Vector3.zero);
var npcPosition = FoundNpc.Transform.position;
var playerPosition = pca.Transform.position;
var planarOffset = new global::UnityEngine.Vector3(npcPosition.x - playerPosition.x, 0f, npcPosition.z - playerPosition.z);
var direction = rotation * planarOffset;
var targetRotation = global::UnityEngine.Quaternion.LookRotation(direction, global::UnityEngine.Vector3.up);
var playerTransform = pca.Transform;
var currentRotation = playerTransform.rotation;
float delta = global::UnityEngine.Time.deltaTime;
var nextRotation = global::UnityEngine.Quaternion.RotateTowards(currentRotation, targetRotation, delta * 400f);
playerTransform.rotation = nextRotation;
}

		public void OnEnter()
		{
// Complete original native body; original gates, state order and failure prefixes retained.
if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerIsAttachMapHandcuffs)
    IsOnlyHeadReaction = true;
if (!IsOnlyHeadReaction)
{
    pca.PlayerAnimationManager.SetBareta(true);
    pca.PlayerAnimationManager.SetFaceLayerWeight(0f);
}
else
{
    var lookAt = pca.LookAtController;
    var eyePosition = pca.PlayerBaretaState.FoundNpc.EyePosition;
    if (lookAt is null) throw new System.NullReferenceException();
    lookAt.EnableLookAt(LookAtController.LookAtLayer.Bareta, eyePosition, true);
    pca.PlayerAnimationManager.SetBaretaFace(true);
    pca.PlayerAnimationManager.SetFaceLayerWeight(1f);
}
enterTime = global::UnityEngine.Time.time;
pca.PlayerAnimationManager.TargetArmLayerWeight = 0f;
pca.PlayerAnimationManager.TargetMoveSpeed = 0f;
pca.PlayerState.IsCrouch = false;
pca.PlayerAnimationManager.SetGamanBaibu(false);
global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerEcstasy = 0f;
}

		public void OnExit()
		{
// Complete original native body; original gates, state order and failure prefixes retained.
pca.PlayerAnimationManager.SetBareta(false);
pca.PlayerAnimationManager.SetBaretaFace(false);
pca.PlayerAnimationManager.TargetArmLayerWeight = 1f;
var state = pca.PlayerState;
var nextAction = global::ExposureUnnoticed2.Master.Action.MAction.Get(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerIsAttachMapHandcuffs ? global::ExposureUnnoticed2.Master.Action.ActionType.HandcuffsAtMap : global::ExposureUnnoticed2.Master.Action.ActionType.None);
if (state is null) throw new System.NullReferenceException();
state.CurrentAction = nextAction;
pca.LookAtController.DisableLookAt(LookAtController.LookAtLayer.Bareta);
IsOnlyHeadReaction = false;
}
	}
}
