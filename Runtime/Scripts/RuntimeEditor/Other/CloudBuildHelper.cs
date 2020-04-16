using UnityEngine;
using UnityEditor;

public static class CloudBuildHelper {
#if UNITY_CLOUD_BUILD && (UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID)
    /// <summary>
    /// This method will be execute by UnityCloud before a build happen.
    /// </summary>
    /// <param name="manifest"></param>
    public static void PreExport(UnityEngine.CloudBuild.BuildManifestObject manifest)
    {
        string build = "";
        if (manifest.TryGetValue("buildNumber", out build)) {
            PlayerSettings.bundleVersion = string.Format("{0}.{1}", PlayerSettings.bundleVersion, build);
            PlayerSettings.iOS.buildNumber = build;
        
            int buildNo = 1;
            int.TryParse(build, out buildNo);
        
            PlayerSettings.Android.bundleVersionCode = buildNo;
        }
    }
#endif
}
