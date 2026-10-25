using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Light
{
	public class DaytimeActiveSwicher : MonoBehaviour
	{
		[SerializeField]
		private GameObject[] targets;

		[SerializeField]
		private bool isActiveNight;

		[SerializeField]
		private bool isActiveDaytime;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnChangeTime(ChangeTimeEvent evt)
		{
		}

		private void UpdateActive()
		{
		}
	}
}
