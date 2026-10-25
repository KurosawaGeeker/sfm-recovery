using System.Collections.Generic;
using ExposureUnnoticed2.Master.Sex;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcAnimationManager
	{
		private enum Layer
		{
			Locomotion = 0,
			Action = 1,
			OverrideAction = 2,
			Face = 3,
			FoundStranger = 4
		}

		public enum FacialLayer
		{
			ForeignObject = 0,
			AlreadyFound = 1
		}

		private NpcComponentAccessor nca;

		private Animator animator;

		private SmoothFloat speed = new SmoothFloat(0.2f);

		private SmoothFloat overrideActionLayerWeight = new SmoothFloat(0.2f);

		private float lastSetSpeed;

		private Dictionary<FacialLayer, bool> faceContemptLayers = new Dictionary<FacialLayer, bool>();

		private static readonly int Speed = Animator.StringToHash("Speed");

		private static readonly int Stagger = Animator.StringToHash("Stagger");

		private static readonly int StaggerX = Animator.StringToHash("StaggerX");

		private static readonly int StaggerY = Animator.StringToHash("StaggerY");

		private static readonly int BuyMachine = Animator.StringToHash("BuyMachine");

		private static readonly int IsFemale = Animator.StringToHash("IsFemale");

		private static readonly int Wait = Animator.StringToHash("IsWait");

		private static readonly int NpcTalk1 = Animator.StringToHash("NpcTalk1");

		private static readonly int NpcTalk2 = Animator.StringToHash("NpcTalk2");

		private static readonly int IsFoundStranger = Animator.StringToHash("IsFoundStranger");

		private static readonly int SitSleep = Animator.StringToHash("SitSleep");

		private static readonly int SitBook = Animator.StringToHash("SitBook");

		private static readonly int IsAwake = Animator.StringToHash("IsAwake");

		private static readonly int SitSumaho = Animator.StringToHash("SitSumaho");

		private static readonly int IsSit = Animator.StringToHash("IsSit");

		private static readonly int ChairSitDown = Animator.StringToHash("ChairSitDown");

		private static readonly int ChairStandUp = Animator.StringToHash("ChairStandUp");

		private static readonly int StandSmartphone = Animator.StringToHash("StandSmartphone");

		private static readonly int NoAction = Animator.StringToHash("NoAction");

		private static readonly int IsFaceContempt = Animator.StringToHash("IsFaceContempt");

		private static readonly int Pinpon = Animator.StringToHash("Pinpon");

		private static readonly int PinponClose = Animator.StringToHash("PinponClose");

		private static readonly int IsMirror = Animator.StringToHash("IsMirror");

		private static readonly int ConbiniCrouch = Animator.StringToHash("ConbiniCrouch");

		private static readonly int ConbiniStand = Animator.StringToHash("ConbiniStand");

		private static readonly int ConbiniShinadashi = Animator.StringToHash("ConbiniShinadashi");

		private static readonly int RegiScan = Animator.StringToHash("RegiScan");

		private static readonly int UsePost = Animator.StringToHash("UsePost");

		private static readonly int IsWorry = Animator.StringToHash("IsWorry");

		private static readonly int ConbiniTakeGoods = Animator.StringToHash("ConbiniTakeGoods");

		private static readonly int HandOver = Animator.StringToHash("HandOver");

		private static readonly int MalePeeing = Animator.StringToHash("MalePeeing");

		private static readonly int HandWash = Animator.StringToHash("HandWash");

		private static readonly int OpenToiletDoor = Animator.StringToHash("OpenToiletDoor");

		private static readonly int CloseToiletDoor = Animator.StringToHash("CloseToiletDoor");

		private static readonly int OpenOutsideToiletDoor = Animator.StringToHash("OpenOutsideToiletDoor");

		private static readonly int OpenInsideToiletDoor = Animator.StringToHash("OpenInsideToiletDoor");

		private static readonly int UseWasher = Animator.StringToHash("UseWasher");

		private static readonly int OverrideIdle = Animator.StringToHash("OverrideIdle");

		private static readonly int ExitActionState = Animator.StringToHash("ForceExitActionState");

		private static readonly int IsSumahoWalk = Animator.StringToHash("IsSumahoWalk");

		private static readonly int SexId = Animator.StringToHash("SexId");

		private static readonly int Sex = Animator.StringToHash("TransSex");

		private static readonly int IsFaceNiya = Animator.StringToHash("IsFaceNiya");

		private static readonly int Ecstasy = Animator.StringToHash("IsEcstasy");

		private static readonly int IsFaceIku = Animator.StringToHash("FaceIku");

		public void Initialize(NpcComponentAccessor nca)
		{
            // Complete original RVA41A980; original parameter capture is shared with the delayed callback.
            this.nca = nca;
            animator = nca.Animator;
            speed.Target = 0f;
            speed.Current = 0f;
            nca.Animator.applyRootMotion = false;
            var layers = System.Linq.Enumerable.ToList(System.Linq.Enumerable.Cast<FacialLayer>(System.Enum.GetValues(typeof(FacialLayer))));
            foreach (FacialLayer layer in layers) faceContemptLayers.Add(layer, false);
            Scheduler.DelayFlame(() =>
            {
                // Original callback41E720: preserve the capture re-reads and cached receiver before the state read.
                nca.Animator.Rebind();
                Animator targetAnimator = nca.Animator;
                int parameter = IsFemale;
                var avatarType = nca.StateModel.AvatarType;
                targetAnimator.SetBool(parameter, (int)avatarType == 0 || (int)avatarType == 4);
            }, 1);
        }

		public void OnUpdate()
		{
            // Complete original RVA41B080; original speed, state, facial and trace branches.
            speed.UpdateToTarget();
            if (UnityEngine.Mathf.Abs(lastSetSpeed - speed.Current) > 0.001f)
            {
                nca.Animator.SetFloat(Speed, speed.Current);
                lastSetSpeed = speed.Current;
            }
            NpcStateModel currentState = nca.StateModel;
            if (currentState is null) throw new System.NullReferenceException();
            float overrideTarget = currentState.CurrentState == NpcStateModel.State.TurnAround ? 1f : 0f;
            overrideActionLayerWeight.UpdateTo(overrideTarget);
            animator.SetLayerWeight(2, overrideActionLayerWeight.Current);
            animator.SetBool(IsSumahoWalk, nca.StateModel.CurrentState == NpcStateModel.State.Idle
                && nca.StateModel.isSumahoWalk && speed.Current >= 0.5f);
            bool contempt = false;
            if (nca.StateModel.CurrentState != NpcStateModel.State.FoundStranger
                && global::ExposureUnnoticed2.Scripts.InGame.SexManager.Instance.ActiveTraceSexPlayerNpc != nca.NpcController)
            {
                foreach (var layer in faceContemptLayers) contempt = layer.Value || contempt;
            }
            animator.SetBool(IsFaceContempt, contempt);
            Animator finalAnimator = animator;
            int finalParameter = IsFaceNiya;
            bool traceFace = global::ExposureUnnoticed2.Scripts.InGame.SexManager.Instance.ActiveTraceSexPlayerNpc == nca.NpcController
                && !global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.IsEcstasyMotion;
            finalAnimator.SetBool(finalParameter, traceFace);
        }

		public void SetMotionSpeed(float motionScale)
		{
		}

		public void SetFaceIku(bool isFaceIku)
		{
            // Complete original RVA 0x41B730, 133 bytes; capture before class initialization.
            Animator capturedAnimator = animator;
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(NpcAnimationManager).TypeHandle);
            if (capturedAnimator is null) throw new System.NullReferenceException();
            capturedAnimator.SetBool(IsFaceIku, isFaceIku);
        }

		public void IsEcstasy(bool isEcstasy)
		{
            // Complete original RVA 0x41B7C0, 133 bytes; capture before class initialization.
            Animator capturedAnimator = animator;
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(NpcAnimationManager).TypeHandle);
            if (capturedAnimator is null) throw new System.NullReferenceException();
            capturedAnimator.SetBool(Ecstasy, isEcstasy);
        }

		public void TransSex(RSex rSex)
		{
		}

		public void SetStagger(Vector3 direct)
		{// Complete original native rule; original Game helper bodies remain intact.
var capturedAnimator = animator;
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(NpcAnimationManager).TypeHandle);
if (capturedAnimator is null) throw new System.NullReferenceException();
capturedAnimator.SetFloat(StaggerX, direct.x);
animator.SetFloat(StaggerY, direct.z);
animator.SetTrigger(Stagger);
}

		public void ForceExitActionState()
        {
            // Complete original RVA 0x41BB30 and original delayed callback RVA 0x41E660.
            Animator capturedAnimator = animator;
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(NpcAnimationManager).TypeHandle);
            if (capturedAnimator is null) throw new System.NullReferenceException();
            capturedAnimator.SetBool(ExitActionState, true);
            Scheduler.DelayFlame(() =>
            {
                // Original callback reads this.animator at callback execution, not the outer cached receiver.
                Animator callbackAnimator = animator;
                System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(NpcAnimationManager).TypeHandle);
                if (callbackAnimator is null) throw new System.NullReferenceException();
                callbackAnimator.SetBool(ExitActionState, false);
            }, 1);
        }

		public void SetSpeed(float speed)
		{
			this.speed.Target = speed;
		}

		public void SetBuyMachine()
		{
            // Complete original RVA 0x41BC70.
            var capturedAnimator = animator;
            capturedAnimator.SetTrigger(BuyMachine);
		}

		public void SetUsePost()
		{
		}

		public void SetUseWasher()
		{
		}

		public void SetHandOver()
		{
		}

		public void SetMalePeeing(bool isPee)
		{
		}

		public void SetHandWash(bool isWash)
		{
		}

		public void SetStandSmartphone()
		{
		}

		public void SetConbiniTakeGoods()
		{
		}

		public void SetNoAction()
		{
		}

		public void SetWait(bool isWait)
		{
		}

		public void SetOverrideIdle(bool isWait)
		{
			animator.SetBool(OverrideIdle, isWait);
		}

		public void SetIsWorry(bool isWorry)
		{
		}

		public float GetCurrentMoveSpeed()
		{
            return speed.Current;
        }

		public void SetNpcTalk(int id)
		{
		}

		public void SetFoundStranger(bool isFound)
		{
			animator.SetBool(IsFoundStranger, isFound);
		}

		public void SetSitSleep()
		{
            animator.SetTrigger(SitSleep);
        }

		public void SetSleepAwake(bool isAwake)
		{
		}

		public void SetSitBook()
		{
            animator.SetTrigger(SitBook);
        }

		public void SetSitSumaho()
		{
            animator.SetTrigger(SitSumaho);
        }

		public void SetIsSit(bool isSIt)
		{
            animator.SetBool(IsSit, isSIt);
        }

		public void SetChairSitDown()
		{
		}

		public void SetChairStandUp()
		{
		}

		public void SetPinpon(bool isOpen, float animSpeed, bool isMirror)
		{
		}

		public void SetFaceContempt(bool isActive, FacialLayer layer)
        {
            faceContemptLayers[layer] = isActive;
        }

		public void SetConbiniCrouch()
		{
		}

		public void SetConbiniStand()
		{
		}

		public void SetConbiniShinadashi()
		{
		}

		public void SetConbiniRegiScan(bool isScan)
		{
		}

		public void SetOpenToiletDoorFromOutside()
		{
		}

		public void SetOpenToiletDoorFromInside()
		{
		}

		public void SetCloseToiletDoor()
		{
		}
	}
}
