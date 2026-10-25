using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.Window
{
	public class WindowMaterialController : MonoBehaviour
	{
		[SerializeField]
		private Material daytimeMaterial;

		[SerializeField]
		private Material nightMaterial;

		[SerializeField]
		private Renderer[] target;

		[SerializeField]
		private int targetIndex;

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
