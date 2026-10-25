using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.CosplayPartsOptionApplyer
{
	public class OptionApplyerNinja : MonoBehaviour
	{
		[SerializeField]
		private int partsId;

		[SerializeField]
		private Texture[] textures;

		[SerializeField]
		private int targetMatIndex;

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
