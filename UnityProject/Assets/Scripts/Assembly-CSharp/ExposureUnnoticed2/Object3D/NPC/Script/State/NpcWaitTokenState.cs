namespace ExposureUnnoticed2.Object3D.NPC.Script.State
{
	public class NpcWaitTokenState
	{
		private NpcComponentAccessor nca;

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
		}

		public void OnEnter()
		{
			nca.NpcAnimationManager.SetSpeed(0f);
		}

		public void OnUpdate()
		{
            // Complete original RVA 0x44BE00 (PE extent ends at 0x44C257).
            var point = nca.StateModel.RouteData.GetNextPoint();
            var pointPosition = point.Position;
            var position = nca.Transform.position;
            var direction = new global::UnityEngine.Vector3(pointPosition.x - position.x, 0f, pointPosition.z - position.z);
            global::System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(
                typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
            var transform = nca.Transform;
            var rotation = transform.rotation;
            transform.rotation = global::UnityEngine.Quaternion.Lerp(rotation,
                global::UnityEngine.Quaternion.LookRotation(direction, global::UnityEngine.Vector3.up), 0.2f);
            if (point.LinkTokenPublisher.TryGetToken())
            {
                nca.StateModel.HasInteractToken = true;
                nca.NpcController.ChangeState(NpcStateModel.State.Idle);
                return;
            }
            if (point.RouteInteractType != global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.Chair &&
                point.RouteInteractType != global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.StandSmartphone)
                return;
            if (point.RouteInteractType == global::ExposureUnnoticed2.Object3D.RoutePoint.RouteInteractType.Chair)
                SitNpcManager.Instance.UseEndByNpcByFloor(point);
            nca.StateModel.RouteData.IncrementIndex();
            nca.NpcController.ChangeState(NpcStateModel.State.Idle);
		}
	}
}
