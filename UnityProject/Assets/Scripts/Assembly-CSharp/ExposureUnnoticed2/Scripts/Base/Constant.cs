using System;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.Base
{
	public class Constant
	{
		public enum Direct
		{
			Forward = 0,
			Back = 1,
			Left = 2,
			Right = 3,
			ForwardLeft = 4,
			ForwardRight = 5,
			BackLeft = 6,
			BackRight = 7
		}

		[Flags]
		public enum DiscomfortDirect
		{
			Forward = 1,
			Back = 2,
			Left = 4,
			Right = 8
		}

		// Original .cctor RVA 0x4B5B80; field initializer retains original BeforeFieldInit.
        public static Vector3 NoDataVector3 = new Vector3(2345f, -5452f, 64562f);
	}
}
