using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Camera
{
	public class FaceCameraController : MonoBehaviour
	{
		private Transform player;

		[SerializeField]
		private bool isFaceCamera;

		private Vector3 offset;

		private UnityEngine.Camera camera;

		private float initRectHeight;

		private float offsetY = 0.83f;

		private float rectScale = 0.38f;

		private Vector3 currentOffset;

		public void Initialize()
		{
            // Original RVA 0x45C740: cache the original camera and bone-relative offsets.
            player = PlayerController.Instance.Pca.Transform;
            camera = GetComponent<UnityEngine.Camera>();
            initRectHeight = camera.orthographicSize;
            Transform facePosition = PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.FaceCameraPosiiton;
            currentOffset = Quaternion.Inverse(player.rotation) *
                (facePosition.position - InGameManager.Instance.InGameReferencer.Player.Pca.PlayerAvatarObjectReferencer.FaceCameraTarget.position);
            offset = Quaternion.Inverse(player.rotation) *
                (transform.position - InGameManager.Instance.InGameReferencer.Player.Pca.PlayerAvatarObjectReferencer.FaceCameraTarget.position);
            if (isFaceCamera)
            {
                offsetY = 0f;
                rectScale = 0f;
            }
		}

		public void OnLateUpdate()
		{
            // Original RVA 0x45D030: follow the existing avatar anchors, then apply body offsets.
            Transform facePosition = PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.FaceCameraPosiiton;
            Vector3 target = InGameManager.Instance.InGameReferencer.Player.Pca.PlayerAvatarObjectReferencer.FaceCameraTarget.position;
            currentOffset = Vector3.Lerp(currentOffset,
                Quaternion.Inverse(player.rotation) *
                (PlayerController.Instance.Pca.PlayerAvatarObjectReferencer.FaceCameraPosiiton.position - target), 0.3f);
            transform.position = player.rotation * currentOffset + target;
            transform.LookAt(target + facePosition.up * 0.07f, facePosition.up);
            PlayerAvatarObjectReferencer avatar = PlayerController.Instance.Pca.PlayerAvatarObjectReferencer;
            Vector3 neckOffset = avatar.Neck.up * (float)unchecked(GameState.PlayerCustomData.CustomizeNeck - 50) * 0.0005f;
            Vector3 spineUp = avatar.Spine.up;
            float spineOffset = GameState.PlayerCustomData.CustomizeUpperBodyLength < 50
                ? (float)unchecked(GameState.PlayerCustomData.CustomizeUpperBodyLength - 50) * 0.0004f
                : (float)unchecked(GameState.PlayerCustomData.CustomizeUpperBodyLength - 50) * 0.0008f;
            transform.position += spineUp * spineOffset + neckOffset;
		}
	}
}
