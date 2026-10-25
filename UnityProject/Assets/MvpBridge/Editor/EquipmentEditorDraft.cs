#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
namespace SfmMvp {
    // Builder helper for a NEW root-owned runtime Project. Does not open/run Unity itself.
    public static class EquipmentEditorDraft {
        static T Original<T>(string guid) where T:UnityEngine.Object {
            string p=AssetDatabase.GUIDToAssetPath(guid);
            if(string.IsNullOrEmpty(p))throw new Exception("Missing original GUID "+guid);
            var a=AssetDatabase.LoadAssetAtPath<T>(p);
            if(!a)throw new Exception("Original GUID has wrong type "+guid+" "+p);
            return a;
        }
        public static EquipmentBootstrapDraft Attach(GameObject ownedHost,CoreGameplayData data,
            ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController player,Transform visibleOriginalManustia) {
            var qa=ownedHost.AddComponent<EquipmentBootstrapDraft>();
            qa.Data=data;qa.Player=player;qa.OriginalManustia=visibleOriginalManustia;
            qa.OriginalInventoryPanelPrefab=Original<GameObject>("5e39bf376776c8446a2a1f17c503d5d1");
            qa.EquipAdClip=Original<AudioClip>("65f0bc289dd9bb243aef242ffa21cc0b");
            qa.SelectClip=Original<AudioClip>("b6061409f8bed9b4f9d2bbd698f3c1dc");
            qa.DecideClip=Original<AudioClip>("a28961a7c7b137042bd71240111af748");
            qa.BuyClip=Original<AudioClip>("a348b19d9225112449343f7de5d4cb9f");
            // Root's single coordinator should await Qa AFTER Body and BEFORE action QA.
            // Isolated -sfmEquipmentSmoke may explicitly set RunQa=true; never run both paths.
            qa.RunQa=false;return qa;
        }
    }
}
#endif
