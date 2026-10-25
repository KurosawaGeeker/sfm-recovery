using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class EventMethodManager : MonoBehaviour
	{
		private List<IStarter> starterList = new List<IStarter>();

		private List<IUpdater> updaterList = new List<IUpdater>();

		private List<ILateUpdater> lateUpdaterList = new List<ILateUpdater>();

		private InGameReferencer inGameReferencer;

		public void Initialize(InGameReferencer inGameReferencer)
		{
			this.inGameReferencer = inGameReferencer;
			starterList.Add(inGameReferencer.TpCameraController);
			updaterList.Add(global::ExposureUnnoticed2.Scripts.Base.CursorController.Instance);
			updaterList.Add(inGameReferencer.InGameManager);
			updaterList.Add(inGameReferencer.Player);
			updaterList.Add(inGameReferencer.NpcManager);
			updaterList.Add(inGameReferencer.SlowAssistManager);
			lateUpdaterList.Add(inGameReferencer.Player);
			lateUpdaterList.Add(inGameReferencer.NpcManager);
			lateUpdaterList.Add(inGameReferencer.InGameUiManager);
			lateUpdaterList.Add(inGameReferencer.InGameManager);
			OnStart();
		}

		private void OnStart()
		{
			foreach (IStarter item in starterList) item.OnStart();
		}

		private void FixedUpdate()
		{
			global::ExposureUnnoticed2.Scripts.Base.InputManager.Instance.OnFixedUpdate();
			inGameReferencer.Player.OnFixedUpdate();
			inGameReferencer.TpCameraController.OnFixedUpdate();
			inGameReferencer.NpcManager.OnFixedUpdate();
		}

		private void Update()
		{
			global::ExposureUnnoticed2.Scripts.Base.InputManager.Instance.OnUpdate();
			foreach (IUpdater item in updaterList) item.OnUpdate();
		}

		private void LateUpdate()
		{
			foreach (ILateUpdater item in lateUpdaterList) item.OnLateUpdate();
			inGameReferencer.TpCameraController.OnLateUpdate();
			inGameReferencer.Player.PlayerAvatarObjectReferencer.FPCameraController.OnLateUpdate();
			inGameReferencer.BaseCameraController.OnLateUpdate();
		}
	}
}
