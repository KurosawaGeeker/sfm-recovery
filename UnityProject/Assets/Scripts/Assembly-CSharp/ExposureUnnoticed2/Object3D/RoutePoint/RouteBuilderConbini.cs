using UnityEngine;

namespace ExposureUnnoticed2.Object3D.RoutePoint
{
	public class RouteBuilderConbini : BaseRouteBuilder
	{
		[SerializeField]
		private GameObject conbiniShinadashiParent;

		[SerializeField]
		private GameObject productPickParent;

		[SerializeField]
		private RoutePoint appearPoint;

		[SerializeField]
		private RoutePoint regiCustomerPoint;

		[SerializeField]
		private RoutePoint regiCustomerJizenPoint;

		private RoutePoint[] conbiniShinadashiList;

		private RoutePoint[] productPickList;

		public override void Initialize()
		{
			base.Initialize();
			conbiniShinadashiList = conbiniShinadashiParent?.GetComponentsInChildren<RoutePoint>();
			productPickList = productPickParent?.GetComponentsInChildren<RoutePoint>();
		}

		private void CalcPathRight(RoutePoint[] path)
		{
			Vector3 direction = path[1].transform.position - path[0].transform.position;
			direction.y = 0f;
			RoutePoint first = path[0];
			Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);
			first.PathRight = rotation * direction.normalized;
			Vector3 previousRight = path[0].PathRight;
			for (int i = 1; i < path.Length - 1; ++i)
			{
			    RoutePoint current = path[i];
			    Vector3 nextDirection = path[i + 1].transform.position - current.transform.position;
			    nextDirection.y = 0f;
			    Vector3 right = Quaternion.Euler(0f, 90f, 0f) * nextDirection.normalized;
			    current.PathRight = (previousRight + right).normalized;
			    previousRight = right;
			}
			path[path.Length - 1].PathRight = previousRight;
		}

		public override RouteData CreateRoute(bool isFemale = false)
		{
			RouteData rd = new RouteData();
			rd.AppearPoint = appearPoint;
			rd.MiddleGoalPoint = regiCustomerPoint;
			rd.GoalPoint = appearPoint;
			rd.Setup();
			InsertPick(rd);
			rd.routeList.Insert(rd.routeList.Count - 2, regiCustomerJizenPoint);
			return rd;
		}

		private void InsertPick(RouteData rd)
		{
			ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.Shuffle(productPickList);
			int count = UnityEngine.Random.Range(1, 8);
			if (count > 0 && rd == null) throw new System.NullReferenceException();
			for (int i = 0; i < count; ++i)
			{
			    RoutePoint selected = productPickList[i];
			    rd.routeList.Insert(1, selected);
			}
		}

		public override RouteData CreateRoute2()
		{
			RouteDataConbini route = new RouteDataConbini();
			route.routeList = global::System.Linq.Enumerable.ToList(conbiniShinadashiList);
			route.AppearPoint = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.GetRandom(route.routeList);
			route.Setup();
			return route;
		}
	}
}
