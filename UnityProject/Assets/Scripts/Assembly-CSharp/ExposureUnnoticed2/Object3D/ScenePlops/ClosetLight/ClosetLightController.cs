using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.ClosetLight
{
	public class ClosetLightController : MonoBehaviour
	{
		[SerializeField]
		private GameObject closetLightObject;

		[SerializeField]
		private GameObject noClosetLightObject;

		public static ClosetLightController Instance { get; private set; }

		private void Awake()
		{
// Complete original native body draft; native/CLR acceptance pending.
Instance = this;
}

		private void OnDestroy()
		{
// Complete original native body draft; native/CLR acceptance pending.
Instance = null;
}

		public void SetClosetLight(bool isActive)
		{
// Complete original native body draft; native/CLR acceptance pending.
closetLightObject.SetActive(isActive); noClosetLightObject.SetActive(!isActive);
}
	}
}
