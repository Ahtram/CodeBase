#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

/// <summary>
/// A static class for extend build-in GUIStyle.
/// </summary>
static public class GUIStylePlus {

	/// <summary>
	/// A modified centered label style.
	/// </summary>
	/// <returns></returns>
	static public GUIStyle CenteredLabel {
		get {
            GUIStyle returnStyle = new GUIStyle(GUI.skin.label);
            returnStyle.alignment = TextAnchor.MiddleCenter;
            return returnStyle;
        }
	}

}

#endif