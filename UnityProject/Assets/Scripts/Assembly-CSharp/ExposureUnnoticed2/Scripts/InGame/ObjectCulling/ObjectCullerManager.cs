using ExposureUnnoticed2.Object3D.NPC.Script;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame.ObjectCulling
{
	public class ObjectCullerManager : MonoBehaviour
	{
		private ObjectCuller[] cullers;

		public static ObjectCullerManager Instance { get; private set; }

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		public bool IsCulling(NpcComponent nc)
		{
			return false;
		}
	}
}
