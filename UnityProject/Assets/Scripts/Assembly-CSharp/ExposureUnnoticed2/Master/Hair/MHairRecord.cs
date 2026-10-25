using System;
using ExposureUnnoticed2.ObjectUI.PcMenuPanel.OnlineShopPanel;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Hair
{
	[CreateAssetMenu(fileName = "MHairRecord", menuName = "ScriptableObjects/MHairRecord")]
	public class MHairRecord : ScriptableObject, IOnlineShopItemRecord
	{
		[Serializable]
		public class HairCustomizeData
		{
			[ES3Serializable]
			public HairType HairType;

			[ES3Serializable]
			public int TextureId;

			[ES3Serializable]
			public int R;

			[ES3Serializable]
			public int G;

			[ES3Serializable]
			public int B;

			[ES3Serializable]
			public int ShadeColor;

			public HairCustomizeData(HairType type)
			{
			// Recovered from GameAssembly.dll RVA 0x3CB800
			HairType = type;
		}

			public HairCustomizeData(HairCustomizeData data)
			{
			// Recovered from GameAssembly.dll RVA 0x469260
			HairType = data.HairType;
			TextureId = data.TextureId;
			R = data.R;
			G = data.G;
			B = data.B;
			ShadeColor = data.ShadeColor;
		}
		}

		[SerializeField]
		public HairType Type;

		[SerializeField]
		public Texture[] HairTextures;

		[SerializeField]
		public HairCustomizeData DefaultValue;

		[SerializeField]
		public Vector3 HatOffsetV;

		[SerializeField]
		public Vector3 HatOffsetR;

		[SerializeField]
		public Vector3 HatOffsetS;

		[field: SerializeField]
		public string NameKey { get; private set; }

		[field: SerializeField]
		public string DescriptionKey { get; set; }

		[field: SerializeField]
		public int Price { get; private set; }

		public int TypeId => 0;

		public void SwitchVisible(int defaultType = -1)
		{
			if (global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CurrentHair == Type)
				global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerHairController.ChangeHair((HairType)defaultType);
			else
				global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerHairController.ChangeHair(Type);
		}
	}
}
