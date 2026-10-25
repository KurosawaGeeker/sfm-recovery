using UnityEngine;

namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionVibeExposeMove : MissionBase
	{
		private Vector3 startPathPosition;

		private bool oldResult;

		private int PathSearchCount;

		private float lastMaxPathLength;

		public override bool IsInCondition()
		{
// Complete original method; original state latch, sampler order and independent player graphs.
var player = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player;
var facade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
if (facade is null) throw new System.NullReferenceException();
var exposed = facade.GetExposedBodyPartsByClothesAndCostume();
var required = RMission.OriginalRecord.ExposedBodyPartsParam;
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
bool result = false;
if ((exposed & required) != 0 && player.Pca.PlayerState.EcstasyRiseByVibratorPerSecond > 0f)
{
    int mode = (int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerVibrationModeType;
    result = mode == 2 || mode == 3;
}
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
// Complete original method; original state latch, sampler order and independent player graphs.
var player = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player;
var record = RMission;
if (record is null) throw new System.NullReferenceException();
if (player is null) throw new System.NullReferenceException();
var accessor = player.Pca;
if (record.FloatParam * satisfy < 10f)
    return accessor.PlayerMoveCalculator.JustMoveAmountRealtime / RMission.FloatParam;
return accessor.PlayerMoveCalculator.JustMoveAmount / RMission.FloatParam;
}

		protected override bool IsRpPerSecCondition()
		{
// Complete original method; original state latch, sampler order and independent player graphs.
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
