namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcConbiniState
	{
		private enum State
		{
			Shinadashi = 0,
			RegiMove = 1,
			RegiWait = 2,
			RegiScan = 3
		}

		private NpcComponentAccessor nca;

		private State currentState;

		private float oldMoveScale;

		private float regiCancelTime;

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
		}

		public void OnUpdate()
		{
			if (ExposureUnnoticed2.Object3D.ScenePlops.Conbini.ConbiniController.Instance != null)
			{
			    switch (currentState)
			    {
			        case State.Shinadashi: UpdateShinadashi(); break;
			        case State.RegiMove: UpdateRegiMove(); break;
			        case State.RegiWait: UpdateRegiWait(); break;
			        case State.RegiScan: UpdateRegiScan(); break;
			    }
			}
		}

		private void UpdateShinadashi()
		{
		}

		private void UpdateRegiMove()
		{
		}

		private void UpdateRegiWait()
		{
		}

		private void UpdateRegiScan()
		{
		}
	}
}
