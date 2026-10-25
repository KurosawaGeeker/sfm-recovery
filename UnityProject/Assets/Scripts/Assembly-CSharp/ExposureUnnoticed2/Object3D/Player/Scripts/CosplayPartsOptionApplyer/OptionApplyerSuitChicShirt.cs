using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.CosplayPartsOptionApplyer
{
	public class OptionApplyerSuitChicShirt : MonoBehaviour
	{
		[SerializeField]
		private int partsId;

		private SkinnedMeshRenderer meshRenderer;

		private int uniqueId;

		private Material[] mats;

		private static readonly int MainTex;

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
