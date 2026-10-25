using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Other
{
	public class PantsSlideAdjuster : MonoBehaviour
	{
		[SerializeField]
		private int slideMankoIndex;

		[SerializeField]
		private int slideAnalIndex;

		private SkinnedMeshRenderer mesh;

		private float oldSlideManko;

		private float oldSlideAnal;

		private void Awake()
		{
		}

		private void Update()
		{
		}
	}
}
