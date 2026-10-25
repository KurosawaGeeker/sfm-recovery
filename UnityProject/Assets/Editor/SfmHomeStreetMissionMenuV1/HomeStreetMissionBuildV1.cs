#if UNITY_EDITOR
using System;
using System.IO;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SfmMvp.Editor
{
    // Packaging only: existing scenes, settings, and independent save identity.
    public static class HomeStreetMissionBuildV1
    {
        const string Project = @"E:\ACTgame\SFM-LayerFS-Work\UnityProject";
        const string Root = @"C:\Users\Administrator\sfm-layerfs-dev\mission-menu-v1";
        static readonly string[] Scenes = {
            "Assets/SfmOriginalGameplayV3/Apartment.unity",
            "Assets/SfmOriginalResidencePortalV1/Night_Demo 1.unity",
            "Assets/SfmOriginalHomeReturnV1/SpringboardLoadEmpty.unity"
        };
        [Serializable] sealed class Receipt { public string result, output, editorGameSHA, playerGameSHA, product; public int errors, warnings; public long bytes; public bool runtimeAccepted; public string[] scenes; }
        static string Hash(string p) { using (var f = File.OpenRead(p)) using (var h = SHA256.Create()) return BitConverter.ToString(h.ComputeHash(f)).Replace("-", ""); }
        static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
        public static void Build()
        {
            Need(Path.GetFullPath(Path.Combine(Application.dataPath, "..")) == Project, "Current native Unity project required");
            Need(PlayerSettings.companyName == "SFMRecovery" && PlayerSettings.productName == "SFMOriginalGameplayRuntimeV2", "Preserve independent save identity");
            Need(PlayerSettings.GetScriptingBackend(BuildTargetGroup.Standalone) == ScriptingImplementation.Mono2x, "Existing Mono backend required");
            string[] args = Environment.GetCommandLineArgs(); int index = Array.IndexOf(args, "-sfmMissionBuildOutput");
            Need(index >= 0 && index + 1 < args.Length, "-sfmMissionBuildOutput required");
            string output = Path.GetFullPath(args[index + 1]);
            Need(output.StartsWith(Path.GetFullPath(Root) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) && !Directory.Exists(output), "Fresh output under mission-menu-v1 required");
            foreach (string scene in Scenes) Need(File.Exists(Path.Combine(Project, scene)), "Original scene exists: " + scene);
            Directory.CreateDirectory(output);
            string productBefore=PlayerSettings.productName;
            string settings=Path.Combine(Project,"ProjectSettings/ProjectSettings.asset"), settingsBefore=Hash(settings);
            int qaIndex=Array.IndexOf(args,"-sfmInputVisualBuildProduct"); string product=productBefore;
            if(qaIndex>=0) {
                Need(qaIndex+1<args.Length,"QA product argument"); product=args[qaIndex+1];
                const string prefix="SFMOriginalGameplayInputVisual-";
                Need(product.StartsWith(prefix,StringComparison.Ordinal)&&product.Length==prefix.Length+32&&Guid.TryParseExact(product.Substring(prefix.Length),"N",out _),"Unique QA build identity");
                string profile=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),"AppData/LocalLow/SFMRecovery",product);
                Need(!Directory.Exists(profile),"QA product profile has never been used");
            }
            BuildSummary summary;
            try {
                if(qaIndex>=0)PlayerSettings.productName=product;
                summary=BuildPipeline.BuildPlayer(new BuildPlayerOptions { scenes=Scenes, locationPathName=Path.Combine(output,"SFM_OriginalGameplay.exe"), target=BuildTarget.StandaloneWindows64, options=BuildOptions.None }).summary;
            } finally {
                if(qaIndex>=0) { PlayerSettings.productName=productBefore; AssetDatabase.SaveAssets(); }
                Need(PlayerSettings.productName==productBefore&&Hash(settings)==settingsBefore,"Product and settings bytes restored; do not launch if this fails");
            }
            string playerGame = Path.Combine(output, "SFM_OriginalGameplay_Data/Managed/Assembly-CSharp.dll");
            var receipt = new Receipt { result = summary.result.ToString(), output = output, product = product, errors = (int)summary.totalErrors, warnings = (int)summary.totalWarnings, bytes = (long)summary.totalSize, scenes = Scenes, editorGameSHA = Hash(Path.Combine(Project, "Library/ScriptAssemblies/Assembly-CSharp.dll")), playerGameSHA = File.Exists(playerGame) ? Hash(playerGame) : null, runtimeAccepted = false };
            File.WriteAllText(Path.Combine(output, "build.json"), JsonUtility.ToJson(receipt, true));
            Need(summary.result == BuildResult.Succeeded && summary.totalErrors == 0, "Player build succeeded without errors");
        }
    }
}
#endif
