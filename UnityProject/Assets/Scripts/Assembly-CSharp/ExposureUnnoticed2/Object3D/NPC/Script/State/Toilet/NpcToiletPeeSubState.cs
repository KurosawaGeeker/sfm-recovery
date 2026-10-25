using System;
using ExposureUnnoticed2.Object3D.ToiletManager;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script.State.Toilet
{
	public class NpcToiletPeeSubState
	{
		public enum State
		{
			Wait = 0,
			Move = 1,
			Peeing = 2
		}

		public State currentState;

		private NpcComponentAccessor nca;

		private ExposureUnnoticed2.Object3D.ToiletManager.ToiletManager toiletManager;

		private Transform peePoint;

		private float peeEndRemainTime;

		private Action onEnd;

		public void Initialize(NpcComponentAccessor nca, Action onEnd)
		{
			this.nca = nca;
			toiletManager = ExposureUnnoticed2.Object3D.ToiletManager.ToiletManager.Instance;
			this.onEnd = onEnd;
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

		private void UpdatePee()
		{
		}
	}
}
