using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.CosplayPartsOptionApplyer
{
	public class OptionApplyerCommonOnlyBool1ToShapeKey : MonoBehaviour
	{
		[SerializeField]
		private CosplayType cosplayType;

		[SerializeField]
		private int partsId;

		[SerializeField]
		private int shapeKeyIndex;

		[SerializeField]
		private int activeBlend;

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
