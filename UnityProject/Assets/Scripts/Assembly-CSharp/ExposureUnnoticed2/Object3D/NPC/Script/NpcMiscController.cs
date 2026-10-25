namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcMiscController
	{
		private NpcComponentAccessor nca;

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
		}

		public void OnUpdate()
		{// Complete original native rule; original Game helper bodies remain intact.
bool lookAtPlayer;
if (!(nca.StrangenessManager.Strangeness.Value >= nca.StateModel.StrangeLookAt) ||
    nca.StateModel.CurrentState == NpcStateModel.State.Stagger ||
    nca.StateModel.CurrentState == NpcStateModel.State.TraceSexPlayer)
    lookAtPlayer = false;
else
    lookAtPlayer = !global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveInvisible;
if (!lookAtPlayer)
{
    lookAtPlayer = nca.StateModel.CurrentFixedType == NpcStateModel.FixedType.Pinpon &&
        nca.PlayerSearcher.IsPlayerFound && !nca.StateModel.IsSkipRiseStrangenessPinponNpc &&
        !global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveInvisible;
}
if (lookAtPlayer)
    nca.LookAtController.EnableLookAt(global::ExposureUnnoticed2.Object3D.Player.Scripts.LookAtController.LookAtLayer.NpcLookPlayer, nca.Player.Pca.PlayerAvatarObjectReferencer.Head, false);
else
    nca.LookAtController.DisableLookAt(global::ExposureUnnoticed2.Object3D.Player.Scripts.LookAtController.LookAtLayer.NpcLookPlayer);
bool faceContempt = nca.StateModel.IsAlreadyFoundStranger && nca.PlayerSearcher.IsPlayerFound;
nca.NpcAnimationManager.SetFaceContempt(faceContempt, NpcAnimationManager.FacialLayer.AlreadyFound);
bool displayed = nca.NpcController.isDisplay;
var component = nca.NpcComponent;
if (!displayed)
{
    var blocker = component.CeilingPlayerBlocker;
    var playerPosition = nca.Player.Pca.Transform.position;
    var npcPosition = nca.Transform.position;
    if (blocker is null) throw new System.NullReferenceException();
    blocker.SetActive(playerPosition.y <= npcPosition.y + 0.3f);
}
else
    component.CeilingPlayerBlocker.SetActive(false);
if (nca.NpcCollider.IsJustArea)
{
    if (nca.StateModel.CurrentState == NpcStateModel.State.Idle && nca.StateModel.CurrentFixedType == NpcStateModel.FixedType.None)
    {
        UnityEngine.Vector3 move = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.MoveV;
        move.y = 0f;
        bool doStagger = false;
        if (move.sqrMagnitude > 0.01f)
        {
            UnityEngine.Vector3 againMove = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.MoveV;
            var direct = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.Direct(global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.GameObject, nca.GameObject);
            doStagger = UnityEngine.Vector3.Dot(againMove, direct) > 0.1f;
        }
        if (doStagger || global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveInvisible)
        {
            var inverse = UnityEngine.Quaternion.Inverse(UnityEngine.Quaternion.LookRotation(nca.Transform.forward));
            var direct = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.Direct(global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.GameObject, nca.GameObject);
            direct = inverse * direct;
            direct.y = 0f;
            direct.Normalize();
            nca.NpcAnimationManager.SetStagger(direct);
            nca.StateModel.StaggerBeforeState = nca.StateModel.CurrentState;
            nca.NpcController.ChangeState(NpcStateModel.State.Stagger);
            nca.StrangenessManager.Strangeness.Value = UnityEngine.Mathf.Clamp(nca.StrangenessManager.Strangeness.Value, 0.5f, 1f);
        }
    }
}
if (nca.NpcCollider.IsJustArea && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveInvisible)
{
    global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveInvisible = false;
    global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveInvisibleReusable = false;
    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("log_disappeared_invisible"));
}
if (nca.StateModel.IsActiveSpermCollider && nca.StateModel.PlayerDist >= 3f)
    nca.StateModel.IsActiveSpermCollider = false;
else
{
    if (nca.StateModel.IsActiveSpermCollider) return;
    if (!(nca.StateModel.PlayerDist < 3f)) return;
    nca.StateModel.IsActiveSpermCollider = true;
}
var colliders = nca.AvatarComponent.SpermColliders;
for (int i = 0; i < colliders.Length; i++)
{
    if (colliders[i]) colliders[i].SetActive(nca.StateModel.IsActiveSpermCollider);
}
}
	}
}
