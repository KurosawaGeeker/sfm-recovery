using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Scripts.Base;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcStrangenessManager
	{
		private const float ListenDist = 2f;

		public StrangenessValueManager Strangeness;

		private PlayerSearcher playerSearcher;

		private PlayerController player;

		private NpcComponentAccessor nca;

		private float addNoisyByPlayerSound;

		public void Initialize(NpcComponentAccessor nca)
		{
			player = nca.Player;
			playerSearcher = nca.PlayerSearcher;
			this.nca = nca;
			Strangeness = new StrangenessValueManager();
			EventManager.AddListener<PlayerSoundEvent>(OnPlayerSound, false);
		}

		public void Dispose()
		{
			EventManager.RemoveListener<PlayerSoundEvent>(OnPlayerSound);
		}

		public void OnUpdate()
		{
			// Original complete OnUpdate RVA435300; all suspicion/noise controls retain native ordering.
			if (nca.StateModel.IsSkipUpdateStrangeness) return;
			if (nca.StateModel.IsSkipRiseStrangenessPinponNpc) return;
			nca.NpcController.IsRiseStrangeness = false;
			NpcStateModel.State current = nca.StateModel.CurrentState;
			StrangenessValueManager decay = Strangeness;
			if (decay is null) throw new System.NullReferenceException();
			decay.OnUpdate(current == NpcStateModel.State.Stagger || current == NpcStateModel.State.TurnAround);
			StrangeData data = ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.CalcStrangerDegreeFrom(nca.NpcController.Transform, nca.LookAtController.Head, nca);
			float paintMaximum = ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.BodyPaintController.GetMaxDeltaAdd();
			float visualCapFactor = 0.3f;
			float difficultyScale;
			if ((int)ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DifficultType == 2)
			{
			    difficultyScale = 4f;
			    visualCapFactor = 1f;
			}
			else difficultyScale = 1.5f;
			float commonScale = CalcCommonAddScale();
			float discomfortDelta = UnityEngine.Time.deltaTime;
			float discomfortStep = UnityEngine.Mathf.Clamp01(UnityEngine.Mathf.Min(data.AddDiscomfort * discomfortDelta * difficultyScale * commonScale, data.MaxDiscomfort - Strangeness.Value));
			float visualStep = 0f;
			if (playerSearcher.IsPlayerFound && (data.StrangerDegree > 0f || data.AddDiscomfort > 0f))
			{
			    float delta = UnityEngine.Time.deltaTime;
			    visualStep = UnityEngine.Mathf.Max(delta * data.StrangerDegree * difficultyScale * commonScale, discomfortStep);
			    nca.StateModel.IsRiseStrangeFromOnlyDiscomfort = data.StrangerDegree <= 0f && data.AddDiscomfort > 0f;
			}
			else if (nca.StateModel.IsAlreadyFoundStranger && playerSearcher.IsPlayerFound)
			    visualStep = data.OnlySightScale * 0.1f;
			else if (nca.StateModel.CurrentFixedType == NpcStateModel.FixedType.Pinpon && playerSearcher.IsPlayerFound)
			{
			    nca.StateModel.IsRiseStrangeFromOnlyDiscomfort = true;
			    float delta = UnityEngine.Time.deltaTime;
			    visualStep = UnityEngine.Mathf.Clamp01(UnityEngine.Mathf.Min(delta * 0.1f * difficultyScale * commonScale, 0.5f - Strangeness.Value));
			}
			if (nca.LookAtController.IsEnable(ExposureUnnoticed2.Object3D.Player.Scripts.LookAtController.LookAtLayer.NpcForeignObject) && !nca.LookAtController.IsEnable(ExposureUnnoticed2.Object3D.Player.Scripts.LookAtController.LookAtLayer.NpcLookPlayer))
			    visualStep *= 0.5f;
			if (visualStep > 0f && !ExposureUnnoticed2.Scripts.InGame.DebugController.Muteki)
			{
			    nca.NpcController.IsRiseStrangeness = true;
			    StrangenessValueManager destination = Strangeness;
			    if (destination is null) throw new System.NullReferenceException();
			    float previous = destination.Value;
			    float delta = UnityEngine.Time.deltaTime;
			    float maximum = delta * (paintMaximum * visualCapFactor);
			    destination.Value = previous + UnityEngine.Mathf.Clamp(visualStep, 0f, maximum);
			    Strangeness.Value = UnityEngine.Mathf.Clamp01(Strangeness.Value);
			}
			if (!nca.StateModel.IsJustHitSpermOnSkin)
			{
			    if (nca.StateModel.IsHeadSet || !(commonScale > 0f)) return;
			    NpcStateModel capturedState = nca.StateModel;
			    float previousIgnore = capturedState.IgnorePlayerNoisyTime;
			    float delta = UnityEngine.Time.deltaTime;
			    capturedState.IgnorePlayerNoisyTime = previousIgnore - delta;
			    if (!(nca.StateModel.IgnorePlayerNoisyTime < 0f)) return;
			    if (!(data.Noisy > 0f || addNoisyByPlayerSound > 0f)) return;
			    float noisyDelta = UnityEngine.Time.deltaTime;
			    NpcStateModel noiseState = nca.StateModel;
			    StrangenessValueManager noiseDestination = Strangeness;
			    if (noiseDestination is null) throw new System.NullReferenceException();
			    float noisyStep = UnityEngine.Mathf.Clamp01(UnityEngine.Mathf.Min(noisyDelta * data.Noisy, noiseState.TurnNoisyLevel - noiseDestination.Value));
			    float combinedNoise = commonScale * addNoisyByPlayerSound + noisyStep * commonScale;
			    noiseDestination.Value = combinedNoise + noiseDestination.Value;
			    nca.StateModel.IsRiseStrangeFromOnlyDiscomfort = false;
			    if (!nca.PlayerSearcher.IsPlayerFound && combinedNoise > 0f && Strangeness.Value >= nca.StateModel.TurnNoisyLevel)
			        nca.NpcController.ChangeState(NpcStateModel.State.TurnAround);
			    addNoisyByPlayerSound = 0f;
			    return;
			}
			nca.StateModel.IsJustHitSpermOnSkin = false;
			if (!nca.StateModel.IsSit)
			{
			    NpcStateModel state = nca.StateModel;
			    if ((state.CurrentFixedType != NpcStateModel.FixedType.Talk && state.CurrentFixedType != NpcStateModel.FixedType.Conbini) || state.IgnoreSpermTurn)
			    {
			        if ((state.CurrentState != NpcStateModel.State.Idle || state.IgnoreSpermTurn) && state.CurrentState != NpcStateModel.State.WaitToken)
			        {
			            if (state.CurrentState != NpcStateModel.State.Interact || state.IgnoreSpermTurn) return;
			            uint type = (uint)state.CurrentRouteInteractType;
			            if (type != 2 && (type > 14 || ((0x6fb2u >> (int)type) & 1u) == 0) && type != 12) return;
			        }
			    }
			    if (Strangeness.Value < nca.StateModel.TurnNoisyLevel)
			        Strangeness.Value = UnityEngine.Mathf.Min(nca.StateModel.TurnNoisyLevel + Strangeness.Value, nca.StateModel.TurnNoisyLevel);
			    if (Strangeness.Value >= nca.StateModel.TurnNoisyLevel)
			        nca.NpcController.ChangeState(NpcStateModel.State.TurnAround);
			    return;
			}
			if (nca.StateModel.StrangeAddScaleSleep <= 0f) return;
			Strangeness.Value = nca.StateModel.TurnNoisyLevel + 0.01f;
		}

		private float CalcCommonAddScale()
		{
			StrangenessValueManager value = nca.StrangenessManager.Strangeness;
			if (value is null) throw new System.NullReferenceException();
			float current = value.Value;
			NpcStateModel initialState = nca.StateModel;
			float threshold = initialState.StrangeLookAt + 0.05f;
			float result = 1f;
			if (initialState.CurrentFixedType == NpcStateModel.FixedType.SitSleep) result = initialState.StrangeAddScaleSleep;
			bool belowLookThreshold = threshold > current;
			ExposureUnnoticed2.Object3D.NPC.Script.State.NpcChairSubState chair = nca.NpcChairSubState;
			if (chair is null) throw new System.NullReferenceException();
			if (chair.IsCurrentChairSleep) result = nca.StateModel.StrangeAddScaleSleep;
			if (!belowLookThreshold) return result;
			if (nca.StateModel.CurrentState == NpcStateModel.State.Idle && nca.StateModel.isSumahoWalk) result = 0.05f;
			if ((int)nca.StateModel.CurrentRouteInteractType == 4) result = 0.05f;
			if ((int)nca.StateModel.CurrentRouteInteractType == 6) result = 0.05f;
			int action = (int)nca.StateModel.CurrentChairActionType;
			if (action == 3 || action == 2) result = 0.05f;
			if (nca.StateModel.CurrentState == NpcStateModel.State.Interact)
			{
			    ExposureUnnoticed2.Object3D.RoutePoint.RouteData route = nca.StateModel.RouteData;
			    if (route is null) throw new System.NullReferenceException();
			    ExposureUnnoticed2.Object3D.RoutePoint.RoutePoint point = route.GetNextPoint();
			    if (point is null) throw new System.NullReferenceException();
			    if ((int)point.RouteInteractType == 3)
			    {
			        ExposureUnnoticed2.Object3D.NPC.Script.State.NpcChairSubState routeChair = nca.NpcChairSubState;
			        if (routeChair is null) throw new System.NullReferenceException();
			        int chairState = (int)routeChair.currentChairState;
			        if (chairState == 0 || chairState == 2) result = 0.05f;
			    }
			}
			if (nca.StateModel.CurrentFixedType == NpcStateModel.FixedType.Talk) result = 0.1f;
			if (nca.StateModel.CurrentFixedType == NpcStateModel.FixedType.SitBook) result = 0.05f;
			if (nca.StateModel.CurrentFixedType == NpcStateModel.FixedType.SitSumaho) result = 0.05f;
			return result;
		}

		public void OnStagger()
		{
		}

		private void OnPlayerSound(PlayerSoundEvent evt)
		{
			if (nca.StateModel.CurrentState == NpcStateModel.State.TraceSexPlayer) return;
			if (nca.StateModel.IsHeadSet) return;
			if (nca.StateModel.IsHidePinponNpc) return;
			if (0f < nca.StateModel.IgnorePlayerNoisyTime) return;
			if (playerSearcher.IsPlayerFound) return;
			if (ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsGameOver) return;
			if (nca.StateModel.TurnNoisyLevel <= Strangeness.Value) return;
			UnityEngine.GameObject playerObject = player.gameObject;
			UnityEngine.GameObject npcObject = nca.GameObject;
			float distance = ExternalMethodManager.DistSqrt(playerObject, npcObject);
			float common = CalcCommonAddScale();
			float falloff = (2f - distance) * 0.5f;
			if (falloff < 0f) falloff = 0f;
			else if (1f < falloff) falloff = 1f;
			float combined = falloff * common;
			if (combined <= 0f) return;
			int reinforcement = ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.ReinforceEffectData.ReduceFootStepSound100;
			float reinforcementScale = reinforcement * 0.01f + 1f;
			reinforcementScale = 0f > reinforcementScale ? 0f : reinforcementScale;
			float amount = 0f;
			int type = (int)evt.SeType;
			switch (type)
			{
			    case 1: case 2: case 6: case 31: case 95: amount = reinforcementScale * 0.05f; break;
			    case 7: case 8: amount = 0.1f; break;
			    case 30: amount = reinforcementScale * 0.02f; break;
			    case 44: amount = evt.IsPistonMachine ? 0.02f : 0.1f; break;
			}
			nca.StateModel.IsRiseStrangeFromOnlyDiscomfort = false;
			float remaining = nca.StateModel.TurnNoisyLevel - Strangeness.Value;
			amount = amount * combined;
			amount = amount < remaining ? amount : remaining;
			if (amount < 0f) amount = 0f;
			else if (1f < amount) amount = 1f;
			addNoisyByPlayerSound = amount;
		}

		public void ForceSetStrangeness(float val)
		{
		}
	}
}
