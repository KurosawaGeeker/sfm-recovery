using UnityEngine;

namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionEscalatorMove : MissionBase
	{
		private Vector3 startPathPosition;

		private bool oldResult;

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
base.Reset();
lastMaxPathLength = 0f;
oldResult = false;
}
	}
}
