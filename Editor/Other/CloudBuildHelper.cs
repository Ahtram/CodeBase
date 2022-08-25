using UnityEngine;
using UnityEditor;

public static class CloudBuildHelper {

#if UNITY_CLOUD_BUILD && (UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID)
    /// <summary>
    /// This method will be execute by UnityCloud before a build happen.
    /// </summary>
    /// <param name="manifest"></param>
    public static void PreExport(UnityEngine.CloudBuild.BuildManifestObject manifest) {
        string build = "";
        string cloudBuildTargetName = "";
        manifest.TryGetValue("cloudBuildTargetName", out cloudBuildTargetName);
        if (manifest.TryGetValue("buildNumber", out build)) {
            //Here we custom to bundleVersion for cloud build.
            PlayerSettings.bundleVersion = string.Format("{0} #{1} {2}", PlayerSettings.bundleVersion, build, cloudBuildTargetName);
            PlayerSettings.iOS.buildNumber = build;
        
            int buildNo = 1;
            int.TryParse(build, out buildNo);
        
            PlayerSettings.Android.bundleVersionCode = buildNo;
        }
    }
#endif

}
