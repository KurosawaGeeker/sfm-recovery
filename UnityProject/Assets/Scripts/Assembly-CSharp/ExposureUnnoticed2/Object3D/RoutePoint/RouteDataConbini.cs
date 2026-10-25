namespace ExposureUnnoticed2.Object3D.RoutePoint
{
	public class RouteDataConbini : RouteData
	{
		private RoutePoint nextPoint;

		public override void Setup()
		{
			nextPoint = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.GetRandom(routeList);
		}

		public override RoutePoint GetNextPoint()
		{
			return nextPoint;
		}

		public override RoutePoint IncrementIndex()
		{
			nextPoint = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.GetRandom(routeList);
			return GetNextPoint();
		}

		public override void SetNextPoint(RoutePoint point)
		{
			nextPoint = point;
		}
	}
}
