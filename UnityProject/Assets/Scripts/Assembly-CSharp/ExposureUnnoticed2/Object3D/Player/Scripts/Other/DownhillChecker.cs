using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Other
{
	public class DownhillChecker : MonoBehaviour
	{
		private int ignoreLayerMask;

		private bool isInArea;

		public bool IsFrontDownhill => !isInArea;

		public bool IsGrounded => isInArea;

		private void Awake()
		{
			int playerLayer = LayerMask.NameToLayer("Player");
			int npcLayer = LayerMask.NameToLayer("Npc");
			int playerFaceLayer = LayerMask.NameToLayer("PlayerFace");
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ExposureUnnoticed2.Scripts.Base.LayerAccessor).TypeHandle);
			int blockerLayer = ExposureUnnoticed2.Scripts.Base.LayerAccessor.PlayerBlocker;
			ignoreLayerMask = (1 << playerLayer) | (1 << npcLayer) | (1 << playerFaceLayer) | (1 << blockerLayer);
		}

		private void FixedUpdate()
		{
			isInArea = false;
		}

		private void OnTriggerStay(Collider collider)
		{
			if (isInArea) return;
			int capturedIgnoreLayerMask = ignoreLayerMask;
			int layer = collider.gameObject.layer;
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
			if ((capturedIgnoreLayerMask & (1 << layer)) != 0) return;
			if (collider.isTrigger) return;
			isInArea = true;
		}
	}
}
