using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops
{
	public class TransparentMaterialFogController : MonoBehaviour
	{
		private class MaterialData
		{
			public Material material;

			public float initSmoottness;

			public Color initColor;

			public void Change(bool isBadVisible)
			{
			}
		}

		[SerializeField]
		private Material[] targetMaterials;

		[SerializeField]
		private int[] targetIndexs;

		[SerializeField]
		private GameObject targetObject;

		private List<MaterialData> targetMaterialData;

		private static readonly int Glossiness;

		private static readonly int Color1;

		private bool oldBad;

		private void Start()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private void Change(bool forceToInit = false)
		{
		}
	}
}
