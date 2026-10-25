using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.BuffPanel
{
	public class StatusPanelStatisticsView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI difficulty;

		[SerializeField]
		private TextMeshProUGUI totalPlayTime;

		[SerializeField]
		private TextMeshProUGUI totalExposeTime;

		[SerializeField]
		private TextMeshProUGUI cosplayBringRate;

		[SerializeField]
		private TextMeshProUGUI missionClearRate;

		[SerializeField]
		private TextMeshProUGUI missionClearCount;

		[SerializeField]
		private TextMeshProUGUI baretaCount;

		[SerializeField]
		private TextMeshProUGUI ecstasyCount;

		[SerializeField]
		private TextMeshProUGUI totalEarnRp;

		[SerializeField]
		private TextMeshProUGUI totalLostRp;

		[SerializeField]
		private TextMeshProUGUI totalUseRp;

		[SerializeField]
		private TextMeshProUGUI highScore;

		[SerializeField]
		private TextMeshProUGUI MissionAchieveCount;

		[SerializeField]
		private TextMeshProUGUI MaxRpBonus;

		[SerializeField]
		private TextMeshProUGUI AverageMaxRpBonus;

		public void Initialize()
		{
		}

		public static string ToTime(uint time)
		{
			// Recovered from GameAssembly.dll RVA 0x5453E0
			return string.Format("{0:00}:{1:00}:{2:00}", (time / 60u) / 60u, (time / 60u) % 60u, time % 60u);
		}
	}
}
