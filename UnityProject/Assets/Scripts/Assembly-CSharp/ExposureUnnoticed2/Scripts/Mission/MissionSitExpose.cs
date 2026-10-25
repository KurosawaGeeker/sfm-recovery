namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionSitExpose : MissionBase
	{
		public override bool IsInCondition()
		{
if (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer is null) throw new System.NullReferenceException();
var facade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
if (facade is null) throw new System.NullReferenceException();
var exposed = facade.GetExposedBodyPartsByClothesAndCostume();
var required = RMission.OriginalRecord.ExposedBodyPartsParam;
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
if ((required & exposed) == 0) return false;
int action = (int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentAction.Type;
return action == 10026 || action == 10027 || (uint)(action - 10101) <= 3U || action == 10105;
		}
	}
}
