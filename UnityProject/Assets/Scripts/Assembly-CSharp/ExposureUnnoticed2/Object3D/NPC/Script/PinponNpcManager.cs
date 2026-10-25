using System.Collections.Generic;
using ExposureUnnoticed2.Object3D.ScenePlops.PinponDoor;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class PinponNpcManager : MonoBehaviour
	{
		private List<PinponDoorController> pinponList;

		public static PinponNpcManager Instance { get; private set; }

		public bool IsInActiveArea { get; private set; }

		private void Awake()
		{
		}

		private void Update()
		{
		}

		public void Register(PinponDoorController controller)
		{
		}

		public void Initialize()
		{
		}

		public void Dispose()
		{
		}

		private void OnChangeTime(ChangeTimeEvent evt)
		{
		}

		private void Spawn()
		{
		}
	}
}
