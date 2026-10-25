using System;

namespace ExposureUnnoticed2.Master.Mission
{
	[Flags]
	public enum ExposedBodyParts
	{
		None = 1,
		Front = 2,
		Hip = 4,
		Upper = 8,
		Breast = 0x10,
		All = 0x20
	}
}
