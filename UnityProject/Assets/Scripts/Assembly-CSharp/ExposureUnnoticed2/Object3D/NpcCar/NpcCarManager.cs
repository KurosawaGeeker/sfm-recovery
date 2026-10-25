using System.Collections.Generic;
using Common.Scripts.Util;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NpcCar
{
	public class NpcCarManager : MonoBehaviour
	{
		[SerializeField]
		private Transform[] spawnPoints;

		[SerializeField]
		private GameObject carPrefab;

		[SerializeField]
		private float destroyDistance;

		[SerializeField]
		private float speedKmPerHour;

		[Header("朝")]
		[SerializeField]
		private float intervalMin;

		[SerializeField]
		private float intervalMax;

		[SerializeField]
		private int MaxCarNum;

		[Header("夜")]
		[SerializeField]
		private float intervalMinNight;

		[SerializeField]
		private float intervalMaxNight;

		[SerializeField]
		private int MaxCarNumNight;

		public List<NpcCarController> CarList;

		private IntervalTime spawnInterval;

		private IntervalTime spawnIntervalNight;

		private int currentCarNum;

		public static NpcCarManager Instance { get; private set; }

		private int currentMax => 0;

		private IntervalTime currentIntervalTime => null;

		public void Initialize()
		{
		}

		public void Dispose()
		{
		}

		public void OnUpdate()
		{
		}
	}
}
