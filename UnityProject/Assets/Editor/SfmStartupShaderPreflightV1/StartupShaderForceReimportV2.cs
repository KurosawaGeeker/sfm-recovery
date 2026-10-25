#if UNITY_EDITOR
using UnityEditor;
namespace SfmMvp.Editor
{
    public static class StartupShaderForceReimportV2
    {
        public static void Check()
        {
            // The old failed import may not have registered missing include dependencies.
            // Reimport the exact unchanged original compute after closing its inputs.
            AssetDatabase.ImportAsset("Assets/ComputeShader/Lut3DBaker.compute",ImportAssetOptions.ForceUpdate|ImportAssetOptions.ForceSynchronousImport);
            StartupShaderPreflightV1.Check();
        }
    }
}
#endif
