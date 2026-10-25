using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.Toilet
{
	public class InSidePlayerChecker : MonoBehaviour
	{
		public bool IsInArea { get; private set; }

		private void OnTriggerEnter(Collider collider)
		{
			if (collider.gameObject == ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.GameObject) IsInArea = true;
		}

		private void OnTriggerExit(Collider collider)
		{
			if (collider.gameObject == ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.GameObject) IsInArea = false;
		}
	}
}
