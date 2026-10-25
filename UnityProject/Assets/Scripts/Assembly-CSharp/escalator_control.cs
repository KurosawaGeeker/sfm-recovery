using UnityEngine;

public class escalator_control : MonoBehaviour
{
	[SerializeField]
	private SkinnedMeshRenderer mesh;

	public float speed;

	private Vector2 vecOffset;

	public bool forward;

	private Animator elevatorAnim;

	private Material[] materials;

	private void Start()
	{
	}

	private void Update()
	{
	}
}
