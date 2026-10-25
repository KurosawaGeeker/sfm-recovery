using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Other
{
	public class PlayerCollisionCorrecter
	{
		private PlayerClassAccessor pca;

		private Collider[] collisionResults;

		private LayerMask collisionLayerMask;

		public void Initialize(PlayerClassAccessor nca)
		{
			// Recovered from GameAssembly.dll RVA 0x4058B0
			pca = nca;
			         collisionLayerMask = (1 << LayerMask.NameToLayer("Npc")) | (1 << global::ExposureUnnoticed2.Scripts.Base.LayerAccessor.LevelLayer) | (1 << LayerMask.NameToLayer("Default"));
		}

		public void CollisionCorrect()
		{
			// GameAssembly.dll RVA 0x405A50. Preserve the original upright capsule query
			// and summed penetration correction; this is separate from CharacterController.Move.
			float halfSegment = pca.CapsuleCollider.height * 0.5f - pca.CapsuleCollider.radius;
			Vector3 bottom = pca.Transform.position + pca.CapsuleCollider.center - Vector3.up * halfSegment;
			Vector3 top = pca.Transform.position + pca.CapsuleCollider.center + Vector3.up * halfSegment;
			int count = Physics.OverlapCapsuleNonAlloc(bottom, top, pca.CapsuleCollider.radius,
				collisionResults, collisionLayerMask);
			Vector3 correction = Vector3.zero;
			for (int i = 0; i < count; ++i)
			{
				Collider other = collisionResults[i];
				if (other != pca.CapsuleCollider && !other.isTrigger)
				{
					Vector3 direction;
					float distance;
					if (Physics.ComputePenetration(pca.CapsuleCollider, pca.Transform.position,
						Quaternion.identity, other, other.transform.position, other.transform.rotation,
						out direction, out distance))
					{
						correction = direction * distance + correction;
					}
				}
			}
			// These native calls occur even though the inverse is not used afterwards.
			Quaternion.Inverse(Quaternion.LookRotation(pca.Transform.forward, Vector3.up));
			pca.Transform.position += correction;
		}
	// Recovered original member RVA 0x406720: .ctor
		public PlayerCollisionCorrecter()
        {
            collisionResults = new UnityEngine.Collider[10];
        }
	}
}
