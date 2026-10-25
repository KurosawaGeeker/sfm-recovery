using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcTurnAroundState
	{
		private NpcComponentAccessor nca;

		private Vector3 playerPositionOnEnter;

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
		}

		public void OnEnter()
		{// Complete original native rule; original Game helper bodies remain intact.
var initialState = nca.StateModel;
if (initialState is null) throw new System.NullReferenceException();
nca.NpcLocomotion.SetMoveZero();
nca.NpcAnimationManager.SetOverrideIdle(true);
nca.NpcAnimationManager.SetSpeed(0f);
nca.NpcLocomotion.SetActiveNavAgent(false);
playerPositionOnEnter = nca.Player.transform.position;
}

		public void OnUpdate()
		{// Complete original native rule; original Game helper bodies remain intact.
bool freezeLook = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveInvisible;
Vector3 targetPosition = !freezeLook ? nca.Player.transform.position : playerPositionOnEnter;
var overridePoint = nca.StateModel.turnAroundPointInTimeStopSperm;
if (overridePoint.HasValue)
{
    freezeLook = true;
    targetPosition = overridePoint.GetValueOrDefault();
}
Vector3 npcPosition = nca.Transform.position;
Vector3 direction = new Vector3(targetPosition.x - npcPosition.x, 0f, targetPosition.z - npcPosition.z);
direction.Normalize();
var rotationTarget = nca.Transform;
var currentRotation = rotationTarget.rotation;
var wantedRotation = Quaternion.LookRotation(direction, Vector3.up);
float fixedDelta = Time.fixedDeltaTime;
rotationTarget.rotation = Quaternion.Lerp(currentRotation, wantedRotation, fixedDelta * 3f);
if (!freezeLook)
    nca.LookAtController.EnableLookAt(global::ExposureUnnoticed2.Object3D.Player.Scripts.LookAtController.LookAtLayer.NpcLookPlayer, nca.Player.PlayerAvatarObjectReferencer.Head, false);
else
    nca.LookAtController.DisableLookAt(global::ExposureUnnoticed2.Object3D.Player.Scripts.LookAtController.LookAtLayer.NpcLookPlayer);
if (!(Time.time - nca.StateModel.stateInTime >= 2.8f)) return;
nca.LookAtController.DisableLookAt(global::ExposureUnnoticed2.Object3D.Player.Scripts.LookAtController.LookAtLayer.NpcLookPlayer);
if (nca.StateModel.BeforeState == NpcStateModel.State.Interact)
{
    nca.StateModel.IsComeBackInteractFromFoundStranger = true;
    nca.NpcController.ChangeState(NpcStateModel.State.Interact);
}
else if (nca.StateModel.BeforeState == NpcStateModel.State.WaitToken)
    nca.NpcController.ChangeState(NpcStateModel.State.WaitToken);
else
    nca.NpcController.ChangeState(NpcStateModel.State.Idle);
nca.StateModel.IgnorePlayerNoisyTime = 10f;
}

		public void OnExit()
		{
			nca.NpcAnimationManager.SetOverrideIdle(false); nca.StateModel.turnAroundPointInTimeStopSperm = null;
		}
	}
}
