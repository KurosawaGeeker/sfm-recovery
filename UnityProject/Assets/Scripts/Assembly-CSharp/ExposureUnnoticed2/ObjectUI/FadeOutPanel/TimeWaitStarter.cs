namespace ExposureUnnoticed2.ObjectUI.FadeOutPanel
{
	public class TimeWaitStarter : IFadeInStarter
	{
		private bool isEnd;

		public TimeWaitStarter(float waitTime)
		{
		}

		bool IFadeInStarter.IsStart()
		{
			return false;
		}
	}
}
