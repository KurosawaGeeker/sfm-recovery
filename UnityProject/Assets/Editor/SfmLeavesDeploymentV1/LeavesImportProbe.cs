using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

namespace SfmRecovery.LeavesDeployment
{
    public static class LeavesImportProbe
    {
        const string ShaderPath="Assets/Shader/AE_Leaves.shader";
        const string MaterialPath="Assets/Material/Flowers.mat";
        const string ShaderSHA="2EDA94771C68374F09967EDAB80DA3C20242A8B73E0681E667BAC01517E87B9F";
        const string MaterialSHA="07C34F63BDABDE5A051DF2FDD56DDA330BA6C8CB6C461967F03114E04742421F";
        [Serializable] sealed class Message { public string phase,severity,file,message,details; public int line; }
        [Serializable] sealed class Report
        {
            public string schema="SfmLeavesImportProbe/v1",utc,unityVersion,graphicsAPI,gameBeforeSHA,gameAfterSHA;
            public bool passed,importClean,defaultMaterialPassesRequested,buildStrippingExecuted,gameUnchanged,inputsUnchanged;
            public string[] materialKeywords;
            public List<string> errors=new List<string>();public List<Message> messages=new List<Message>();
            public List<string> requestedPasses=new List<string>();
            public int engineErrorCount; public string firstEngineError,firstEngineStack;
        }
        static string Hash(string path)
        {
            using(var stream=File.OpenRead(path))using(var sha=SHA256.Create())return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-","");
        }
        static void Require(bool ok,string why){if(!ok)throw new InvalidOperationException(why);}
        static bool Messages(Shader shader,string phase,Report report)
        {
            bool ok=true;
            foreach(var m in ShaderUtil.GetShaderMessages(shader))
            {
                report.messages.Add(new Message{phase=phase,severity=m.severity.ToString(),file=m.file,line=m.line,message=m.message,details=m.messageDetails});
                if(m.severity==ShaderCompilerMessageSeverity.Error)ok=false;
            }
            return ok;
        }
        public static void Run()
        {
            string output=Environment.GetEnvironmentVariable("SFM_LEAVES_IMPORT_REPORT");
            Require(!string.IsNullOrEmpty(output) && !File.Exists(output),"Fresh SFM_LEAVES_IMPORT_REPORT required");
            var report=new Report{utc=DateTime.UtcNow.ToString("o"),unityVersion=Application.unityVersion,graphicsAPI=SystemInfo.graphicsDeviceType.ToString(),buildStrippingExecuted=false};
            Application.LogCallback log=(message,stack,type)=>{
                if(type!=LogType.Error && type!=LogType.Assert && type!=LogType.Exception)return;
                report.engineErrorCount++;if(report.firstEngineError==null){report.firstEngineError=message;report.firstEngineStack=stack;}
            };
            Application.logMessageReceived+=log;
            try
            {
                Require(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SFM_LEAVES_DOMAIN_MODE")),"Import probe must run without build filter; it cannot certify build-time stripping");
                Require(Hash(ShaderPath)==ShaderSHA && Hash(MaterialPath)==MaterialSHA,"Shader/material source pin mismatch");
                Require(AssetDatabase.AssetPathToGUID(ShaderPath)=="19093b6811b98ad44bba3e777af73023","Shader GUID mismatch");
                report.gameBeforeSHA=Hash("Library/ScriptAssemblies/Assembly-CSharp.dll");
                AssetDatabase.ImportAsset(ShaderPath,ImportAssetOptions.ForceUpdate|ImportAssetOptions.ForceSynchronousImport);
                var shader=AssetDatabase.LoadAssetAtPath<Shader>(ShaderPath);Require(shader!=null,"Shader import returned null");
                report.importClean=Messages(shader,"after-import",report);
                Require(report.importClean,"Import already reports errors; build stripping has not run and is not a workaround");
                Require(SystemInfo.graphicsDeviceType==GraphicsDeviceType.Direct3D11,"Default variant probe requires a D3D11 Editor; do not use -nographics");
                var mat=AssetDatabase.LoadAssetAtPath<Material>(MaterialPath);Require(mat!=null && mat.shader==shader,"Flowers binding mismatch");
                report.materialKeywords=mat.shaderKeywords;
                Require(mat.passCount==5,"Expected the original five passes");
                for(int i=0;i<mat.passCount;i++)
                {
                    report.requestedPasses.Add(i+":"+mat.GetPassName(i));
                    ShaderUtil.CompilePass(mat,i,true);
                    Require(Messages(shader,"default-material-pass-"+i,report),"An Editor-selected default material variant failed; keep guards and inspect messages");
                }
                report.defaultMaterialPassesRequested=true;
                report.inputsUnchanged=Hash(ShaderPath)==ShaderSHA && Hash(MaterialPath)==MaterialSHA;
                report.gameAfterSHA=Hash("Library/ScriptAssemblies/Assembly-CSharp.dll");report.gameUnchanged=report.gameAfterSHA==report.gameBeforeSHA;
                Require(report.inputsUnchanged && report.gameUnchanged && report.engineErrorCount==0,"Source/Game changed or engine error observed");
                report.passed=true;
            }
            catch(Exception ex){report.errors.Add(ex.ToString());}
            finally
            {
                Application.logMessageReceived-=log;
                Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(output)));
                File.WriteAllText(output,JsonUtility.ToJson(report,true));
            }
            EditorApplication.Exit(report.passed?0:2);
        }
    }
}
