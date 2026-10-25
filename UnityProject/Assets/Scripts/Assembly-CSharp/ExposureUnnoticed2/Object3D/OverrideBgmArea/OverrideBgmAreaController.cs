using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.OverrideBgmArea
{
	public class OverrideBgmAreaController : MonoBehaviour
	{
		private AudioSource audioSource;

		private float defaultVolume;

		private SmoothFloat volumeScale;

		private SmoothFloat timeStopScale;

		private bool isInArea;

		private Collider collider;

		private void Awake()
		{
		}

		private void Update()
		{
		}

		private void OnTriggerEnter(Collider collider)
		{
		}

		private void OnTriggerExit(Collider collider)
		{
		}
	}
}
