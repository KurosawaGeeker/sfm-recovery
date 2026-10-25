using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.Sound;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcSoundController
	{
		private NpcComponentAccessor nca;

		private SmoothFloat elevatorOutsideVolumeScale = new SmoothFloat(0.5f, 1f, 1f);

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
		}

		public void OnUpdate()
		{
		}

		public void PlaySe(InGameSoundManager.SeType type)
		{
// Complete original body and delayed callback; original scheduler, audio selection, event reuse and rereads.
var source = nca.AudioSource;
if ((int)type == 10) source = nca.NpcComponent.DokinAudioSource;
if (InGameSoundManager.Instance.IsFootSe(type) || InGameSoundManager.Instance.IsGroupSe(type))
{
    var target = nca.NpcController.GameObject;
    var clip = InGameSoundManager.Instance.GetSound(type, target);
    if (source is null) throw new System.NullReferenceException();
    source.PlayOneShot(clip, 1f);
}
else
{
    var clip = InGameSoundManager.Instance.GetSound(type);
    if (clip)
    {
        if (source is null) throw new System.NullReferenceException();
        source.PlayOneShot(clip, 1f);
    }
    else InGameSoundManager.Instance.PlayOneShotAudioData(source, type, -1f);
}
}
	}
}
