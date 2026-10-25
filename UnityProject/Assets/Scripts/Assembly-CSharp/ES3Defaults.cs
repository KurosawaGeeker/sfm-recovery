using UnityEngine;

public class ES3Defaults : ScriptableObject
{
	[SerializeField]
	public ES3SerializableSettings settings;

	public bool addMgrToSceneAutomatically;

	public bool autoUpdateReferences;

	public bool addAllPrefabsToManager;

	public int collectDependenciesDepth;

	public int collectDependenciesTimeout;

	public bool updateReferencesWhenSceneChanges;

	public bool updateReferencesWhenSceneIsSaved;

	public bool updateReferencesWhenSceneIsOpened;

	[Tooltip("Folders listed here will be searched for references every time the reference manager is refreshed. Path should be relative to the project folder.")]
	public string[] referenceFolders;

	public bool logDebugInfo;

	public bool logWarnings;

	public bool logErrors;
}
