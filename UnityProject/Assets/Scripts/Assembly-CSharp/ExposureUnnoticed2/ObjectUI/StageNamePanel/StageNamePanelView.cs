using ExposureUnnoticed2.Master.Stage;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.StageNamePanel
{
	public class StageNamePanelView : MonoBehaviour
	{
		private static StageNamePanelView instance;

		[SerializeField]
		private TextMeshProUGUI stageName;

		private void Initialize(MStageRecord stageRecord)
		{
		}

		public void OnAnimEnd()
		{
		}

		public static void Open(MStageRecord stageRecord)
		{
		}
	}
}
