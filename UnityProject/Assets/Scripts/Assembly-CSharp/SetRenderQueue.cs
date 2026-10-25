using UnityEngine;

[AddComponentMenu("Rendering/SetRenderQueue")]
public class SetRenderQueue : MonoBehaviour
{
	[SerializeField]
	protected int[] m_queues;

	protected void Awake()
	{
	}
}
