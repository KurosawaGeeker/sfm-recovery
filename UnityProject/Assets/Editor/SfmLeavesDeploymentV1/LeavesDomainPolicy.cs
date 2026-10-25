using System;
using System.Collections.Generic;

namespace SfmRecovery.LeavesDeployment
{
    [Serializable] public sealed class DomainRow
    {
        public int passIndex, stageIndex, blobIndex, gpuProgramType, requirementBits;
        public string keywords, originalDXBCSHA256, candidateProgramKey;
    }
    [Serializable] public sealed class DomainManifest
    {
        public string schema, shaderGuid, shaderPath, shaderSourceSHA256, sourceOriginalRawSHA256;
        public string[] passNames;
        public DomainRow[] originalRows, excludedRows;
    }
    public enum DomainDecision { Original, ProvenAbsentNonDirectional, Unknown }
    // Pure finite-set classifier. No Unity API, shader source rewrite, or fallback variants.
    public sealed class LeavesDomainPolicy
    {
        readonly Dictionary<string,DomainRow> originals = new Dictionary<string,DomainRow>(StringComparer.Ordinal);
        readonly HashSet<string> exclusions = new HashSet<string>(StringComparer.Ordinal);
        public static string Normalize(IEnumerable<string> keywords)
        {
            var names = new SortedSet<string>(keywords,StringComparer.Ordinal);
            if (names.Contains("") || names.Contains(null)) throw new ArgumentException("Empty keyword name");
            return string.Join(" ",names);
        }
        static string Key(int pass, int stage, string words) { return pass+":"+stage+":"+words; }
        public static bool NonDirectional(int pass, string words)
        {
            var set = new HashSet<string>(words.Split(new[]{' '},StringSplitOptions.RemoveEmptyEntries),StringComparer.Ordinal);
            return (pass==0 || pass==2) && (set.Contains("LIGHTMAP_ON") || set.Contains("DYNAMICLIGHTMAP_ON")) && !set.Contains("DIRLIGHTMAP_COMBINED");
        }
        public LeavesDomainPolicy(DomainManifest manifest)
        {
            if (manifest.schema!="SfmLeavesOriginalDomain/v1" || manifest.originalRows.Length!=624 || manifest.excludedRows.Length!=312)
                throw new ArgumentException("Unrecognized domain manifest");
            foreach (var r in manifest.originalRows)
            {
                if (r.passIndex<0 || r.passIndex>4 || r.stageIndex<0 || r.stageIndex>1 || r.originalDXBCSHA256==null || r.originalDXBCSHA256.Length!=64 || NonDirectional(r.passIndex,r.keywords))
                    throw new ArgumentException("Invalid original row");
                originals.Add(Key(r.passIndex,r.stageIndex,r.keywords),r);
            }
            foreach (var r in manifest.excludedRows)
            {
                string k=Key(r.passIndex,r.stageIndex,r.keywords);
                if (r.stageIndex<0 || r.stageIndex>1 || !NonDirectional(r.passIndex,r.keywords) || originals.ContainsKey(k) || !exclusions.Add(k))
                    throw new ArgumentException("Invalid proven-absent row");
            }
        }
        public DomainDecision Classify(int pass, int stage, IEnumerable<string> words, out DomainRow original)
        {
            string k=Key(pass,stage,Normalize(words));
            if (originals.TryGetValue(k,out original)) return DomainDecision.Original;
            return exclusions.Contains(k) ? DomainDecision.ProvenAbsentNonDirectional : DomainDecision.Unknown;
        }
    }
}
