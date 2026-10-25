using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.CustomLookAtTarget
{
	public class CustomLookAtTargetComponent : MonoBehaviour
	{
		public static CustomLookAtTargetComponent Instance { get; private set; }

		private void Awake()
		{
// Complete original native body draft; native/CLR acceptance pending.
Instance = this;
}

		private void OnDestroy()
		{
// Complete original native body draft; native/CLR acceptance pending.
Instance = null;
}

		private void Update()
		{
// Complete original native body draft; native/CLR acceptance pending.

if (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance == null) return;
var source = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.Transform;
var destination = transform;
Vector3 position = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.Head.position;
Vector3 forward = source.forward;
destination.position = new Vector3(forward.x * 5f + position.x, forward.y * 5f + position.y, forward.z * 5f + position.z);

}
	}
}
