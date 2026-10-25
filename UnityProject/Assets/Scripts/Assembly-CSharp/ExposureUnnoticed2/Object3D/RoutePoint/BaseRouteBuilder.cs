using UnityEngine;

namespace ExposureUnnoticed2.Object3D.RoutePoint
{
	public class BaseRouteBuilder : MonoBehaviour
	{
		public static BaseRouteBuilder Instance;

		public virtual void Initialize()
		{
			Instance = this;
		}

		public virtual void OnDestroy()
		{// Complete original method; original thresholds, array capture, RNG and lifecycle order.
Instance = null;
}

		public virtual RouteData CreateRoute(bool isFemale = false)
		{
			return null;
		}

		public virtual RouteData CreateRoute2()
		{
			return null;
		}

		protected RoutePoint CalcNearestPointByStep(RoutePoint goalRp, RoutePoint[] path, int startIndex, int step)
		{
			return null;
		}

		protected int CalcNearPointIndex(GameObject start, RoutePoint[] points)
		{
			return 0;
		}
	}
}
