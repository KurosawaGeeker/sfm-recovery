using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Other
{
	public class CosplayDynamicBoneController : MonoBehaviour
	{
		[SerializeField]
		private CosplayType cosplayType;

		[SerializeField]
		private int id;

		[SerializeField]
		private int[] otherIds;

		[SerializeField]
		private DynamicBone[] targets;

		private RCosplayParts[] cosplayPartsRecords;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnChangeCostumeCosplayEvent(ChangeCostumeCosplayEvent evt)
		{
		}
	}
}
