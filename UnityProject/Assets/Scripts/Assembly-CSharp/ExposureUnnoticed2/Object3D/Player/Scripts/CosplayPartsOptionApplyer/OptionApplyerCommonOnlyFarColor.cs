using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.CosplayPartsOptionApplyer
{
	public class OptionApplyerCommonOnlyFarColor : MonoBehaviour
	{
		[SerializeField]
		private CosplayType cosplayType;

		[SerializeField]
		private int partsId;

		[SerializeField]
		private int[] targetMaterialIndexs;

		private SkinnedMeshRenderer meshRenderer;

		private int uniqueId;

		private Material[] mats;

		private static readonly int BaseColor;

		private static readonly int FirstShadeColor;

		private static readonly int Color1;

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
