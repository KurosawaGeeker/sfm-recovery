using ExposureUnnoticed2.Object3D.ForeignObject;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcForeignObjectLooker
	{
		public StrangenessValueManager Strangeness = new StrangenessValueManager();

		private NpcComponentAccessor nca;

		private ForeignObjectController target;

		private float coolDownEndTime;

		private float facialEndTime;

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
		}

		public void OnUpdate()
        {
            if (nca.StateModel.CurrentFixedType != NpcStateModel.FixedType.None) return;
            if (target != null && Strangeness.Value >= nca.StateModel.StrangeLookAt && nca.StateModel.CurrentState != NpcStateModel.State.Stagger)
                nca.LookAtController.EnableLookAt(ExposureUnnoticed2.Object3D.Player.Scripts.LookAtController.LookAtLayer.NpcForeignObject, target.transform, true);
            else nca.LookAtController.DisableLookAt(ExposureUnnoticed2.Object3D.Player.Scripts.LookAtController.LookAtLayer.NpcForeignObject);
            switch (nca.StateModel.CurrentForeignObjectLookerType)
            {
                case NpcStateModel.ForeignObjectLookerType.Idle: UpdateIdle(); break;
                case NpcStateModel.ForeignObjectLookerType.InFacial: UpdateInFacial(); break;
                case NpcStateModel.ForeignObjectLookerType.CoolDown: UpdateCoolDown(); break;
            }
        }

		private void UpdateIdle()
        {
            target = null;
            float maximum = float.MinValue;
            Strangeness.OnUpdate(false);
            foreach (ForeignObjectController candidate in nca.StateModel.InSightForeignObjectList)
            {
                if (candidate.MaxDiscomfort > maximum)
                {
                    maximum = candidate.MaxDiscomfort;
                    target = candidate;
                }
            }
            if (target == null) return;
            if (nca.LookAtController.IsEnable(ExposureUnnoticed2.Object3D.Player.Scripts.LookAtController.LookAtLayer.NpcLookPlayer)) return;
            if (nca.StateModel.isSumahoWalk) return;
            Strangeness.Value += CalcSightScale(target.transform, nca.AvatarComponent.Head) * 0.5f * Time.deltaTime;
            Strangeness.Value = Mathf.Clamp(Strangeness.Value, 0f, 0.5f);
            if (Strangeness.Value >= target.MaxDiscomfort)
            {
                nca.StateModel.AlreadyLookedForeignObjectSet.Add(target.gameObject);
                nca.StateModel.CurrentForeignObjectLookerType = NpcStateModel.ForeignObjectLookerType.InFacial;
                nca.NpcAnimationManager.SetFaceContempt(true, NpcAnimationManager.FacialLayer.ForeignObject);
                facialEndTime = Time.time + 5f;
            }
        }

		private void UpdateInFacial()
        {
            if (Time.time > facialEndTime)
            {
                nca.StateModel.CurrentForeignObjectLookerType = NpcStateModel.ForeignObjectLookerType.CoolDown;
                Strangeness.Value = 0f;
                coolDownEndTime = Time.time + 5f;
                nca.NpcAnimationManager.SetFaceContempt(false, NpcAnimationManager.FacialLayer.ForeignObject);
            }
        }

		private void UpdateCoolDown()
        {
            if (Time.time > coolDownEndTime)
                nca.StateModel.CurrentForeignObjectLookerType = NpcStateModel.ForeignObjectLookerType.Idle;
        }

		private float CalcSightScale(Transform foreignObjectTransform, Transform head)
        {
            Vector3 from = nca.Transform.position;
            Vector3 to = foreignObjectTransform.position;
            float x = from.x - to.x, y = from.y - to.y, z = from.z - to.z;
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(System.Math).TypeHandle);
            float magnitude = (float)System.Math.Sqrt((double)((y * y + x * x) + z * z));
            float inverse = Mathf.Clamp01(1f / magnitude);
            Vector3 forward = head.forward;
            Vector3 foreignPosition = foreignObjectTransform.position;
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
            Vector3 npcPosition = nca.Transform.position;
            x = foreignPosition.x - npcPosition.x;
            y = 0f;
            z = foreignPosition.z - npcPosition.z;
            magnitude = (float)System.Math.Sqrt((double)((y * y + x * x) + z * z));
            Vector3 direction;
            if (magnitude > 0.00001f) direction = new Vector3(x / magnitude, y / magnitude, z / magnitude);
            else direction = Vector3.zero;
            float dot = Mathf.Clamp01((forward.y * direction.y + forward.x * direction.x) + forward.z * direction.z);
            return (((dot * dot) * dot) * dot) * inverse;
        }
	}
}
