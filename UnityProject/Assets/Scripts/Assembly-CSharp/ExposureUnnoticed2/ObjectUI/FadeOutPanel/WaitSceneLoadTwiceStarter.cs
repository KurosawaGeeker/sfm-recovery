namespace ExposureUnnoticed2.ObjectUI.FadeOutPanel
{
	public class WaitSceneLoadTwiceStarter : IFadeInStarter
	{
		private int loadCount;

		private bool IsCompletedLoad;

		bool IFadeInStarter.IsStart()
		{
			// Recovered from GameAssembly.dll RVA 0x514070
			return IsCompletedLoad;
		}
	// Recovered original member RVA 0x513FD0: .ctor
		public WaitSceneLoadTwiceStarter()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnRecoveredSceneLoaded;
        }
	// Recovered original member RVA 0x514080: <.ctor>g__OnSceneLoaded|2_0
		private void OnRecoveredSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            ++loadCount;
            if (loadCount < 2) return;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnRecoveredSceneLoaded;
            IsCompletedLoad = true;
        }
	}
}
