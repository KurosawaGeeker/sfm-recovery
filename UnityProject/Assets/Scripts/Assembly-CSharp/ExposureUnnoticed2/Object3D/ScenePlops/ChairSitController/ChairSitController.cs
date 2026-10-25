using ExposureUnnoticed2.Object3D.InteractArea;
using ExposureUnnoticed2.Object3D.RoutePoint;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using HadakaCoat.Objects3D.Dildo;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.ChairSitController
{
	public class ChairSitController : BaseInteractTokenPublisher
	{
		public enum DildoState
		{
			NoDildo = 0,
			PutPussy = 1,
			PutAnal = 2
		}

		[SerializeField]
		private Transform sitHeightPosition;

		[SerializeField]
		private float[] floorHeights;

		[SerializeField]
		private bool ignoreInteractAreaInit;

		[SerializeField]
		private bool ignoreRegisterNpc;

		[SerializeField]
		private bool isInverseStandDirect;

		[SerializeField]
		private Transform ridePosition;

		[SerializeField]
		private PlayerEnterAreaChecker playerEnterAreaChecker;

		[SerializeField]
		public Transform DildoParent;

		[SerializeField]
		private float dildoParentZOffset;

		private SmoothFloat dildoPositionBlend;

		private int isRidePlayerRemainFlame;

		public DildoController DildoController;

		[field: SerializeField]
		public InteractAreaController interactAreaController { get; private set; }

		[field: SerializeField]
		public ExposureUnnoticed2.Object3D.RoutePoint.RoutePoint routePoint { get; private set; }

		public Transform InteractPoint => null;

		public DildoState CurrentDildoState { get; set; }

		public new bool TryGetToken()
		{
			return false;
		}

		public new void ReturnToken()
		{
		}

		public void RegisterDildo(DildoController dildoController)
		{
		}

		public void RemoveDildo()
		{
		}

		public void MoveDildo(DildoState state)
		{
		}

		private void Update()
		{
		}

		private new void Awake()
		{
		}

		private void Start()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnChangeTimeB(ChangeTimeEvent evt)
		{
		}

		public Vector3 GetNpcPosition()
		{
			return default;
		}

		public Vector3 GetNpcOffset()
		{
			return default;
		}
	}
}
