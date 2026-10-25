using UnityEngine;

namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionMoveVerticalFromClothes : MissionBase
	{
		private Vector3 startPathPosition;

		private float lastMaxPathLength;

		public override bool IsInCondition()
		{
			return false;
		}

		protected override float GetAddSatisfy()
		{
			return 0f;
		}

		public override void Reset()
		{
		}
	}
}
