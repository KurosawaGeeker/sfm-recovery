#if UNITY_EDITOR
using System;using System.IO;using System.Collections.Generic;using System.Text.RegularExpressions;using UnityEditor;using UnityEngine;
namespace SfmMvp.Editor
{
    public static class StartupShaderPreflightV1
    {
        [Serializable]public sealed class ShaderRow{public string path,name;public bool found,supported,hasError;}
        [Serializable]public sealed class KernelRow{public string name;public bool hasKernel;public int index;public uint x,y,z;}
        [Serializable]public sealed class Report{public bool passed,PlayMode=false,originalGameCalls=false,renderAccepted=false;public string device,failure;public List<ShaderRow> shaders=new List<ShaderRow>();public List<KernelRow> kernels=new List<KernelRow>();}
        public static void Check()
        {
            const string Project="E:/ACTgame/SFM/ReverseEngineering/tools/unity-editor/core-gameplay-bootstrap-runtime-candidate-v2/Project";
            if(Path.GetFullPath(Path.Combine(Application.dataPath,".."))!=Path.GetFullPath(Project)||EditorApplication.isPlayingOrWillChangePlaymode)throw new InvalidOperationException("Exact independent EditMode");
            var args=Environment.GetCommandLineArgs();int k=Array.IndexOf(args,"-sfmStartupShaderOutputV1");if(k<0||k+1>=args.Length)throw new InvalidOperationException("Explicit fresh report");string output=Path.GetFullPath(args[k+1]);if(File.Exists(output))throw new InvalidOperationException("Fresh report");Directory.CreateDirectory(Path.GetDirectoryName(output));var r=new Report{device=SystemInfo.graphicsDeviceType+":"+SystemInfo.graphicsDeviceName};
            try
            {
                foreach(string path in new[]{"Assets/Resources/fillcrop.shader","Assets/Resources/efficientblur.shader","Assets/Shader/Hidden_PostProcessing_Uber.shader","Assets/Shader/Hidden_PostProcessing_SubpixelMorphologicalAntialiasing.shader"})
                {
                    var shader=AssetDatabase.LoadAssetAtPath<Shader>(path);var row=new ShaderRow{path=path,name=shader?shader.name:null,found=shader&&Shader.Find(shader.name)==shader,supported=shader&&shader.isSupported,hasError=!shader||ShaderUtil.ShaderHasError(shader)};r.shaders.Add(row);if(!row.found||!row.supported||row.hasError)throw new InvalidOperationException("Startup shader compile/find: "+path);
                }
                const string computePath="Assets/ComputeShader/Lut3DBaker.compute";var compute=AssetDatabase.LoadAssetAtPath<ComputeShader>(computePath);if(!compute)throw new InvalidOperationException("Original LUT compute");
                foreach(Match match in Regex.Matches(File.ReadAllText(Path.Combine(Project,computePath)),@"(?m)^#pragma kernel\s+(\w+)")){string name=match.Groups[1].Value;var row=new KernelRow{name=name,hasKernel=compute.HasKernel(name)};r.kernels.Add(row);if(!row.hasKernel)throw new InvalidOperationException("Original kernel unavailable: "+name);row.index=compute.FindKernel(name);compute.GetKernelThreadGroupSizes(row.index,out row.x,out row.y,out row.z);if(row.x==0||row.y==0||row.z==0)throw new InvalidOperationException("Kernel group size");}
                if(r.kernels.Count!=4)throw new InvalidOperationException("Exact four original LUT kernels");r.passed=true;
            }
            catch(Exception e){r.failure=e.ToString();}
            File.WriteAllText(output,JsonUtility.ToJson(r,true)+"\n");Debug.Log("SFM_STARTUP_SHADER_PREFLIGHT "+r.passed);if(!r.passed)throw new InvalidOperationException(r.failure);
        }
    }
}
#endif
