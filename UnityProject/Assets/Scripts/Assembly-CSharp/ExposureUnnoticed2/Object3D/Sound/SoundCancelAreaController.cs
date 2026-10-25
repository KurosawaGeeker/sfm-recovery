using ExposureUnnoticed2.Object3D.ScenePlops.Toilet;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Sound
{
	public class SoundCancelAreaController : MonoBehaviour
	{
		[SerializeField]
		private InSidePlayerChecker targetArea;

		[SerializeField]
		private float constantDist;

		[SerializeField]
		private float fadeDist;

		private void Update()
		{
		}

		private void OnDrawGizmos()
		{
		}
	}
}
