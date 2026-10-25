using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.RoutePoint
{
	public class RouteBuilderMansion : BaseRouteBuilder
	{
		private RoutePoint[] stationSidePath;

		private RoutePoint[] bridgePath;

		[SerializeField]
		private GameObject elevatorPointParent;

		[SerializeField]
		private GameObject elevatorExitPointParent;

		[SerializeField]
		private GameObject stairDownGoalPointParent;

		[SerializeField]
		private GameObject stairUpGoalPointParent;

		[SerializeField]
		private GameObject[] doorPointParentByFloor;

		[SerializeField]
		private GameObject toExternalPointParent;

		[SerializeField]
		private GameObject[] mansionPaths;

		[SerializeField]
		private GameObject[] outsidePaths;

		[SerializeField]
		private GameObject postReceiveParent;

		private RoutePoint[] toExternalList;

		private RoutePoint[] elevatorExitPoints;

		private RoutePoint[] stairDownGoalPoints;

		private RoutePoint[] stairUpGoalPoints;

		private RoutePoint[] postReceivePoints;

		private Dictionary<int, List<RoutePoint>> doorFloorDict;

		private Dictionary<int, RoutePoint[]> mansionPathDict;

		private Dictionary<int, RoutePoint[]> outsidePathDict;

		public new static RouteBuilderMansion Instance { get; private set; }

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

		private RouteData DoorToEx()
		{
			return null;
		}

		private RouteData ExToDoor()
		{
			return null;
		}

		private void InsertElevator(RouteData rd)
		{
		}

		private void InsertStair(RouteData rd)
		{
		}

		private void InsertFloor1Middle(RouteData rd)
		{
		}

		private void InsertMiddlePath(RouteData rd)
		{
		}

		private int GetWeightRandomFloor()
		{
			return 0;
		}
	}
}
