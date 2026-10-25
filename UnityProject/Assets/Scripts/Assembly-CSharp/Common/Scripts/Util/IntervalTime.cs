namespace Common.Scripts.Util
{
	public class IntervalTime
	{
		private bool isUnscale;

		private float interval;

		private float lastTime;

		private float randomMin;

		private float randomMax;

		private bool isRandom;

		private float time => 0f;

		public IntervalTime(float interval, bool isUnscale = false)
		{
		}

		public IntervalTime(float randomMin, float randomMax, bool isUnscale = false)
		{
		}

		public bool ProceedIfInCondition()
		{
			return false;
		}
	}
}
