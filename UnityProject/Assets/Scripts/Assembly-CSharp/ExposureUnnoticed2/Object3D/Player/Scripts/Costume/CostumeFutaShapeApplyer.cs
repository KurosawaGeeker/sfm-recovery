using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Costume
{
	public class CostumeFutaShapeApplyer : MonoBehaviour
	{
		[SerializeField]
		private int futaShapeKeyIndex;

		[SerializeField]
		private int activeShapeSmallWeight;

		[SerializeField]
		private int activeShapeBigWeight;

		private SkinnedMeshRenderer meshRenderer;

		private SmoothFloat weight;

		private float old;

		private void Start()
		{
		}

		private void Update()
		{
		}
	}
}
