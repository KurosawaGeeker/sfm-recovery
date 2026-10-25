using System.Collections.Generic;
using ExposureUnnoticed2.Object3D.Environment.ChangingRoom;
using ExposureUnnoticed2.Object3D.NPC.Script;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.FashionShop
{
	public class ChangingRoomManager : MonoBehaviour
	{
		[SerializeField]
		private ChangingRoomCurtainController[] changingRoomCurtainController;

		private Dictionary<int, NpcController> roomTokenDict;

		private int[] indexArray;

		public static ChangingRoomManager Instance { get; private set; }

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void Reset()
		{
		}

		private void OnChangeTime(ChangeTimeEvent evt)
		{
		}

		public ChangingRoomCurtainController TryGetTokenRoom(NpcController npc)
		{
			return null;
		}

		public void ReturnTokenRoom(NpcController npc)
		{
		}
	}
}
