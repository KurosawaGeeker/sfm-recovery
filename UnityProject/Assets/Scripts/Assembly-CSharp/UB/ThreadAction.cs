using System;
using System.Timers;

namespace UB
{
	internal class ThreadAction
	{
		private Timer _timer;

		public Action Action { get; set; }

		public bool IsReady { get; set; }

		public bool StopOnTimeScale { get; set; }

		public string GroupName { get; set; }

		public void Start(float delay)
		{
		}

		public void Pause()
		{
		}

		public void Continue()
		{
		}
	}
}
