using System.Collections.Generic;
using ExposureUnnoticed2.Master.Hair;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Other
{
	public class HairCustomAdjuster : MonoBehaviour
	{
		private static readonly int BaseColor;

		private static readonly int MainTex;

		private static readonly int FirstShadeColor;

		[SerializeField]
		private SkinnedMeshRenderer mesh;

		[SerializeField]
		private SkinnedMeshRenderer[] ignoreCommonMaterialList;

		[SerializeField]
		private HairType HairType;

		private MHairRecord record;

		private Material targetMaterial;

		private bool isInitialized;

		private static HashSet<HairType> AddFlagSet;

		private void Start()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDestroy()
		{
		}

		private void InitializeIfNeed()
		{
		}

		private void OnChangeBody(ChangeBodyEvent evt)
		{
		}
	}
}
