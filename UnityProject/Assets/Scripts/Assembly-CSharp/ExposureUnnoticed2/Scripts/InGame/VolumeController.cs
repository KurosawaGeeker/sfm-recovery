using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class VolumeController : MonoBehaviour
	{
		private PostProcessVolume volume;

		private PostProcessLayer layer;

		private LensDistortion lensDistortion;

		private void Awake()
		{
            volume = GetComponent<global::UnityEngine.Rendering.PostProcessing.PostProcessVolume>();
            layer = GetComponent<global::UnityEngine.Rendering.PostProcessing.PostProcessLayer>();
            lensDistortion = volume.profile.GetSetting<global::UnityEngine.Rendering.PostProcessing.LensDistortion>();
		}

		public void SetPaniniProjection(bool isActive)
		{
            lensDistortion.active = isActive;
		}
	}
}
