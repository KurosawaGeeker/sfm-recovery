using System.Collections.Generic;
using UnityEngine;
using ch.sycoforge.Decal;

[RequireComponent(typeof(EasyDecal))]
public class CandidateFilter : MonoBehaviour
{
	public GameObject ExclusiveReceiver;

	private EasyDecal decal;

	private void Start()
	{
	}

	private void bp_OnCandidatesProcessed(List<Collider> colliders)
	{
	}
}
