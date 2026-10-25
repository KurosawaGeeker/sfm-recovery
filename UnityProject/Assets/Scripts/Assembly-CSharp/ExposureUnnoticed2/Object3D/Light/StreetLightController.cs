using ExposureUnnoticed2.Scripts.InGame.Util;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Light
{
	public class StreetLightController : MonoBehaviour
	{
		private enum LightColliderType
		{
			Sphere = 0,
			Fan = 1
		}

		[SerializeField]
		private LightColliderType type;

		[Range(0f, 1f)]
		[SerializeField]
		private float maxBareyasusa;

		[SerializeField]
		private bool isNoChangeByTime;

		private UnityEngine.Light light;

		private bool oldIsDaytime;

		private IColliderTriggerController colliderTrigger;

		private bool oldTouch;

		private void Start()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		public float CalcBareyasusa(GameObject playerObject)
		{
			float distance = ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DistSqrt(colliderTrigger.CenterObject, playerObject);
			float radius = colliderTrigger.CenterObject.transform.localScale.x * 0.5f;
			float value = (1f - distance / radius) * 3f;
			if (type == LightColliderType.Fan)
			{
			    float penetration = colliderTrigger.PenetrateDist / 0.2f;
			    if (penetration < 0f) penetration = 0f;
			    else if (1f < penetration) penetration = 1f;
			    value = value < penetration ? value : penetration;
			}
			if (value < 0f) value = 0f;
			else if (1f < value) value = 1f;
			return value * maxBareyasusa;
		}
	}
}
