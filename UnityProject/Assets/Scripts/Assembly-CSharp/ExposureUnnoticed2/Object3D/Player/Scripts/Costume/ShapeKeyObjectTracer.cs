using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Costume
{
	public class ShapeKeyObjectTracer : MonoBehaviour
	{
		[SerializeField]
		private GameObject traceTargetObject;

		[SerializeField]
		private int targetShapeKeyIndex;

		[SerializeField]
		private int noActiveShapeWeight;

		[SerializeField]
		private int activeShapeWeight;

		private SkinnedMeshRenderer meshRenderer;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnChange()
		{
		}

		private void OnChangeEvent(ChangeCostumeCosplayEvent evt)
		{
		}

		private void OnChangeAdultGoodsEvent(ChangeAdultGoodsEvent evt)
		{
		}
	}
}
