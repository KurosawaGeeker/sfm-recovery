using AkilliMum.Standard.Mirror;
using ExposureUnnoticed2.Object3D.Camera;
using ExposureUnnoticed2.Object3D.IngameManager;
using ExposureUnnoticed2.Object3D.NPC.Script;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Object3D.UIPlayer;
using UnityEngine;
using UnityEngine.AI;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class InGameReferencer : MonoBehaviour
	{
		[SerializeField]
		public InGameManager inGameManager;

		[field: SerializeField]
		public InGameUiManager InGameUiManager { get; private set; }

		[field: SerializeField]
		public PlayerController Player { get; set; }

		[field: SerializeField]
		public BaseCameraController BaseCameraController { get; private set; }

		[field: SerializeField]
		public TPCameraController TpCameraController { get; private set; }

		[field: SerializeField]
		public InventoryCameraController InventoryCameraController { get; private set; }

		[field: SerializeField]
		public NpcManager NpcManager { get; private set; }

		[field: SerializeField]
		public VolumeController GlobalVolume { get; private set; }

		[field: SerializeField]
		public GameOverController GameOverController { get; private set; }

		[field: SerializeField]
		public NavMeshAgent ClothesPathSearchAgent { get; private set; }

		[field: SerializeField]
		public SlowAssistManager SlowAssistManager { get; private set; }

		[field: SerializeField]
		public UiPlayerController UiPlayerController { get; private set; }

		[field: SerializeField]
		public UiOptionApplyer UiOptionApplyer { get; private set; }

		[field: SerializeField]
		public InGameGraphicsOptionApplyer InGameGraphicsOptionApplyer { get; private set; }

		[field: SerializeField]
		public GameObject SelfCameraParent { get; private set; }

		[field: SerializeField]
		public FaceCameraController FaceCameraController { get; private set; }

		[field: SerializeField]
		public GameObject BodyCameraObject { get; private set; }

		public InGameManager InGameManager => inGameManager;

		public MirrorManager MirrorManager { get; private set; }

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x4990B0
			MirrorManager = BaseCameraController.GetComponent<AkilliMum.Standard.Mirror.MirrorManager>();
		}
	}
}
