using ExposureUnnoticed2.Master.Stage;
using ExposureUnnoticed2.Object3D.Portal;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class PortalManager : MonoBehaviour
	{
		public PortalController[] Portals { get; private set; }

		public void Initialize()
		{
            // Recovered complete original native body.
            Portals = GetComponentsInChildren<PortalController>();
        }

		public Transform GetPortalTransform(StageType type)
		{
            // Recovered complete original native body.
            foreach (PortalController portal in Portals)
            {
                if (!portal) return null;
                if (portal.StageType == type) return portal.transform;
            }
            return null;
        }

		public Transform GetEscapeTarget()
		{
            // Recovered complete original native body.
            return Portals[0].transform;
        }

		public Transform GetSpawnPoint(StageType fromStageType, int id)
		{
            // Recovered complete original native body.
            foreach (PortalController portal in Portals)
                if (portal.StageType == fromStageType && portal.Id == id) return portal.SpawnPoint;
            return null;
        }

		public PortalController GetFirstPortal()
		{
            // Recovered complete original native body.
            return Portals[0];
        }

		public PortalController GetNearPortal()
		{
            // Recovered complete original native body.
            float minimum = float.MaxValue;
            PortalController nearest = null;
            foreach (PortalController portal in Portals)
            {
                float distance = ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DistSqrt(
                    portal.gameObject, ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.GameObject);
                if (distance < minimum)
                {
                    minimum = distance;
                    nearest = portal;
                }
            }
            return nearest;
        }
	}
}
