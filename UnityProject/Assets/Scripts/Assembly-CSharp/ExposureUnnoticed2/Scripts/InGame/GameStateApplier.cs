namespace ExposureUnnoticed2.Scripts.InGame
{
	public class GameStateApplier
	{
		private InGameReferencer inGameReferencer;

		private InGameManager inGameManager;

		private float lastApplyTime;

		public void Initialize(InGameReferencer inGameReferencer, InGameManager inGameManager)
		{
			// Recovered from GameAssembly.dll RVA 0x48D520
			this.inGameReferencer = inGameReferencer;
			this.inGameManager = inGameManager;
		}

		public void OnLateUpdate(bool isIgnoreWaitTime = false)
		{
			// Original complete RVA 0x48D5D0..0x48D8EE.
			if (inGameManager.TempInGameState.IsStopGameStateApplier) return;
			if (!isIgnoreWaitTime && UnityEngine.Time.unscaledTime - lastApplyTime < 1f) return;
			lastApplyTime = UnityEngine.Time.unscaledTime;
			var positionData = GameState.GameStateData;
			var position = inGameReferencer.Player.transform.position;
			if (positionData is null) throw new System.NullReferenceException();
			positionData.PlayerPosition = position;
			var rotationData = GameState.GameStateData;
			var rotation = inGameReferencer.Player.transform.rotation;
			if (rotationData is null) throw new System.NullReferenceException();
			rotationData.PlayerRotation = rotation;
			global::ExposureUnnoticed2.Object3D.NPC.Script.NpcManager.Instance.ApplyGameState();
		}

		public void UpdateIgnoreWaitTime()
		{
			// Original RVA 0x48D8F0: tail-call OnLateUpdate(true).
			OnLateUpdate(true);
		}
	}
}
