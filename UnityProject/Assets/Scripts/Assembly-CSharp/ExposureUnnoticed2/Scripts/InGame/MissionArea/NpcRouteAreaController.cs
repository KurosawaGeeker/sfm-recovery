using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame.MissionArea
{
	public class NpcRouteAreaController : MonoBehaviour
	{
		public bool IsInArea { get; private set; }

		private void OnTriggerEnter(Collider collider)
		{
            // Original complete0x4ADA80 compares Collider.gameObject to actual player's GameObject.
            var entered = collider.gameObject;
            var player = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance;
            if (player is null) throw new System.NullReferenceException();
            if (entered == player.Pca.GameObject) IsInArea = true;
        }

		private void OnTriggerExit(Collider collider)
		{
            // Original complete0x4ADC30; one matching exit clears the bool (not a contact counter).
            var exited = collider.gameObject;
            var player = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance;
            if (player is null) throw new System.NullReferenceException();
            if (exited == player.Pca.GameObject) IsInArea = false;
        }
	}
}
