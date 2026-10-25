using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.ConbiniBoxFog
{
	public class BoxFogController : MonoBehaviour
	{
		[SerializeField]
		private Color daytimeColor;

		[SerializeField]
		private Color nightColor;

		private Material material;

		private bool oldBad;

		private void Start()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnChangeTime(ChangeTimeEvent evt)
		{
		}

		private void ChangeColor()
		{
		}
	}
}
