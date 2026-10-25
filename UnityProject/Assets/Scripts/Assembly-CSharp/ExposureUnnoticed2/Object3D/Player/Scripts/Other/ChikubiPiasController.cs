using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Other
{
	public class ChikubiPiasController : MonoBehaviour
	{
		[SerializeField]
		private float smallScale;

		[SerializeField]
		private float bigScale;

		[SerializeField]
		private Vector3 smallPosition;

		[SerializeField]
		private Vector3 bigPosition;

		private Vector3 initLocalPosition;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnEnable()
		{
		}

		private void OnChangeBody(ChangeBodyEvent evt)
		{
		}
	}
}
