// Owned dispatch adapter. UNITY_EDITOR excludes this entire class from normal Player code.
#if UNITY_EDITOR
using System;
using UnityEngine;
namespace SfmMvp
{
 [DefaultExecutionOrder(10000)] public sealed class NpcAnimationFrameDriverV2 : MonoBehaviour
 {
  public static Action TickCallback;
  private void Update(){TickCallback?.Invoke();}
 }
}
#endif
