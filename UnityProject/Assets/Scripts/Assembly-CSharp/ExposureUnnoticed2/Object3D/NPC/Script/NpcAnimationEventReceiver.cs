using ExposureUnnoticed2.Scripts.Sound;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcAnimationEventReceiver : MonoBehaviour
	{
		private NpcComponentAccessor nca;

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
		}

		public void FootL()
		{
		}

		public void FootR()
		{
		}

		public void OnAnimBaretaSound()
		{
// Complete original body and delayed callback; original scheduler, audio selection, event reuse and rereads.
global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayScaledTime(() =>
{
    global::ExposureUnnoticed2.Scripts.Base.Events.FoundStrangerEvent.FoundNpc = nca.NpcController;
    global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.FoundStrangerEvent);
}, 0.6f);
nca.NpcSoundController.PlaySe((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)10);
}

		public void OnPlaySe(InGameSoundManager.SeType type)
		{
		}

		public void OnEndInteract()
		{
            // Complete original RVA 0x41A810: the original animation event publishes completion.
            nca.StateModel.IsJustOnEndInteract = true;
		}

		public void OnEndChairSitDown()
		{
		}

		public void CommonOnEndAnim()
		{
		}

		public void OnAnimJustHandOver()
		{
		}

		public void OnClothesTakeFromWasher()
		{
		}
	}
}
