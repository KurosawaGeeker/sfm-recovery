#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SfmMvp.Editor
{
    // Packaging only: existing scenes, settings, and independent save identity.
    public static class SkillLearningPlayerBuildV1
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
        const string ExpectedProduct = "SFMOriginalGameplayInputVisual-31014133f09b4dad99db9b29b82fe544";
        const string ExpectedProfile = @"C:\Users\Administrator\AppData\LocalLow\SFMRecovery\SFMOriginalGameplayInputVisual-31014133f09b4dad99db9b29b82fe544";
        const string ContractRoot = @"C:\Users\Administrator\sfm-layerfs-dev\skill-learning-player-v1";
        [Serializable] sealed class Pin { public string path, sha256; public long bytes; }
        [Serializable] sealed class Contract { public string schema, product, profile, output; public bool ready; public Pin[] inputs, testSaves, protectedSaves; }
        static string Arg(string name) { var a=Environment.GetCommandLineArgs();int i=Array.IndexOf(a,name);Need(i>=0&&i+1<a.Length,"Missing "+name);return a[i+1]; }
        static void CheckPins(Pin[] pins) {
            Need(pins!=null,"Pins required");
            foreach(var pin in pins)Need(File.Exists(pin.path)&&new FileInfo(pin.path).Length==pin.bytes&&Hash(pin.path)==pin.sha256,"Bound file differs: "+pin.path);
        }
        static void CheckProfileInventory(string profile, Pin[] pins) {
            string root=Path.GetFullPath(profile).TrimEnd(Path.DirectorySeparatorChar,Path.AltDirectorySeparatorChar);
            Need(Directory.Exists(root),"Preserve the existing isolated skill profile");
            Need(pins!=null&&(pins.Length==3||pins.Length==4),"Exactly three saves and optional explicitly bound root Player.log");
            var comparer=StringComparer.OrdinalIgnoreCase;
            var required=new[]{Path.Combine(root,"SaveData","0.sd"),Path.Combine(root,"SaveData","s.sd"),Path.Combine(root,"SaveDataBackup","0.sd")};
            string log=Path.Combine(root,"Player.log");
            var expected=new System.Collections.Generic.HashSet<string>(comparer);
            foreach(var pin in pins) {
                Need(pin!=null&&!string.IsNullOrEmpty(pin.path)&&Path.IsPathRooted(pin.path),"Absolute profile pin required");
                string path=Path.GetFullPath(pin.path);
                Need(required.Contains(path,comparer)||comparer.Equals(path,log),"Unexpected contracted profile file: "+path);
                Need(expected.Add(path),"Duplicate contracted profile file: "+path);
            }
            Need(required.All(expected.Contains),"All three required save files must be bound");
            var actual=new System.Collections.Generic.HashSet<string>(comparer);
            var directories=new System.Collections.Generic.List<string>{root};
            for(int i=0;i<directories.Count;i++) {
                string directory=directories[i];
                Need((File.GetAttributes(directory)&FileAttributes.ReparsePoint)==0,"Profile directory reparse point rejected: "+directory);
                foreach(string entry in Directory.GetFileSystemEntries(directory)) {
                    var attributes=File.GetAttributes(entry);
                    Need((attributes&FileAttributes.ReparsePoint)==0,"Profile entry reparse point rejected: "+entry);
                    if((attributes&FileAttributes.Directory)!=0)directories.Add(entry);
                    else Need(actual.Add(Path.GetFullPath(entry)),"Duplicate actual profile file: "+entry);
                }
            }
            Need(actual.SetEquals(expected),"Exact complete unchanged isolated profile inventory");
            CheckPins(pins);
        }
        static void CheckProfile(Contract c) {
            CheckProfileInventory(ExpectedProfile,c.testSaves);
            CheckPins(c.protectedSaves);
        }
        static Contract ReadContract() {
            string path=Path.GetFullPath(Arg("-sfmSkillPlayerBuildContract"));
            Need(path.StartsWith(ContractRoot+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)&&Hash(path)==Arg("-sfmSkillPlayerBuildContractSHA"),"Exact reviewed packaging contract");
            var c=JsonUtility.FromJson<Contract>(File.ReadAllText(path));
            Need(c.schema=="SfmSkillPlayerBuildContract/v1"&&c.ready&&c.product==ExpectedProduct&&Path.GetFullPath(c.profile)==ExpectedProfile,"Dedicated existing test identity");
            Need(c.inputs!=null&&c.inputs.Length>0,"Packaging inputs required");CheckPins(c.inputs);CheckProfile(c);return c;
        }

        public static void Build()
        {
            Need(Path.GetFullPath(Path.Combine(Application.dataPath, "..")) == Project, "Current native Unity project required");
            Need(PlayerSettings.companyName == "SFMRecovery" && PlayerSettings.productName == "SFMOriginalGameplayRuntimeV2", "Preserve independent save identity");
            Need(PlayerSettings.GetScriptingBackend(BuildTargetGroup.Standalone) == ScriptingImplementation.Mono2x, "Existing Mono backend required");
            var contract=ReadContract();
            string[] args = Environment.GetCommandLineArgs(); int index = Array.IndexOf(args, "-sfmMissionBuildOutput");
            Need(index >= 0 && index + 1 < args.Length, "-sfmMissionBuildOutput required");
            string output = Path.GetFullPath(args[index + 1]);
            Need(output.StartsWith(Path.GetFullPath(Root) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) && !Directory.Exists(output), "Fresh output under mission-menu-v1 required");
            foreach (string scene in Scenes) Need(File.Exists(Path.Combine(Project, scene)), "Original scene exists: " + scene);
            Need(output==Path.GetFullPath(contract.output),"Exact fresh contracted output");
            Directory.CreateDirectory(output);
            string productBefore=PlayerSettings.productName;
            string settings=Path.Combine(Project,"ProjectSettings/ProjectSettings.asset"), settingsBefore=Hash(settings);
            int qaIndex=Array.IndexOf(args,"-sfmInputVisualBuildProduct"); string product=productBefore;
            Need(qaIndex>=0,"Explicit isolated QA product required");
            if(qaIndex>=0) {
                Need(qaIndex+1<args.Length,"QA product argument"); product=args[qaIndex+1];
                const string prefix="SFMOriginalGameplayInputVisual-";
                Need(product.StartsWith(prefix,StringComparison.Ordinal)&&product.Length==prefix.Length+32&&Guid.TryParseExact(product.Substring(prefix.Length),"N",out _),"Unique QA build identity");
                string profile=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),"AppData/LocalLow/SFMRecovery",product);
                Need(product==ExpectedProduct&&Path.GetFullPath(profile)==ExpectedProfile,"Existing hash-bound isolated profile; never recreate or seed");CheckProfile(contract);
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
            CheckPins(contract.inputs);CheckProfile(contract);
            Need(summary.result == BuildResult.Succeeded && summary.totalErrors == 0, "Player build succeeded without errors");
        }
    }
}
#endif
