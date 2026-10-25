using UnityEngine;

[AddComponentMenu("Dynamic Bone/Dynamic Bone Plane Collider")]
public class DynamicBonePlaneCollider : DynamicBoneColliderBase
{
	private Plane m_Plane;

	private void OnValidate()
	{
	}

	public override void Prepare()
	{
	}

	public override bool Collide(ref Vector3 particlePosition, float particleRadius)
	{
		return false;
	}

	private void OnDrawGizmosSelected()
	{
	}
}
