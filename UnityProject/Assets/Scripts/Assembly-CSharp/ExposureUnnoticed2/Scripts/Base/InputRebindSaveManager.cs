using UnityEngine;
using UnityEngine.InputSystem;

namespace ExposureUnnoticed2.Scripts.Base
{
	public class InputRebindSaveManager : MonoBehaviour
	{
		[SerializeField]
		private InputActionAsset _actionAsset;

		[SerializeField]
		private string _savePath = "InputActionOverrides.json";

		public static InputRebindSaveManager Instance { get; private set; }

		private void Awake()
		{
// Complete original native production body.
if (!Instance) Instance = this;
}

		private void OnDestroy()
		{
// Complete original native production body.
if (Instance == this) Instance = null;
}

		public void Save()
		{
// Complete original native production body.
if (!_actionAsset) return;
string json = _actionAsset.SaveBindingOverridesAsJson();
string path = System.IO.Path.Combine(Application.persistentDataPath, _savePath);
System.IO.File.WriteAllText(path, json);
}

		public void Load()
		{
// Complete original native production body.
if (!_actionAsset) return;
string path = System.IO.Path.Combine(Application.persistentDataPath, _savePath);
if (System.IO.File.Exists(path))
{
    string json = System.IO.File.ReadAllText(path);
    _actionAsset.LoadBindingOverridesFromJson(json, true);
}
}
	}
}
