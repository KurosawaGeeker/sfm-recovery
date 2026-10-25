using ExposureUnnoticed2.Master.Mission;
using ExposureUnnoticed2.Scripts.Base;

namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionCountEcstasy : MissionBase
	{
		public override void Initialize(MMissionOverrideRecord record)
		{
		}

		public override void Dispose()
		{
		}

		public override bool IsInCondition()
		{
			return false;
		}

		private void OnCount(MissionCountEvent evt)
		{
		}
	}
}
