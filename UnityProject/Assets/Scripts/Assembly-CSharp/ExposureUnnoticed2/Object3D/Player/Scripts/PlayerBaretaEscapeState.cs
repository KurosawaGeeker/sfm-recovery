namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerBaretaEscapeState
	{
		private PlayerClassAccessor pca;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x3CAB80
			this.pca = pca;
		}

		public void OnUpdate()
		{
		    // Original GameAssembly RVA 0x630B70..0x631173. No added arrival or zero-vector guard.
		    var steeringTarget = pca.NavAgent.steeringTarget;
		    var sourcePosition = pca.Transform.position;
		    var direction = new global::UnityEngine.Vector3(
		        steeringTarget.x - sourcePosition.x,
		        steeringTarget.y - sourcePosition.y,
		        steeringTarget.z - sourcePosition.z);
		    var desiredRotation = global::UnityEngine.Quaternion.LookRotation(direction, global::UnityEngine.Vector3.up);
		    var rotationTransform = pca.Transform;
		    var currentRotation = rotationTransform.rotation;
		    float rotationDelta = global::UnityEngine.Time.deltaTime;
		    rotationTransform.rotation = global::UnityEngine.Quaternion.RotateTowards(currentRotation, desiredRotation, rotationDelta * 400f);

		    float speed = global::ExposureUnnoticed2.Master.Common.MCommon.PlayerJogSpeed;
		    var facingForAngle = pca.Transform.forward;
		    float angle = global::UnityEngine.Vector3.Angle(direction, facingForAngle);
		    if (70f < angle) speed *= 0.3f;

		    var movementTransform = pca.Transform;
		    var positionBeforeMove = movementTransform.position;
		    var forward = pca.Transform.forward;
		    float movementDelta = global::UnityEngine.Time.deltaTime;
		    // Keep the original per-component MULSS/MULSS/ADDSS order.
		    var positionAfterMove = new global::UnityEngine.Vector3(
		        forward.x * speed * movementDelta + positionBeforeMove.x,
		        forward.y * speed * movementDelta + positionBeforeMove.y,
		        forward.z * speed * movementDelta + positionBeforeMove.z);
		    movementTransform.position = positionAfterMove;

		    pca.NavAgent.SetDestination(global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.CurrentStageController.GetEscapeTarget().position);
		    pca.NavAgent.nextPosition = pca.Transform.position;
		}
	}
}
