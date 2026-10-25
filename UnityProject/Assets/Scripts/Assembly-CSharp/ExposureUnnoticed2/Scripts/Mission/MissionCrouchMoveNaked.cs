using UnityEngine;

namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionCrouchMoveNaked : MissionBase
	{
		private Vector3 startPathPosition;

		private bool oldResult;

		private int PathSearchCount;

		private float lastMaxPathLength;

		public override bool IsInCondition()
		{
var player = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player;
var facade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
if (facade is null) throw new System.NullReferenceException();
var exposed = facade.GetExposedBodyPartsByClothesAndCostume();
var required = RMission.OriginalRecord.ExposedBodyPartsParam;
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
bool result = (required & exposed) != 0 && player.Pca.PlayerState.IsCrouch;
if (!oldResult && result)
{
    startPathPosition = player.Pca.Transform.position;
    PathSearchCount = 0;
    lastMaxPathLength = 0f;
}
oldResult = result;
return result;
}

		protected override float GetAddSatisfy()
		{
float scaled = RMission.FloatParam * satisfy;
var player = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance;
if (6f > scaled) return player.Pca.PlayerMoveCalculator.JustMoveAmountRealtime / RMission.FloatParam;
return player.Pca.PlayerMoveCalculator.JustMoveAmount / RMission.FloatParam;
}

		protected override bool IsRpPerSecCondition()
		{
if (!IsInCondition()) return false;
return global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerMoveCalculator.LastUpdateJustMoveAmount > 0f;
}

		public override void Reset()
		{
base.Reset();
lastMaxPathLength = 0f;
oldResult = false;
}
	}
}
