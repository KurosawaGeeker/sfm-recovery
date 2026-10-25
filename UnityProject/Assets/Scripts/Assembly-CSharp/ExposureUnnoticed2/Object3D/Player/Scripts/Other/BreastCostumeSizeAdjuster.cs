using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Other
{
	public class BreastCostumeSizeAdjuster : MonoBehaviour
	{
		[SerializeField]
		private int bigIndex;

		[SerializeField]
		private int smallIndex;

		[SerializeField]
		private int integrateIndex;

		[SerializeField]
		private int overBigIndex;

		[SerializeField]
		private int chikubiIndex;

		[SerializeField]
		private int moreBigFixIndex;

		[SerializeField]
		private int addChikubiSizeBlendScale;

		[SerializeField]
		private int maxBigOffset;

		[SerializeField]
		private int maxSmallOffset;

		[SerializeField]
		private int moreBigFixMax;

		[SerializeField]
		private int bigMax;

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

		private void Update()
		{
		}

		private void OnChangeBody(ChangeBodyEvent evt)
		{
		}
	}
}
