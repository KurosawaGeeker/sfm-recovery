using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.CosplayPartsOptionApplyer
{
	public class OptionApplyerJumpSuit : MonoBehaviour
	{
		[SerializeField]
		private Material[] materialTypes;

		[SerializeField]
		private GameObject[] zippers;

		private SkinnedMeshRenderer meshRenderer;

		private int uniqueId;

		private int oldMaterialIndex;

		private static readonly int BaseColor;

		private static readonly int FirstShadeColor;

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
