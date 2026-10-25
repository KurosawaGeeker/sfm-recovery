using ExposureUnnoticed2.Master.Stage;
using UnityEngine;

namespace ExposureUnnoticed2.Master.FastTravel
{
	[CreateAssetMenu(fileName = "RFastTravel", menuName = "ScriptableObjects/RFastTravel")]
	public class RFastTravel : ScriptableObject
	{
		[SerializeField]
		public string NameKey;

		[SerializeField]
		public StageType StageType;

		[SerializeField]
		public StageType PortalType;

		[SerializeField]
		public Sprite ScreenShot;

		[SerializeField]
		public bool IsProhibitedFastTravelAtNight;

		public int Id { get; set; }
	}
}
