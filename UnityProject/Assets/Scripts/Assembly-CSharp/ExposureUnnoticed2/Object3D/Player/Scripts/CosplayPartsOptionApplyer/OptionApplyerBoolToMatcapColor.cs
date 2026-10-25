using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.CosplayPartsOptionApplyer
{
	public class OptionApplyerBoolToMatcapColor : MonoBehaviour
	{
		[SerializeField]
		private CosplayType CosplayType;

		[SerializeField]
		private int partsId;

		[SerializeField]
		private int targetMaterialIndex;

		[SerializeField]
		private bool isBool2;

		private SkinnedMeshRenderer meshRenderer;

		private int uniqueId;

		private Material[] mats;

		private static readonly int MainTex;

		private static readonly int BaseColor;

		private static readonly int FirstShadeColor;

		private static readonly int MatCapColor;

		private Color initMatCapColor;

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
