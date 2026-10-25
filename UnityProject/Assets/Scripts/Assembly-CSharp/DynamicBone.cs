using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[AddComponentMenu("Dynamic Bone/Dynamic Bone")]
public class DynamicBone : MonoBehaviour
{
	public enum UpdateMode
	{
		Normal = 0,
		AnimatePhysics = 1,
		UnscaledTime = 2,
		Default = 3
	}

	public enum FreezeAxis
	{
		None = 0,
		X = 1,
		Y = 2,
		Z = 3
	}

	private class Particle
	{
		public Transform m_Transform;

		public int m_ParentIndex;

		public int m_ChildCount;

		public float m_Damping;

		public float m_Elasticity;

		public float m_Stiffness;

		public float m_Inert;

		public float m_Friction;

		public float m_Radius;

		public float m_BoneLength;

		public bool m_isCollide;

		public bool m_TransformNotNull;

		public Vector3 m_Position;

		public Vector3 m_PrevPosition;

		public Vector3 m_EndOffset;

		public Vector3 m_InitLocalPosition;

		public Quaternion m_InitLocalRotation;

		public Vector3 m_TransformPosition;

		public Vector3 m_TransformLocalPosition;

		public Matrix4x4 m_TransformLocalToWorldMatrix;
	}

	private class ParticleTree
	{
		public Transform m_Root;

		public Vector3 m_LocalGravity;

		public Matrix4x4 m_RootWorldToLocalMatrix;

		public float m_BoneTotalLength;

		public List<Particle> m_Particles;

		public Vector3 m_RestGravity;
	}

	[Tooltip("The roots of the transform hierarchy to apply physics.")]
	public Transform m_Root;

	public List<Transform> m_Roots;

	[Tooltip("Internal physics simulation rate.")]
	public float m_UpdateRate;

	public UpdateMode m_UpdateMode;

	[Range(0f, 1f)]
	[Tooltip("How much the bones slowed down.")]
	public float m_Damping;

	public AnimationCurve m_DampingDistrib;

	[Range(0f, 1f)]
	[Tooltip("How much the force applied to return each bone to original orientation.")]
	public float m_Elasticity;

	public AnimationCurve m_ElasticityDistrib;

	[Range(0f, 1f)]
	[Tooltip("How much bone's original orientation are preserved.")]
	public float m_Stiffness;

	public AnimationCurve m_StiffnessDistrib;

	[Tooltip("How much character's position change is ignored in physics simulation.")]
	[Range(0f, 1f)]
	public float m_Inert;

	public AnimationCurve m_InertDistrib;

	[Tooltip("How much the bones slowed down when collide.")]
	public float m_Friction;

	public AnimationCurve m_FrictionDistrib;

	[Tooltip("Each bone can be a sphere to collide with colliders. Radius describe sphere's size.")]
	public float m_Radius;

	public AnimationCurve m_RadiusDistrib;

	[Tooltip("If End Length is not zero, an extra bone is generated at the end of transform hierarchy.")]
	public float m_EndLength;

	[Tooltip("If End Offset is not zero, an extra bone is generated at the end of transform hierarchy.")]
	public Vector3 m_EndOffset;

	[Tooltip("The force apply to bones. Partial force apply to character's initial pose is cancelled out.")]
	public Vector3 m_Gravity;

	[Tooltip("The force apply to bones.")]
	public Vector3 m_Force;

	[Range(0f, 1f)]
	[Tooltip("Control how physics blends with existing animation.")]
	public float m_BlendWeight;

	[Tooltip("Collider objects interact with the bones.")]
	public List<DynamicBoneColliderBase> m_Colliders;

	[Tooltip("Bones exclude from physics simulation.")]
	public List<Transform> m_Exclusions;

	[Tooltip("Constrain bones to move on specified plane.")]
	public FreezeAxis m_FreezeAxis;

	[Tooltip("Disable physics simulation automatically if character is far from camera or player.")]
	public bool m_DistantDisable;

	public Transform m_ReferenceObject;

	public float m_DistanceToObject;

	[HideInInspector]
	public bool m_Multithread;

	private Vector3 m_ObjectMove;

	private Vector3 m_ObjectPrevPosition;

	private float m_ObjectScale;

	private float m_Time;

	private float m_Weight;

	private bool m_DistantDisabled;

	private int m_PreUpdateCount;

	private List<ParticleTree> m_ParticleTrees;

	private float m_DeltaTime;

	private List<DynamicBoneColliderBase> m_EffectiveColliders;

	private bool m_WorkAdded;

	private static List<DynamicBone> s_PendingWorks;

	private static List<DynamicBone> s_EffectiveWorks;

	private static AutoResetEvent s_AllWorksDoneEvent;

	private static int s_RemainWorkCount;

	private static Semaphore s_WorkQueueSemaphore;

	private static int s_WorkQueueIndex;

	private static int s_UpdateCount;

	private static int s_PrepareFrame;

	private bool isInitAnimationCurveKeyLenght;

	private int dampKeyLength;

	private int elasKeyLength;

	private int stiffKeyLength;

	private int inertKeyLength;

	private int frictKeyLength;

	private int radiusKeyLength;

	private void Start()
	{
	}

	private void FixedUpdate()
	{
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
	}

	private void Prepare()
	{
	}

	private bool IsNeedUpdate()
	{
		return false;
	}

	private void PreUpdate()
	{
	}

	private void CheckDistance()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnValidate()
	{
	}

	private bool IsRootChanged()
	{
		return false;
	}

	private void OnDidApplyAnimationProperties()
	{
	}

	private void OnDrawGizmosSelected()
	{
	}

	private void DrawGizmos(ParticleTree pt)
	{
	}

	public void SetWeight(float w)
	{
	}

	public float GetWeight()
	{
		return 0f;
	}

	private void UpdateParticles()
	{
	}

	public void SetupParticles()
	{
	}

	private void AppendParticleTree(Transform root)
	{
	}

	private void AppendParticles(ParticleTree pt, Transform b, int parentIndex, float boneLength)
	{
	}

	public void UpdateParameters()
	{
	}

	private void InitKey()
	{
	}

	private void UpdateParameters(ParticleTree pt)
	{
	}

	private void InitTransforms()
	{
	}

	private void InitTransforms(ParticleTree pt)
	{
	}

	private void ResetParticlesPosition()
	{
	}

	private void ResetParticlesPosition(ParticleTree pt)
	{
	}

	private void UpdateParticles1(float timeVar, int loopIndex)
	{
	}

	private void UpdateParticles1(ParticleTree pt, float timeVar, int loopIndex)
	{
	}

	private void UpdateParticles2(float timeVar)
	{
	}

	private void UpdateParticles2(ParticleTree pt, float timeVar)
	{
	}

	private void SkipUpdateParticles()
	{
	}

	private void SkipUpdateParticles(ParticleTree pt)
	{
	}

	private static Vector3 MirrorVector(Vector3 v, Vector3 axis)
	{
		return default;
	}

	private void ApplyParticlesToTransforms()
	{
	}

	private void ApplyParticlesToTransforms(ParticleTree pt, Vector3 ax, Vector3 ay, Vector3 az, bool nx, bool ny, bool nz)
	{
	}

	private static void AddPendingWork(DynamicBone db)
	{
	}

	private static void AddWorkToQueue(DynamicBone db)
	{
	}

	private static DynamicBone GetWorkFromQueue()
	{
		return null;
	}

	private static void ThreadProc()
	{
	}

	private static void InitThreadPool()
	{
	}

	private static void ExecuteWorks()
	{
	}
}
