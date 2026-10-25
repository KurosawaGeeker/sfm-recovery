namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionUsingWasher : MissionBase
	{
		public override bool IsInCondition()
		{
// Complete original method; original state latch, sampler order and independent player graphs.
var reference = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer;
if (reference is null) throw new System.NullReferenceException();
var facade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
if (facade is null) throw new System.NullReferenceException();
if (facade.IsHideSomePartsByCosplay())
{
    isCancelByHideCosplay = true;
    satisfy = 0f;
}
else if ((int)reference.Player.Pca.PlayerState.CurrentClothes.ClothesType != -1)
    isCancelByHideCosplay = false;
if (isCancelByHideCosplay) return false;
var laundry = global::ExposureUnnoticed2.Scripts.InGame.SceneProcessManager.LaundryManager.Instance;
if (laundry != null)
{
    laundry = global::ExposureUnnoticed2.Scripts.InGame.SceneProcessManager.LaundryManager.Instance;
    if (laundry is null) throw new System.NullReferenceException();
    return (int)laundry.GetRpBuffSourceType() != -1;
}
return false;
}
	}
}
