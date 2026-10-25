using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[IncludeInSettings(true)]
public class ES3AutoSaveMgr : MonoBehaviour
{
	public enum LoadEvent
	{
		None = 0,
		Awake = 1,
		Start = 2
	}

	public enum SaveEvent
	{
		None = 0,
		OnApplicationQuit = 1,
		OnApplicationPause = 2
	}

	public static ES3AutoSaveMgr _current;

	public string key;

	public SaveEvent saveEvent;

	public LoadEvent loadEvent;

	public ES3SerializableSettings settings;

	public HashSet<ES3AutoSave> autoSaves;

	public static ES3AutoSaveMgr Current => null;

	public static ES3AutoSaveMgr Instance => null;

	public void Save()
	{
	}

	public void Load()
	{
	}

	private void Start()
	{
	}

	public void Awake()
	{
	}

	private void OnApplicationQuit()
	{
	}

	private void OnApplicationPause(bool paused)
	{
	}

	public static void AddAutoSave(ES3AutoSave autoSave)
	{
	}

	public static void RemoveAutoSave(ES3AutoSave autoSave)
	{
	}

	public void GetAutoSaves()
	{
	}

	private static int GetDepth(Transform t)
	{
		return 0;
	}
}
