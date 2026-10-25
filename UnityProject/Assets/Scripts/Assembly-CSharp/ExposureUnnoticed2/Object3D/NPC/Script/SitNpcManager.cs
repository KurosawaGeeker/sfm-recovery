using System.Collections.Generic;
using ExposureUnnoticed2.Object3D.RoutePoint;
using ExposureUnnoticed2.Object3D.ScenePlops.ChairSitController;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class SitNpcManager : MonoBehaviour
	{
		private List<ChairSitController> chairList;

		private Dictionary<ExposureUnnoticed2.Object3D.RoutePoint.RoutePoint, ChairSitController> chairDict;

		private Dictionary<int, List<ChairSitController>> nonRouteTargetedChairFloorDict;

		public static SitNpcManager Instance { get; private set; }

		private void Awake()
		{
		}

		public void Initialize()
		{
		}

		private void Spawn()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnChangeTime(ChangeTimeEvent evt)
		{
		}

		public void RegisterChair(ChairSitController chair)
		{
		}

		public ChairSitController PublishEmptyChairByFloor(int floor)
		{
			return null;
		}

		public void UseEndByNpcByFloor(ExposureUnnoticed2.Object3D.RoutePoint.RoutePoint rp)
		{
			List<ChairSitController> list = GetChairFloorList(rp.Floor);
			ChairSitController chair = chairDict[rp];
			if (!list.Contains(chair)) list.Add(chair);
		}

		public ChairSitController GetChairBy(ExposureUnnoticed2.Object3D.RoutePoint.RoutePoint rp)
		{
			return null;
		}

		private List<ChairSitController> GetChairFloorList(int floor)
		{
			if (!nonRouteTargetedChairFloorDict.ContainsKey(floor))
			    nonRouteTargetedChairFloorDict.Add(floor, new List<ChairSitController>());
			return nonRouteTargetedChairFloorDict[floor];
		}
	}
}
