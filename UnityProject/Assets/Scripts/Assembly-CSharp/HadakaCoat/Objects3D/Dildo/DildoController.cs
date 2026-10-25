using ExposureUnnoticed2.Object3D.InteractArea;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;

namespace HadakaCoat.Objects3D.Dildo
{
	public class DildoController : MonoBehaviour
	{
		public enum DildoTarget
		{
			Pussy = 0,
			Anal = 1,
			Fella = 2
		}

		public const int IsWallFlag = 1;

		public const int IsFloorFlag = 0;

		[SerializeField]
		private bool isWall;

		[SerializeField]
		private Transform[] bones;

		[SerializeField]
		private Transform directBone;

		[SerializeField]
		private Transform meshParent;

		[SerializeField]
		private Transform heelScaleTarget;

		private Animator animator;

		private PlayerController player;

		private static readonly int X;

		private static readonly int Z;

		private InteractAreaController areaController;

		private float initMeshY;

		private DildoTarget currentTarget;

		public bool IsSit;

		private Quaternion initLookAtRotation;

		private Vector3 initHeelScaleTargetScale;

		public DroppedItemManager.DropItemType DildoId { get; set; }

		public void SetTarget(DildoTarget target)
		{
		}

		private void Start()
		{
		}

		private bool CommonCheckIsCantUse()
		{
			return false;
		}

		private void LateUpdate()
		{
		}
	}
}
