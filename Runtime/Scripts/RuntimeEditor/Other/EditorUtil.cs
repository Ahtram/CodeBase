using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditorInternal;
using System.Reflection;

static class EditorUtil {
    
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
