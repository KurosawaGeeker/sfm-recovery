namespace ExposureUnnoticed2.Scripts.Base
{
	public class LayerAccessor
	{
		private static int npcLayer = -1;

		private static int npcCarLayer = -1;

		private static int playerLayer = -1;

		private static int cameraVisible = -1;

		private static int foreignObjectLayer = -1;

		private static int levelLayer = -1;

		private static int playerBlocker = -1;

		private static int spermBullet = -1;

		private static int spermCollider = -1;

		private static int ignoreSperm = -1;

		private static int xRayLevel = -1;

		private static int npcSightBlocker = -1;

		private static int ignorePeeParticle = -1;

		private static int censorFaceCamera = -1;

		public static int NpcLayer
        {
            get
            {
                if (npcLayer == -1)
                    npcLayer = UnityEngine.LayerMask.NameToLayer("Npc");
                return npcLayer;
            }
        }

		public static int NpcCarLayer
        {
            get
            {
                if (npcCarLayer == -1)
                    npcCarLayer = UnityEngine.LayerMask.NameToLayer("Car");
                return npcCarLayer;
            }
        }

		public static int PlayerLayer
        {
            get
            {
                if (playerLayer == -1)
                    playerLayer = UnityEngine.LayerMask.NameToLayer("Player");
                return playerLayer;
            }
        }

		public static int CameraVisible
        {
            get
            {
                if (cameraVisible == -1)
                    cameraVisible = UnityEngine.LayerMask.NameToLayer("CameraVisible");
                return cameraVisible;
            }
        }

		public static int ForeignObjectLayer
        {
            get
            {
                if (foreignObjectLayer == -1)
                    foreignObjectLayer = UnityEngine.LayerMask.NameToLayer("ForeignObject");
                return foreignObjectLayer;
            }
        }

		public static int LevelLayer
        {
            get
            {
                if (levelLayer == -1)
                    levelLayer = UnityEngine.LayerMask.NameToLayer("Level");
                return levelLayer;
            }
        }

		public static int PlayerBlocker
        {
            get
            {
                if (playerBlocker == -1)
                    playerBlocker = UnityEngine.LayerMask.NameToLayer("PlayerBlocker");
                return playerBlocker;
            }
        }

		public static int SpermBullet
        {
            get
            {
                if (spermBullet == -1)
                    spermBullet = UnityEngine.LayerMask.NameToLayer("SpermBullet");
                return spermBullet;
            }
        }

		public static int SpermCollider
        {
            get
            {
                if (spermCollider == -1)
                    spermCollider = UnityEngine.LayerMask.NameToLayer("SpermCollider");
                return spermCollider;
            }
        }

		public static int IgnoreSperm
        {
            get
            {
                if (ignoreSperm == -1)
                    ignoreSperm = UnityEngine.LayerMask.NameToLayer("IgnoreSperm");
                return ignoreSperm;
            }
        }

		public static int XRayLevel
        {
            get
            {
                if (xRayLevel == -1)
                    xRayLevel = UnityEngine.LayerMask.NameToLayer("XRayLevel");
                return xRayLevel;
            }
        }

		public static int NpcSightBlocker
        {
            get
            {
                if (npcSightBlocker == -1)
                    npcSightBlocker = UnityEngine.LayerMask.NameToLayer("NpcSightBlocker");
                return npcSightBlocker;
            }
        }

		public static int IgnorePeeParticle
        {
            get
            {
                if (ignorePeeParticle == -1)
                    ignorePeeParticle = UnityEngine.LayerMask.NameToLayer("IgnorePeeParticle");
                return ignorePeeParticle;
            }
        }

		public static int CensorFaceCamera
        {
            get
            {
                if (censorFaceCamera == -1)
                    censorFaceCamera = UnityEngine.LayerMask.NameToLayer("CensorFaceCamera");
                return censorFaceCamera;
            }
        }
	// Recovered original member RVA 0x4B90D0: .cctor
		// Recovered original .cctor RVA 0x4B90D0: ordered field initializers preserve BeforeFieldInit.
	}
}
