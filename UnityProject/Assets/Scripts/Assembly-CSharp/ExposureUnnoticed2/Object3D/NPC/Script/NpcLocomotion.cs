using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcLocomotion
	{
		private NpcComponentAccessor nca;

		private float lastCalcNearNpcTime;

		private Vector3 moveV;

		private bool isDoCollisionCorrect;

		private Vector3 lastSetDestinationPoint;

		private Quaternion targetRotation;

		private float lastUpdateTime = -1f;

		private int skipFixedUpdateFrame;

		public void Initialize(NpcComponentAccessor npcComponentAccessor)
		{
			this.nca = npcComponentAccessor;
		}

		public void OnEnter()
		{
            lastSetDestinationPoint = Vector3.zero;
            SetActiveNavAgent(true);
        }

		public void OnUpdate()
		{
            Transform avatarTarget = nca.AvatarComponent.transform;
            Vector3 local = nca.AvatarComponent.transform.localPosition;
            Vector3 zero = Vector3.zero;
            avatarTarget.localPosition = new Vector3((zero.x - local.x) * 0.1f + local.x,
                (zero.y - local.y) * 0.1f + local.y, (zero.z - local.z) * 0.1f + local.z);
            Transform rotationTarget = nca.NpcController.Transform;
            Quaternion currentRotation = nca.NpcController.Transform.rotation;
            Quaternion wantedRotation = targetRotation;
            float delta = Time.deltaTime;
            rotationTarget.rotation = Quaternion.RotateTowards(currentRotation, wantedRotation, delta * 90f);
            if (Time.time - lastUpdateTime < 0.1f) return;
            float now = Time.time;
            float random = Random.value;
            lastUpdateTime = random * 0.05f + now;
            if (nca.StateModel.CurrentState == NpcStateModel.State.TraceSexPlayer)
            {
                Vector3 playerPosition = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CacheForNpcPosition;
                NpcComponentAccessor accessor = nca;
                Transform own = accessor.Transform;
                NpcAnimationManager animation = accessor.NpcAnimationManager;
                Vector3 ownPosition = own.position;
                float dx = playerPosition.x - ownPosition.x;
                float dy = playerPosition.y - ownPosition.y;
                float dz = playerPosition.z - ownPosition.z;
                animation.SetSpeed((dy * dy + dx * dx) + dz * dz > 0.5f ? 1f : 0f);
                nca.NavAgent.SetDestination(playerPosition);
            }
            else
            {
                global::ExposureUnnoticed2.Object3D.RoutePoint.RoutePoint point = nca.StateModel.RouteData.GetNextPoint();
                global::ExposureUnnoticed2.Object3D.RoutePoint.RouteData route = nca.StateModel.RouteData;
                Vector3 position = point.Position;
                float scale = route.PathRightScale;
                float maximum = point.MaxPathRightScale;
                Vector3 right = point.PathRight;
                Vector3 destination = new Vector3(scale * right.x * maximum + position.x,
                    scale * right.y * maximum + position.y, scale * right.z * maximum + position.z);
                global::ExposureUnnoticed2.Object3D.RoutePoint.BaseInteractTokenPublisher token = point.LinkTokenPublisher;
                if (token && !nca.StateModel.HasInteractToken)
                {
                    GameObject self = nca.NpcController.GameObject;
                    GameObject target = point.gameObject;
                    if (global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DistSqrt(self, target) < 1f)
                    { nca.NpcController.ChangeState(NpcStateModel.State.WaitToken); return; }
                }
                GameObject ownObject = nca.NpcController.GameObject;
                GameObject pointObject = point.gameObject;
                if (global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DistSqrt(ownObject, pointObject) < 1f)
                {
                    token = point.LinkTokenPublisher;
                    if (token && (int)point.RouteInteractType == 3)
                    {
                        var chair = point.LinkTokenPublisher as global::ExposureUnnoticed2.Object3D.ScenePlops.ChairSitController.ChairSitController;
                        if (!object.ReferenceEquals(chair, null) && (int)chair.CurrentDildoState != 0)
                        {
                            ((global::ExposureUnnoticed2.Object3D.RoutePoint.BaseInteractTokenPublisher)chair).ReturnToken();
                            nca.StateModel.RouteData.IncrementIndex();
                            SitNpcManager.Instance.UseEndByNpcByFloor(point);
                            nca.NpcController.ChangeState(NpcStateModel.State.Idle);
                        }
                    }
                }
                GameObject ownDestination = nca.NpcController.GameObject;
                if (global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DistSqrt(ownDestination, destination) < 0.2f)
                {
                    NpcComponentAccessor accessor = nca;
                    if ((int)point.RouteInteractType != 0)
                    { accessor.NpcController.ChangeState(NpcStateModel.State.Interact); return; }
                    global::ExposureUnnoticed2.Object3D.RoutePoint.RoutePoint next = accessor.StateModel.RouteData.IncrementIndex();
                    if (next) nca.NavAgent.SetDestination(destination);
                    else { nca.NpcController.ChangeState(NpcStateModel.State.FadeOut); return; }
                }
            }
            Vector3 steering = nca.NavAgent.steeringTarget;
            Vector3 controllerPosition = nca.NpcController.Transform.position;
            float directX = steering.x - controllerPosition.x;
            float directY = steering.y - controllerPosition.y;
            float directZ = steering.z - controllerPosition.z;
            Vector3 direct = new Vector3(directX, directY, directZ);
            targetRotation = !((directY * directY + directX * directX) + directZ * directZ <= 0.001f)
                ? Quaternion.LookRotation(direct) : Quaternion.identity;
            float speed = global::ExposureUnnoticed2.Master.Common.MCommon.PlayerWalkSpeed * 0.9f;
            Vector3 facingForAngle = nca.NpcController.Transform.forward;
            float angle = Vector3.Angle(direct, facingForAngle);
            float magnitude = direct.magnitude;
            if (magnitude < 4f)
            {
                if (angle > 60f) speed *= 0.3f;
                else if (angle >= 90f) speed *= 0.1f;
            }
            Vector3 forward = nca.NpcController.Transform.forward;
            float moveScale = nca.StateModel.MoveSpeedScale;
            float animationSpeed = nca.NpcAnimationManager.GetCurrentMoveSpeed();
            moveV = new Vector3(forward.x * speed * moveScale * animationSpeed,
                forward.y * speed * moveScale * animationSpeed, forward.z * speed * moveScale * animationSpeed);
            if (Time.unscaledTime - lastCalcNearNpcTime > 0.5f)
            {
                NpcStateModel state = nca.StateModel;
                NpcManager manager = NpcManager.Instance;
                NpcController self = nca.NpcController;
                NpcController nearest = manager.GetNearOtherNpc(self);
                state.NearNpc = nearest;
                float time = Time.unscaledTime;
                float nextRandom = Random.value;
                lastCalcNearNpcTime = nextRandom * 0.1f + time;
            }
            global::ExposureUnnoticed2.Object3D.RoutePoint.RoutePoint nextPoint = nca.StateModel.RouteData.GetNextPoint();
            Vector3 previous = lastSetDestinationPoint;
            if (!object.ReferenceEquals(nextPoint, null))
            {
                Vector3 position = nextPoint.Position;
                float dx = previous.x - position.x;
                float dy = previous.y - position.y;
                float dz = previous.z - position.z;
                if (!((dy * dy + dx * dx) + dz * dz < 9.9999994e-11f))
                {
                    global::ExposureUnnoticed2.Object3D.RoutePoint.RouteData route = nca.StateModel.RouteData;
                    position = nextPoint.Position;
                    float scale = route.PathRightScale;
                    float maximum = nextPoint.MaxPathRightScale;
                    global::UnityEngine.AI.NavMeshAgent agent = nca.NavAgent;
                    Vector3 right = nextPoint.PathRight;
                    agent.SetDestination(new Vector3(right.x * scale * maximum + position.x,
                        right.y * scale * maximum + position.y, right.z * scale * maximum + position.z));
                    lastSetDestinationPoint = nextPoint.Position;
                }
                NpcComponentAccessor accessor = nca;
                global::UnityEngine.AI.NavMeshAgent agentForNextPosition = accessor.NavAgent;
                Vector3 positionForNext = accessor.NpcController.Transform.position;
                agentForNextPosition.nextPosition = positionForNext;
                return;
            }
            throw new System.NullReferenceException();
        }

		public void SimpleUpdateFromOtherState(Vector3 targetPosition, float stopDist = 0.2f)
		{
		}

		public void SetMoveZero()
		{
            moveV = Vector3.zero;
        }

		public void OnFixedUpdate()
		{
            if ((int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType == 11)
            {
                float playerDistance = nca.StateModel.PlayerDist;
                if (playerDistance > 20f)
                {
                    if (skipFixedUpdateFrame < 2) { ++skipFixedUpdateFrame; return; }
                }
                else if (playerDistance > 30f)
                {
                    if (skipFixedUpdateFrame < 4) { ++skipFixedUpdateFrame; return; }
                }
            }
            Transform transform = nca.NpcController.Transform;
            Vector3 position = transform.position;
            Vector3 velocity = moveV;
            float delta = Time.fixedDeltaTime;
            float frames = unchecked(skipFixedUpdateFrame + 1);
            transform.position = new Vector3(velocity.x * delta * frames + position.x,
                velocity.y * delta * frames + position.y, velocity.z * delta * frames + position.z);
            skipFixedUpdateFrame = 0;
            float radius = nca.CapsuleCollider.radius;
            isDoCollisionCorrect = false;
            float diameter = radius + radius;
            if (nca.StateModel.PlayerDist < diameter) isDoCollisionCorrect = true;
            else if (nca.StateModel.NearNpc != null && nca.StateModel.NearNpc.NpcComponent)
            {
                Transform other = nca.StateModel.NearNpc.Transform;
                Transform own = nca.Transform;
                Vector3 otherPosition = other.position;
                Vector3 ownPosition = own.position;
                float dx = otherPosition.x - ownPosition.x;
                float dy = otherPosition.y - ownPosition.y;
                float dz = otherPosition.z - ownPosition.z;
                if ((dy * dy + dx * dx) + dz * dz < diameter * diameter)
                    isDoCollisionCorrect = true;
            }
            if (isDoCollisionCorrect) nca.NpcCollisionCorrecter.CollisionCorrect();
        }

		public void SetActiveNavAgent(bool isActiveNavAgent)
		{
            if (nca.NavAgent && nca.NavAgent.gameObject)
            {
                if (!isActiveNavAgent)
                {
                    nca.NavAgent.updatePosition = false;
                    nca.NavAgent.updatePosition = false;
                    nca.NavAgent.updateUpAxis = false;
                    nca.NavAgent.isStopped = true;
                }
                else
                {
                    global::UnityEngine.AI.NavMeshAgent agent = nca.NavAgent;
                    Vector3 position = nca.NpcController.Transform.position;
                    agent.Warp(position);
                    nca.NavAgent.updatePosition = true;
                    nca.NavAgent.updateRotation = true;
                    nca.NavAgent.updateUpAxis = true;
                    nca.NavAgent.isStopped = false;
                }
            }
        }
	}
}
