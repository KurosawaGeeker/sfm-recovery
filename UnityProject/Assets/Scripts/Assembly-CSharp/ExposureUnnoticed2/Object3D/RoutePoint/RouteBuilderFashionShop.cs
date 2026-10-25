using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.RoutePoint
{
	public class RouteBuilderFashionShop : BaseRouteBuilder
	{
		[SerializeField]
		private RoutePoint regiClerkPoint;

		[SerializeField]
		private RoutePoint appearPoint;

		[SerializeField]
		private RoutePoint roomWaitPoint;

		[SerializeField]
		private RoutePoint regiWaitPoint;

		[SerializeField]
		private GameObject middleGoalTakeGoodsParent;

		[SerializeField]
		private Transform pathListParent;

		private RoutePoint[] middleGoalTakeGoodsPoints;

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

		public override RouteData CreateRoute2()
		{
			return null;
		}

		private void InsertPick(RouteData rd)
		{
		}

		private void InsertMiddlePath(RouteData rd)
		{
		}
	}
}
