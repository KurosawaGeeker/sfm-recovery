using System.Collections.Generic;
using UnityEngine;

namespace AkilliMum.Standard.D2WeatherEffects
{
	public class EffectBase : MonoBehaviour
	{
		public static Dictionary<string, RenderTexture> AlreadyRendered;

		private static bool _insiderendering;

		public static bool InsideRendering
		{
			get
			{
				return false;
			}
			set
			{
			}
		}
	}
}
