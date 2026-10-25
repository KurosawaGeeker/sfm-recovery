// Editor-only, opt-in candidate. Preserves the original ShaderLab guards and all HLSL files.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

namespace SfmRecovery.LeavesDeployment
{
    public sealed class LeavesOriginalDomainProcessor : IPreprocessShaders, IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        const string ManifestSHA = "31F6D96A1CE1C61C5B36AAFA5C2829C4BD15D41A1A836112C1BAAE6384C0D613";
        const string Guid = "19093b6811b98ad44bba3e777af73023";
        const string ShaderPath = "Assets/Shader/AE_Leaves.shader";
        static DomainManifest manifest;
        static LeavesDomainPolicy policy;
        static string mode, output;
        static int callbacks, retained, removed, unknown;
        public int callbackOrder { get { return 0; } }
        [Serializable] sealed class Variant
        {
            public string keywords, decision, platform, tier, originalDXBCSHA256;
            public int originalBlobIndex, requirementBits;
        }
        [Serializable] sealed class Receipt
        {
            public string kind, utc, mode, manifestSHA256, shaderSHA256, unityVersion, passName, passType, stage;
            public int input, originalCandidates, excludedCandidates, kept, dropped, unknown, callbackCount;
            public bool listModified, completedBuild, callbacksObserved, allObservedKeptOriginal;
            public List<Variant> rows = new List<Variant>();
        }
        static string Hash(string p)
        {
            using(var file=File.OpenRead(p)) using(var sha=SHA256.Create())
                return BitConverter.ToString(sha.ComputeHash(file)).Replace("-","");
        }
        static bool Active()
        {
            string value=Environment.GetEnvironmentVariable("SFM_LEAVES_DOMAIN_MODE");
            if (string.IsNullOrEmpty(value)) return false;
            if (value!="audit" && value!="strip-nondirectional") throw new BuildFailedException("Unknown SFM_LEAVES_DOMAIN_MODE");
            if (mode!=null && mode!=value) throw new BuildFailedException("Leaves domain mode changed during build");
            mode=value; return true;
        }
        static void Write(Receipt r)
        {
            r.utc=DateTime.UtcNow.ToString("o");r.mode=mode;r.unityVersion=Application.unityVersion;
            File.AppendAllText(output,JsonUtility.ToJson(r)+Environment.NewLine);
        }
        static void Initialize()
        {
            if(policy!=null)return;
            string domain=Environment.GetEnvironmentVariable("SFM_LEAVES_DOMAIN_MANIFEST");
            output=Environment.GetEnvironmentVariable("SFM_LEAVES_DOMAIN_REPORT");
            if(string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(output)) throw new BuildFailedException("Leaves domain manifest/report path missing");
            if(Hash(domain)!=ManifestSHA)throw new BuildFailedException("Leaves domain manifest pin mismatch");
            if(File.Exists(output))throw new BuildFailedException("Leaves report must be a fresh file");
            manifest=JsonUtility.FromJson<DomainManifest>(File.ReadAllText(domain));
            if(manifest.shaderGuid!=Guid || manifest.shaderPath!=ShaderPath)throw new BuildFailedException("Leaves target identity mismatch");
            policy=new LeavesDomainPolicy(manifest);
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(output)));
            Write(new Receipt{kind="begin",manifestSHA256=ManifestSHA,shaderSHA256=manifest.shaderSourceSHA256});
        }
        static void CheckTarget(bool requireBuildingPlayer=true)
        {
            if(AssetDatabase.AssetPathToGUID(ShaderPath)!=Guid || Hash(ShaderPath)!=manifest.shaderSourceSHA256)
                throw new BuildFailedException("Leaves shader path/GUID/source pin mismatch");
            if(mode=="strip-nondirectional" && requireBuildingPlayer)
            {
                if(!BuildPipeline.isBuildingPlayer || EditorUserBuildSettings.activeBuildTarget!=BuildTarget.StandaloneWindows64)
                    throw new BuildFailedException("Leaves stripping is scoped to a Windows64 Player build");
                var apis=PlayerSettings.GetGraphicsAPIs(BuildTarget.StandaloneWindows64);
                if(PlayerSettings.GetUseDefaultGraphicsAPIs(BuildTarget.StandaloneWindows64) || apis.Length!=1 || apis[0]!=GraphicsDeviceType.Direct3D11)
                    throw new BuildFailedException("Leaves stripping requires explicit D3D11-only target APIs");
            }
        }
        static int Pass(PassType type)
        {
            switch(type)
            {
                case PassType.ForwardBase:return 0;
                case PassType.ForwardAdd:return 1;
                case PassType.Deferred:return 2;
                case PassType.ShadowCaster:return 3;
                case PassType.Meta:return 4;
                default:return -1;
            }
        }
        public void OnPreprocessBuild(BuildReport report)
        {
            if(!Active())return;
            // One fresh report/session per build; no work on other shader families.
            manifest=null;policy=null;callbacks=retained=removed=unknown=0;
            Initialize();CheckTarget();
        }
        public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data)
        {
            if(!Active())return;
            string path=AssetDatabase.GetAssetPath(shader);
            if(AssetDatabase.AssetPathToGUID(path)!=Guid)return;
            Initialize();CheckTarget();
            int p=Pass(snippet.passType),s=snippet.shaderType==ShaderType.Vertex?0:snippet.shaderType==ShaderType.Fragment?1:-1;
            if(p<0 || s<0 || snippet.passName!=manifest.passNames[p])throw new BuildFailedException("Unexpected Leaves pass/stage");
            if(data.Count>10000 || callbacks>=100)throw new BuildFailedException("Leaves bounded audit limit exceeded");
            var r=new Receipt{kind="snippet",passName=snippet.passName,passType=snippet.passType.ToString(),stage=snippet.shaderType.ToString(),input=data.Count};
            var decisions=new List<DomainDecision>(data.Count);
            foreach(var item in data)
            {
                var names=item.shaderKeywordSet.GetShaderKeywords().Select(k=>k.name).ToArray();
                DomainRow original;var choice=policy.Classify(p,s,names,out original);
                if(item.shaderCompilerPlatform!=ShaderCompilerPlatform.D3D)choice=DomainDecision.Unknown;
                if(original!=null && (int)item.shaderRequirements!=original.requirementBits)choice=DomainDecision.Unknown;
                decisions.Add(choice);
                r.rows.Add(new Variant{keywords=LeavesDomainPolicy.Normalize(names),decision=choice.ToString(),
                    platform=item.shaderCompilerPlatform.ToString(),tier=item.graphicsTier.ToString(),requirementBits=(int)item.shaderRequirements,
                    originalDXBCSHA256=original==null?null:original.originalDXBCSHA256,originalBlobIndex=original==null?-1:original.blobIndex});
                if(choice==DomainDecision.Original)r.originalCandidates++;
                else if(choice==DomainDecision.ProvenAbsentNonDirectional)r.excludedCandidates++;
                else r.unknown++;
            }
            ++callbacks;r.callbackCount=callbacks;
            if(r.unknown!=0)
            {
                unknown+=r.unknown;Write(r);
                throw new BuildFailedException("Unknown Leaves variants logged; no entry in this callback was removed");
            }
            // Only after every entry is classified may the exact proved-absent subset be removed.
            if(mode=="strip-nondirectional")
                for(int i=decisions.Count-1;i>=0;--i)if(decisions[i]==DomainDecision.ProvenAbsentNonDirectional)data.RemoveAt(i);
            r.kept=data.Count;r.dropped=r.input-data.Count;r.listModified=r.dropped>0;
            r.allObservedKeptOriginal=mode=="strip-nondirectional" || r.excludedCandidates==0;
            retained+=r.kept;removed+=r.dropped;Write(r);
        }
        public void OnPostprocessBuild(BuildReport report)
        {
            if(!Active())return;
            Initialize();CheckTarget(false);
            Write(new Receipt{kind="end",completedBuild=report.summary.result==BuildResult.Succeeded,callbacksObserved=callbacks>0,
                callbackCount=callbacks,kept=retained,dropped=removed,unknown=unknown,
                allObservedKeptOriginal=callbacks>0 && retained>0 && unknown==0 && mode=="strip-nondirectional"});
        }
    }
}
