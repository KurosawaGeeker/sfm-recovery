using ExposureUnnoticed2.Object3D.Player.Scripts;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class PlayerSearcher : MonoBehaviour
	{
		private Vector3 offset = new Vector3(0f, 1.427f, 0.111f);

		private Vector3 sitOffset = new Vector3(0f, 1.05f, -0.28f);

		private Vector3 sitOffsetInverse = new Vector3(0f, 1.05f, 0.28f);

		private Vector3 pinponOffset = new Vector3(0f, 0.05f, 0.1f);

		[SerializeField]
		private PlayerSearcherChild[] cube;

		[SerializeField]
		private PlayerSearcherChild sphere;

		[SerializeField]
		private Transform eye;

		public bool IsPlayerFound;

		private PlayerController player;

		private int layerMask;

		private Transform traceTarget;

		private NpcComponentAccessor nca;

		private float skipEndTime;

		private bool isHideColliderState;

		private bool isFoundForeignObject;

		public void RegisterTraceTransform(Transform target)
		{
			traceTarget = target;
			offset.y = nca.StateModel.AvatarSizeScale * offset.y;
		}

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
			player = nca.Player;
			int ignoreRaycast = UnityEngine.LayerMask.NameToLayer("Coat");
			int playerLayer = UnityEngine.LayerMask.NameToLayer("Car");
			int blocker = ExposureUnnoticed2.Scripts.Base.LayerAccessor.PlayerBlocker;
			int sperm = ExposureUnnoticed2.Scripts.Base.LayerAccessor.SpermCollider;
			int xray = ExposureUnnoticed2.Scripts.Base.LayerAccessor.XRayLevel;
			int pee = ExposureUnnoticed2.Scripts.Base.LayerAccessor.IgnorePeeParticle;
			layerMask = (1 << ignoreRaycast) | (1 << playerLayer) | (1 << blocker) | (1 << sperm) | (1 << xray) | (1 << pee);
			PlayerSearcherChild[] children = cube;
			for (int i = 0; i < children.Length; i++) children[i].Initialize(nca);
			sphere.Initialize(nca);
		}

		public void OnUpdate()
		{
			if (traceTarget != null)
			{
			    Transform destination = transform;
			    Vector3 position = nca.Transform.position;
			    Vector3 forward = nca.Transform.forward;
			    Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
			    Vector3 result = position + rotation * offset;
			    if (destination is null) throw new System.NullReferenceException();
			    destination.position = result;
			    if (nca.StateModel.IsSit)
			    {
			        Vector3 local = sitOffset * nca.StateModel.AvatarSizeScale;
			        if (nca.StateModel.IsSitInverse) local = sitOffsetInverse * nca.StateModel.AvatarSizeScale;
			        destination = transform;
			        position = nca.Transform.position;
			        forward = nca.Transform.forward;
			        rotation = Quaternion.LookRotation(forward, Vector3.up);
			        result = position + rotation * local;
			        if (destination is null) throw new System.NullReferenceException();
			        destination.position = result;
			    }
			    if (nca.StateModel.CurrentFixedType == NpcStateModel.FixedType.Pinpon)
			    {
			        Vector3 local = pinponOffset * nca.StateModel.AvatarSizeScale;
			        destination = transform;
			        position = nca.AvatarComponent.Head.position;
			        forward = nca.Transform.forward;
			        rotation = Quaternion.LookRotation(forward, Vector3.up);
			        result = position + rotation * local;
			        if (destination is null) throw new System.NullReferenceException();
			        destination.position = result;
			    }
			    if (!isHideColliderState)
			    {
			        ForForeignUpdate();
			        ForPlayerUpdate();
			        if (IsPlayerFound || isFoundForeignObject) return;
			        isHideColliderState = true;
			        float now = Time.time;
			        float random = UnityEngine.Random.value;
			        skipEndTime = (random * 0.1f + now) + 0.5f;
			        if (!cube[0].gameObject.activeInHierarchy) return;
			        cube[0].DeActive();
			        cube[1].DeActive();
			        sphere.DeActive();
			    }
			    else
			    {
			        float previous = skipEndTime;
			        float now = Time.time;
			        if (!(previous < now)) return;
			        isHideColliderState = false;
			        cube[0].gameObject.SetActive(true);
			        cube[1].gameObject.SetActive(true);
			        sphere.gameObject.SetActive(true);
			    }
			}
			else UnityEngine.Object.Destroy(gameObject);
		}

		private void ForPlayerUpdate()
		{
			// Complete original RVA437FF0: overlap volumes, front hemisphere and actual player-hit raycasts.
			IsPlayerFound = false;
			bool inCube = false;
			PlayerSearcherChild[] capturedCube = cube;
			for (int i = 0; i < capturedCube.Length; i++)
			{
			    PlayerSearcherChild child = capturedCube[i];
			    if (child is null) throw new System.NullReferenceException();
			    if (child.IsInArea) { inCube = true; break; }
			}
			Vector3 playerPosition = ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CacheForNpcPosition;
			Vector3 npcPosition = nca.StateModel.CurrentPosition;
			Vector3 forward = nca.Transform.forward;
			bool inside = false;
			if (inCube)
			{
			    if (sphere is null) throw new System.NullReferenceException();
			    inside = sphere.IsInArea;
			}
			float dot = forward.y * (playerPosition.y - npcPosition.y) + forward.x * (playerPosition.x - npcPosition.x) + forward.z * (playerPosition.z - npcPosition.z);
			if (!(inside && dot > 0f)) return;
			float maxDistance = ExposureUnnoticed2.Master.Common.MCommon.Instance.MaxFoundDist;
			Vector3 origin = eye.position;
			int exposed = (int)ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CacheForNpcExposedBodyParts;
			RaycastHit hit;
			if ((exposed & 42) != 0)
			{
			    Vector3 target = player.Pca.PlayerAvatarObjectReferencer.StrangeSightPoint.position;
			    if (Physics.Raycast(origin, target - origin, out hit, maxDistance, ~layerMask, QueryTriggerInteraction.Ignore))
			    {
			        Collider collider = hit.collider;
			        if (collider is null) throw new System.NullReferenceException();
			        GameObject hitObject = collider.gameObject;
			        if (hitObject == ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.GameObject)
			        {
			            IsPlayerFound = true;
			            return;
			        }
			    }
			}
			Vector3 hip = player.Pca.PlayerAvatarObjectReferencer.Hip.position;
			if (!Physics.Raycast(origin, hip - origin, out hit, maxDistance, ~layerMask, QueryTriggerInteraction.Ignore)) return;
			Collider hipCollider = hit.collider;
			if (hipCollider is null) throw new System.NullReferenceException();
			GameObject hipObject = hipCollider.gameObject;
			if (hipObject == ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.GameObject) IsPlayerFound = true;
		}

		private void ForForeignUpdate()
        {
            isFoundForeignObject = false;
            nca.StateModel.InSightForeignObjectList.Clear();
            foreach (ExposureUnnoticed2.Object3D.ForeignObject.ForeignObjectController candidate in sphere.ForeignObjectList)
            {
                if (candidate == null) continue;
                PlayerSearcherChild[] children = cube;
                bool overlap = false;
                for (int i = 0; i < children.Length; i++)
                {
                    if (children[i].ForeignObjectList.Contains(candidate)) { overlap = true; break; }
                }
                if (!overlap) continue;
                float maximum = ExposureUnnoticed2.Master.Common.MCommon.Instance.MaxFoundDist;
                Vector3 origin = eye.position;
                Vector3 target = candidate.transform.position;
                RaycastHit hit;
                if (Physics.Raycast(origin, target - origin, out hit, maximum, ~layerMask, QueryTriggerInteraction.Ignore))
                {
                    if (hit.collider.gameObject == candidate.gameObject)
                        nca.StateModel.InSightForeignObjectList.Add(candidate);
                }
            }
            for (int i = 0; i < sphere.ForeignObjectList.Count; i++)
            {
                if (sphere.ForeignObjectList[i] == null)
                {
                    sphere.ForeignObjectList.Remove(sphere.ForeignObjectList[i]);
                    i--;
                }
            }
            foreach (PlayerSearcherChild child in cube)
            {
                for (int i = 0; i < child.ForeignObjectList.Count; i++)
                {
                    if (child.ForeignObjectList[i] == null)
                    {
                        child.ForeignObjectList.Remove(child.ForeignObjectList[i]);
                        i--;
                    }
                }
            }
        }

		public void OnLateUpdate()
		{
			if (traceTarget != null)
			{
			    Transform destination = transform;
			    Quaternion rotation = traceTarget.rotation;
			    if (destination is null) throw new System.NullReferenceException();
			    destination.rotation = rotation;
			}
		}
	}
}
