#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace SfmRecovery.LeavesDeployment
{
    // Packaging configuration only. The parent restores the exact previous settings
    // file after the owned Editor/build process exits, including failed builds.
    public static class LeavesPlayerGraphicsV1
    {
        const string Project = @"E:\ACTgame\SFM-LayerFS-Work\UnityProject";
        const string OutputRoot = @"C:\Users\Administrator\sfm-layerfs-dev\leaves-player-graphics-v1";
        static readonly string Settings = Path.Combine(Project,"ProjectSettings/ProjectSettings.asset");
        static readonly string Game = Path.Combine(Project,"Library/ScriptAssemblies/Assembly-CSharp.dll");
        [Serializable] sealed class Report
        {
            public string schema="SfmLeavesPlayerGraphicsConfiguration/v1", utc, beforeSettingsSHA, configuredSettingsSHA;
            public string beforeGameSHA, afterGameSHA, product, editorDevice, error;
            public bool originalUseDefault, configured, gameUnchanged, productUnchanged;
            public string[] originalAPIs, configuredAPIs;
        }
        static void Need(bool ok,string label) { if(!ok)throw new InvalidOperationException(label); }
        static string Hash(string path)
        {
            using(var f=File.OpenRead(path))using(var h=SHA256.Create())
                return BitConverter.ToString(h.ComputeHash(f)).Replace("-","");
        }
        public static void Configure()
        {
            string output=Environment.GetEnvironmentVariable("SFM_LEAVES_GRAPHICS_REPORT");
            Need(!string.IsNullOrEmpty(output),"Fresh graphics configuration report is required");
            output=Path.GetFullPath(output);
            Need(output.StartsWith(OutputRoot+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)
                && !File.Exists(output),"Report must be new and in the dedicated controller directory");
            Need(Path.GetFullPath(Path.Combine(Application.dataPath,".."))==Project,"Only the sole native project");
            Need(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SFM_LEAVES_DOMAIN_MODE")),"Configuration is not a shader build");
            Need(PlayerSettings.companyName=="SFMRecovery" && PlayerSettings.productName=="SFMOriginalGameplayRuntimeV2","Original project identity");
            Need(EditorUserBuildSettings.activeBuildTarget==BuildTarget.StandaloneWindows64,"Windows64 active target required");
            Need(Hash(Settings)==Environment.GetEnvironmentVariable("SFM_LEAVES_SETTINGS_BEFORE_SHA"),"External exact settings snapshot required");
            Need(Hash(Game)==Environment.GetEnvironmentVariable("SFM_LEAVES_GAME_BEFORE_SHA"),"External exact Game snapshot required");
            var r=new Report{utc=DateTime.UtcNow.ToString("o"),beforeSettingsSHA=Hash(Settings),beforeGameSHA=Hash(Game),
                product=PlayerSettings.productName,editorDevice=SystemInfo.graphicsDeviceType.ToString(),
                originalUseDefault=PlayerSettings.GetUseDefaultGraphicsAPIs(BuildTarget.StandaloneWindows64),
                originalAPIs=PlayerSettings.GetGraphicsAPIs(BuildTarget.StandaloneWindows64).Select(x=>x.ToString()).ToArray()};
            try
            {
                PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.StandaloneWindows64,false);
                PlayerSettings.SetGraphicsAPIs(BuildTarget.StandaloneWindows64,new[]{GraphicsDeviceType.Direct3D11});
                AssetDatabase.SaveAssets();
                r.configuredAPIs=PlayerSettings.GetGraphicsAPIs(BuildTarget.StandaloneWindows64).Select(x=>x.ToString()).ToArray();
                r.configuredSettingsSHA=Hash(Settings);r.afterGameSHA=Hash(Game);
                r.gameUnchanged=r.beforeGameSHA==r.afterGameSHA;r.productUnchanged=r.product==PlayerSettings.productName;
                Need(!PlayerSettings.GetUseDefaultGraphicsAPIs(BuildTarget.StandaloneWindows64)
                    && r.configuredAPIs.SequenceEqual(new[]{"Direct3D11"}),"Explicit D3D11-only Player configuration");
                Need(r.gameUnchanged && r.productUnchanged,"Configuration must preserve Game and product");
                r.configured=true;
            }
            catch(Exception ex) { r.error=ex.ToString();throw; }
            finally
            {
                Directory.CreateDirectory(Path.GetDirectoryName(output));
                File.WriteAllText(output,JsonUtility.ToJson(r,true));
            }
        }
    }
}
#endif
