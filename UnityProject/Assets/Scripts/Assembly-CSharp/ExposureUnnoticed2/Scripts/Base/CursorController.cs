using ExposureUnnoticed2.Scripts.InGame;

namespace ExposureUnnoticed2.Scripts.Base
{
	public class CursorController : IUpdater
	{
		private static CursorController instance;

		public static CursorController Instance
        {
            get
            {
                if (instance == null) instance = new CursorController();
                return instance;
            }
        }

		public bool IsVisible { get; private set; }

		private CursorController()
		{
			// Recovered from GameAssembly.dll RVA 0x4B5DA0
			UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.Locked;
			EventManager.AddListener<CursorVisibleEvent>(OnCursorVisibleEvent, false);
		}

		public void OnUpdate()
		{
		}

		private void SetDisplayMouse(bool isDisplay)
		{
			// Recovered from GameAssembly.dll RVA 0x4B5EA0
			IsVisible = isDisplay;
			UnityEngine.Cursor.visible = isDisplay;
			UnityEngine.Cursor.lockState = isDisplay ? UnityEngine.CursorLockMode.None : UnityEngine.CursorLockMode.Locked;
		}

		private void OnCursorVisibleEvent(CursorVisibleEvent cursorVisibleEvent)
		{
			// Recovered from GameAssembly.dll RVA 0x4B5F40
			SetDisplayMouse(cursorVisibleEvent.IsVisible);
		}

		public static void SetVisible(bool isVisible)
		{
			// Recovered from GameAssembly.dll RVA 0x4B5F60
			if (instance != null) instance.SetDisplayMouse(isVisible);
		}
	}
}
