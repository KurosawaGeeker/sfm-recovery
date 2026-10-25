using ExposureUnnoticed2.Master.RankRelease;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.MissionMenuPanel
{
	public class RankConfirmReleaseItemView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI categoryText;

		[SerializeField]
		private TextMeshProUGUI nameText;

		public void Initialize(MRankReleaseRecord record, bool isForceOpenInfo = false)
		{
var data = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
if (data is null) throw new System.NullReferenceException();
string key = unchecked(data.ConfirmedReleasedInfoRank + 1) < record.ReleaseRank && !isForceOpenInfo ? record.HideNameKey : record.NameKey;
categoryText.text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(record.CategoryNameKey);
nameText.text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(key);
}
	}
}
