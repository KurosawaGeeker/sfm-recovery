using UnityEngine;

namespace AkilliMum.Standard.Mirror
{
	public class RenderTextureManager : ManagerBase
	{
		public struct RenderTextureSettings
		{
			public bool IsDebug { get; set; }

			public bool HDR { get; set; }

			public RenderTextureSize Size { get; set; }

			public double ManualSize { get; set; }

			public MSAALevel MSAALevel { get; set; }

			public FilterMode FilterMode { get; set; }

			public bool MipMapping { get; set; }
		}

		private RenderTexture _leftOrCenterRT1;

		private RenderTexture _leftOrCenterRT2;

		private RenderTexture _leftOrCenterRT3;

		private RenderTexture _rightRT1;

		private RenderTexture _rightRT2;

		private RenderTexture _rightRT3;

		private RenderTexture _leftOrCenterDepthRT1;

		private RenderTexture _rightDepthRT1;

		private RenderTextureSettings _settings;

		public RenderTexture LeftOrCenterRT1
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public RenderTexture LeftOrCenterRT2
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public RenderTexture LeftOrCenterRT3
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public RenderTexture RightRT1
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public RenderTexture RightRT2
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public RenderTexture RightRT3
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public RenderTexture LeftOrCenterDepthRT1
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public RenderTexture RightDepthRT1
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public RenderTextureManager(RenderTextureSettings settings)
        {
            _settings = settings;
        }

		public void SetSettings(RenderTextureSettings settings)
		{
		}

		public RenderTextureSettings GetSettings()
		{
			return default;
		}

		public void GenerateMipMaps(StereoTargetEyeMask stereoTargetEyeMask)
		{
		}

		public void CreateRenderTextures(CameraManager cameraManager)
		{
		}

		private RenderTexture Create(string name, int width, int height, int depth, RenderTextureFormat textureFormat, RenderTextureFormat textureFormatHDR)
		{
			return null;
		}

		private RenderTexture CreateDepth(string name, int width, int height, int depth)
		{
			return null;
		}

		private int[] GetTextureSizes(CameraManager cameraManager)
		{
			return null;
		}

		public override void Destroy()
		{
		}
	}
}
