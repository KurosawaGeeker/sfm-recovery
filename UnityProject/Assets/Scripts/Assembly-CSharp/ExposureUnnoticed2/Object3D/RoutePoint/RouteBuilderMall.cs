using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.RoutePoint
{
	public class RouteBuilderMall : BaseRouteBuilder
	{
		private enum RoutePattern
		{
			DoorMidDoor = 0,
			ExDoor = 1,
			DoorEx = 2,
			EndEnum = 3
		}

		[SerializeField]
		private GameObject elevatorPointParent;

		[SerializeField]
		private GameObject elevatorExitPointParent;

		[SerializeField]
		private GameObject toExternalPointParent;

		[SerializeField]
		private GameObject middleGoalPointParent;

		[SerializeField]
		private GameObject doorPointParent;

		[SerializeField]
		private GameObject escalatorPointParent;

		[SerializeField]
		private GameObject[] floor1Paths;

		[SerializeField]
		private GameObject[] floor2Paths;

		[SerializeField]
		private GameObject[] floor3Paths;

		[SerializeField]
		private Transform escalatorA;

		[SerializeField]
		private Transform escalatorB;

		private Transform[] routeList;

		private RoutePoint[] elevatorExitPoints;

		private RoutePoint[] toExternalList;

		private RoutePoint[] middleGoalList;

		private RoutePoint[] doorList;

		private RoutePoint[] escalatorList;

		private Dictionary<int, List<RoutePoint>> doorFloorDict;

		private Dictionary<int, List<RoutePoint>> middleFloorDict;

		private Dictionary<int, List<RoutePoint[]>> middlePathDict;

		[SerializeField]
		private RoutePoint escalatorA1to2;

		[SerializeField]
		private RoutePoint escalatorA2to1;

		[SerializeField]
		private RoutePoint escalatorA2to3;

		[SerializeField]
		private RoutePoint escalatorA3to2;

		[SerializeField]
		private RoutePoint escalatorB1to2;

		[SerializeField]
		private RoutePoint escalatorB2to1;

		public new static RouteBuilderMall Instance { get; private set; }

		public RoutePoint[] ElevatorPoints { get; private set; }

		private void Awake()
		{
		}

		public override void OnDestroy()
		{
		}

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

		private RouteData CreateDoorMidDoorData()
		{
			return null;
		}

		private RoutePoint SelectMiddlePoint()
		{
			return null;
		}

		private RouteData CreateExDoorData()
		{
			return null;
		}

		private RouteData CreateDoorExData()
		{
			return null;
		}

		private float GetEscalatorRate()
		{
			return 0f;
		}

		private void InsertElevator(RouteData rd)
		{
		}

		private void InsertEscalator(RouteData rd)
		{
		}

		private void InsertMiddlePath(RouteData rd)
		{
		}

		private RoutePoint GetNearEscalator12(RoutePoint beforePoint)
		{
			return null;
		}

		private int GetWeightRandomFloor()
		{
			return 0;
		}
	}
}
