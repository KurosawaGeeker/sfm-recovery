using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;
using ch.sycoforge.Decal;

namespace ExposureUnnoticed2.Object3D.SpermBullet
{
	public class SpermDecalController : MonoBehaviour
	{
		[SerializeField]
		private Transform decalObject;

		[SerializeField]
		private EasyDecal decal;

		private SmoothFloat smoothScale;

		private SmoothFloat destroyScale;

		private float dickScale;

		private Vector3 initScale;

		private bool startedDestroy;

		public void Initialize(bool isOnNpc)
		{
		}

		private void Update()
		{
		}

		public void StartDestroy()
		{
		}
	}
}
