using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Shiofuki
{
	public class ShiofukiParticleController : MonoBehaviour
	{
		[SerializeField]
		public bool IsEmitting;

		private bool oldIsEmitting;

		private ParticleSystem particleSystem;

		private void Awake()
		{
		}

		private void Update()
		{
		}
	}
}
