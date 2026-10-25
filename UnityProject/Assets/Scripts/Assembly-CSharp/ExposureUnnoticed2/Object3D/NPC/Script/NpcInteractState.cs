namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcInteractState
	{
		private NpcComponentAccessor nca;

		private float interactTime;

		private float interactRemainTime;

		private int statePhase;

		private bool onEndEscalator;

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
		}

		public void OnEnter()
		{
            // Complete original RVA 0x43BF40, including the original fourteen-way dispatch.
            nca.StateModel.LastRouteInteractType = global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.None;
            var point = nca.StateModel.RouteData.GetNextPoint();
            nca.StateModel.CurrentRouteInteractType = point.RouteInteractType;
            switch (nca.StateModel.CurrentRouteInteractType)
            {
                case global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.BuyMachine:
                    if (!nca.StateModel.IsComeBackInteractFromFoundStranger)
                    {
                        nca.NpcAnimationManager.SetBuyMachine();
                        interactRemainTime = 10f / nca.StateModel.MoveSpeedScale;
                    }
                    else
                    {
                        nca.StateModel.HasInteractToken = false;
                        point.LinkTokenPublisher.ReturnToken();
                        nca.StateModel.RouteData.IncrementIndex();
                        nca.NpcController.ChangeState(NpcStateModel.State.Idle);
                        nca.NpcAnimationManager.ForceExitActionState();
                    }
                    break;
                case global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.Escalator:
                    nca.NpcAnimationManager.SetWait(true);
                    nca.Rigidbody.isKinematic = false;
                    nca.NavAgent.enabled = false;
                    onEndEscalator = false;
                    break;
                case global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.Chair:
                    if (!nca.StateModel.IsComeBackInteractFromFoundStranger) nca.NpcChairSubState.OnEnter();
                    break;
                case global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.StandSmartphone:
                    nca.NpcComponent.Smartphone.gameObject.SetActive(true);
                    nca.NpcAnimationManager.SetStandSmartphone();
                    if (!nca.StateModel.IsComeBackInteractFromFoundStranger)
                        interactTime = global::UnityEngine.Random.value * 60f + 30f;
                    break;
                case global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.ConbiniShinadashi:
                    if (!nca.StateModel.IsComeBackInteractFromFoundStranger)
                    {
                        nca.NpcAnimationManager.SetConbiniCrouch();
                        float random = global::UnityEngine.Random.value;
                        statePhase = 0;
                        interactTime = random * 20f + 10f;
                        nca.NavAgent.enabled = false;
                    }
                    else
                    {
                        nca.StateModel.HasInteractToken = false;
                        nca.StateModel.RouteData.IncrementIndex();
                        nca.NpcController.ChangeState(NpcStateModel.State.Idle);
                        nca.NpcAnimationManager.ForceExitActionState();
                    }
                    break;
                case global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.ConbiniRegiScan:
                    if (!nca.StateModel.IsComeBackInteractFromFoundStranger)
                    {
                        nca.NpcAnimationManager.SetSpeed(0f);
                        statePhase = 0;
                    }
                    break;
                case global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.Elevator:
                    nca.NpcElevatorSubState.OnEnter();
                    break;
                case global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.MansionPostReceive:
                    if (!nca.StateModel.IsComeBackInteractFromFoundStranger) nca.NpcAnimationManager.SetUsePost();
                    else
                    {
                        point.LinkTokenPublisher.ReturnToken();
                        nca.StateModel.RouteData.IncrementIndex();
                        nca.NpcController.ChangeState(NpcStateModel.State.Idle);
                    }
                    break;
                case global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.ConbiniProductPick:
                    if (!nca.StateModel.IsComeBackInteractFromFoundStranger)
                    {
                        nca.NavAgent.enabled = false;
                        ++nca.StateModel.ConbiniBringProductNum;
                        statePhase = 0;
                        interactRemainTime = global::UnityEngine.Random.value * 16f + 4f;
                        nca.NpcAnimationManager.SetIsWorry(true);
                    }
                    else
                    {
                        point.LinkTokenPublisher.ReturnToken();
                        nca.StateModel.RouteData.IncrementIndex();
                        nca.NpcController.ChangeState(NpcStateModel.State.Idle);
                        nca.NpcAnimationManager.SetIsWorry(false);
                    }
                    break;
                case global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.ConbiniRegiCustomer:
                    if (!nca.StateModel.IsComeBackInteractFromFoundStranger)
                    {
                        nca.NpcAnimationManager.SetHandOver();
                        float duration = 0.75f / nca.StateModel.MoveSpeedScale;
                        statePhase = 0;
                        interactRemainTime = duration;
                        nca.NpcAnimationManager.SetSpeed(0f);
                    }
                    else
                    {
                        if (statePhase == 0)
                        {
                            statePhase = 1;
                            if (global::ExposureUnnoticed2.Object3D.ScenePlops.Conbini.ConbiniController.Instance != null)
                                global::ExposureUnnoticed2.Object3D.ScenePlops.Conbini.ConbiniController.Instance.OnJustHandOver(nca.NpcController);
                        }
                        if (statePhase == 1) statePhase = 2;
                    }
                    break;
                case global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.LookBoard:
                    if (!nca.StateModel.IsComeBackInteractFromFoundStranger)
                    {
                        nca.NpcAnimationManager.SetIsWorry(true);
                        interactRemainTime = global::UnityEngine.Random.value * 15f + 5f;
                        nca.NpcAnimationManager.SetSpeed(0f);
                    }
                    else
                    {
                        nca.NpcAnimationManager.SetIsWorry(false);
                        nca.StateModel.HasInteractToken = false;
                        point.LinkTokenPublisher.ReturnToken();
                        nca.StateModel.RouteData.IncrementIndex();
                        nca.NpcController.ChangeState(NpcStateModel.State.Idle);
                    }
                    break;
                case global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.Toilet:
                    nca.NpcToiletSubState.OnEnter();
                    break;
                case global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.FashionShopRoomWait:
                    nca.NpcFashionRoomSubState.OnEnter();
                    break;
                case global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.CoinLaundry:
                    nca.NpcLaundrySubState.OnEnter();
                    break;
            }
		}

		public void OnExit()
		{
            // Complete original RVA 0x43CC50.
            if (onEndEscalator) nca.Rigidbody.isKinematic = true;
            nca.NavAgent.enabled = true;
            if (nca.StateModel.CurrentRouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.StandSmartphone)
            {
                nca.NpcComponent.Smartphone.gameObject.SetActive(false);
                nca.NpcAnimationManager.SetNoAction();
            }
            else if (nca.StateModel.CurrentRouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.ConbiniProductPick)
                nca.NpcAnimationManager.SetIsWorry(false);
            nca.StateModel.LastRouteInteractType = nca.StateModel.CurrentRouteInteractType;
            nca.StateModel.CurrentRouteInteractType = global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.None;
		}

		public void OnUpdate()
		{
            // Complete original RVA 0x43CF30. Delegated substates retain their original calls.
            interactRemainTime -= global::UnityEngine.Time.deltaTime;
            var point = nca.StateModel.RouteData.GetNextPoint();
            var pointPosition = point.transform.position;
            var pointEuler = point.transform.rotation.eulerAngles;
            if (point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.Escalator)
            {
                var target = nca.Transform;
                var rotation = nca.Transform.rotation;
                target.rotation = global::UnityEngine.Quaternion.Lerp(rotation, global::UnityEngine.Quaternion.Euler(pointEuler), 0.1f);
                if (global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DistSqrt(point.EscalatorTarget, nca.Transform) < 1f)
                {
                    onEndEscalator = true;
                    nca.NpcAnimationManager.SetWait(false);
                    nca.StateModel.RouteData.IncrementIndex();
                    nca.NpcController.ChangeState(NpcStateModel.State.Idle);
                }
                return;
            }
            if (point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.Chair)
            { nca.NpcChairSubState.OnUpdate(); return; }
            if (point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.Toilet)
            { nca.NpcToiletSubState.OnUpdate(); return; }
            if (point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.CoinLaundry)
            { nca.NpcLaundrySubState.OnUpdate(); return; }
            if (point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.FashionShopRoomWait)
            { nca.NpcFashionRoomSubState.OnUpdate(); return; }
            if (point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.StandSmartphone ||
                point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.ConbiniShinadashi ||
                point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.ConbiniRegiScan)
            {
                var target = nca.Transform;
                var position = nca.Transform.position;
                target.position = new global::UnityEngine.Vector3(
                    (pointPosition.x - position.x) * 0.1f + position.x,
                    (pointPosition.y - position.y) * 0.1f + position.y,
                    (pointPosition.z - position.z) * 0.1f + position.z);
                target = nca.Transform;
                var rotation = nca.Transform.rotation;
                target.rotation = global::UnityEngine.Quaternion.Lerp(rotation, global::UnityEngine.Quaternion.Euler(pointEuler), 0.2f);
                if (point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.StandSmartphone)
                {
                    if (global::UnityEngine.Time.time - nca.StateModel.stateInTime > interactTime)
                    {
                        nca.StateModel.RouteData.IncrementIndex();
                        nca.NpcController.ChangeState(NpcStateModel.State.Idle);
                    }
                    return;
                }
                if (point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.ConbiniShinadashi)
                {
                    if (statePhase == 0 && global::UnityEngine.Time.time - nca.StateModel.stateInTime > interactTime)
                    {
                        statePhase = 1;
                        nca.NpcAnimationManager.SetConbiniStand();
                    }
                    if (statePhase == 1 && global::UnityEngine.Time.time - nca.StateModel.stateInTime > interactTime + 1f)
                    {
                        nca.NavAgent.enabled = true;
                        statePhase = 2;
                        nca.StateModel.RouteData.IncrementIndex();
                        nca.NpcController.ChangeState(NpcStateModel.State.Idle);
                    }
                    return;
                }
                if (statePhase == 0 && nca.StateModel.RegiScanRemainNum > 0)
                {
                    statePhase = 1;
                    interactRemainTime = (float)nca.StateModel.RegiScanRemainNum * 1.5f / nca.StateModel.MoveSpeedScale;
                    nca.NpcAnimationManager.SetConbiniRegiScan(true);
                }
                if (statePhase == 1 && interactRemainTime <= 0f)
                {
                    statePhase = 0;
                    nca.StateModel.RegiScanRemainNum = 0;
                    nca.NpcAnimationManager.SetConbiniRegiScan(false);
                }
                if (nca.StateModel.ConbiniEndShinadashi)
                {
                    nca.StateModel.ConbiniEndShinadashi = false;
                    nca.StateModel.RouteData.IncrementIndex();
                    nca.NpcAnimationManager.SetSpeed(1f);
                    nca.NpcController.ChangeState(NpcStateModel.State.Idle);
                }
                return;
            }
            if (point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.BuyMachine ||
                point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.MansionPostReceive ||
                point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.LookBoard ||
                point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.ConbiniRegiCustomer ||
                point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.ConbiniProductPick)
            {
                var agent = nca.NavAgent;
                agent.nextPosition = nca.NpcController.Transform.position;
                var target = nca.Transform;
                var position = nca.Transform.position;
                target.position = new global::UnityEngine.Vector3(
                    (pointPosition.x - position.x) * 0.1f + position.x,
                    (pointPosition.y - position.y) * 0.1f + position.y,
                    (pointPosition.z - position.z) * 0.1f + position.z);
                target = nca.Transform;
                var rotation = nca.Transform.rotation;
                target.rotation = global::UnityEngine.Quaternion.Lerp(rotation, global::UnityEngine.Quaternion.Euler(pointEuler), 0.2f);
                if (point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.BuyMachine ||
                    point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.MansionPostReceive)
                {
                    if (point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.BuyMachine && interactRemainTime <= 0f)
                    {
                        interactRemainTime = 10f / nca.StateModel.MoveSpeedScale;
                        nca.NpcAnimationManager.SetBuyMachine();
                    }
                    if (nca.StateModel.IsJustOnEndInteract)
                    {
                        if (point.LinkTokenPublisher != null)
                        {
                            nca.StateModel.HasInteractToken = false;
                            point.LinkTokenPublisher.ReturnToken();
                        }
                        nca.StateModel.RouteData.IncrementIndex();
                        nca.NpcController.ChangeState(NpcStateModel.State.Idle);
                    }
                    return;
                }
                if (point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.LookBoard)
                {
                    if (interactRemainTime <= 0f)
                    {
                        nca.NpcAnimationManager.SetIsWorry(false);
                        if (point.LinkTokenPublisher != null)
                        {
                            nca.StateModel.HasInteractToken = false;
                            point.LinkTokenPublisher.ReturnToken();
                        }
                        nca.StateModel.RouteData.IncrementIndex();
                        nca.NpcController.ChangeState(NpcStateModel.State.Idle);
                    }
                    return;
                }
                if (point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.ConbiniRegiCustomer)
                {
                    if (statePhase == 0 && interactRemainTime <= 0f)
                    {
                        statePhase = 1;
                        if (global::ExposureUnnoticed2.Object3D.ScenePlops.Conbini.ConbiniController.Instance != null)
                            global::ExposureUnnoticed2.Object3D.ScenePlops.Conbini.ConbiniController.Instance.OnJustHandOver(nca.NpcController);
                    }
                    if (nca.StateModel.IsJustOnEndInteract) statePhase = 2;
                    if (statePhase == 2 && global::ExposureUnnoticed2.Object3D.ScenePlops.Conbini.ConbiniController.Instance.PutGoodsNum == 0)
                    {
                        if (point.LinkTokenPublisher != null) nca.StateModel.HasInteractToken = false;
                        nca.StateModel.RouteData.IncrementIndex();
                        nca.NpcController.ChangeState(NpcStateModel.State.Idle);
                    }
                    return;
                }
                if (statePhase == 0 && interactRemainTime <= 0f)
                {
                    statePhase = 1;
                    nca.NpcAnimationManager.SetIsWorry(false);
                    nca.NpcAnimationManager.SetConbiniTakeGoods();
                }
                if (nca.StateModel.IsJustOnEndInteract)
                {
                    if (point.LinkTokenPublisher != null)
                    {
                        nca.StateModel.HasInteractToken = false;
                        point.LinkTokenPublisher.ReturnToken();
                    }
                    nca.NavAgent.enabled = false;
                    nca.StateModel.RouteData.IncrementIndex();
                    nca.NpcController.ChangeState(NpcStateModel.State.Idle);
                }
                return;
            }
            if (point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.Elevator)
                nca.NpcElevatorSubState.OnUpdate();
		}
	}
}
