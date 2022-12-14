#if UNITY_EDITOR

using System;
using UnityEditorInternal;
using System.Reflection;

static public class EditorUtil {

    /// <summary>
    /// Get all sorting layer names in the current project.
    /// </summary>
    /// <returns></returns>
    static public string[] GetSortingLayerNames() {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }

}

#endif