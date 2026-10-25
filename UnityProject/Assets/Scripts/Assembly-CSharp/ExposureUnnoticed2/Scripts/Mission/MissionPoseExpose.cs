namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionPoseExpose : MissionBase
	{
		public override bool IsInCondition()
		{
if (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer is null) throw new System.NullReferenceException();
var facade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
if (facade is null) throw new System.NullReferenceException();
var exposed = facade.GetExposedBodyPartsByClothesAndCostume();
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
if ((int)exposed == 1) return false;
int action = (int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentAction.Type;
if (action <= 49999 || action >= 60001) return false;
var area = global::ExposureUnnoticed2.Scripts.InGame.MissionArea.NpcRouteAreaManager.Instance;
if (area != null)
{
    area = global::ExposureUnnoticed2.Scripts.InGame.MissionArea.NpcRouteAreaManager.Instance;
    if (area is null) throw new System.NullReferenceException();
    return area.IsInArea;
}
return true;
		}
	}
}
