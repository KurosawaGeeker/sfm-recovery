namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcFoundStrangerState
	{
		private float stateTime;

		private NpcController npcController;

		private NpcComponentAccessor nca;

		public void Initialize(NpcController npcController, NpcComponentAccessor nca)
		{
			this.npcController = npcController; this.nca = nca;
		}

		public void OnEnter()
		{
			nca.NpcAnimationManager.SetFoundStranger(true);
		}

		public void OnUpdate()
		{
			float previousTime = stateTime;
			float delta = UnityEngine.Time.deltaTime;
			stateTime = delta + previousTime;
			if (!(stateTime > 3f))
			{
			    if (nca.StateModel.IsSit) return;
			    UnityEngine.Vector3 playerPosition = nca.Player.transform.position;
			    UnityEngine.Vector3 npcPosition = npcController.Transform.position;
			    UnityEngine.Vector3 direction = playerPosition - npcPosition;
			    direction = ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.GetSetY(direction, 0f);
			    UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.LookRotation(direction);
			    UnityEngine.Transform transform = npcController.Transform;
			    UnityEngine.Quaternion currentRotation = transform.rotation;
			    float rotationDelta = UnityEngine.Time.deltaTime;
			    transform.rotation = UnityEngine.Quaternion.RotateTowards(currentRotation, targetRotation, rotationDelta * 200f);
			    return;
			}
			if (nca.StateModel.CurrentFixedType == NpcStateModel.FixedType.None
			    && !nca.StateModel.IsSit
			    && nca.StateModel.AvatarType == NpcAvatarComponent.AvatarType.NewMale
			    && (nca.StateModel.BeforeState != NpcStateModel.State.Interact || nca.StateModel.LastRouteInteractType != ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.Escalator))
			{
			    bool skipRegister = nca.StateModel.BeforeState == NpcStateModel.State.Interact
			        && nca.StateModel.LastRouteInteractType == ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.Toilet
			        && nca.NpcToiletSubState.IsPeeing;
			    if (!skipRegister)
			    {
			        ExposureUnnoticed2.Scripts.InGame.SexManager manager = ExposureUnnoticed2.Scripts.InGame.SexManager.Instance;
			        NpcController npc = nca.NpcController;
			        if (manager is null) throw new System.NullReferenceException();
			        if (manager.TryRegisterTrace(npc))
			        {
			            ExposureUnnoticed2.Object3D.RoutePoint.RoutePoint point = nca.StateModel.RouteData.GetNextPoint();
			            if (point.LinkTokenPublisher != null) point.LinkTokenPublisher.ReturnToken();
			            npcController.ChangeState(NpcStateModel.State.TraceSexPlayer);
			            return;
			        }
			    }
			}
			if (nca.StateModel.BeforeState == NpcStateModel.State.Interact)
			{
			    nca.StateModel.IsComeBackInteractFromFoundStranger = true;
			    npcController.ChangeState(NpcStateModel.State.Interact);
			}
			else npcController.ChangeState(NpcStateModel.State.Idle);
		}

		public void OnExit()
		{
			nca.StrangenessManager.Strangeness.Value = 0f; nca.NpcAnimationManager.SetFoundStranger(false);
		}
	}
}
