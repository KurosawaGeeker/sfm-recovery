using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.RoutePoint
{
	public class RouteBuilderStationFront : BaseRouteBuilder
	{
		private enum RoutePattern
		{
			DoorMidDoor = 0,
			ExDoor = 1,
			DoorEx = 2,
			EndEnum = 3
		}

		[SerializeField]
		private GameObject stationSideToExternalPointParent;

		[SerializeField]
		private GameObject stationSideMiddleGoalPointParent;

		[SerializeField]
		private GameObject stationSideDoorPointParent;

		[SerializeField]
		private GameObject shopSideToExternalPointParent;

		[SerializeField]
		private GameObject shopSideMiddleGoalPointParent;

		[SerializeField]
		private GameObject shopSideDoorPointParent;

		[SerializeField]
		private GameObject stationSidePathParent;

		[SerializeField]
		private GameObject[] shopSideSilentPathParent;

		[SerializeField]
		private GameObject[] shopSideShopPathParent;

		[SerializeField]
		private RoutePoint bridgePointStationSide;

		[SerializeField]
		private RoutePoint bridgePointShopSide;

		[SerializeField]
		private RoutePoint laundryPoint;

		[SerializeField]
		private GameObject bridgePathParent;

		private RoutePoint[] stationSideToExternalList;

		private RoutePoint[] stationSideMiddleGoalList;

		private RoutePoint[] stationSideDoorList;

		private RoutePoint[] shopSideToExternalList;

		private RoutePoint[] shopSideMiddleGoalList;

		private RoutePoint[] shopSideDoorList;

		private List<RoutePoint[]> shopSideShopPathList;

		private List<RoutePoint[]> shopSideSilentPathList;

		private Dictionary<int, List<RoutePoint>> doorFloorDict;

		private Dictionary<int, List<RoutePoint>> middleFloorDict;

		private Dictionary<int, List<RoutePoint[]>> middlePathDict;

		private RoutePoint[] stationSidePath;

		private RoutePoint[] bridgePath;

		public override void Initialize()
		{
		}

		private void CalcPathRight(RoutePoint[] path)
		{
		}

		public override RouteData CreateRoute(bool isFemale = false)
		{
			return null;
		}

		private RoutePoint GetRandomManyShop(RoutePoint[] list)
		{
			return null;
		}

		private RouteData CreateBridgeData()
		{
			return null;
		}

		private RouteData CreateShopSideDoorExData()
		{
			return null;
		}

		private RouteData CreateShopSideExDoorData()
		{
			return null;
		}

		private RouteData CreateShopSideBothMidBothData()
		{
			return null;
		}

		private RouteData CreateStationSideDoorExData()
		{
			return null;
		}

		private RouteData CreateStationSideExDoorData()
		{
			return null;
		}

		private RouteData CreateStationSideBothMidBothData()
		{
			return null;
		}

		private RoutePoint SelectMiddlePoint()
		{
			return null;
		}

		private void InsertMiddlePath(RouteData rd, bool isStationSide)
		{
		}

		private void InsertBridgePath(RouteData rd)
		{
		}

		private int GetWeightRandomFloor()
		{
			return 0;
		}
	}
}
