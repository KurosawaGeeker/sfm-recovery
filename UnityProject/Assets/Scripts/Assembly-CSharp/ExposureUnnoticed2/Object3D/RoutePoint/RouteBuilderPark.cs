using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.RoutePoint
{
	public class RouteBuilderPark : BaseRouteBuilder
	{
		[SerializeField]
		private GameObject toExternalPointParent;

		[SerializeField]
		private GameObject middleGoalBuyMachineParent;

		[SerializeField]
		private GameObject middleGoalLookBoardParent;

		[SerializeField]
		private Transform pathListParent;

		[SerializeField]
		private RoutePoint toiletMalePoint;

		[SerializeField]
		private RoutePoint toiletFemalePoint;

		private RoutePoint[] toExternalPoints;

		private RoutePoint[] middleGoalBuyMachinePoints;

		private RoutePoint[] middleGoalLookBoardPoints;

		private List<RoutePoint[]> pathList;

		public override void Initialize()
		{
		}

		private void CalcRight(RoutePoint rp)
		{
		}

		private void CalcPathRight(RoutePoint[] path)
		{
		}

		public override RouteData CreateRoute(bool isFemale = false)
		{
			return null;
		}

		private RoutePoint GetNear(RoutePoint[] rps, Vector3 start)
		{
			return null;
		}

		private void InsertMiddlePath(RouteData rd)
		{
		}
	}
}
