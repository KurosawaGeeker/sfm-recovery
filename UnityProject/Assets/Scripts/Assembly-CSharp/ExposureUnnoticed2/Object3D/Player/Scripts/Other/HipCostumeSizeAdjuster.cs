using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Other
{
	public class HipCostumeSizeAdjuster : MonoBehaviour
	{
		[SerializeField]
		private int bigIndex;

		private SkinnedMeshRenderer mesh;

		public bool IsForceFlattenTit { get; set; }

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnEnable()
		{
		}

		private void OnChangeBody(ChangeBodyEvent evt)
		{
		}
	}
}
