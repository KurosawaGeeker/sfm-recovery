using System.Collections.Generic;
using ExposureUnnoticed2.Object3D.NPC.Script;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.TalkPoint
{
	public class TalkNpcManager : MonoBehaviour
	{
		private class TalkModel
		{
			public NpcController Npc1;

			public NpcController Npc2;

			private float lastIntervalTime;

			private float intervalTime;

			private bool reverse;

			public void SwitchIfInCondition()
			{
			}
		}

		private List<TalkModel> modelList;

		private TalkPoint[] points;

		public void Initialize()
		{
		}

		public void Dispose()
		{
		}

		private void Spawn()
		{
		}

		public void OnUpdate()
		{
		}

		private void OnChangeTime(ChangeTimeEvent evt)
		{
		}
	}
}
