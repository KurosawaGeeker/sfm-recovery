using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.CurrentRpPanel
{
	public class CurrentRpPanelView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI rpText;

		[SerializeField]
		private bool isEarnRp;

		private int oldRp;

		private void LateUpdate()
		{
            // Original RVA 0x519FA0: display only; never mutate currency or earnings.
            if (!isEarnRp)
            {
                int value = unchecked((int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentRp);
                rpText.SetText("{0}RP", value, 0f, 0f, 0f, 0f, 0f, 0f, 0f);
                return;
            }
            int earnedOrLost = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsGameOver
                ? global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.LostRp
                : unchecked((int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentEarnThisTimeRp);
            rpText.SetText("{0}", earnedOrLost, 0f, 0f, 0f, 0f, 0f, 0f, 0f);
		}
	}
}
