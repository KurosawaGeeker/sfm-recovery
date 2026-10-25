using ExposureUnnoticed2.Object3D.ToiletManager;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script.State.Toilet
{
	public class NpcToiletHandWashState
	{
		private enum State
		{
			Wait = 0,
			Move = 1,
			Washing = 2
		}

		private State currentState;

		private NpcComponentAccessor nca;

		private ExposureUnnoticed2.Object3D.ToiletManager.ToiletManager toiletManager;

		private Transform washPoint;

		private float washEndRemainTime;

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
			toiletManager = ExposureUnnoticed2.Object3D.ToiletManager.ToiletManager.Instance;
		}

		public void OnUpdate()
		{
		}

		private void UpdateWait()
		{
		}

		private void UpdateMove()
		{
		}

		private void UpdateWash()
		{
		}
	}
}
