#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
namespace SfmMvp.Editor
{
    // Owned packaging only. Original Home/Portal scenes own all gameplay.
    public static class OriginalGameplayPlayerBuildV7
    {
        const string Project=@"E:\ACTgame\SFM\ReverseEngineering\tools\unity-editor\core-gameplay-bootstrap-runtime-candidate-v2\Project";
        const string Stage=@"E:\ACTgame\SFM\ReverseEngineering\tools\unity-editor\original-gameplay-player-candidate-v7";
        const string Home="Assets/SfmOriginalGameplayV3/Apartment.unity";
        const string Residence="Assets/SfmOriginalResidencePortalV1/Night_Demo 1.unity";
        const string ReturnTransit="Assets/SfmOriginalHomeReturnV1/SpringboardLoadEmpty.unity"; // Required loading transit, not a playable map.
        [Serializable] sealed class Pin { public string path=null,sha256=null;public long bytes=0; }
        [Serializable] sealed class Contract { public bool ready=false;public string editorGameSHA=null;public Pin[] inputs=null; }
        [Serializable] sealed class Result { public string result,editorGameSHA,output;public int errors,warnings;public long bytes;public string[] scenes;public bool GameFieldsWritten,GameMethodsCalled,scenesModified,settingsModified,PlayerRuntimeAccepted; }
        static string Arg(string name){var a=Environment.GetCommandLineArgs();int i=Array.IndexOf(a,name);if(i<0||i+1>=a.Length)throw new InvalidOperationException("Missing "+name);return a[i+1];}
                // Windows long-path exact file reads copied from the reviewed V11 observer.
        [System.Runtime.InteropServices.DllImport("kernel32.dll",CharSet=System.Runtime.InteropServices.CharSet.Unicode,SetLastError=true)]static extern IntPtr CreateFileW(string path,uint access,uint share,IntPtr security,uint creation,uint flags,IntPtr template);
        [System.Runtime.InteropServices.DllImport("kernel32.dll",SetLastError=true)]static extern bool GetFileSizeEx(IntPtr handle,out long size);
        [System.Runtime.InteropServices.DllImport("kernel32.dll",SetLastError=true)]static extern bool ReadFile(IntPtr handle,byte[] buffer,uint count,out uint read,IntPtr overlapped);
        [System.Runtime.InteropServices.DllImport("kernel32.dll",SetLastError=true)]static extern bool CloseHandle(IntPtr handle);
        static byte[] ReadBoundFile(string p){string full=Path.GetFullPath(p);if(full.Length<260)return File.ReadAllBytes(full);IntPtr h=CreateFileW(@"\\?\"+full,0x80000000,7,IntPtr.Zero,3,128,IntPtr.Zero);if(h==new IntPtr(-1))throw new System.ComponentModel.Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error(),full);try{long size;if(!GetFileSizeEx(h,out size)||size<0||size>int.MaxValue)throw new IOException("Bound size");var b=new byte[(int)size];uint n;if(!ReadFile(h,b,(uint)b.Length,out n,IntPtr.Zero)||n!=b.Length)throw new IOException("Exact read "+full);return b;}finally{CloseHandle(h);}}
        static string HashBytes(byte[] bytes){using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(bytes)).Replace("-","");}
        static string Hash(string p){if(Path.GetFullPath(p).Length>=260)return HashBytes(ReadBoundFile(p));using(var h=SHA256.Create())using(var f=File.OpenRead(p))return BitConverter.ToString(h.ComputeHash(f)).Replace("-","");}

        static void Need(bool v,string label){if(!v)throw new InvalidOperationException(label);}
        static void Check(Contract c){foreach(var p in c.inputs){if(Path.GetFullPath(p.path).Length>=260){var b=ReadBoundFile(p.path);Need(b.LongLength==p.bytes&&HashBytes(b)==p.sha256,"Bound long input changed: "+p.path);}else Need(new FileInfo(p.path).Length==p.bytes&&Hash(p.path)==p.sha256,"Bound input changed: "+p.path);}}
        public static void Build()
        {
            Need(Path.GetFullPath(Path.Combine(Application.dataPath,".."))==Project,"Exact independent Project");
            string contractPath=Path.GetFullPath(Arg("-sfmOriginalPlayerV7Contract"));
            Need(contractPath.StartsWith(Stage+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)&&Hash(contractPath)==Arg("-sfmOriginalPlayerV7ContractSHA"),"Exact prepared packaging contract");
            var c=JsonUtility.FromJson<Contract>(File.ReadAllText(contractPath));Need(c.ready&&c.inputs!=null&&c.inputs.Length>0,"Passed whole-source binding prerequisite");Check(c);
            string game=typeof(InGameManager).Assembly.Location;Need(Hash(game)==c.editorGameSHA,"Exact fully bound original Game");
            Need(PlayerSettings.companyName=="SFMRecovery"&&PlayerSettings.productName=="SFMOriginalGameplayRuntimeV2","Independent save identity");
            Need(PlayerSettings.GetScriptingBackend(BuildTargetGroup.Standalone)==ScriptingImplementation.Mono2x,"Existing reviewed Mono backend");
            Need(Hash(Path.Combine(Project,Home))=="2A8DE3F5C1C566FB991F54EB890E1F7B1BA93CF8446F62E1CEF944C538B4849E"&&Hash(Path.Combine(Project,Residence))=="F18AC4C0F9A4B21A6DD42F44BAEA38C83040B0409F4C9762B84BB7EC3572C9C1","Exact reviewed original Home and Residence alias");
            Need(Hash(Path.Combine(Project,ReturnTransit))=="DFD9AAE1F1523F25B432765E26F4376B6005FDFD71A7908AE0A23D9990521567","Exact original Home-return loading transit");
            string output=Path.GetFullPath(Arg("-sfmOriginalPlayerV7Output"));Need(output.StartsWith(Stage+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)&&!Directory.Exists(output),"Fresh owned output");Directory.CreateDirectory(output);
            var scenes=new[]{Home,Residence,ReturnTransit};var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=scenes,locationPathName=Path.Combine(output,"SFM_OriginalGameplay.exe"),target=BuildTarget.StandaloneWindows64,options=BuildOptions.None});
            var r=new Result{result=report.summary.result.ToString(),errors=(int)report.summary.totalErrors,warnings=(int)report.summary.totalWarnings,bytes=(long)report.summary.totalSize,editorGameSHA=c.editorGameSHA,output=output,scenes=scenes,GameFieldsWritten=false,GameMethodsCalled=false,scenesModified=false,settingsModified=false,PlayerRuntimeAccepted=false};
            File.WriteAllText(Path.Combine(output,"build.json"),JsonUtility.ToJson(r,true)+"\n");Check(c);Need(Hash(game)==c.editorGameSHA,"Original Editor Game remains exact");
            Need(report.summary.result==BuildResult.Succeeded,"Original gameplay Player build failed");
        }
    }
}
#endif
