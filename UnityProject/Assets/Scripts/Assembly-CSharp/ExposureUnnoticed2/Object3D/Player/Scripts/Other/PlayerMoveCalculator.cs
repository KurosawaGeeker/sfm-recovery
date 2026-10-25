using UnityEngine;
using UnityEngine.AI;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Other
{
	public class PlayerMoveCalculator
	{
		private const float StandStartMoveDist = 5f;

		private const float CrouchStartMoveDist = 2.5f;

		private PlayerClassAccessor pca;

		private Vector3 currentTirePoint;

		private float calcCoolUTime;

		private NavMeshPath path;

		private Vector3 oldPositionForRealtime;

		public float JustMoveAmount { get; private set; }

		public float JustMoveAmountRealtime { get; private set; }

		public float LastUpdateJustMoveAmount { get; private set; }

		public float StartMoveDist => pca.PlayerState.IsCrouch ? 2.5f : 5f;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x406800
			this.pca = pca;
			path = new UnityEngine.AI.NavMeshPath();
			currentTirePoint = pca.Transform.position;
		}

		public void OnWarp()
		{
			// Recovered from GameAssembly.dll RVA 0x4069D0
			Vector3 position = pca.Transform.position;
			currentTirePoint = position;
			oldPositionForRealtime = position;
		}

		public void OnUpdate()
		{
Vector3 old = oldPositionForRealtime;
JustMoveAmountRealtime = (old - pca.Transform.position).magnitude;
oldPositionForRealtime = pca.Transform.position;
if (JustMoveAmountRealtime > 5f) JustMoveAmountRealtime = 0f;
JustMoveAmount = 0f;
calcCoolUTime -= Time.unscaledDeltaTime;
if (calcCoolUTime > 0f) return;
calcCoolUTime = 0.2f;
int stage = (int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType;
int areaMask = stage == 10 || (uint)(stage - 11) <= 1U ? 8 : -1;
Vector3 tire = currentTirePoint;
NavMesh.CalculatePath(tire, pca.Transform.position, areaMask, path);
if (path.status != NavMeshPathStatus.PathComplete)
{
    currentTirePoint = pca.Transform.position;
}
else
{
    Vector3[] corners = path.corners;
    Vector3 start = currentTirePoint;
    float maxRange = pca.PlayerLocomotion.CurrentMoveSpeed * 0.22f;
    float remaining = maxRange;
    Vector3 point = Vector3.zero;
    float length = 0f;
    for (int i = 1; i < corners.Length; i++)
    {
        Vector3 next = corners[i];
        Vector3 prev = i == 1 ? start : corners[i - 1];
        float distance = Vector3.Distance(next, prev);
        length += distance;
        if (distance > remaining && point == Vector3.zero)
        {
            point = prev + Vector3.Normalize(next - prev) * remaining;
        }
        else remaining -= distance;
    }
    if (point != Vector3.zero)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(point, out hit, maxRange * 2f, areaMask)) point = hit.position;
    }
    if (length > StartMoveDist)
    {
        JustMoveAmount = (point - currentTirePoint).magnitude;
        currentTirePoint = point;
    }
}
LastUpdateJustMoveAmount = JustMoveAmount;
}
	}
}
