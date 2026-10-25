using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.CosplayPartsOptionApplyer
{
	public class OptionApplyerCommonOnlyBool2ToShapeKey : MonoBehaviour
	{
		[SerializeField]
		private CosplayType cosplayType;

		[SerializeField]
		private int partsId;

		[SerializeField]
		private int shapeKeyIndex;

		[SerializeField]
		private int activeBlend;

		[SerializeField]
		private bool isInverse;

		private SkinnedMeshRenderer meshRenderer;

		private int uniqueId;

		private Material[] mats;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnChangeEvent(ChangeCosplayPartsOptionEvent evt)
		{
		}

		private void ApplyFromGameState()
		{
		}
	}
}
