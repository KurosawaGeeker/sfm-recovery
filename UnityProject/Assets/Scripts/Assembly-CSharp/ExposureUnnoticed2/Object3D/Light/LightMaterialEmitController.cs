using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Light
{
	public class LightMaterialEmitController : MonoBehaviour
	{
		[SerializeField]
		private MeshRenderer[] sameMaterialTargets;

		[SerializeField]
		private Material emitMaterial;

		[SerializeField]
		private Material noEmitMaterial;

		private bool oldIsDaytime;

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void UpdateMaterial()
		{
		}
	}
}
