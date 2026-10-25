using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerFootStepController
	{
		private PlayerClassAccessor pca;

		private float originalBareyasusaByHeel;

		private float startBareyasusa;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x3CAB80
			this.pca = pca;
		}

		public void OnUpdate()
		{
			// SOURCE DRAFT: original64DD10. The private decay store precedes clamp and a potential state-null failure.
			float oldHeel = originalBareyasusaByHeel;
			float delta = UnityEngine.Time.deltaTime;
			float remaining = oldHeel - delta * 0.35f;
			originalBareyasusaByHeel = remaining;
			var capturedState = pca.PlayerState;
			float clamped;
			if (remaining < 0f) clamped = 0f;
			else if (startBareyasusa < remaining) clamped = startBareyasusa;
			else clamped = remaining;
			if (capturedState is null) throw new System.NullReferenceException();
			capturedState.BareyasusaByHeel = clamped;
		}

		public void OnFootStep(AnimationEvent type)
		{
		    // Original GameAssembly RVA 0x64DDC0; objectReferenceParameter is a checked cast.
		    var record = (global::ExposureUnnoticed2.Object3D.Player.ScriptableObject.MFootAnimationEventRecord)type.objectReferenceParameter;
		    var initialState = pca.PlayerState;
		    if (!initialState.IsCrouch && !initialState.IsGamanBaibu && initialState.IsStrafe)
		    {
		        var direction = pca.PlayerAnimationManager.NearStrafeDirect();
		        if (record is null) throw new global::System.NullReferenceException();
		        if (direction != record.Direct) return;
		    }
		    var action = pca.PlayerState.CurrentAction;
		    if (action is null) throw new global::System.NullReferenceException();
		    if (action.CanMove && pca.PlayerState.CurrentAction.FloatParam != 0f)
		    {
		        if (record is null) throw new global::System.NullReferenceException();
		        if (!record.IsMoveAction) return;
		    }
		    bool isHeel = false;
		    if (pca.PlayerState.CurrentShoesRecord is not null)
		        isHeel = pca.PlayerState.CurrentShoesRecord.EffectType == global::ExposureUnnoticed2.Master.Costume.CostumeEffectType.Heel;
		    bool isSneaker = false;
		    if (pca.PlayerState.CurrentShoesRecord is not null)
		        isSneaker = pca.PlayerState.CurrentShoesRecord.EffectType == global::ExposureUnnoticed2.Master.Costume.CostumeEffectType.Sneaker;

		    // Original checks the record even when the selected bare-foot branch won't read it.
		    if (record is null) throw new global::System.NullReferenceException();
		    var soundState = pca.PlayerState;
		    global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType se;
		    if (soundState.IsCrouch)
		        se = global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType.Barefoot;
		    else if (isHeel)
		        se = record.SeType;
		    else if (isSneaker)
		        se = soundState.IsDash ? global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType.FootStepSneakerJog : global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType.FootStepSneaker;
		    else
		        se = global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType.Barefoot;
		    pca.PlayerSoundManager.PlaySe(se);
		    if (isHeel && !pca.PlayerState.IsCrouch)
		    {
		        startBareyasusa = pca.PlayerState.IsDash ? 0.301f : 0.201f;
		        originalBareyasusaByHeel = startBareyasusa + 0.1f;
		    }
		}
	}
}
