using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcCollisionCorrecter
	{
		private NpcComponentAccessor nca;

		private Collider[] collisionResults = new Collider[10];

		private LayerMask collisionLayerMask;

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
			collisionLayerMask = 1 << UnityEngine.LayerMask.NameToLayer("Npc");
		}

		public void CollisionCorrect()
		{
            float height = nca.CapsuleCollider.height;
            float firstRadius = nca.CapsuleCollider.radius;
            float halfSegment = height * 0.5f - firstRadius;
            Vector3 firstPosition = nca.Transform.position;
            Vector3 firstCenter = nca.CapsuleCollider.center;
            Vector3 firstOrigin = new Vector3(firstPosition.x + firstCenter.x, firstPosition.y + firstCenter.y, firstPosition.z + firstCenter.z);
            Vector3 firstUp = Vector3.up;
            Vector3 secondPosition = nca.Transform.position;
            Vector3 secondCenter = nca.CapsuleCollider.center;
            Vector3 secondOrigin = new Vector3(secondPosition.x + secondCenter.x, secondPosition.y + secondCenter.y, secondPosition.z + secondCenter.z);
            Vector3 secondUp = Vector3.up;
            float queryRadius = nca.CapsuleCollider.radius;
            Collider[] queryResults = collisionResults;
            LayerMask queryMask = collisionLayerMask;
            Vector3 bottom = new Vector3(firstOrigin.x - firstUp.x * halfSegment, firstOrigin.y - firstUp.y * halfSegment, firstOrigin.z - firstUp.z * halfSegment);
            Vector3 top = new Vector3(secondUp.x * halfSegment + secondOrigin.x, secondUp.y * halfSegment + secondOrigin.y, secondUp.z * halfSegment + secondOrigin.z);
            int count = Physics.OverlapCapsuleNonAlloc(bottom, top, queryRadius, queryResults, queryMask);
            Vector3 correction = Vector3.zero;
            for (int i = 0; i < count; ++i)
            {
                Collider other = collisionResults[i];
                CapsuleCollider ownCollider = nca.CapsuleCollider;
                if (other != ownCollider && !other.isTrigger)
                {
                    NpcComponentAccessor accessor = nca;
                    Transform ownTransform = accessor.Transform;
                    CapsuleCollider ownShape = accessor.CapsuleCollider;
                    Vector3 ownPosition = ownTransform.position;
                    Quaternion ownRotation = Quaternion.identity;
                    Vector3 otherPosition = other.transform.position;
                    Quaternion otherRotation = other.transform.rotation;
                    Vector3 direction = Vector3.zero;
                    float distance = 0f;
                    if (Physics.ComputePenetration(ownShape, ownPosition, ownRotation, other, otherPosition, otherRotation, out direction, out distance))
                    {
                        correction = new Vector3(direction.x * distance + correction.x, direction.y * distance + correction.y, direction.z * distance + correction.z);
                    }
                }
            }
            Vector3 forward = nca.Transform.forward;
            Vector3 up = Vector3.up;
            Quaternion rotation = Quaternion.LookRotation(forward, up);
            Quaternion inverse = Quaternion.Inverse(rotation);
            float twiceZ = inverse.z + inverse.z;
            float twiceY = inverse.y + inverse.y;
            float localX = (1f - (twiceZ * inverse.z + twiceY * inverse.y)) * correction.x
                + (twiceY * inverse.x - twiceZ * inverse.w) * correction.y
                + (twiceZ * inverse.x + twiceY * inverse.w) * correction.z;
            int side = localX >= 0f ? 1 : -1;
            if (localX == 0f) side = 0;
            Transform positionTarget = nca.Transform;
            Vector3 position = positionTarget.position;
            Vector3 right = nca.Transform.right;
            float sign = side;
            positionTarget.position = new Vector3(right.x * 0.01f * sign + correction.x + position.x,
                right.y * 0.01f * sign + correction.y + position.y, right.z * 0.01f * sign + correction.z + position.z);
        }
	}
}
