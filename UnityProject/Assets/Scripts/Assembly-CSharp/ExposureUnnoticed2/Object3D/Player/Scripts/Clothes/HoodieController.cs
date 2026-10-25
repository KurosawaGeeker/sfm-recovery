using ExposureUnnoticed2.Master.Mission;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Clothes
{
	public class HoodieController : MonoBehaviour, IClothesController
	{
		private SkinnedMeshRenderer clothesMesh;

		private PlayerClassAccessor pca;

		private SmoothFloat clothesMainBlend;

		private SmoothFloat coatSubBlend;

		public void Initialize(PlayerClassAccessor pca)
		{
		}

		public void OnUpdate()
		{
		}

		public ExposedBodyParts GetExposedBodyParts()
		{
			return (ExposedBodyParts)0;
		}

		private float TransAStateToBlend(PlayerStateModel.ClothesState aState)
		{
			return 0f;
		}

		public void OnPick()
		{
		}

		public void ForceSetClothesStateBlend(PlayerStateModel.ClothesState state)
		{
		}

		public PlayerStateModel.ClothesState GetNextState(PlayerStateModel.CoatActionType type)
		{
			return PlayerStateModel.ClothesState.CloseA;
		}

		public PlayerStateModel.ClothesState GetPrevState(PlayerStateModel.CoatActionType type)
		{
			return PlayerStateModel.ClothesState.CloseA;
		}

		public bool IsEnoughSkillLevel(PlayerStateModel.CoatActionType type)
		{
			return false;
		}
	}
}
