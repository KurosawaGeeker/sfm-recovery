using ExposureUnnoticed2.Master.DroneMission;
using ExposureUnnoticed2.Master.Stage;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.DroneMissionPanel
{
	public class DroneMissionRecordItemView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI stageName;

		[SerializeField]
		private TextMeshProUGUI achieveCount;

		[SerializeField]
		private TextMeshProUGUI maxRpBonus;

		public void Initialize(RDroneMission mission, StageType stageType)
		{
		}
	}
}
