#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditorInternal;
using System.Reflection;

static class EditorUtilPlus {

    /// <summary>
    /// Get all sorting layer names in the current project.
    /// </summary>
    /// <returns></returns>
    static public string[] GetSortingLayerNames() {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }

    //https://answers.unity.com/questions/36388/how-to-play-audioclip-in-edit-mode.html
    /// <summary>
    /// Preview AudioClip
    /// </summary>
    /// <param name="clip"></param>
    public static void PlayClip(AudioClip clip) {
        if (clip != null) {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod(
                "PlayClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new System.Type[] {
                typeof(AudioClip)
                },
                null
            );
            method.Invoke(
                null,
                new object[] {
                clip
                }
            );
        }
    }

    /// <summary>
    /// Stop previewing Clips.
    /// </summary>
    public static void StopAllClips() {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "StopAllClips",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new System.Type[] { },
            null
        );
        method.Invoke(
            null,
            new object[] { }
        );
    }

}

#endif