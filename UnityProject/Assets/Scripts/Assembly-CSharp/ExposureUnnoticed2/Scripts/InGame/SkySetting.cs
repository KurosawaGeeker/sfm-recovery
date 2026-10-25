using System;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	[Serializable]
	public class SkySetting
	{
		[SerializeField]
		public Material SkyboxMaterial;

		[SerializeField]
		public Color EnvironmentLightColor;

		[SerializeField]
		public Color EnvironmentLightColorEquator;

		[SerializeField]
		public Color EnvironmentLightColorGround;

		[SerializeField]
		public Color FogColor;

		[SerializeField]
		public Color WallFogColor;

		[SerializeField]
		public float FogDensity;
	}
}
