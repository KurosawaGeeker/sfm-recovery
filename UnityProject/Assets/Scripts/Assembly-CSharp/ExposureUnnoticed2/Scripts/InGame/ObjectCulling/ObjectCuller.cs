using ExposureUnnoticed2.Object3D.NPC.Script;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame.ObjectCulling
{
	public class ObjectCuller : MonoBehaviour
	{
		[SerializeField]
		private GameObject cullObject;

		[SerializeField]
		private NpcCullArea npcCullArea;

		private ObjectCullCollider[] child;

		private bool isPlayerInDisplayArea;

		private void Awake()
		{
		}

		private void Update()
		{
		}

		public bool IsCulling(NpcComponent nc)
		{
			return false;
		}
	}
}
