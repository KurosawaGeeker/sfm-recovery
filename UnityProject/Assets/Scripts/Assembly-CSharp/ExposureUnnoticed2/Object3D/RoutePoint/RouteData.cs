using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.RoutePoint
{
	public class RouteData
	{
		public RoutePoint AppearPoint;

		public RoutePoint MiddleGoalPoint;

		public RoutePoint MiddleGoalPoint2;

		public RoutePoint GoalPoint;

		public List<RoutePoint> routeList;

		public float PathRightScale;

		private int currentIndex;

		public virtual void Setup()
		{
            float value = UnityEngine.Random.value;
            PathRightScale = (value + value) - 1f;
            routeList = new List<RoutePoint>();
            routeList.Add(AppearPoint);
            if (MiddleGoalPoint) routeList.Add(MiddleGoalPoint);
            if (MiddleGoalPoint2) routeList.Add(MiddleGoalPoint2);
            routeList.Add(GoalPoint);
            ++currentIndex;
        }

		public virtual RoutePoint GetNextPoint()
		{
            if (routeList.Count <= currentIndex) return null;
            return routeList[currentIndex];
        }

		public RoutePoint GetNextNextPoint()
		{
			return null;
		}

		public virtual RoutePoint IncrementIndex()
		{
            ++currentIndex;
            return GetNextPoint();
        }

		public Vector3 CalcApplyPathRightPosition(RoutePoint rp)
		{
            Vector3 position = rp.Position;
            float scale = PathRightScale;
            float maximum = rp.MaxPathRightScale;
            Vector3 right = rp.PathRight;
            return new Vector3(scale * right.x * maximum + position.x,
                               scale * right.y * maximum + position.y,
                               scale * right.z * maximum + position.z);
        }

		public void ProceedRandom()
		{
			int index = UnityEngine.Random.Range(0, routeList.Count - 1);
			while (routeList[index].RouteInteractType != RouteInteractType.None) --index;
			for (int i = 0; i < index; ++i)
			{
			    if (routeList[i].RouteInteractType == RouteInteractType.Chair)
			        global::ExposureUnnoticed2.Object3D.NPC.Script.SitNpcManager.Instance.UseEndByNpcByFloor(routeList[i]);
			    if (routeList[i].RouteInteractType == RouteInteractType.CoinLaundry)
			        global::ExposureUnnoticed2.Scripts.InGame.SceneProcessManager.LaundryManager.Instance.ReturnToken();
			}
			AppearPoint = routeList[index];
			currentIndex = index + 1;
		}

		public virtual void SetNextPoint(RoutePoint point)
		{
		}
	}
}
