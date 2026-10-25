#if UNITY_EDITOR
using System;
using UnityEngine;
namespace SfmMvp
{
    // Owned Editor fixture: callbacks live in Editor; this actual component is
    // compiled in the same runtime assembly as the actor and receives real frames.
    [DefaultExecutionOrder(10000)]
    public sealed class PlayerCoreFrameDriverV1 : MonoBehaviour
    {
        public static Action InitializeCallback;
        public static Action ObserveCallback;
        private void Start() { InitializeCallback(); }
        private void LateUpdate() { ObserveCallback(); }
    }
}
#endif
