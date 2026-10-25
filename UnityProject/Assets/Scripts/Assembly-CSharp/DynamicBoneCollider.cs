using UnityEngine;

[AddComponentMenu("Dynamic Bone/Dynamic Bone Collider")]
public class DynamicBoneCollider : DynamicBoneColliderBase
{
	[Tooltip("The radius of the sphere or capsule.")]
	public float m_Radius;

	[Tooltip("The height of the capsule.")]
	public float m_Height;

	[Tooltip("The other radius of the capsule.")]
	public float m_Radius2;

	private float m_ScaledRadius;

	private float m_ScaledRadius2;

	private Vector3 m_C0;

	private Vector3 m_C1;

	private float m_C01Distance;

	private int m_CollideType;

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

	private static bool OutsideSphere(ref Vector3 particlePosition, float particleRadius, Vector3 sphereCenter, float sphereRadius)
	{
		return false;
	}

	private static bool InsideSphere(ref Vector3 particlePosition, float particleRadius, Vector3 sphereCenter, float sphereRadius)
	{
		return false;
	}

	private static bool OutsideCapsule(ref Vector3 particlePosition, float particleRadius, Vector3 capsuleP0, Vector3 capsuleP1, float capsuleRadius, float dirlen)
	{
		return false;
	}

	private static bool InsideCapsule(ref Vector3 particlePosition, float particleRadius, Vector3 capsuleP0, Vector3 capsuleP1, float capsuleRadius, float dirlen)
	{
		return false;
	}

	private static bool OutsideCapsule2(ref Vector3 particlePosition, float particleRadius, Vector3 capsuleP0, Vector3 capsuleP1, float capsuleRadius0, float capsuleRadius1, float dirlen)
	{
		return false;
	}

	private static bool InsideCapsule2(ref Vector3 particlePosition, float particleRadius, Vector3 capsuleP0, Vector3 capsuleP1, float capsuleRadius0, float capsuleRadius1, float dirlen)
	{
		return false;
	}

	private void OnDrawGizmosSelected()
	{
	}

	private static void DrawCapsule(Vector3 c0, Vector3 c1, float radius0, float radius1)
	{
	}
}
