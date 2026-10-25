namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionEcstasyByNearNpc : MissionBase
	{
		public override bool IsInCondition()
		{
var player = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player;
var state = player.Pca.PlayerState;
if (!state.IsEcstasyMotion || !state.IsNearNpcForMissionSecondary) return false;
var facade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
if (facade is null) throw new System.NullReferenceException();
var exposed = facade.GetExposedBodyPartsByClothesAndCostume();
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
return (int)exposed != 1;
}
	}
}
