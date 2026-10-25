using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerHipAttachmentTracer : MonoBehaviour
	{
		[SerializeField]
		private float XRotation;

		[SerializeField]
		private float NormalRotation;

		[SerializeField]
		private float MaxRotation;

		[SerializeField]
		private Transform rotateTarget;

		[SerializeField]
		private float kaikyakuPositionScale;

		private float initZ;

		private void Awake()
		{
		}

		private void LateUpdate()
		{
		}
	}
}
