using System.Collections.Generic;
using ExposureUnnoticed2.Object3D.InteractArea;

namespace ExposureUnnoticed2.Scripts.Base
{
	public class InteractManager
	{
		private static List<InteractModel> interactList = new List<InteractModel> { new InteractModel { Type = InteractType.None } };

		public static InteractModel CurrentInteractModel;

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x4B7900
			EventManager.AddListener<InteractEvent>(OnInteract, true);
		}

		public void Dispose()
		{
			// Recovered from GameAssembly.dll RVA 0x4B79C0
			EventManager.RemoveListener<InteractEvent>(OnInteract);
		}

		public static InteractModel GetHead()
		{
			// Recovered from GameAssembly.dll RVA 0x4B7A80
			return interactList[interactList.Count - 1];
		}

		public static void Register(InteractType type, InteractAreaController controller)
		{
			// Recovered from GameAssembly.dll RVA 0x4B7B40
			var list = interactList;
			var model = new InteractModel { Type = type, Controller = controller };
			list.Add(model);
		}

		public static void Unregister(InteractType type, InteractAreaController controller)
		{
			// Recovered from GameAssembly.dll RVA 0x4B7C50
			foreach (var model in interactList)
			{
			    if (model.Type != type) continue;
			    if (model.Controller != controller) continue;
			    interactList.Remove(model);
			    return;
			}
		}

		public static bool IsRegister(InteractModel model)
		{
			// Recovered from GameAssembly.dll RVA 0x4B7F50
			return interactList.Contains(model);
		}

		private void OnInteract(InteractEvent evt)
		{
			// Recovered from GameAssembly.dll RVA 0x4B8020
			CurrentInteractModel = evt.Model;
		}
	}
}
