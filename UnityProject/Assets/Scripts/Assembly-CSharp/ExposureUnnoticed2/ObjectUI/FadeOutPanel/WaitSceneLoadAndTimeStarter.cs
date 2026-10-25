namespace ExposureUnnoticed2.ObjectUI.FadeOutPanel
{
	public class WaitSceneLoadAndTimeStarter : IFadeInStarter
	{
		private bool isCompletedLoad;

		public WaitSceneLoadAndTimeStarter(float waitTime)
		{
            // Original complete0x513D90 + original display1_0 callbacks0x513EC0/0x513FB0.
            global::UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            void OnSceneLoaded(global::UnityEngine.SceneManagement.Scene scene, global::UnityEngine.SceneManagement.LoadSceneMode mode)
            {
                global::UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
                global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayUnscaledTime(() => isCompletedLoad = true, waitTime);
            }
        }

		bool IFadeInStarter.IsStart()
		{
			// Original shared complete getter0x4816E0.
            return isCompletedLoad;
		}
	}
}
