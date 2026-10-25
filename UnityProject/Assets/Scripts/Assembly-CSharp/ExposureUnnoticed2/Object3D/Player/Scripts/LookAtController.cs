using System.Collections.Generic;
using ExposureUnnoticed2.Object3D.NPC.Script;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class LookAtController
	{
		public enum LookAtLayer
		{
			Base = 0,
			NpcTalkTarget = 1,
			NpcForeignObject = 2,
			NpcLookPlayer = 3,
			LookNpc = 4,
			Strafe = 5,
			Closet = 6,
			Bareta = 7,
			FoundStranger = 8
		}

		private class LookAtModel
		{
			public Transform Target;

			public bool IsForceLook;

			public bool IsLookTarget;

			public LookAtLayer Layer;

			public LookAtModel(LookAtLayer layer)
			{
			// Recovered from GameAssembly.dll RVA 0x469E10
			Layer = layer;
		}
		}

		public Transform Head;

		private Transform parentCharacterTransform;

		private Transform lookCameraHead;

		private Transform lookCameraTarget;

		private PlayerClassAccessor pca;

		private NpcComponentAccessor nca;

		private bool isPlayer;

		private int layerMask;

		private int layerMaskLookNpc;

		private Dictionary<LookAtLayer, LookAtModel> lookAtDict = new Dictionary<LookAtLayer, LookAtModel>();

		public void EnableLookAt(LookAtLayer layer, Transform target, bool isForceTarget = false)
		{
			// Recovered from GameAssembly.dll RVA 0x621110
			LookAtModel model = lookAtDict[layer];
			model.Target = target;
			model.IsForceLook = isForceTarget;
			model.IsLookTarget = true;
		}

		public void DisableLookAt(LookAtLayer layer)
		{
			// Recovered from GameAssembly.dll RVA 0x6211F0
			LookAtModel model = lookAtDict[layer];
			model.Target = null;
			model.IsForceLook = false;
			model.IsLookTarget = false;
		}

		public void DisableAll()
		{
			// Recovered from GameAssembly.dll RVA 0x6212B0
			foreach (var pair in lookAtDict)
			    pair.Value.IsLookTarget = false;
		}

		public bool IsEnable(LookAtLayer layer)
		{
			// Recovered from GameAssembly.dll RVA 0x621430
			return lookAtDict[layer].IsLookTarget;
		}

		public void InitializeForPlayer(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x6214A0
			InitializeLookAtDict();
			this.pca = pca;
			parentCharacterTransform = pca.PlayerAvatarObjectReferencer.Chest;
			isPlayer = true;
			Head = pca.PlayerAvatarObjectReferencer.Head;
			GameObject head = new GameObject("lookCameraHead");
			head.transform.parent = Head;
			lookCameraHead = head.transform;
			lookCameraHead.localPosition = Vector3.zero;
			lookCameraHead.localRotation = Quaternion.identity;
			GameObject target = new GameObject("lookCameraTarget");
			target.transform.parent = Head;
			lookCameraTarget = target.transform;
			lookCameraTarget.localPosition = Vector3.zero;
			lookCameraTarget.localRotation = Quaternion.identity;
			layerMask = (1 << LayerMask.NameToLayer("Npc")) | (1 << LayerMask.NameToLayer("Coat")) | (1 << global::ExposureUnnoticed2.Scripts.Base.LayerAccessor.SpermCollider);
			layerMaskLookNpc = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("PlayerFace")) | (1 << LayerMask.NameToLayer("Coat")) | (1 << global::ExposureUnnoticed2.Scripts.Base.LayerAccessor.SpermCollider);
		}

		public void InitializeForNpc(NpcComponentAccessor nca)
		{
			// Recovered from GameAssembly.dll RVA 0x621D40
			InitializeLookAtDict();
			this.nca = nca;
			parentCharacterTransform = nca.Transform;
			isPlayer = false;
			Head = nca.AvatarComponent.Head;
			if (Head == null) Head = SearchHead(nca.NpcComponent.AvaterObject.transform);
			GameObject head = new GameObject("lookCameraHead");
			head.transform.parent = Head;
			lookCameraHead = head.transform;
			lookCameraHead.localPosition = Vector3.zero;
			lookCameraHead.localRotation = Quaternion.identity;
			GameObject target = new GameObject("lookCameraTarget");
			target.transform.parent = Head;
			lookCameraTarget = target.transform;
			lookCameraTarget.localPosition = Vector3.zero;
			lookCameraTarget.localRotation = Quaternion.identity;
			layerMask = (1 << LayerMask.NameToLayer("Npc")) | (1 << LayerMask.NameToLayer("Coat")) | (1 << global::ExposureUnnoticed2.Scripts.Base.LayerAccessor.SpermCollider);
			layerMaskLookNpc = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("PlayerFace")) | (1 << LayerMask.NameToLayer("Coat")) | (1 << global::ExposureUnnoticed2.Scripts.Base.LayerAccessor.SpermCollider);
		}

		private void InitializeLookAtDict()
		{
			// Recovered from GameAssembly.dll RVA 0x622750
			foreach (LookAtLayer layer in System.Linq.Enumerable.ToList(System.Linq.Enumerable.Cast<LookAtLayer>(System.Enum.GetValues(typeof(LookAtLayer)))))
			    lookAtDict.Add(layer, new LookAtModel(layer));
		}

		public void OnLateUpdate()
		{
			// Recovered from GameAssembly.dll RVA 0x622BA0
			if (isPlayer) UpdateForPlayer();
			else UpdateForNpc();
		}

		private void UpdateForPlayer()
		{
			// Recovered from GameAssembly.dll RVA 0x622C30
			LookAtModel model = GetTargetModel();
			UpdatePlayerLookAt();
			if (pca.PlayerState.CurrentState == PlayerStateModel.PlayerState.Bareta && !pca.PlayerBaretaState.IsOnlyHeadReaction) return;
			UpdateCommon(model);
		}

		private void UpdateForNpc()
		{
			// Recovered from GameAssembly.dll RVA 0x622CA0
			UpdateCommon(GetTargetModel());
		}

		private LookAtModel GetTargetModel()
		{
			// Recovered from GameAssembly.dll RVA 0x622CD0
			LookAtModel target = lookAtDict[LookAtLayer.Base];
			foreach (var pair in lookAtDict)
			    if (pair.Value.IsLookTarget) target = pair.Value;
			return target;
		}

		private void UpdateCommon(LookAtModel model)
		{
		}

		private void UpdatePlayerLookAt()
		{
		}

		private Transform SearchHead(Transform start)
		{
			// Recovered from GameAssembly.dll RVA 0x624120
			List<Transform> current = new List<Transform>();
			List<Transform> next = new List<Transform>();
			current.Add(start);
			while (current.Count > 0)
			{
			    next.Clear();
			    foreach (Transform parent in current)
			    {
			        foreach (Transform child in parent)
			        {
			            if (child.name == "Head") return child;
			            next.Add(child);
			        }
			    }
			    List<Transform> swap = current;
			    current = next;
			    next = swap;
			}
			return null;
		}
	}
}
