using UnityEngine;

namespace ExposureUnnoticed2.Master.DroneMission
{
	[CreateAssetMenu(fileName = "RDroneMission", menuName = "ScriptableObjects/RDroneMission")]
	public class RDroneMission : ScriptableObject
	{
		[SerializeField]
		public string NameKey;

		[SerializeField]
		public string MissionClassName;

		[SerializeField]
		public string DescriptionKey;

		[SerializeField]
		public string StartConditionDescriptionKey1;

		[SerializeField]
		public string StartConditionDescriptionKey2;

		[SerializeField]
		public string StartConditionDescriptionKey3;

		[SerializeField]
		public string ClearConditionDescriptionKey;

		[SerializeField]
		public int RewardRp;

		[SerializeField]
		public int ReleaseLevel;

		[SerializeField]
		public float FadePanelSecond;

		[SerializeField]
		public bool IsPlayerRandomStart;

		[SerializeField]
		public bool IsDisplayDirect;

		[SerializeField]
		public bool IsVibe;

		[SerializeField]
		public bool IsDropVibeRemocon;

		[SerializeField]
		public GoalSelectType GoalExternalMap;

		[SerializeField]
		public GoalSelectType StartExternalMap;

		public int Id { get; set; }
	}
}
