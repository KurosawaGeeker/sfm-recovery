using ExposureUnnoticed2.Master.Mission;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Clothes
{
	public interface IClothesController
	{
		void Initialize(PlayerClassAccessor pca);

		void OnUpdate();

		void OnPick();

		void ForceSetClothesStateBlend(PlayerStateModel.ClothesState state);

		ExposedBodyParts GetExposedBodyParts();

		PlayerStateModel.ClothesState GetNextState(PlayerStateModel.CoatActionType type);

		PlayerStateModel.ClothesState GetPrevState(PlayerStateModel.CoatActionType type);

		bool IsEnoughSkillLevel(PlayerStateModel.CoatActionType type);
	}
}
