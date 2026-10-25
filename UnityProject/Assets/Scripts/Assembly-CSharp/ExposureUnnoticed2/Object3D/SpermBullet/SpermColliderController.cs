using ExposureUnnoticed2.Object3D.NPC.Script;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.SpermBullet
{
	public class SpermColliderController : MonoBehaviour
	{
		[SerializeField]
		private NpcAvatarComponent avatarComponent;

		[SerializeField]
		private bool isAlwaysSkin;

		[SerializeField]
		private int[] skinBodyIndex;

		[SerializeField]
		private int[] activeBodyIndex;

		[SerializeField]
		private int[] activeHairIndex;

		public bool IsSkin { get; private set; }

		private void Awake()
		{
		}

		public NpcController GetNpcController()
		{
			return null;
		}
	}
}
