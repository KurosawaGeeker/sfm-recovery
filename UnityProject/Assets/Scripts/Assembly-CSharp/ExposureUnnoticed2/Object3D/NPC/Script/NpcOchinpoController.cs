using ExposureUnnoticed2.Object3D.Player.Scripts;
using HadakaCoat.Objects3D.Dildo;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcOchinpoController : MonoBehaviour
	{
		[SerializeField]
		private Transform directBone;

		private DildoController.DildoTarget currentTarget;

		private PlayerController player;

		private Quaternion initLookAtRotation;

		private void Start()
		{
		}

		private void LateUpdate()
		{
		}
	}
}
