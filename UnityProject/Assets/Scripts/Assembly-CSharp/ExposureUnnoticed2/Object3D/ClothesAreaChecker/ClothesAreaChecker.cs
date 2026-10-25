using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ClothesAreaChecker
{
	public class ClothesAreaChecker : MonoBehaviour
	{
		[SerializeField]
		private Transform[] areas;

		private List<Bounds> boundsList;

		public static ClothesAreaChecker Instance { get; private set; }

		public bool IsInArea { get; private set; }

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}
	}
}
