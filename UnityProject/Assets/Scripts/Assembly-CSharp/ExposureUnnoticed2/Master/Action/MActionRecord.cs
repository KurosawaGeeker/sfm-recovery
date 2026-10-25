using ExposureUnnoticed2.Master.Mission;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Action
{
	[CreateAssetMenu(fileName = "MActionRecord", menuName = "ScriptableObjects/MActionRecord")]
	public class MActionRecord : ScriptableObject
	{
		[SerializeField]
		private ActionType type;

		[SerializeField]
		private bool canMove;

		[SerializeField]
		public bool CantCancelDash;

		[SerializeField]
		public bool IsMoveOnlyForwardWithStrafe;

		[SerializeField]
		public bool IsMoveToCancel;

		[SerializeField]
		public float HandcuffsWeight;

		[SerializeField]
		public bool IsIgnoreYOffset;

		[SerializeField]
		public bool IsKeepClothesArmLayer;

		[SerializeField]
		public bool IsSlidePants;

		[SerializeField]
		public bool IsUseAnal;

		[SerializeField]
		public bool IsUsePussy;

		[SerializeField]
		public bool CanTransEcstasy;

		[SerializeField]
		public bool IsEcstasyToIdle;

		[SerializeField]
		public bool IsFixBreast;

		[SerializeField]
		public bool IsIgnoreClothesArmPulse;

		[SerializeField]
		public bool IsIgnoreClothesArmLayerByEcstasy;

		[SerializeField]
		public bool CanUnlock;

		[SerializeField]
		public bool CanStartPee;

		[SerializeField]
		public bool CanCancelByGamanVibe;

		[SerializeField]
		public int AnotherMotionCount;

		[SerializeField]
		public float AddDokidoki100;

		[SerializeField]
		public bool DeActiveHandcuffOverrideArm;

		[SerializeField]
		public float Discomfort;

		[SerializeField]
		public float Strange;

		[SerializeField]
		public bool IsDisableLookAt;

		[SerializeField]
		public ExposedBodyParts[] ExposedBodyParts;

		[Range(1f, 2f)]
		[SerializeField]
		public float StrangenessScale;

		[SerializeField]
		public float AddEcstasy;

		[SerializeField]
		public float RotateYOffset;

		[SerializeField]
		public float FloatParam;

		[SerializeField]
		public Vector3 VectorParam;

		public ActionType Type => type;

		public bool CanMove => canMove;
	}
}
