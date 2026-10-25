using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame.MissionArea
{
	public class NpcRouteAreaManager : MonoBehaviour
	{
		private NpcRouteAreaController[] areas;

		public static NpcRouteAreaManager Instance { get; private set; }

		public bool IsInArea { get; private set; }

		private void Awake()
		{
            // Original complete0x4ADEC0; first live singleton wins, then query active children.
            if (Instance != null) return;
            Instance = this;
            areas = GetComponentsInChildren<NpcRouteAreaController>(false);
        }

		private void OnDestroy()
		{
            // Original complete0x4AE050; use Unity object identity/liveness equality.
            if (Instance == this) Instance = null;
        }

		private void Update()
		{
            // Original complete0x4AE1A0; preserve first-true short circuit and failure prefix.
            var current = areas;
            for (int i = 0; i < current.Length; i++)
            {
                if (current[i].IsInArea)
                {
                    IsInArea = true;
                    return;
                }
            }
            IsInArea = false;
        }
	}
}
