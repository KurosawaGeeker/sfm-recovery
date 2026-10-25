using UnityEngine;

namespace ExposureUnnoticed2.Scripts.Tools
{
	public class Corevale_SwapLightmap : MonoBehaviour
	{
		[SerializeField]
		private Texture2D[] lightMap;

		[SerializeField]
		private Texture2D[] dir;

		[SerializeField]
		private Texture2D[] shadowMask;

		[SerializeField]
		private LightProbes[] lightProbe;

		private LightmapData[] brightDatas;

		private LightmapData[] darkDatas;

		private bool currentIsDaytime;

		private void Start()
		{
		}

		public void SwapLightmapData(bool isDaytime, bool isForce)
		{
		}

		private void DarkLightmap()
		{
		}

		private void BrightLightmap()
		{
		}
	}
}
