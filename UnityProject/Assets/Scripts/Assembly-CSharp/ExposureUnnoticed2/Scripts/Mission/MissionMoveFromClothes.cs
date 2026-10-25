using UnityEngine;

namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionMoveFromClothes : MissionBase
	{
		private Vector3 startPathPosition;

		private bool oldResult;

		private int PathSearchCount;

		private float lastMaxPathLength;

		private float lastPathLength;

		public override bool IsInCondition()
		{
            var player = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player;
            var exposed = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.GetExposedBodyPartsByClothesAndCostume();
            bool noClothes = !global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.OnlyHasFlag(exposed, (global::ExposureUnnoticed2.Master.Mission.ExposedBodyParts)1)
                && global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentClothes.ClothesType == (global::ExposureUnnoticed2.Master.Clothes.ClothesType)(-1);
            if (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.IsHideSomePartsByCosplay())
            {
                isCancelByHideCosplay = true;
                lastMaxPathLength = 0f;
                satisfy = 0f;
            }
            else if (player.Pca.PlayerState.CurrentClothes.ClothesType != (global::ExposureUnnoticed2.Master.Clothes.ClothesType)(-1))
                isCancelByHideCosplay = false;
            bool result = !isCancelByHideCosplay && noClothes;
            if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)5)
                && !isCancelByHideCosplay && !result && satisfy < 1f)
                Reset();
            oldResult = result;
            return result;
        }

		protected override float GetAddSatisfy()
		{
            PathSearchCount = unchecked(PathSearchCount + 1);
            if (PathSearchCount % 10 != 9 || !oldResult) return 0f;
            Vector3 start;
            var drop = global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.Instance.GetDropClothesInSameArea();
            if (drop != null)
                start = drop.Position;
            else
            {
                if (!global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.EntryStageInitPosition.HasValue) return 0f;
                start = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.EntryStageInitPosition.Value;
            }
            Vector3 destination = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.Transform.position;
            float length = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.CalcPathLength(start, destination);
            float add = 0f;
            if (length > lastMaxPathLength)
            {
                add = (length - lastMaxPathLength) / RMission.FloatParam;
                lastMaxPathLength = length;
            }
            lastPathLength = length;
            return add;
        }

		protected override bool IsRpPerSecCondition()
		{
            PathSearchCount = unchecked(PathSearchCount + 1);
            if (PathSearchCount % 10 == 9)
            {
                var drop = global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.Instance.GetDropClothesInSameArea();
                if (drop == null) return false;
                var referencer = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer;
                if (referencer is null) throw new global::System.NullReferenceException();
                Vector3 start = drop.Position;
                Vector3 destination = referencer.Player.Pca.Transform.position;
                lastPathLength = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.CalcPathLength(start, destination);
            }
            return IsInCondition() && lastPathLength >= RMission.FloatParam;
        }

		public override void Reset()
		{
base.Reset();
lastMaxPathLength = 0f;
oldResult = false;
}
	}
}
