using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;
using ch.sycoforge.Decal;

namespace ExposureUnnoticed2.Object3D.PeeDecal
{
	public class PeeDecalController : MonoBehaviour
	{
		[SerializeField]
		private EasyDecal decal;

		private Material material;

		private bool isCreatedMaterial;

		private static readonly int Tint;

		private SmoothFloat alpha;

		private Color targetColor;

		private Color currentColor;

		public void Initialize(Vector3 position, Vector3 normalV)
		{
		}

		private void Update()
		{
		}
	}
}
