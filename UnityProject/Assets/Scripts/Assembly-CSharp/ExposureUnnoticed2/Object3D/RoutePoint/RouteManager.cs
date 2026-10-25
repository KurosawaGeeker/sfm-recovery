using UnityEngine;

namespace ExposureUnnoticed2.Object3D.RoutePoint
{
	public class RouteManager : BaseRouteBuilder
	{
		private enum RoutePattern
		{
			DoorMidDoor = 0,
			ExDoor = 1,
			DoorEx = 2,
			ExEx = 3,
			EndEnum = 4
		}

		[SerializeField]
		private GameObject toExternalPointParent;

		[SerializeField]
		private GameObject middleGoalPointParent;

		[SerializeField]
		private GameObject doorPointParent;

		private Transform[] routeList;

		private RoutePoint[] toExternalList;

		private RoutePoint[] middleGoalList;

		private RoutePoint[] doorList;

		public override void Initialize()
		{// Complete original method; original thresholds, array capture, RNG and lifecycle order.
base.Initialize();
routeList = GetComponentsInChildren<Transform>();
toExternalList = toExternalPointParent?.GetComponentsInChildren<RoutePoint>();
middleGoalList = middleGoalPointParent?.GetComponentsInChildren<RoutePoint>();
doorList = doorPointParent?.GetComponentsInChildren<RoutePoint>();
}

		public override RouteData CreateRoute(bool isFemale = false)
		{// Complete original method; original thresholds, array capture, RNG and lifecycle order.
float value = UnityEngine.Random.value;
if ((double)value <= 0.4d) return CreateDoorMidDoorData();
if ((double)value <= 0.7d) return CreateExDoorData();
if (value <= 1f) return CreateDoorExData();
return CreateExExData();
}

		private RouteData CreateDoorMidDoorData()
		{// Complete original method; original thresholds, array capture, RNG and lifecycle order.
var data = new RouteData();
data.AppearPoint = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.GetRandom(doorList);
data.GoalPoint = data.AppearPoint;
data.MiddleGoalPoint = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.GetRandom(middleGoalList);
data.Setup();
return data;
}

		private RouteData CreateExDoorData()
		{// Complete original method; original thresholds, array capture, RNG and lifecycle order.
var data = new RouteData();
data.AppearPoint = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.GetRandom(toExternalList);
data.GoalPoint = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.GetRandom(doorList);
data.Setup();
return data;
}

		private RouteData CreateDoorExData()
		{// Complete original method; original thresholds, array capture, RNG and lifecycle order.
var data = new RouteData();
data.AppearPoint = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.GetRandom(doorList);
data.GoalPoint = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.GetRandom(toExternalList);
data.Setup();
return data;
}

		private RouteData CreateExExData()
		{// Complete original method; original thresholds, array capture, RNG and lifecycle order.
var data = new RouteData();
int first = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.GetRandomIndex(toExternalList);
data.AppearPoint = toExternalList[first];
var capturedGoalArray = toExternalList;
int second = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.GetRandomIndex(capturedGoalArray, first);
data.GoalPoint = capturedGoalArray[second];
data.Setup();
return data;
}

		public Transform GetRandomPoint()
		{// Complete original method; original thresholds, array capture, RNG and lifecycle order.
var captured = routeList;
return captured[UnityEngine.Random.Range(0, captured.Length)];
}
	}
}
