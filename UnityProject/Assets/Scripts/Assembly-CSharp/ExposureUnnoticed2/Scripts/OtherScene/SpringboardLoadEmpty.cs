using UnityEngine;

namespace ExposureUnnoticed2.Scripts.OtherScene
{
	public class SpringboardLoadEmpty : MonoBehaviour
	{
		public static bool IsPathSpringboardOnce;

		private void Start()
		{
			// Recovered from GameAssembly.dll RVA 0x4780E0
			global::ExposureUnnoticed2.Scripts.Base.TimeScaleManager.Instance.SwitchMode((global::ExposureUnnoticed2.Scripts.Base.TimeScaleManager.TimeScaleMode)0, false);
			IsPathSpringboardOnce = true;
			var record = global::ExposureUnnoticed2.Master.Stage.MStage.Get(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType);
			UnityEngine.SceneManagement.SceneManager.LoadScene(record.SceneName);
		}

		private void Update()
		{
		}
	}
}
