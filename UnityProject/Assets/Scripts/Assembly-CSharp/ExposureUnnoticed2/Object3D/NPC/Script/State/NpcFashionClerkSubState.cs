using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script.State
{
	public class NpcFashionClerkSubState
	{
		private enum State
		{
			RegiWait = 0,
			RegiScan = 1
		}

		private NpcComponentAccessor nca;

		private State currentState;

		private float oldMoveScale;

		private float regiCancelTime;

		private float interactEndTime;

		private int statePhase;

		private Quaternion initRotate;

		public void Initialize(NpcComponentAccessor nca)
		{
		}

		public void OnUpdate()
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
