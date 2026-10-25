using ExposureUnnoticed2.Object3D.NPC.Script;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class GameStateChecker
	{
		private NpcManager npcManager;

		public void Initialize(NpcManager npcManager)
		{
			// Recovered from GameAssembly.dll RVA 0x3CAB80
			this.npcManager = npcManager;
		}

		public void OnUpdate()
		{
			// Recovered from GameAssembly.dll RVA 0x48D900
			if (GameState.GameStateData.PlayerLife < 1 || GameState.GameStateData.PlayerMental <= 0f)
			{
			    if (!InGameManager.Instance.TempInGameState.IsGameOver && (int)GameState.GameStateData.CurrentStageType != 0)
			    {
			        InGameManager.Instance.TempInGameState.IsGameOver = true;
			        global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.GameOverEvent);
			    }
			}
		}
	}
}
