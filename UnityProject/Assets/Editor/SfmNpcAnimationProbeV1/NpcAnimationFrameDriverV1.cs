#if UNITY_EDITOR
using UnityEngine;
namespace SfmMvp.Editor
{
 // Scheduler keeps its original default execution order; this adapter dispatches only animation.
 [DefaultExecutionOrder(10000)] public sealed class NpcAnimationFrameDriverV1 : MonoBehaviour
 {
  private void Update(){NpcAnimationPlayModeProbeV1.Tick();}
 }
}
#endif
