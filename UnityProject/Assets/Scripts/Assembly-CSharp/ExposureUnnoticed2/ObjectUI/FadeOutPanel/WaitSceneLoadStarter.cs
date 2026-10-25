namespace ExposureUnnoticed2.ObjectUI.FadeOutPanel
{
	public class WaitSceneLoadStarter : IFadeInStarter
	{
		bool IFadeInStarter.IsStart()
		{
// Complete original RVA 0x4816E0; PE/typed identity is audited in this owned stage.
return isCompletedLoad;
}

        // Original constructor513C50 and original local callback513CF0, not an owned scene bypass.
        public WaitSceneLoadStarter()
        {
            global::UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            void OnSceneLoaded(global::UnityEngine.SceneManagement.Scene scene, global::UnityEngine.SceneManagement.LoadSceneMode mode)
            {
                global::UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
                isCompletedLoad = true;
            }
        }

		private bool isCompletedLoad;
	}
}
