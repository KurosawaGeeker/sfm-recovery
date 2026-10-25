namespace ExposureUnnoticed2.ObjectUI.FadeOutPanel
{
	public class NoWaitStarter : IFadeInStarter
	{
		bool IFadeInStarter.IsStart()
		{
			return false;
		}
	}
}
