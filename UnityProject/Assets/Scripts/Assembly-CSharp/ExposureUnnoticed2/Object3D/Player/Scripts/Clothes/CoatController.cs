using ExposureUnnoticed2.Master.Mission;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Clothes
{
	public class CoatController : MonoBehaviour, IClothesController
	{
		private SkinnedMeshRenderer coatMesh;

		private PlayerClassAccessor pca;

		private SmoothFloat coatMainBlend = new SmoothFloat(0.2f, 100f, 0f, false, false, -1f);

		public SmoothFloat coatSubBlend = new SmoothFloat(0.2f, 100f, 0f, false, false, -1f);

		private SmoothFloat coatBigBreastBlendScale = new SmoothFloat(0.2f, 1f, 0f, false, false, -1f);

		private SmoothFloat gamanBaibu = new SmoothFloat(0.2f, 100f, 0f, false, false, -1f);

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x4133B0
			this.pca = pca;
			coatMesh = GetComponent<SkinnedMeshRenderer>();
		}

		public void OnUpdate()
		{
            float targetA = TransAStateToBlend(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState);
            var main = coatMainBlend;
            float scale = TimeScaleManager.Instance.GetCurrentScale;
            main.SetSecond(scale * 0.2f);
            coatMainBlend.UpdateTo(targetA);
            float closed = 100f - coatMainBlend.Current;
            if (closed < 0f) closed = 0f; else if (closed > 100f) closed = 100f;
            float first = coatMainBlend.Current;
            if (first < 0f) first = 0f; else if (first > 100f) first = 100f;
            float second = coatMainBlend.Current - 100f;
            if (second < 0f) second = 0f; else if (second > 100f) second = 100f;
            float threshold = coatMainBlend.Current;
            float triangle = coatMainBlend.Current;
            triangle = triangle + triangle;
            if (!(threshold < 50f)) triangle = 200f - triangle;
            if (triangle < 0f) triangle = 0f; else if (triangle > 100f) triangle = 100f;
            coatMesh.SetBlendShapeWeight(1, closed);
            coatMesh.SetBlendShapeWeight(2, first);
            coatMesh.SetBlendShapeWeight(3, second);
            coatMesh.SetBlendShapeWeight(7, triangle);
            float targetB = pca.PlayerClothes.TargetClothStateBBlend switch
            {
                PlayerStateModel.ClothesState.CloseB => 0f,
                PlayerStateModel.ClothesState.OpenB1 => 100f,
                _ => throw new System.ArgumentOutOfRangeException("PlayerCurrentClothesAState", global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState, null),
            };
            var sub = coatSubBlend;
            scale = TimeScaleManager.Instance.GetCurrentScale;
            sub.SetSecond(scale * 0.2f);
            coatSubBlend.UpdateTo(targetB);
            float subCurrent = coatSubBlend.Current;
            float subWeight = subCurrent <= 100f ? subCurrent - 0f : 200f - subCurrent;
            if (subWeight < 0f) subWeight = 0f; else if (subWeight > 100f) subWeight = 100f;
            coatMesh.SetBlendShapeWeight(4, subWeight);
            var breast = coatBigBreastBlendScale;
            scale = TimeScaleManager.Instance.GetCurrentScale;
            breast.SetSecond(scale * 0.2f);
            coatBigBreastBlendScale.UpdateTo(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState == PlayerStateModel.ClothesState.CloseA ? 1f : 0f);
            var adjuster = pca.PlayerBodyCustomizeAdjuster;
            if (adjuster is null) throw new System.NullReferenceException();
            var breastScale = coatBigBreastBlendScale;
            if (breastScale is null) throw new System.NullReferenceException();
            var receiver = coatMesh;
            if (receiver is null) throw new System.NullReferenceException();
            float breastWeight = (float)adjuster.BigBreastSize * breastScale.Current;
            receiver.SetBlendShapeWeight(5, breastWeight);
            receiver = coatMesh;
            int butt = unchecked(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeButt * 2 - 100);
            if (butt < 0) butt = 0;
            if (receiver is null) throw new System.NullReferenceException();
            receiver.SetBlendShapeWeight(12, (float)butt);
            receiver = coatMesh;
            int thighs = unchecked(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeThighs * 2 - 100);
            if (thighs < 0) thighs = 0;
            if (receiver is null) throw new System.NullReferenceException();
            receiver.SetBlendShapeWeight(13, (float)thighs);
            float gamanTarget = pca.PlayerState.IsGamanBaibu ? 100f : 0f;
            gamanBaibu.UpdateTo(gamanTarget);
            float gaman = gamanBaibu.Current;
            coatMesh.SetBlendShapeWeight(14, gaman);
        }

		private float TransAStateToBlend(PlayerStateModel.ClothesState aState)
		{
			// Recovered from GameAssembly.dll RVA 0x413D90
			switch (aState)
			{
			    case PlayerStateModel.ClothesState.CloseA: return 0f;
			    case PlayerStateModel.ClothesState.OpenA1: return 100f;
			    case PlayerStateModel.ClothesState.OpenA2: return 200f;
			    case PlayerStateModel.ClothesState.DropClothes: return 300f;
			    default: throw new System.ArgumentOutOfRangeException("PlayerCurrentClothesAState", global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState, null);
			}
		}

		public void OnPick()
		{
// Complete original 0x413EB0 including its distinct switch exception contract.
            var target = coatMainBlend;
            var state = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState;
            float value = state switch
            {
                PlayerStateModel.ClothesState.CloseA => 0f,
                PlayerStateModel.ClothesState.OpenA1 => 100f,
                PlayerStateModel.ClothesState.OpenA2 => 200f,
                PlayerStateModel.ClothesState.DropClothes => 300f,
                _ => throw new System.Runtime.CompilerServices.SwitchExpressionException(state),
            };
            target.Current = value;
        }

		public void ForceSetClothesStateBlend(PlayerStateModel.ClothesState state)
		{
			// Recovered from GameAssembly.dll RVA 0x413FA0
			var target = coatMainBlend;
			float value = TransAStateToBlend(state);
			target.Current = value;
		}

		public ExposedBodyParts GetExposedBodyParts()
		{
			// Recovered from GameAssembly.dll RVA 0x413FD0
			var aState = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState;
			uint a = aState switch
			{
			    PlayerStateModel.ClothesState.CloseA => 1u,
			    PlayerStateModel.ClothesState.OpenA1 => 2u,
			    PlayerStateModel.ClothesState.OpenA2 => 8u,
			    PlayerStateModel.ClothesState.DropClothes => 32u,
			    _ => throw new System.Runtime.CompilerServices.SwitchExpressionException(aState),
			};
			var bState = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesBState;
			uint b = bState switch
			{
			    PlayerStateModel.ClothesState.CloseB => 1u,
			    PlayerStateModel.ClothesState.OpenB1 => 4u,
			    _ => throw new System.Runtime.CompilerServices.SwitchExpressionException(bState),
			};
			return (ExposedBodyParts)unchecked((~(a & b) - 1u) & (a | b));
		}

		public PlayerStateModel.ClothesState GetNextState(PlayerStateModel.CoatActionType type)
		{
            if (type == PlayerStateModel.CoatActionType.TypeB)
                return (PlayerStateModel.ClothesState)((int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesBState switch { 10 => 11, 11 => -1, _ => throw new System.ArgumentOutOfRangeException() });
            return (PlayerStateModel.ClothesState)((int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState switch { 0 => 1, 1 => 2, 2 => 5, 5 => -1, 11 => -1, _ => throw new System.ArgumentOutOfRangeException() });
        }

		public PlayerStateModel.ClothesState GetPrevState(PlayerStateModel.CoatActionType type)
		{
            if (type == PlayerStateModel.CoatActionType.TypeB)
                return (PlayerStateModel.ClothesState)((int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesBState switch { 10 => -1, 11 => 10, _ => throw new System.ArgumentOutOfRangeException() });
            return (PlayerStateModel.ClothesState)((int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState switch { 0 => -1, 1 => 0, 2 => 1, 5 => 2, 11 => -1, _ => throw new System.ArgumentOutOfRangeException() });
        }

		public bool IsEnoughSkillLevel(PlayerStateModel.CoatActionType type)
		{
            if (type == PlayerStateModel.CoatActionType.TypeA)
            {
                var release = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState switch
                {
                    PlayerStateModel.ClothesState.CloseA => (global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)(-1),
                    PlayerStateModel.ClothesState.OpenA1 => (global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)2,
                    PlayerStateModel.ClothesState.OpenA2 => (global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)3,
                    PlayerStateModel.ClothesState.DropClothes => (global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)(-1),
                    _ => throw new System.ArgumentOutOfRangeException(),
                };
                return global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease(release);
            }
            else
            {
                var release = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesBState switch
                {
                    PlayerStateModel.ClothesState.CloseB => (global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)1,
                    PlayerStateModel.ClothesState.OpenB1 => (global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)(-1),
                    _ => throw new System.ArgumentOutOfRangeException(),
                };
                return global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease(release);
            }
        }
	}
}
