using ExposureUnnoticed2.Object3D.Camera;
using ExposureUnnoticed2.Object3D.Player.Scripts.Clothes;
using ExposureUnnoticed2.Object3D.Player.Scripts.Costume;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerAvatarObjectReferencer : MonoBehaviour
	{
		[SerializeField]
		[Header("雑")]
		public Transform StrangeSightPoint;

		[SerializeField]
		public Transform AnalParent;

		[SerializeField]
		public Transform DildoTargetPussy;

		[SerializeField]
		public Transform DildoTargetFella;

		[SerializeField]
		public Transform DildoTargetAnal;

		[SerializeField]
		public Transform HoldIdParent;

		[SerializeField]
		public ParticleSystem PeeParticle;

		[SerializeField]
		public AudioSource PickingAudioSource;

		[SerializeField]
		public HandcuffsMapChecker HandcuffsMapChecker;

		[SerializeField]
		public Transform CameraTarget;

		[SerializeField]
		public Transform FaceCameraTarget;

		[SerializeField]
		public FPCameraController FPCameraController;

		[SerializeField]
		public Transform ToeL;

		[SerializeField]
		public Transform ToeR;

		[SerializeField]
		public Transform FootL;

		[SerializeField]
		public Transform FootR;

		[SerializeField]
		public Transform LegL;

		[SerializeField]
		public Transform LegR;

		[SerializeField]
		public Transform LowerLegL;

		[SerializeField]
		public Transform LowerLegR;

		[SerializeField]
		public Transform Head;

		[SerializeField]
		public Transform Neck;

		[SerializeField]
		public Transform BreastRotateL;

		[SerializeField]
		public Transform BreastRotateR;

		[SerializeField]
		public Transform ButtL;

		[SerializeField]
		public Transform ButtR;

		[SerializeField]
		public Transform Spine;

		[SerializeField]
		public Transform Chest;

		[SerializeField]
		public Transform ShoulderL;

		[SerializeField]
		public Transform ShoulderR;

		[SerializeField]
		public Transform UpperArmL;

		[SerializeField]
		public Transform UpperArmR;

		[SerializeField]
		public Transform LowerArmL;

		[SerializeField]
		public Transform LowerArmR;

		[SerializeField]
		public Transform ThumbR;

		[SerializeField]
		public Transform Hip;

		[SerializeField]
		public DynamicBone BreastDBL;

		[SerializeField]
		public DynamicBone BreastDBR;

		[SerializeField]
		public DynamicBone HipDB;

		[SerializeField]
		public Transform FPCameraTarget;

		[SerializeField]
		public ParticleSystem BreathParticle;

		[SerializeField]
		public ParticleSystem ShiofukiParticle;

		[SerializeField]
		public Transform FaceCameraPosiiton;

		[SerializeField]
		public GameObject CensorPussy;

		[SerializeField]
		public GameObject CensorAnal;

		[SerializeField]
		public GameObject RubberSuitDummy;

		[SerializeField]
		public InmouController InmouController;

		[SerializeField]
		[Header("ふたなり")]
		public GameObject FutanariObject;

		[SerializeField]
		public Transform FutanariRoot;

		[SerializeField]
		public Transform FutanariSaoParent;

		[SerializeField]
		public Transform FutanariTamaParent;

		[SerializeField]
		public SkinnedMeshRenderer FutanariMeshSao;

		[SerializeField]
		public SkinnedMeshRenderer FutanariMeshTama;

		[SerializeField]
		public Transform PeeParent;

		[SerializeField]
		public Transform FutanariDirectTarget;

		[SerializeField]
		public Transform FutanariDirectBone;

		[SerializeField]
		public Transform FutanariLockAtDirect;

		[SerializeField]
		public Transform FutanariDefaultDirect;

		[SerializeField]
		public ParticleSystem SpermParticle;

		[SerializeField]
		[Header("雑オブジェクト")]
		public GameObject Handcuffs;

		[SerializeField]
		public GameObject BottleWater;

		[SerializeField]
		public SkinnedMeshRenderer bodyMeshRenderer;

		[SerializeField]
		public SkinnedMeshRenderer OverKneeSockMesh;

		[SerializeField]
		public SkinnedMeshRenderer FaceMesh;

		[SerializeField]
		public GameObject Face;

		[SerializeField]
		public GameObject BasketObject;

		[SerializeField]
		public GameObject VibratorRemocon;

		[Header("服")]
		[SerializeField]
		public GameObject ClothesParent;

		[SerializeField]
		public GameObject Coat;

		[SerializeField]
		public GameObject Shoes;

		[SerializeField]
		public CoatController CoatController;

		[Header("アクセサリー")]
		[SerializeField]
		public GameObject[] Accessorys;

		[SerializeField]
		public GameObject PantsParent;

		[SerializeField]
		public GameObject BraParent;

		[SerializeField]
		public GameObject[] Panties;

		[SerializeField]
		[Header("髪")]
		public GameObject HairParent;

		[SerializeField]
		public GameObject[] Hairs;

		[Header("コスチューム")]
		[SerializeField]
		public GameObject[] Costumes;

		[SerializeField]
		[Header("コスプレ（CosplayType順）")]
		public GameObject CosplayParent;

		[SerializeField]
		public CosplayController[] CosplayControllers;

		[SerializeField]
		public GameObject[] OtherCosplatPartForPRSC;

		[Header("オンリー影対象オブジェクト")]
		[SerializeField]
		public GameObject[] OnlyShadowCloneObjects;
	}
}
