using UnityEngine;

namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionMoveFromClothesInArea : MissionBase
	{
		private Vector3 startPathPosition;

		private bool oldResult;

		private int PathSearchCount;

		private float lastMaxPathLength;

		private float lastPathLength;

		public override bool IsInCondition()
		{
			return false;
		}

		protected override float GetAddSatisfy()
		{
			return 0f;
		}

		protected override bool IsRpPerSecCondition()
		{
			return false;
		}

		public override void Reset()
		{
base.Reset();
lastMaxPathLength = 0f;
oldResult = false;
}
	}
}
