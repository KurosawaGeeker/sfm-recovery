using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Other
{
	public class BlackCensorController : MonoBehaviour
	{
		[SerializeField]
		private bool isCapsule;

		[SerializeField]
		private bool isNoResize;

		private MeshRenderer meshRenderer;

		private float initScale;

		private void Start()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnChange(OptionChangeEvent evt)
		{
		}
	}
}
