using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.CosplayPartsOptionApplyer
{
	public class OptionApplyerAnalPlugLight : MonoBehaviour
	{
		[SerializeField]
		private UnityEngine.Light[] lights;

		[SerializeField]
		private LensFlare lensFlare;

		[SerializeField]
		private GameObject parent;

		private SkinnedMeshRenderer meshRenderer;

		private int uniqueId;

		private Animator animator;

		private static readonly int IsBlink;

		private void Awake()
		{
		}

		private void OnEnable()
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
