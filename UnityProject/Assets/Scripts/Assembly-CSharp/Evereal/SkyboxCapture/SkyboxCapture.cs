using UnityEngine;

namespace Evereal.SkyboxCapture
{
	[RequireComponent(typeof(Camera))]
	[ExecuteInEditMode]
	public class SkyboxCapture : MonoBehaviour
	{
		[SerializeField]
		public KeyCode captureKey;

		[SerializeField]
		public string saveFolder;

		private string saveFolderFullPath;

		[SerializeField]
		public ProjectionType projectionType;

		[SerializeField]
		public CubemapSizeType cubemapSize;

		[SerializeField]
		public CubemapLayoutType cubemapLayout;

		[SerializeField]
		public ImageFormatType imageFormat;

		[SerializeField]
		public int jpgQuality;

		[SerializeField]
		public ImageSizeType imageSize;

		[SerializeField]
		public AntiAliasingType antiAliasing;

		[SerializeField]
		public Material equirectMaterial;

		[SerializeField]
		public Material cubemapMaterial;

		private Camera captureCamera;

		protected bool includeCameraRotation;

		private CubemapFace[] faces;

		private Vector3[] faceAngles;

		private string imageSavePath;

		private byte[] imageData;

		protected string LOG_FORMAT;

		public int CubemapSize => 0;

		public int ImageWidth => 0;

		public int ImageHeight => 0;

		public int AntiAliasing => 0;

		public void StartCapture()
		{
		}

		private void WriteTextureImage(RenderTexture outputTexture)
		{
		}

		private void WriteTexture2DImage(Texture2D texture2D)
		{
		}

		private void WriteImageProcess()
		{
		}

		private void Update()
		{
		}
	}
}
