using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcCollider : MonoBehaviour
	{
		private bool isInArea;

		private NpcComponent npc;

		public bool IsJustArea { get; private set; }

		public bool IsInArea => isInArea;

		private void Awake()
        {
            npc = transform.parent.GetComponent<NpcComponent>();
        }

		public void OnUpdate()
		{
			IsJustArea = false;
		}

		private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject == ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.GameObject)
            {
                IsJustArea = true;
                isInArea = true;
            }
        }
	}
}
