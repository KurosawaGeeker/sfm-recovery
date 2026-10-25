using System.Collections.Generic;
using UnityEngine;

public class ES3AutoSave : MonoBehaviour, ISerializationCallbackReceiver
{
	public bool saveLayer;

	public bool saveTag;

	public bool saveName;

	public bool saveHideFlags;

	public bool saveActive;

	public bool saveChildren;

	private bool isQuitting;

	public List<Component> componentsToSave;

	public void Reset()
	{
	}

	public void Awake()
	{
	}

	public void OnApplicationQuit()
	{
	}

	public void OnDestroy()
	{
	}

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
	}
}
