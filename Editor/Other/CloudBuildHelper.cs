using UnityEngine;
using UnityEditor;
using Teamuni.Codebase;

public static class CloudBuildHelper {
#if UNITY_CLOUD_BUILD && (UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID)
    /// <summary>
    /// This method will be execute by UnityCloud before a build happen.
    /// </summary>
    /// <param name="manifest"></param>
    public static void PreExport(UnityEngine.CloudBuild.BuildManifestObject manifest)
    {
        //[Dep]: I don't like this.
        // string build = "";
        // if (manifest.TryGetValue("buildNumber", out build)) {
        //     PlayerSettings.bundleVersion = string.Format("{0}.{1}", PlayerSettings.bundleVersion, build);
        // }

        //Test: Setup the manifest buildNumber?? Will this work?!
        manifest.SetValue("buildNumber", Version.BundleVersion());

        //[iOS stuff]: a string buildNumber
        // PlayerSettings.iOS.buildNumber = build;
    
        //[Android stuff]: An int less than 100000.
        // PlayerSettings.Android.bundleVersionCode = buildNo;
    }
#endif
}
