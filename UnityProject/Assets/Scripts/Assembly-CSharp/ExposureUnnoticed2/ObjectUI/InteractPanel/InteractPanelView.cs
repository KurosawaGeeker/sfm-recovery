using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InteractPanel
{
	public class InteractPanelView : MonoBehaviour
	{
		[SerializeField]
		private GameObject contents;

		[SerializeField]
		private TextMeshProUGUI text;

		public void Initialize()
		{
		}

		public void OnLateUpdate()
		{
			// Recovered from GameAssembly.dll RVA 0x4FBCA0
			if (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsGameOver)
			{
			    contents.SetActive(false);
			    return;
			}
			var firstController = global::ExposureUnnoticed2.Scripts.Base.InteractManager.GetHead().Controller;
			var controller = firstController != null ? global::ExposureUnnoticed2.Scripts.Base.InteractManager.GetHead().Controller : null;
			if (controller != null)
			{
			    string label = controller.InteractPanelText();
			    var capturedContents = contents;
			    var ui = global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance;
			    if (ui is null) throw new global::System.NullReferenceException();
			    bool active = false;
			    if (!ui.isOpenSomeAllowInputUi && !ui.IsOpenSomeCancelInputUi)
			    {
			        var currentAction = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.PlayerState.CurrentAction;
			        active = (int)currentAction.Type == -1 || controller.CanInteractWithAction;
			    }
			    capturedContents.SetActive(active);
			    var capturedText = text;
			    string translated = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(label);
			    capturedText.text = translated;
			    return;
			}
			contents.SetActive(false);
		}
	}
}
