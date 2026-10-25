using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.CosplayPartsOptionApplyer
{
	public class OptionApplyerLongBoots : MonoBehaviour
	{
		[SerializeField]
		private Material[] materialTypes;

		[SerializeField]
		private Material[] materialSubTypes;

		private SkinnedMeshRenderer meshRenderer;

		private int uniqueId;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnChangeBodyEvent(ChangeBodyEvent evt)
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
