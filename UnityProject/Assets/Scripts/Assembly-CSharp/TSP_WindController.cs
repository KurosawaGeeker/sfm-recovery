using UnityEngine;

[ExecuteInEditMode]
public class TSP_WindController : MonoBehaviour
{
	[Range(0f, 5f)]
	public float speed;

	[Range(0f, 0.04f)]
	public float worldFrequency;

	[Range(0f, 0.1f)]
	public float bendAmount;

	private void Start()
	{
	}

	private void Update()
	{
	}
}
