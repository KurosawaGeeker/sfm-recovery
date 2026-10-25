using UnityEngine;

namespace ExposureUnnoticed2.Master.Mission
{
	[CreateAssetMenu(fileName = "MMissionRecord", menuName = "ScriptableObjects/MMissionRecord")]
	public class MMissionRecord : ScriptableObject
	{
		[SerializeField]
		public string NameKey;

		[SerializeField]
		public string MissionClassName;

		[SerializeField]
		public MissionType MissionType;

		[SerializeField]
		public bool IsOverrideAddSatisfy;

		[SerializeField]
		public bool IsHideEyeMask;

		[SerializeField]
		public bool IsHideDroneMission;

		[SerializeField]
		public bool IsShowInSightVisibleNpc;

		[SerializeField]
		public bool IsShowInSightVisibleRiseStrangenessNpc;

		[SerializeField]
		public ExposedBodyParts ExposedBodyPartsParam;

		[SerializeField]
		public MissionCategoryType CategoryType;

		[SerializeField]
		public bool IsAfterPerSec;

		[SerializeField]
		public float RpPerSecScale;

		public int Id { get; set; }
	}
}
