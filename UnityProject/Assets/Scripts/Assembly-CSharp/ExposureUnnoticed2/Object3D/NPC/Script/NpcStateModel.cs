using System.Collections.Generic;
using ExposureUnnoticed2.Object3D.ForeignObject;
using ExposureUnnoticed2.Object3D.NPC.Script.State;
using ExposureUnnoticed2.Object3D.RoutePoint;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcStateModel
	{
		public enum State
		{
			Idle = 0,
			FoundStranger = 1,
			TurnAround = 2,
			FadeOut = 3,
			Stagger = 4,
			Interact = 5,
			WaitToken = 6,
			TraceSexPlayer = 7
		}

		public enum FixedType
		{
			None = 0,
			Car = 1,
			Talk = 2,
			SitWork = 3,
			SitTalk = 4,
			SitSleep = 5,
			SitBook = 6,
			SitSumaho = 7,
			StandSumaho = 8,
			Pinpon = 9,
			Conbini = 10
		}

		public enum ForeignObjectLookerType
		{
			Idle = 0,
			InFacial = 1,
			CoolDown = 2
		}

		public State CurrentState;

		public State BeforeState;

		public RouteInteractType LastRouteInteractType;

		public bool IsComeBackInteractFromFoundStranger;

		public bool IsAlreadyFoundStranger;

		public bool IsPolice;

		public bool IsSit;

		public bool IsSitInverse;

		public bool IsDestroy;

		public bool isFemale;

		public bool IsSkipUpdateStrangeness = true;

		public float IgnorePlayerNoisyTime;

		public float stateInTime;

		public float MoveSpeedScale;

		public bool IsRiseStrangeFromOnlyDiscomfort;

		public NpcController NearNpc;

		public RouteData RouteData;

		public bool HasInteractToken;

		public NpcAvatarComponent.AvatarType AvatarType;

		public State StaggerBeforeState;

		public bool IsHeadSet;

		public bool IsGlassees;

		public FixedType CurrentFixedType;

		public float AvatarSizeScale;

		public float StrangeAddScaleSleep = 1f;

		public RouteInteractType CurrentRouteInteractType;

		public List<ForeignObjectController> InSightForeignObjectList = new List<ForeignObjectController>();

		public ForeignObjectLookerType CurrentForeignObjectLookerType;

		public HashSet<GameObject> AlreadyLookedForeignObjectSet = new HashSet<GameObject>();

		public NpcChairSubState.ChairActionType CurrentChairActionType;

		public bool IsHidePinponNpc;

		public bool IsSkipRiseStrangenessPinponNpc;

		public int RegiScanRemainNum;

		public bool ConbiniEndShinadashi;

		public int ConbiniBringProductNum;

		public SmoothFloat SoundCancelAreaScale = new SmoothFloat(1f);

		public bool IsJustHitSpermOnSkin;

		public Quaternion initTalkRotation;

		public bool IgnoreSpermTurn;

		public Vector3? turnAroundPointInTimeStopSperm;

		public bool isSumahoWalk;

		public float TurnNoisyLevel;

		public float StrangeLookAt;

		public bool IsActiveSpermCollider;

		public float PlayerDist = 99f;

		public Vector3 CurrentPosition;

		public bool IsJustOnEndInteract;

		public bool IsJustOnEndChairSitDown;

		public void OnUpdateReset()
		{
		}
	}
}
