using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class StageRandomPositionManager : MonoBehaviour
	{
		[Range(0f, 50f)]
		[SerializeField]
		[Header("0なら無条件")]
		private float noSpawnRange;

		private List<Transform> childPositionList = new List<Transform>();

		private List<Transform> targetList = new List<Transform>();

		public void Initialize()
		{
            foreach (Transform child in transform) childPositionList.Add(child);
        }

		public Transform GetRandomPosition(Vector3 playerPosition)
		{
// Complete original RVA4A6600; strict distance, list version and sampler ordering.
targetList.Clear();
foreach (Transform point in childPositionList)
{
    if (noSpawnRange == 0f || global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DistSqrt(point.gameObject, playerPosition) > noSpawnRange)
        targetList.Add(point);
}
var selected = targetList;
if (selected.Count == 0) throw new System.Exception("半径外にあるRandomPositionが存在しません");
return selected[UnityEngine.Random.Range(0, selected.Count)];
}

		private void OnDrawGizmos()
		{
		}
	}
}
