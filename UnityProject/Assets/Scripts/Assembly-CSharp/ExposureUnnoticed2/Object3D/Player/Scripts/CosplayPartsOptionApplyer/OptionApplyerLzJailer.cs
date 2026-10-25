using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.CosplayPartsOptionApplyer
{
	public class OptionApplyerLzJailer : MonoBehaviour
	{
		[SerializeField]
		private Texture[] materialTextureTypes;

		[SerializeField]
		private Texture[] matcapTexture;

		[SerializeField]
		private int partsId;

		[SerializeField]
		private int targetMaterialIndex;

		private SkinnedMeshRenderer meshRenderer;

		private int uniqueId;

		private int oldMaterialIndex;

		private Material[] mats;

		private static readonly int BaseColor;

		private static readonly int FirstShadeColor;

		private static readonly int MatCapSampler;

		private static readonly int MainTex;

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
