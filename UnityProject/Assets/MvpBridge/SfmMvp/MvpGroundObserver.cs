using UnityEngine;
namespace SfmMvp { public sealed class MvpGroundObserver : MonoBehaviour { public int stays; void OnTriggerStay(Collider other) { stays++; } } }
