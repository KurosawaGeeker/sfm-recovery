using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcAvatarComponent : MonoBehaviour
	{
		public enum AvatarType
		{
			NewFemale = 0,
			NewMale = 1,
			NewOji = 2,
			NewOba = 3,
			PreviousFemale = 4,
			PreviousMale = 5
		}

		[SerializeField]
		public GameObject MeshParent;

		[SerializeField]
		public Transform Head;

		[SerializeField]
		public Transform Eye;

		[SerializeField]
		public Transform HandL;

		[SerializeField]
		public Transform HandR;

		[SerializeField]
		public Renderer[] MeshBodys;

		[SerializeField]
		public Renderer[] MeshHairs;

		[SerializeField]
		public Renderer[] MeshFaces;

		[SerializeField]
		public Renderer MeshGlasses;

		[SerializeField]
		public Renderer PoliceBody;

		[SerializeField]
		public Renderer ConbiniBody;

		[SerializeField]
		public Renderer Headset;

		[SerializeField]
		public Renderer ConbiniScanner;

		[SerializeField]
		public GameObject MeshChipo;

		[SerializeField]
		public GameObject MeshChipoSex;

		[SerializeField]
		public SkinnedMeshRenderer[] MeshChinpoSexRenderers;

		[SerializeField]
		public SkinnedMeshRenderer[] previousMaleBodyRenderers;

		[SerializeField]
		public GameObject Peeticle;

		[SerializeField]
		public GameObject[] HairTransformParents;

		[SerializeField]
		public GameObject[] SpermColliders;

		[SerializeField]
		private GameObject longHairSpermCollider;

		[SerializeField]
		private GameObject policeCap;

		public ParticleSystem PeeParticleSystem;

		public Dictionary<SkinnedMeshRenderer, Material[]> previousMaleMaterialDict = new Dictionary<SkinnedMeshRenderer, Material[]>();

		public Dictionary<SkinnedMeshRenderer, Material[]> fresnelTargetDict = new Dictionary<SkinnedMeshRenderer, Material[]>();

		public int bodyIndex;

		public int hairIndex;

		private int faceIndex;

		public NpcComponentAccessor nca;

		public bool IsVisible => BodyRenderer.isVisible;

		private Renderer BodyRenderer { get; set; }

		private Renderer HairRenderer { get; set; }

		private Renderer FaceRenderer { get; set; }

		public Material bodyInitMaterial { get; private set; }

		public Material hairInitMaterial { get; private set; }

		public Material faceInitMaterial { get; private set; }

		public Material headsetInitMaterial { get; private set; }

		public Material glassesInitMaterial { get; private set; }

		public void Initialize(NpcComponentAccessor nca)
		{
            this.nca = nca;
            if (nca.AvatarComponent.ConbiniScanner != null) nca.AvatarComponent.ConbiniScanner.gameObject.SetActive(false);
            nca.StateModel.AvatarSizeScale = (transform.localScale.x + UnityEngine.Random.value * 0.1f) - 0.05f;
            if (nca.StateModel.CurrentFixedType == NpcStateModel.FixedType.Conbini || nca.StateModel.CurrentFixedType == NpcStateModel.FixedType.Pinpon)
                nca.StateModel.AvatarSizeScale = 1f;
            else UnityEngine.Object.Destroy(ConbiniScanner);
            transform.localScale = Vector3.one * nca.StateModel.AvatarSizeScale;
            nca.StateModel.IsHeadSet = (double)UnityEngine.Random.value >= 0.9;
            if (nca.StateModel.CurrentFixedType == NpcStateModel.FixedType.Conbini) nca.StateModel.IsHeadSet = false;
            var avatarType = nca.StateModel.AvatarType;
            if (avatarType == AvatarType.NewFemale || avatarType == AvatarType.NewMale) InitializeNewYoung();
            else if (avatarType == AvatarType.NewOji || avatarType == AvatarType.NewOba) InitializeNewOld();
            else if (avatarType == AvatarType.PreviousFemale || avatarType == AvatarType.PreviousMale) InitializePrevious();
            if (Peeticle) PeeParticleSystem = Peeticle.GetComponent<ParticleSystem>();
            nca.NpcComponent.Book.SetParent(nca.AvatarComponent.HandL, false);
            nca.NpcComponent.Smartphone.SetParent(nca.AvatarComponent.HandR, false);
            if (MeshChipo) MeshChipo.SetActive(false);
            if (MeshChipoSex)
            {
                MeshChipoSex.SetActive(false);
                foreach (SkinnedMeshRenderer target in MeshChinpoSexRenderers) fresnelTargetDict.Add(target, target.materials);
            }
            if (Peeticle) Peeticle.SetActive(false);
            if (BodyRenderer)
            {
                BodyRenderer.receiveShadows = ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime;
                bodyInitMaterial = BodyRenderer.material;
            }
            if (HairRenderer)
            {
                HairRenderer.receiveShadows = ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime;
                hairInitMaterial = HairRenderer.material;
            }
            if (FaceRenderer)
            {
                FaceRenderer.receiveShadows = ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime;
                faceInitMaterial = FaceRenderer.material;
            }
            if (MeshGlasses)
            {
                MeshGlasses.receiveShadows = ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime;
                glassesInitMaterial = MeshGlasses.material;
            }
            if (Headset)
            {
                Headset.receiveShadows = ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime;
                headsetInitMaterial = Headset.material;
            }
            ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() =>
            {
                foreach (SkinnedMeshRenderer target in previousMaleBodyRenderers)
                    if (target) previousMaleMaterialDict.Add(target, target.materials);
            }, 1);
        }

		public void ChangeFresnelMaterial(bool isFresnel)
		{
		}

		private void InitializeNewYoung()
		{
            bodyIndex = UnityEngine.Random.Range(0, MeshBodys.Length - (nca.StateModel.AvatarType == AvatarType.NewFemale ? 1 : 0));
            hairIndex = UnityEngine.Random.Range(0, MeshHairs.Length);
            faceIndex = UnityEngine.Random.Range(0, MeshFaces.Length);
            for (int i = 0; i < MeshBodys.Length; ++i)
                if (i != bodyIndex) UnityEngine.Object.Destroy(MeshBodys[i].gameObject);
            for (int i = 0; i < MeshHairs.Length; ++i)
            {
                if (i != hairIndex)
                {
                    UnityEngine.Object.Destroy(MeshHairs[i].gameObject);
                    if (HairTransformParents[i]) UnityEngine.Object.Destroy(HairTransformParents[i]);
                }
            }
            for (int i = 0; i < MeshFaces.Length; ++i)
                if (i != faceIndex) UnityEngine.Object.Destroy(MeshFaces[i].gameObject);
            if (nca.StateModel.AvatarType == AvatarType.NewFemale)
            {
                if (bodyIndex == 10)
                {
                    nca.StateModel.IsPolice = true;
                    nca.StateModel.IsHeadSet = false;
                }
                policeCap.SetActive(nca.StateModel.IsPolice);
            }
            BodyRenderer = MeshBodys[bodyIndex].GetComponent<Renderer>();
            HairRenderer = MeshHairs[hairIndex].GetComponent<Renderer>();
            FaceRenderer = MeshFaces[faceIndex].GetComponent<Renderer>();
            nca.StateModel.IsGlassees = UnityEngine.Random.value >= 0.8f;
            MeshGlasses.gameObject.SetActive(nca.StateModel.IsGlassees);
            Headset.gameObject.SetActive(nca.StateModel.IsHeadSet);
            if (nca.StateModel.CurrentFixedType == NpcStateModel.FixedType.Conbini)
            {
                UnityEngine.Object.Destroy(BodyRenderer, 0f);
                BodyRenderer = ConbiniBody;
            }
            else UnityEngine.Object.Destroy(ConbiniBody, 0f);
            if (longHairSpermCollider && hairIndex >= 2) longHairSpermCollider.SetActive(false);
        }

		private void InitializeNewOld()
		{
            nca.StateModel.IsHeadSet = false;
            BodyRenderer = MeshBodys[0].GetComponent<Renderer>();
            FaceRenderer = MeshFaces[0].GetComponent<Renderer>();
        }

		private void InitializePrevious()
		{
            bodyIndex = UnityEngine.Random.Range(0, MeshBodys.Length);
            hairIndex = UnityEngine.Random.Range(0, MeshHairs.Length);
            for (int i = 0; i < MeshBodys.Length; ++i)
                if (i != bodyIndex) UnityEngine.Object.Destroy(MeshBodys[i].gameObject);
            for (int i = 0; i < MeshHairs.Length; ++i)
                if (i != hairIndex) UnityEngine.Object.Destroy(MeshHairs[i].gameObject);
            BodyRenderer = MeshBodys[bodyIndex].GetComponent<Renderer>();
            HairRenderer = MeshHairs[hairIndex].GetComponent<Renderer>();
            Headset.gameObject.SetActive(nca.StateModel.IsHeadSet);
        }

		public void AddTargetRenderer(List<Material> list)
		{
			int avatarType = (int)nca.StateModel.AvatarType;
			if (avatarType == 0 || avatarType == 1)
			{
			    list.Add(nca.AvatarComponent.HairRenderer.material);
			    list.Add(nca.AvatarComponent.BodyRenderer.material);
			    list.Add(nca.AvatarComponent.FaceRenderer.material);
			    list.Add(nca.AvatarComponent.MeshGlasses.material);
			    return;
			}
			avatarType = (int)nca.StateModel.AvatarType;
			if (avatarType == 3 || avatarType == 2)
			{
			    list.Add(nca.AvatarComponent.BodyRenderer.material);
			    list.Add(nca.AvatarComponent.FaceRenderer.material);
			    return;
			}
			avatarType = (int)nca.StateModel.AvatarType;
			if (avatarType != 4 && avatarType != 5) return;
			list.Add(nca.AvatarComponent.HairRenderer.material);
			foreach (Material material in nca.AvatarComponent.BodyRenderer.materials)
			    list.Add(material);
			foreach (Renderer renderer in BodyRenderer.gameObject.GetComponentsInChildren<Renderer>())
			    foreach (Material material in renderer.materials)
			        list.Add(material);
		}

		private void OnAnimatorMove()
		{
		}
	}
}
