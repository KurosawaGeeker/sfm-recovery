using System.Collections.Generic;
using UnityEngine;

namespace AkilliMum.Standard.Mirror
{
	public class EffectBase : MonoBehaviour
	{
		public Dictionary<string, RenderTexture> AlreadyRendered;

		private bool _insiderendering;

		public bool InsideRendering
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
