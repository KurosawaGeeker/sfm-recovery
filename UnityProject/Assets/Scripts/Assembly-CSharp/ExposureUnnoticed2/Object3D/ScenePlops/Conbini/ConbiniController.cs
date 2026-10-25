using ExposureUnnoticed2.Object3D.InteractArea;
using ExposureUnnoticed2.Object3D.NPC.Script;
using ExposureUnnoticed2.Object3D.RoutePoint;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.Conbini
{
	public class ConbiniController : MonoBehaviour
	{
		[SerializeField]
		private ConbiniCallRegiAreaController areaController;

		[SerializeField]
		private InteractAreaController interactAreaController;

		[SerializeField]
		private GameObject takeGoodsParent;

		[SerializeField]
		private GameObject regiTargetObject;

		[SerializeField]
		private BaseInteractTokenPublisher regiCustomerTokenPublisher;

		[SerializeField]
		private AudioSource enterSound;

		private bool isForPlayer;

		private NpcController conbiniNpc;

		private int playerBringGoodsNum;

		private float canPlayEnterSoundRemainTime;

		[field: SerializeField]
		public ExposureUnnoticed2.Object3D.RoutePoint.RoutePoint RegiPoint { get; private set; }

		public static ConbiniController Instance { get; private set; }

		public bool IsWaitCustomer => false;

		public int PutGoodsNum { get; set; }

		public bool IsInResiExposeMission => false;

		public void Initialize()
		{
			Spawn();
			Instance = this;
			regiTargetObject.SetActive(false);
			interactAreaController.Initialize(() =>
			{
			    if (!global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.CanUseHand(true)) return;
			    if (playerBringGoodsNum < 1)
			    {
			        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_not_have_product"));
			        return;
			    }
			    if (!regiCustomerTokenPublisher.TryGetToken())
			    {
			        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_use_npc"));
			        return;
			    }
			    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.SmoothMoveTo(interactAreaController.transform);
			    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransAction(global::ExposureUnnoticed2.Master.Action.ActionType.HandOver);
			});
			foreach (InteractAreaController interactArea in takeGoodsParent.GetComponentsInChildren<InteractAreaController>())
			{
			    interactArea.Initialize(() =>
			    {
			        if (!global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.CanUseHand(true)) return;
			        ++playerBringGoodsNum;
			        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.SmoothMoveTo(interactArea.transform);
			        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransAction(global::ExposureUnnoticed2.Master.Action.ActionType.ConbiniTakeGoods);
			    });
			}
			EventManager.AddListener<ChangeTimeEvent>(OnChangeTime, false);
			PlayEnterSoundIfInCondition();
			EventManager.AddListener<TimeStopEvent>(OnTimeStopEvent, false);
		}

		private void Update()
		{
			if (!global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsTimeStop)
			{
			    float previous = canPlayEnterSoundRemainTime;
			    float delta = UnityEngine.Time.deltaTime;
			    canPlayEnterSoundRemainTime = previous - delta;
			}
		}

		public void PlayEnterSoundIfInCondition()
		{
			if ((int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType == 7 || canPlayEnterSoundRemainTime > 0f) return;
			canPlayEnterSoundRemainTime = 6f;
			enterSound.Play();
		}

		private void OnChangeTime(ChangeTimeEvent evt)
		{
			PutGoodsNum = 0;
			Spawn();
			areaController.OnChangeTime();
		}

		private void Spawn()
		{
			conbiniNpc = NpcManager.Instance.CreateFixedNpc(NpcStateModel.FixedType.Conbini);
		}

		public void OnJustHandOver(NpcController npc = null)
		{
		}

		public void OnEndConbiniRegiScan()
		{
		}

		private void OnDestroy()
		{
			if (Instance != this) return;
			Instance = null;
			EventManager.RemoveListener<ChangeTimeEvent>(OnChangeTime);
			EventManager.RemoveListener<TimeStopEvent>(OnTimeStopEvent);
			interactAreaController.Dispose();
			foreach (InteractAreaController interactArea in takeGoodsParent.GetComponentsInChildren<InteractAreaController>())
			    interactArea.Dispose();
		}

		private void OnTimeStopEvent(TimeStopEvent evt)
		{
			if ((int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType == 7) return;
			if (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsTimeStop)
			    enterSound.Pause();
			else
			    enterSound.UnPause();
		}
	}
}
