using System.Collections.Generic;
using ExposureUnnoticed2.Master.Clothes;
using ExposureUnnoticed2.Master.Mission;
using ExposureUnnoticed2.Object3D.Player.Scripts.Clothes;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerClothes
	{
		private PlayerClassAccessor pca;

		private static readonly int ClothesId = Animator.StringToHash("ClothesId");

		private static readonly int ChangeClothes = Animator.StringToHash("ChangeClothes");

		private List<IClothesController> clothes;

		private static readonly int DirectAfterPick = Animator.StringToHash("DirectAfterPick");

		public PlayerStateModel.ClothesState TargetClothStateBBlend { get; set; }

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x63FFB0
			this.pca = pca;
			EventManager.AddListener<ChangeEquipmentEvent>(OnChangeClothes, false);
			clothes.Add(pca.PlayerAvatarObjectReferencer.Coat.GetComponent<IClothesController>());
			foreach (IClothesController controller in clothes)
			    controller.Initialize(pca);
			if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothes == ClothesType.None)
			{
			    this.pca.PlayerAvatarObjectReferencer.Coat.SetActive(false);
			    global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState = PlayerStateModel.ClothesState.DropClothes;
			    pca.PlayerAnimationManager.DirectDropClothes();
			}
			else if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothes != ClothesType.Coat)
			{
			    this.pca.PlayerAvatarObjectReferencer.Coat.SetActive(false);
			    GameObject mesh = pca.PlayerClothes.TransMeshReference(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothes);
			    if (mesh is not null) mesh.SetActive(true);
			    pca.Animator.SetInteger(ClothesId, (int)pca.PlayerState.CurrentClothes.ClothesType);
			    pca.Animator.SetTrigger(ChangeClothes);
			}
			if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DropDataDict[(global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.DropItemType)3].IsDropped)
			    pca.PlayerAvatarObjectReferencer.PantsParent.SetActive(false);
			if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DropDataDict[(global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.DropItemType)4].IsDropped)
			    pca.PlayerAvatarObjectReferencer.BraParent.SetActive(false);
		}

		public void Dispose()
		{
			// Recovered from GameAssembly.dll RVA 0x640930
			EventManager.RemoveListener<ChangeEquipmentEvent>(OnChangeClothes);
		}

		public void OnUpdate()
		{
			// SOURCE DRAFT: original6409F0 and caller's interface slot1.
			var selected = TransController(pca.PlayerState.CurrentClothes.ClothesType);
			if (selected is not null) selected.OnUpdate();
		}

		private void OnChangeClothes(ChangeEquipmentEvent evt)
		{
		}

		private void ChangeTryClothes(ChangeEquipmentEvent evt)
		{
		}

		public void DropClothes(Transform point = null)
		{
// Complete original 0x641430; external calls can change the accessor graph.
            if (!pca.PlayerState.SkipDropClothes)
            {
                Vector3 position = pca.Transform.position;
                Quaternion rotation = pca.Transform.rotation;
                if (point)
                {
                    position = point.position;
                    rotation = point.rotation;
                }
                var accessor = pca;
                var type = global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.ToDropItemType(accessor.PlayerState.CurrentClothes.ClothesType);
                var data = accessor.DroppedItemManager.Drop(position, rotation, type);
                global::ExposureUnnoticed2.ObjectUI.InGame.Compass.CompassView.Instance.RegisterTarget(data);
            }
            pca.PlayerState.SkipDropClothes = false;
            GameObject mesh = TransMeshReference(pca.PlayerState.CurrentClothes.ClothesType);
            if (mesh is not null) mesh.SetActive(false);
            var state = pca.PlayerState;
            state.CurrentClothes = MClothes.Get(ClothesType.None);
            var gameState = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
            gameState.PlayerCurrentClothes = pca.PlayerState.CurrentClothes.ClothesType;
            pca.PlayerAnimationManager.TargetArmLayerWeight = 0f;
            if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesBState == PlayerStateModel.ClothesState.OpenB1)
                pca.PlayerAnimationManager.DirectNoHip();
            Scheduler.DelayFlame(() =>
            {
                // Original callback RVA 0x642610; use then-current state, not captured GSD.
                var targetState = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
                pca.PlayerState.CurrentClothesBStateCache = PlayerStateModel.ClothesState.CloseB;
                targetState.PlayerCurrentClothesBState = PlayerStateModel.ClothesState.CloseB;
                pca.PlayerClothes.TargetClothStateBBlend = PlayerStateModel.ClothesState.CloseB;
            }, 1);
        }

		public void PickClothes()
		{
// Complete original 0x641A90. These state/renderer/animation writes are ordered.
            var gameState = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
            gameState.PlayerCurrentClothes = pca.PlayerState.LastPickClothesType;
            GameObject mesh = pca.PlayerClothes.TransMeshReference(pca.PlayerState.LastPickClothesType);
            if (mesh is not null) mesh.SetActive(true);
            var state = pca.PlayerState;
            state.CurrentClothes = MClothes.Get(state.LastPickClothesType);
            var accessor = pca;
            var type = global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.ToDropItemType(accessor.PlayerState.LastPickClothesType);
            accessor.DroppedItemManager.Collect(type);
            pca.PlayerClothesAction.OnCompletePickClothes();
            pca.PlayerAnimationManager.TargetArmLayerWeight = 1f;
            gameState = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
            gameState.PlayerCurrentClothesAState = pca.PlayerState.CurrentClothes.ClothesStateAAfterPick;
            gameState = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
            pca.PlayerState.CurrentClothesBStateCache = PlayerStateModel.ClothesState.CloseB;
            gameState.PlayerCurrentClothesBState = PlayerStateModel.ClothesState.CloseB;
            pca.Animator.SetTrigger(DirectAfterPick);
            pca.Animator.SetInteger(ClothesId, (int)pca.PlayerState.CurrentClothes.ClothesType);
            pca.Animator.SetTrigger(ChangeClothes);
            var controller = TransController(pca.PlayerState.CurrentClothes.ClothesType);
            if (controller is not null) controller.OnPick();
        }

		private GameObject TransMeshReference(ClothesType type)
		{
			// Recovered from GameAssembly.dll RVA 0x641F90
			return type switch
			{
			    ClothesType.None => null,
			    ClothesType.Coat => pca.PlayerAvatarObjectReferencer.Coat,
			    _ => throw new System.Runtime.CompilerServices.SwitchExpressionException(type),
			};
		}

		public bool IsEnoughSkillLevel(PlayerStateModel.CoatActionType type)
		{
            if (pca.PlayerState.CurrentClothes.ClothesType == ClothesType.None) return false;
            return TransController(pca.PlayerState.CurrentClothes.ClothesType).IsEnoughSkillLevel(type);
        }

		private IClothesController TransController(ClothesType type)
		{
			// Recovered from GameAssembly.dll RVA 0x6420D0
			return type == ClothesType.None ? null : clothes[(int)type];
		}

		public PlayerStateModel.ClothesState GetNextState(PlayerStateModel.CoatActionType type)
		{
            return TransController(pca.PlayerState.CurrentClothes.ClothesType).GetNextState(type);
        }

		public PlayerStateModel.ClothesState GetPrevState(PlayerStateModel.CoatActionType type)
		{
            return TransController(pca.PlayerState.CurrentClothes.ClothesType).GetPrevState(type);
        }

		public ExposedBodyParts GetExposedBodyParts()
		{
			// Recovered from GameAssembly.dll RVA 0x642280
			IClothesController controller = TransController(pca.PlayerState.CurrentClothes.ClothesType);
			ExposedBodyParts result = controller == null ? ExposedBodyParts.All : controller.GetExposedBodyParts();
			// Native inlined extension helper retains its declaring-type initialization before the final state check.
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ExternalMethodManager).TypeHandle);
			if (pca.PlayerState is null) throw new System.NullReferenceException();
			return result;
		}

		public void ForceSetClothesStateBlend(PlayerStateModel.ClothesState state)
		{
		}
	// Recovered original member RVA 0x6423E0: .ctor
		public PlayerClothes()
        {
            TargetClothStateBBlend = (PlayerStateModel.ClothesState)10;
            clothes = new List<IClothesController>();
        }
	// Recovered original member RVA 0x6424A0: .cctor
		// Recovered original .cctor RVA 0x6424A0: ordered field initializers preserve BeforeFieldInit.
	}
}
