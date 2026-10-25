using UnityEngine;

namespace ExposureUnnoticed2.Master.Common
{
	[CreateAssetMenu(fileName = "MCustomizeColor", menuName = "ScriptableObjects/MCustomizeColor")]
	public class MCustomizeColor : ScriptableObject
	{
		[SerializeField]
		public SkinColor[] SkinPattern;

		[SerializeField]
		public Texture[] EyeTextures;

		[SerializeField]
		public Texture[] EyeEmitTextures;

		[SerializeField]
		public Texture NormalSkinTexture;

		[SerializeField]
		public Texture HinnyuSkinTexture;

		private static MCustomizeColor instance;

		public static MCustomizeColor Instance
        {
            get
            {
                if (instance == null)
                    instance = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MCustomizeColor;
                return instance;
            }
        }
	}
}
