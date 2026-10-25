using System.Collections.Generic;
using UnityEngine;

namespace AkilliMum.Standard.Mirror
{
	public class RendererManager : ManagerBase
	{
		public struct RendererSettings
		{
			public bool IsDebug { get; set; }

			public UpVector UpVector { get; set; }

			public IList<string> CustomShaders { get; set; }

			public bool DrawAlways { get; set; }
		}

		private GameObject _first;

		private List<Renderer> _renderers;

		private List<Material> _materials;

		private RendererSettings _settings;

		public RendererManager(GameObject[] reflectiveObjects, RendererSettings renderSettings)
        {
            _settings = renderSettings;
            SetReflectiveObjects(reflectiveObjects);
        }

		public void SetSettings(RendererSettings renderSettings)
		{
		}

		public void SetReflectiveObjects(GameObject[] reflectiveObjects)
		{
		}

		public bool IsObjectVisible(Camera cam)
		{
			return false;
		}

		public Vector3 GetPosition()
		{
			return default;
		}

		public GameObject GetFirst()
		{
			return null;
		}

		public Vector3 GetNormal(bool invert)
		{
			return default;
		}

		public void UpdateMaterials(RenderTextureManager renderTextureManager, CameraManager cameraManager)
		{
		}

		public override void Destroy()
		{
		}
	}
}
