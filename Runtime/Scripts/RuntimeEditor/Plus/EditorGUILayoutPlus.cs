#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// A static class for extend build-in Unity EditorGUILayout serious classes.
/// </summary>
static public class EditorGUILayoutPlus {

    //For the copy/paste tricks.
    static public Vector3 vector3CopyBuffer;
    static public Vector2 vector2CopyBuffer;
    static public Vec2 vec2CopyBuffer;
    static public List<Vec2> vec2ListCopyBuffer;
    static public Vec3 vec3CopyBuffer;
    static public List<Vec3> vec3ListCopyBuffer;
    static public Rect rectCopyBuffer;
    static public List<string> stringListBuffer;
    static public List<int> intListBuffer;
    static public List<bool> boolListBuffer;
    static public List<float> floatListBuffer;
    static public Color colorCopyBuffer;
    static public float floatCopyBuffer;
    static public int intCopyBuffer;
    static public object objectCopyBuffer;
    static public List<object> objectListCopyBuffer;

    /// <summary>
    /// Draw a set of tabs with the built-in editor mini buttons.
    /// </summary>
    /// <param name="options">A string array for display name of all tab.</param>
    /// <param name="activeIndex">Input the current active tab's index.</param>
    /// <returns>The new active tab index.</returns>
    static public int Tabs(string[] options, int activeIndex) {
        int returnIndex = activeIndex;

        for (int i = 0; i < options.Length; ++i) {
            bool thisToggleIsOn = false;
            if (i == activeIndex) {
                thisToggleIsOn = true;
            } else {
                GUI.color = Color.white;
            }

            GUIStyle usingStyle = EditorStyles.miniButtonMid;
            if (options.Length >= 2) {
                if (i == 0) {
                    usingStyle = EditorStyles.miniButtonLeft;
                } else if (i == options.Length - 1) {
                    usingStyle = EditorStyles.miniButtonRight;
                }
            }

            if (GUILayout.Toggle(thisToggleIsOn, options[i], usingStyle)) {
                if (!thisToggleIsOn) {
                    returnIndex = i;
                }
            }
        }

        GUI.color = Color.white;
        return returnIndex;
    }

    /// <summary>
    /// Draw a set of tabs with the built-in editor mini buttons.
    /// </summary>
    /// <param name="options">A string array for display name of all tab.</param>
    /// <param name="activeIndex">Input the current active tab's index.</param>
    /// <returns>The new active tab index.</returns>
    /// <summary>
    static public int Tabs(string[] options, int activeIndex, GUIStyle guiStyle) {
        int returnIndex = activeIndex;

        if (options != null) {
            for (int i = 0; i < options.Length; ++i) {
                bool thisToggleIsOn;
                if (i == activeIndex) {
                    thisToggleIsOn = true;
                } else {
                    thisToggleIsOn = false;
                }

                if (GUILayout.Toggle(thisToggleIsOn, options[i], guiStyle)) {
                    if (!thisToggleIsOn) {
                        returnIndex = i;
                    }
                }
            }
        }

        GUI.color = Color.white;
        return returnIndex;
    }

    /// <summary>
    /// A template object list editor.
    /// </summary>
    /// <param name="title">Title string.</param>
    /// <param name="objectList">List to edit.</param>
    /// <param name="objectEditFunc">Editor GUI drawing function for editing a single object.</param>
    /// <returns>If any changes has been made.</returns>
    static public bool EditObjectList<T>(string title, List<T> objectList, Func<T, bool> objectEditFunc) where T : Cloneable<T>, new() {
        return EditObjectList(new GUIContent(title), objectList, objectEditFunc);
    }

    /// <summary>
    /// A template object list editor.
    /// </summary>
    /// <param name="guiContent">Title string.</param>
    /// <param name="objectList">List to edit.</param>
    /// <param name="objectEditFunc">Editor GUI drawing function for editing a single object.</param>
    /// <returns>If any changes has been made.</returns>
    static public bool EditObjectList<T>(GUIContent guiContent, List<T> objectList, Func<T, bool> objectEditFunc) where T : Cloneable<T>, new() {

        bool hasChanged = false;

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
            GUILayout.FlexibleSpace();
            Color tempColor1 = GUI.color;
            GUI.color = Color.yellow;
            if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, GUILayout.Width(66.0f))) {
                objectList.Add(new T());
                hasChanged = true;
            }
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                objectListCopyBuffer = objectList.Select((o) => o.Clone()).ToList<object>();
            }

            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                if (objectListCopyBuffer != null && objectListCopyBuffer.Count > 0) {
                    objectList.Clear();
                    for (int i = 0; i < objectListCopyBuffer.Count; i++) {
                        //Make sure it's the right type.
                        if (objectListCopyBuffer[i] is T) {
                            objectList.Add((T)objectListCopyBuffer[i]);
                        }
                    }
                    hasChanged = true;
                }
            }

            GUI.color = Color.magenta;
            if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                objectList.Clear();
                hasChanged = true;
            }
            GUI.color = tempColor1;
        }
        EditorGUILayout.EndHorizontal();

        int removeIndex = -1;
        int moveIndexUp = -1;
        int moveIndexDown = -1;

        for (int i = 0; i < objectList.Count; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("[" + i.ToString("00") + "]", GUILayout.Width(CalcLabelWidth("[" + i.ToString("00") + "]")));
                if (objectEditFunc(objectList[i])) {
                    hasChanged = true;
                }

                Color tempColor = GUI.color;
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                    moveIndexUp = i;
                }
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↓", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                    moveIndexDown = i;
                }
                GUI.color = tempColor;

                GUI.color = Color.red;
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                    removeIndex = i;
                }
                GUI.color = tempColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex != -1) {
            objectList.RemoveAt(removeIndex);
            hasChanged = true;
        }

        if (moveIndexUp != -1 && moveIndexUp > 0) {
            T moving = objectList[moveIndexUp];
            objectList.RemoveAt(moveIndexUp);
            objectList.Insert(moveIndexUp - 1, moving);
            hasChanged = true;
        }

        if (moveIndexDown != -1 && moveIndexDown < objectList.Count - 1) {
            T moving = objectList[moveIndexDown];
            objectList.RemoveAt(moveIndexDown);
            objectList.Insert(moveIndexDown + 1, moving);
            hasChanged = true;
        }

        return hasChanged;
    }

    /// <summary>
    /// A convenient string list editor.
    /// </summary>
    /// <param name="title">An optional title string.</param>
    /// <param name="editingStringList">The string list we are editing.</param>
    static public bool EditStringList(string title, List<string> editingStringList) {
        return EditStringList(new GUIContent(title), editingStringList);
    }

    /// <summary>
    /// A convenient string list editor.
    /// </summary>
    /// <param name="guiContent">An optional title string.</param>
    /// <param name="editingStringList">The string list we are editing.</param>
    static public bool EditStringList(GUIContent guiContent, List<string> editingStringList) {

        bool hasChanged = false;

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
            GUILayout.FlexibleSpace();
            Color tempColor1 = GUI.color;
            GUI.color = Color.yellow;
            if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, GUILayout.Width(66.0f))) {
                editingStringList.Add("");
                hasChanged = true;
            }
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                stringListBuffer = new List<string>(editingStringList);
            }

            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                if (stringListBuffer != null && stringListBuffer.Count > 0) {
                    editingStringList.Clear();
                    editingStringList.AddRange(stringListBuffer);
                    hasChanged = true;
                }
            }

            GUI.color = ColorPlus.SpringGreen;
            if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                List<string> pastedMatrix = Util.StringSplitByNewLineTab(EditorGUIUtility.systemCopyBuffer);
                editingStringList.Clear();
                editingStringList.AddRange(pastedMatrix);
                hasChanged = true;
            }
            GUI.color = Color.magenta;
            if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                editingStringList.Clear();
                hasChanged = true;
            }
            GUI.color = tempColor1;
        }
        EditorGUILayout.EndHorizontal();

        int removeIndex = -1;
        int moveIndexUp = -1;
        int moveIndexDown = -1;

        for (int i = 0; i < editingStringList.Count; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("[" + i.ToString("00") + "]", GUILayout.Width(CalcLabelWidth("[" + i.ToString("00") + "]")));

                Color tempColor = GUI.color;
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                    moveIndexUp = i;
                }
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↓", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                    moveIndexDown = i;
                }
                GUI.color = tempColor;

                string newStr = EditorGUILayoutPlus.TextField(editingStringList[i]);
                if (newStr != editingStringList[i]) {
                    editingStringList[i] = newStr;
                    hasChanged = true;
                }

                GUI.color = Color.red;
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                    removeIndex = i;
                }
                GUI.color = tempColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex != -1) {
            editingStringList.RemoveAt(removeIndex);
            hasChanged = true;
        }

        if (moveIndexUp != -1 && moveIndexUp > 0) {
            string moving = editingStringList[moveIndexUp];
            editingStringList.RemoveAt(moveIndexUp);
            editingStringList.Insert(moveIndexUp - 1, moving);
            hasChanged = true;
        }

        if (moveIndexDown != -1 && moveIndexDown < editingStringList.Count - 1) {
            string moving = editingStringList[moveIndexDown];
            editingStringList.RemoveAt(moveIndexDown);
            editingStringList.Insert(moveIndexDown + 1, moving);
            hasChanged = true;
        }

        return hasChanged;
    }

    /// <summary>
    /// A convenient string array editor.
    /// </summary>
    /// <param name="title">An optional title string.</param>
    /// <param name="editingStringArray">The string array we are editing.</param>
    static public void EditStringArray(string title, string[] editingStringArray) {
        EditStringArray(new GUIContent(title), editingStringArray);
    }

    /// <summary>
    /// A convenient string array editor.
    /// </summary>
    /// <param name="guiContent">An optional title string.</param>
    /// <param name="editingStringArray">The string array we are editing.</param>
    static public void EditStringArray(GUIContent guiContent, string[] editingStringArray) {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
        }
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < editingStringArray.Length; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("[" + i.ToString("00") + "]", GUILayout.Width(CalcLabelWidth("[" + i.ToString("00") + "]")));
                editingStringArray[i] = EditorGUILayoutPlus.TextField(editingStringArray[i]);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    /// <summary>
    /// A convenient string array editor.
    /// </summary>
    /// <param name="editingStringArray">The string array we are editing.</param>
    static public void EditStringArray(string[] editingStringArray) {
        for (int i = 0; i < editingStringArray.Length; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("[" + i.ToString("00") + "]", GUILayout.Width(CalcLabelWidth("[" + i.ToString("00") + "]")));
                editingStringArray[i] = EditorGUILayoutPlus.TextField(editingStringArray[i]);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    /// <summary>
    /// A string list viewer.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="editingStringList"></param>
    static public void ViewStringList(string title, List<string> editingStringList) {
        ViewStringList(new GUIContent(title), editingStringList);
    }

    /// <summary>
    /// A string list viewer.
    /// </summary>
    /// <param name="guiContent"></param>
    /// <param name="editingStringList"></param>
    static public void ViewStringList(GUIContent guiContent, List<string> editingStringList) {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
        }
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < editingStringList.Count; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("[" + i.ToString("00") + "]", GUILayout.Width(CalcLabelWidth("[" + i.ToString("00") + "]")));
                EditorGUILayoutPlus.TextField(editingStringList[i]);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    /// <summary>
    /// A string araray viewer.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="editingStringArray"></param>
    static public void ViewStringArray(string title, string[] editingStringArray) {
        ViewStringArray(new GUIContent(title), editingStringArray);
    }

    /// <summary>
    /// A string araray viewer.
    /// </summary>
    /// <param name="guiContent"></param>
    /// <param name="editingStringArray"></param>
    static public void ViewStringArray(GUIContent guiContent, string[] editingStringArray) {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
        }
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < editingStringArray.Length; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("[" + i.ToString("00") + "]", GUILayout.Width(CalcLabelWidth("[" + i.ToString("00") + "]")));
                EditorGUILayoutPlus.TextField(editingStringArray[i]);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    /// <summary>
    /// A convenient int list editor.
    /// </summary>
    /// <param name="title">An optional title string.</param>
    /// <param name="editingIntList">The int list we are editing.</param>
    static public bool EditIntList(string title, List<int> editingIntList) {
        return EditIntList(new GUIContent(title), editingIntList);
    }

    /// <summary>
    /// A convenient int list editor.
    /// </summary>
    /// <param name="guiContent">An optional title string.</param>
    /// <param name="editingIntList">The int list we are editing.</param>
    static public bool EditIntList(GUIContent guiContent, List<int> editingIntList) {

        bool hasChanged = false;

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
            GUILayout.FlexibleSpace();
            Color tempColor1 = GUI.color;
            GUI.color = Color.yellow;
            if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, GUILayout.Width(66.0f))) {
                editingIntList.Add(0);
                hasChanged = true;
            }
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                intListBuffer = new List<int>(editingIntList);
            }

            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                if (intListBuffer != null && intListBuffer.Count > 0) {
                    editingIntList.Clear();
                    editingIntList.AddRange(intListBuffer);
                    hasChanged = true;
                }
            }

            GUI.color = ColorPlus.SpringGreen;
            if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                List<int> pastedMatrix = Util.StringSplitByNewLineTabAsInt(EditorGUIUtility.systemCopyBuffer);
                editingIntList.Clear();
                editingIntList.AddRange(pastedMatrix);
                hasChanged = true;
            }
            GUI.color = Color.magenta;
            if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                editingIntList.Clear();
                hasChanged = true;
            }
            GUI.color = tempColor1;
        }
        EditorGUILayout.EndHorizontal();

        int removeIndex = -1;
        int moveIndexUp = -1;
        int moveIndexDown = -1;

        for (int i = 0; i < editingIntList.Count; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("[" + i.ToString("00") + "]", GUILayout.Width(CalcLabelWidth("[" + i.ToString("00") + "]")));

                Color tempColor = GUI.color;
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                    moveIndexUp = i;
                }
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↓", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                    moveIndexDown = i;
                }
                GUI.color = tempColor;

                int newInt = EditorGUILayoutPlus.IntField(editingIntList[i]);
                if (newInt != editingIntList[i]) {
                    editingIntList[i] = newInt;
                    hasChanged = true;
                }

                GUI.color = Color.red;
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                    removeIndex = i;
                }
                GUI.color = tempColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex != -1) {
            editingIntList.RemoveAt(removeIndex);
            hasChanged = true;
        }

        if (moveIndexUp != -1 && moveIndexUp > 0) {
            int moving = editingIntList[moveIndexUp];
            editingIntList.RemoveAt(moveIndexUp);
            editingIntList.Insert(moveIndexUp - 1, moving);
            hasChanged = true;
        }

        if (moveIndexDown != -1 && moveIndexDown < editingIntList.Count - 1) {
            int moving = editingIntList[moveIndexDown];
            editingIntList.RemoveAt(moveIndexDown);
            editingIntList.Insert(moveIndexDown + 1, moving);
            hasChanged = true;
        }

        return hasChanged;
    }

    /// <summary>
    /// A int list viewer.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="editingIntList"></param>
    static public void ViewIntList(string title, List<int> editingIntList) {
        ViewIntList(new GUIContent(title), editingIntList);
    }

    /// <summary>
    /// A int list viewer.
    /// </summary>
    /// <param name="guiContent"></param>
    /// <param name="editingIntList"></param>
    static public void ViewIntList(GUIContent guiContent, List<int> editingIntList) {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
        }
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < editingIntList.Count; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("[" + i.ToString("00") + "]", GUILayout.Width(CalcLabelWidth("[" + i.ToString("00") + "]")));
                EditorGUILayoutPlus.TextField(editingIntList[i].ToString());
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    /// <summary>
    /// A convenient float list editor.
    /// </summary>
    /// <param name="title">An optional title string.</param>
    /// <param name="editingFloatList">The float list we are editing.</param>
    static public bool EditFloatList(string title, List<float> editingFloatList) {
        return EditFloatList(new GUIContent(title), editingFloatList);
    }

    /// <summary>
    /// A convenient float list editor.
    /// </summary>
    /// <param name="guiContent">An optional title string.</param>
    /// <param name="editingFloatList">The float list we are editing.</param>
    static public bool EditFloatList(GUIContent guiContent, List<float> editingFloatList) {

        bool hasChanged = false;

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
            GUILayout.FlexibleSpace();
            Color tempColor1 = GUI.color;
            GUI.color = Color.yellow;
            if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, GUILayout.Width(66.0f))) {
                editingFloatList.Add(0);
                hasChanged = true;
            }
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                floatListBuffer = new List<float>(editingFloatList);
            }

            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                if (floatListBuffer != null && floatListBuffer.Count > 0) {
                    editingFloatList.Clear();
                    editingFloatList.AddRange(floatListBuffer);
                    hasChanged = true;
                }
            }

            GUI.color = ColorPlus.SpringGreen;
            if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                List<float> pastedMatrix = Util.StringSplitByNewLineTabAsFloat(EditorGUIUtility.systemCopyBuffer);
                editingFloatList.Clear();
                editingFloatList.AddRange(pastedMatrix);
                hasChanged = true;
            }
            GUI.color = Color.magenta;
            if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                editingFloatList.Clear();
                hasChanged = true;
            }
            GUI.color = tempColor1;
        }
        EditorGUILayout.EndHorizontal();

        int removeIndex = -1;
        int moveIndexUp = -1;
        int moveIndexDown = -1;

        for (int i = 0; i < editingFloatList.Count; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("[" + i.ToString("00") + "]", GUILayout.Width(CalcLabelWidth("[" + i.ToString("00") + "]")));

                Color tempColor = GUI.color;
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                    moveIndexUp = i;
                }
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↓", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                    moveIndexDown = i;
                }
                GUI.color = tempColor;

                float newFloat = EditorGUILayoutPlus.FloatField(editingFloatList[i]);
                if (newFloat != editingFloatList[i]) {
                    editingFloatList[i] = newFloat;
                    hasChanged = true;
                }

                GUI.color = Color.red;
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                    removeIndex = i;
                }
                GUI.color = tempColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex != -1) {
            editingFloatList.RemoveAt(removeIndex);
            hasChanged = true;
        }

        if (moveIndexUp != -1 && moveIndexUp > 0) {
            float moving = editingFloatList[moveIndexUp];
            editingFloatList.RemoveAt(moveIndexUp);
            editingFloatList.Insert(moveIndexUp - 1, moving);
            hasChanged = true;
        }

        if (moveIndexDown != -1 && moveIndexDown < editingFloatList.Count - 1) {
            float moving = editingFloatList[moveIndexDown];
            editingFloatList.RemoveAt(moveIndexDown);
            editingFloatList.Insert(moveIndexDown + 1, moving);
            hasChanged = true;
        }

        return hasChanged;
    }

    /// <summary>
    /// A float list viewer.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="editingFloatList"></param>
    static public void ViewFloatList(string title, List<float> editingFloatList) {
        ViewFloatList(new GUIContent(title), editingFloatList);
    }

    /// <summary>
    /// A float list viewer.
    /// </summary>
    /// <param name="guiContent"></param>
    /// <param name="editingFloatList"></param>
    static public void ViewFloatList(GUIContent guiContent, List<float> editingFloatList) {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
        }
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < editingFloatList.Count; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("[" + i.ToString("00") + "]", GUILayout.Width(CalcLabelWidth("[" + i.ToString("00") + "]")));
                EditorGUILayoutPlus.TextField(editingFloatList[i].ToString());
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    /// <summary>
    /// An alternative version of Vector3 field.
    /// </summary>
    static public void EditExplicitVector3(float x, float y, float z, ref float outX, ref float outY, ref float outZ) {
        EditorGUILayout.BeginHorizontal();
        {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                vector3CopyBuffer = new Vector3(x, y, z);
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                x = vector3CopyBuffer.x;
                y = vector3CopyBuffer.y;
                z = vector3CopyBuffer.z;
            }
            GUI.color = tempColor;
            EditorGUIUtility.labelWidth = 15;
            outX = EditorGUILayout.FloatField("x", x, GUILayout.MinWidth(30.0f));
            outY = EditorGUILayout.FloatField("y", y, GUILayout.MinWidth(30.0f));
            outZ = EditorGUILayout.FloatField("z", z, GUILayout.MinWidth(30.0f));
            EditorGUIUtility.labelWidth = 0;
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// A vector field with copy/paste button.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="vec3"></param>
    /// <returns></returns>
    static public Vector3 Vector3Field(string title, Vector3 vec3, params GUILayoutOption[] options) {
        return Vector3Field(new GUIContent(title), vec3, options);
    }

    /// <summary>
    /// A vector field with copy/paste button.
    /// </summary>
    /// <param name="guiContent"></param>
    /// <param name="vec3"></param>
    /// <returns></returns>
    static public Vector3 Vector3Field(GUIContent guiContent, Vector3 vec3, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(16.0f));
            {
                Color tempColor = GUI.color;
                GUI.color = ColorPlus.LightSalmon;
                if (GUILayout.Button("c", EditorStyles.miniButton)) {
                    vector3CopyBuffer = vec3;
                }
                GUI.color = ColorPlus.LightBlue;
                if (GUILayout.Button("p", EditorStyles.miniButton)) {
                    vec3 = vector3CopyBuffer;
                }
                GUI.color = tempColor;
            }
            EditorGUILayout.EndVertical();
            EditorGUIUtility.labelWidth = CalcLabelWidth(guiContent.text);
            vec3 = EditorGUILayout.Vector3Field(guiContent, vec3);
            EditorGUIUtility.labelWidth = 0;
        }
        EditorGUILayout.EndHorizontal();
        return vec3;
    }

    /// <summary>
    /// A label field with copy/paste button.
    /// </summary>
    /// <param name="editText"></param>
    /// <returns></returns>
    static public void LabelField(string text, bool withCopyButton = true, GUIStyle guiStyle = null, params GUILayoutOption[] options) {
        if (withCopyButton) {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButton, GUILayout.Width(20.0f))) {
                EditorGUIUtility.systemCopyBuffer = text;
            }
            GUI.color = tempColor;
        }
        List<GUILayoutOption> newOptions = new List<GUILayoutOption>();
        newOptions.Add(GUILayout.Width(CalcLabelWidth(text)));
        if (options != null) {
            newOptions.AddRange(options);
        }

        if (guiStyle == null) {
            EditorGUILayout.LabelField(text, newOptions.ToArray());
        } else {
            EditorGUILayout.LabelField(text, guiStyle, newOptions.ToArray());
        }
    }

    /// <summary>
    /// A label field with copy/paste button.
    /// </summary>
    /// <param name="editText"></param>
    /// <returns></returns>
    static public void LabelField(GUIContent guiContent, bool withCopyButton = true, GUIStyle guiStyle = null, params GUILayoutOption[] options) {
        if (withCopyButton) {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButton, GUILayout.Width(20.0f))) {
                EditorGUIUtility.systemCopyBuffer = guiContent.text;
            }
            GUI.color = tempColor;
        }
        List<GUILayoutOption> newOptions = new List<GUILayoutOption>();
        newOptions.Add(GUILayout.Width(CalcLabelWidth(guiContent.text)));
        if (options != null) {
            newOptions.AddRange(options);
        }

        if (guiStyle == null) {
            EditorGUILayout.LabelField(guiContent, newOptions.ToArray());
        } else {
            EditorGUILayout.LabelField(guiContent, guiStyle, newOptions.ToArray());
        }
    }

    /// <summary>
    /// A text field with copy/paste button.
    /// </summary>
    /// <param name="editText"></param>
    /// <returns></returns>
    static public string TextField(string editText, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                EditorGUIUtility.systemCopyBuffer = editText;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                editText = EditorGUIUtility.systemCopyBuffer;
            }
            GUI.color = tempColor;
            editText = EditorGUILayout.TextField(editText, GUILayout.MinWidth(30.0f));
        }
        EditorGUILayout.EndHorizontal();
        return editText;
    }

    /// <summary>
    /// A text field with copy/paste button.
    /// </summary>
    /// <param name="editText"></param>
    /// <returns></returns>
    static public string TextField(string label, string editText, params GUILayoutOption[] options) {
        return TextField(new GUIContent(label), editText, options);
    }

    /// <summary>
    /// A text field with copy/paste button.
    /// </summary>
    /// <param name="editText"></param>
    /// <returns></returns>
    static public string TextField(GUIContent guiContent, string editText, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                EditorGUIUtility.systemCopyBuffer = editText;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                editText = EditorGUIUtility.systemCopyBuffer;
            }
            GUI.color = tempColor;
            EditorGUIUtility.labelWidth = CalcLabelWidth(guiContent.text);
            editText = EditorGUILayout.TextField(guiContent, editText, GUILayout.MinWidth(30.0f));
            EditorGUIUtility.labelWidth = 0;
        }
        EditorGUILayout.EndHorizontal();
        return editText;
    }

    /// <summary>
    /// A text area with copy/paste button.
    /// </summary>
    /// <param name="editText"></param>
    /// <returns></returns>
    static public string TextArea(string editText, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                EditorGUIUtility.systemCopyBuffer = editText;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                editText = EditorGUIUtility.systemCopyBuffer;
            }
            GUI.color = tempColor;
            editText = EditorGUILayout.TextArea(editText, GUILayout.MinWidth(30.0f));
        }
        EditorGUILayout.EndHorizontal();
        return editText;
    }

    /// <summary>
    /// A text area with copy/paste button.
    /// </summary>
    /// <param name="editText"></param>
    /// <returns></returns>
    static public string TextArea(string label, string editText, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                EditorGUIUtility.systemCopyBuffer = editText;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                editText = EditorGUIUtility.systemCopyBuffer;
            }
            GUI.color = tempColor;
            EditorGUILayout.LabelField(label, GUILayout.Width(CalcLabelWidth(label)));
            editText = EditorGUILayout.TextArea(editText, GUILayout.MinWidth(30.0f));
        }
        EditorGUILayout.EndHorizontal();
        return editText;
    }

    /// <summary>
    /// A text area with copy/paste button.
    /// </summary>
    /// <param name="editText"></param>
    /// <returns></returns>
    static public string TextArea(GUIContent guiContent, string editText, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                EditorGUIUtility.systemCopyBuffer = editText;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                editText = EditorGUIUtility.systemCopyBuffer;
            }
            GUI.color = tempColor;
            EditorGUILayout.LabelField(guiContent, GUILayout.Width(CalcLabelWidth(guiContent.text)));
            editText = EditorGUILayout.TextArea(editText, GUILayout.MinWidth(30.0f));
        }
        EditorGUILayout.EndHorizontal();
        return editText;
    }

    /// <summary>
    /// A float field with copy/paste button.
    /// </summary>
    /// <param name="editFloat"></param>
    /// <returns></returns>
    static public float FloatField(float editFloat, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                floatCopyBuffer = editFloat;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                editFloat = floatCopyBuffer;
            }
            GUI.color = tempColor;
            editFloat = EditorGUILayout.FloatField(editFloat, GUILayout.MinWidth(30.0f));
        }
        EditorGUILayout.EndHorizontal();
        return editFloat;
    }

    /// <summary>
    /// A float field with copy/paste button.
    /// </summary>
    /// <param name="editFloat"></param>
    /// <returns></returns>
    static public float FloatField(string label, float editFloat, params GUILayoutOption[] options) {
        return FloatField(new GUIContent(label), editFloat, options);
    }

    /// <summary>
    /// A float field with copy/paste button.
    /// </summary>
    /// <param name="editFloat"></param>
    /// <returns></returns>
    static public float FloatField(GUIContent guiContent, float editFloat, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                floatCopyBuffer = editFloat;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                editFloat = floatCopyBuffer;
            }
            GUI.color = tempColor;
            EditorGUIUtility.labelWidth = CalcLabelWidth(guiContent.text);
            editFloat = EditorGUILayout.FloatField(guiContent, editFloat, GUILayout.MinWidth(30.0f));
            EditorGUIUtility.labelWidth = 0;
        }
        EditorGUILayout.EndHorizontal();
        return editFloat;
    }

    /// <summary>
    /// An int field with copy/paste button.
    /// </summary>
    /// <param name="editInt"></param>
    /// <returns></returns>
    static public int IntField(int editInt, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                intCopyBuffer = editInt;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                editInt = intCopyBuffer;
            }
            GUI.color = tempColor;
            editInt = EditorGUILayout.IntField(editInt, GUILayout.MinWidth(30.0f));
        }
        EditorGUILayout.EndHorizontal();
        return editInt;
    }

    /// <summary>
    /// An int field with copy/paste button.
    /// </summary>
    /// <param name="editInt"></param>
    /// <returns></returns>
    static public int IntField(string label, int editInt, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                intCopyBuffer = editInt;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                editInt = intCopyBuffer;
            }
            GUI.color = tempColor;
            EditorGUIUtility.labelWidth = CalcLabelWidth(label);
            editInt = EditorGUILayout.IntField(label, editInt, GUILayout.MinWidth(30.0f));
            EditorGUIUtility.labelWidth = 0;
        }
        EditorGUILayout.EndHorizontal();
        return editInt;
    }

    /// <summary>
    /// An int field with copy/paste button.
    /// </summary>
    /// <param name="editInt"></param>
    /// <returns></returns>
    static public int IntField(GUIContent guiContent, int editInt, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                intCopyBuffer = editInt;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                editInt = intCopyBuffer;
            }
            GUI.color = tempColor;
            EditorGUIUtility.labelWidth = CalcLabelWidth(guiContent.text);
            editInt = EditorGUILayout.IntField(guiContent, editInt, GUILayout.MinWidth(30.0f));
            EditorGUIUtility.labelWidth = 0;
        }
        EditorGUILayout.EndHorizontal();
        return editInt;
    }

    /// <summary>
    /// An auto-sized toggle button.
    /// </summary>
    /// <param name="label">Label to display</param>
    /// <param name="b">Edit content bool</param>
    /// <param name="options"></param>
    /// <returns></returns>
    static public bool Toggle(string label, bool b, params GUILayoutOption[] options) {
        EditorGUIUtility.labelWidth = CalcLabelWidth(label);
        b = EditorGUILayout.Toggle(label, b, GUILayout.Width(CalcLabelWidth(label) + 16.0f));
        EditorGUIUtility.labelWidth = 0;
        return b;
    }

    /// <summary>
    /// An auto-sized toggle button.
    /// </summary>
    /// <param name="guiContent">Label to display</param>
    /// <param name="b">Edit content bool</param>
    /// <param name="options"></param>
    /// <returns></returns>
    static public bool Toggle(GUIContent guiContent, bool b, params GUILayoutOption[] options) {
        EditorGUIUtility.labelWidth = CalcLabelWidth(guiContent.text);
        b = EditorGUILayout.Toggle(guiContent, b, GUILayout.Width(CalcLabelWidth(guiContent.text) + 16.0f));
        EditorGUIUtility.labelWidth = 0;
        return b;
    }

    /// <summary>
    /// A convenient bool list editor.
    /// </summary>
    /// <param name="guiContent">An optional title string.</param>
    /// <param name="editingToggleList">The bool list we are editing.</param>
    static public bool EditToggleList(string label, List<bool> editingToggleList, List<string> contentDislayList = null) {
        return EditToggleList(new GUIContent(label), editingToggleList, contentDislayList);
    }

    /// <summary>
    /// A convenient bool list editor.
    /// </summary>
    /// <param name="guiContent">An optional title string.</param>
    /// <param name="editingToggleList">The bool list we are editing.</param>
    static public bool EditToggleList(GUIContent guiContent, List<bool> editingToggleList, List<string> contentDislayList = null) {

        bool hasChanged = false;

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
            GUILayout.FlexibleSpace();
            Color tempColor1 = GUI.color;
            GUI.color = Color.yellow;
            if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, GUILayout.Width(66.0f))) {
                editingToggleList.Add(false);
                hasChanged = true;
            }
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                boolListBuffer = new List<bool>(editingToggleList);
            }

            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                if (boolListBuffer != null && boolListBuffer.Count > 0) {
                    editingToggleList.Clear();
                    for (int i = 0; i < boolListBuffer.Count; i++) {
                        editingToggleList.Add(boolListBuffer[i]);
                    }
                    hasChanged = true;
                }
            }

            GUI.color = Color.magenta;
            if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                editingToggleList.Clear();
                hasChanged = true;
            }

            GUI.color = ColorPlus.HoneyDew;
            if (GUILayout.Button("All", EditorStyles.miniButtonLeft, GUILayout.Width(45.0f))) {
                for (int i = 0; i < editingToggleList.Count; i++) {
                    editingToggleList[i] = true;
                }
                hasChanged = true;
            }
            if (GUILayout.Button("None", EditorStyles.miniButtonRight, GUILayout.Width(45.0f))) {
                for (int i = 0; i < editingToggleList.Count; i++) {
                    editingToggleList[i] = false;
                }
                hasChanged = true;
            }
            GUI.color = tempColor1;
        }
        EditorGUILayout.EndHorizontal();

        int removeIndex = -1;
        int moveIndexUp = -1;
        int moveIndexDown = -1;

        for (int i = 0; i < editingToggleList.Count; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("[" + i.ToString("00") + "]", GUILayout.Width(CalcLabelWidth("[" + i.ToString("00") + "]")));

                Color tempColor = GUI.color;
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                    moveIndexUp = i;
                }
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↓", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                    moveIndexDown = i;
                }
                GUI.color = tempColor;

                string toggleLabel = "";
                if (contentDislayList != null && i < contentDislayList.Count) {
                    toggleLabel = contentDislayList[i];
                } else {
                    toggleLabel = "Toggle[" + i + "]";
                }

                bool newToggleLeft = ToggleLeft(toggleLabel, editingToggleList[i]);
                if (!newToggleLeft.Equals(editingToggleList[i])) {
                    editingToggleList[i] = newToggleLeft;
                    hasChanged = true;
                }

                GUILayout.FlexibleSpace();

                GUI.color = Color.red;
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                    removeIndex = i;
                }
                GUI.color = tempColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex != -1) {
            editingToggleList.RemoveAt(removeIndex);
            hasChanged = true;
        }

        if (moveIndexUp != -1 && moveIndexUp > 0) {
            bool moving = editingToggleList[moveIndexUp];
            editingToggleList.RemoveAt(moveIndexUp);
            editingToggleList.Insert(moveIndexUp - 1, moving);
            hasChanged = true;
        }

        if (moveIndexDown != -1 && moveIndexDown < editingToggleList.Count - 1) {
            bool moving = editingToggleList[moveIndexDown];
            editingToggleList.RemoveAt(moveIndexDown);
            editingToggleList.Insert(moveIndexDown + 1, moving);
            hasChanged = true;
        }

        return hasChanged;
    }

    /// <summary>
    /// An auto-sized toggle button.
    /// </summary>
    /// <param name="label">Label to display</param>
    /// <param name="b">Edit content bool</param>
    /// <param name="options"></param>
    /// <returns></returns>
    static public bool ToggleLeft(string label, bool b, params GUILayoutOption[] options) {
        EditorGUIUtility.labelWidth = CalcLabelWidth(label);
        b = EditorGUILayout.ToggleLeft(label, b, GUILayout.Width(CalcLabelWidth(label) + 16.0f));
        EditorGUIUtility.labelWidth = 0;
        return b;
    }

    /// <summary>
    /// An auto-sized toggle button.
    /// </summary>
    /// <param name="label">Label to display</param>
    /// <param name="b">Edit content bool</param>
    /// <param name="options"></param>
    /// <returns></returns>
    static public bool ToggleLeft(GUIContent guiContent, bool b, params GUILayoutOption[] options) {
        EditorGUIUtility.labelWidth = CalcLabelWidth(guiContent.text);
        b = EditorGUILayout.ToggleLeft(guiContent, b, GUILayout.Width(CalcLabelWidth(guiContent.text) + 16.0f));
        EditorGUIUtility.labelWidth = 0;
        return b;
    }

    /// <summary>
    /// A convenient bool list editor.
    /// </summary>
    /// <param name="guiContent">An optional title string.</param>
    /// <param name="editingToggleLeftList">The bool list we are editing.</param>
    static public bool EditToggleLeftList(string label, string toggleLabel, List<bool> editingToggleLeftList) {
        return EditToggleLeftList(new GUIContent(label), toggleLabel, editingToggleLeftList);
    }

    /// <summary>
    /// A convenient bool list editor.
    /// </summary>
    /// <param name="guiContent">An optional title string.</param>
    /// <param name="editingToggleLeftList">The bool list we are editing.</param>
    static public bool EditToggleLeftList(GUIContent guiContent, string toggleLabel, List<bool> editingToggleLeftList) {

        bool hasChanged = false;

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
            GUILayout.FlexibleSpace();
            Color tempColor1 = GUI.color;
            GUI.color = Color.yellow;
            if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, GUILayout.Width(66.0f))) {
                editingToggleLeftList.Add(false);
                hasChanged = true;
            }
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                boolListBuffer = new List<bool>(editingToggleLeftList);
            }

            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                if (boolListBuffer != null && boolListBuffer.Count > 0) {
                    editingToggleLeftList.Clear();
                    for (int i = 0; i < boolListBuffer.Count; i++) {
                        editingToggleLeftList.Add(boolListBuffer[i]);
                    }
                    hasChanged = true;
                }
            }

            GUI.color = Color.magenta;
            if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                editingToggleLeftList.Clear();
                hasChanged = true;
            }
            GUI.color = tempColor1;
        }
        EditorGUILayout.EndHorizontal();

        int removeIndex = -1;
        int moveIndexUp = -1;
        int moveIndexDown = -1;

        for (int i = 0; i < editingToggleLeftList.Count; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("[" + i.ToString("00") + "]", GUILayout.Width(CalcLabelWidth("[" + i.ToString("00") + "]")));

                Color tempColor = GUI.color;
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                    moveIndexUp = i;
                }
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↓", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                    moveIndexDown = i;
                }
                GUI.color = tempColor;

                bool newToggleLeft = ToggleLeft(toggleLabel, editingToggleLeftList[i]);
                if (!newToggleLeft.Equals(editingToggleLeftList[i])) {
                    editingToggleLeftList[i] = newToggleLeft;
                    hasChanged = true;
                }

                GUILayout.FlexibleSpace();

                GUI.color = Color.red;
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                    removeIndex = i;
                }
                GUI.color = tempColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex != -1) {
            editingToggleLeftList.RemoveAt(removeIndex);
            hasChanged = true;
        }

        if (moveIndexUp != -1 && moveIndexUp > 0) {
            bool moving = editingToggleLeftList[moveIndexUp];
            editingToggleLeftList.RemoveAt(moveIndexUp);
            editingToggleLeftList.Insert(moveIndexUp - 1, moving);
            hasChanged = true;
        }

        if (moveIndexDown != -1 && moveIndexDown < editingToggleLeftList.Count - 1) {
            bool moving = editingToggleLeftList[moveIndexDown];
            editingToggleLeftList.RemoveAt(moveIndexDown);
            editingToggleLeftList.Insert(moveIndexDown + 1, moving);
            hasChanged = true;
        }

        return hasChanged;
    }

    /// <summary>
    /// A convenient enum list editor.
    /// </summary>
    /// <param name="guiContent">An optional title string.</param>
    /// <param name="editingToggleList">The enum list we are editing.</param>
    static public bool EditEnumList<T>(string label, List<T> editingEnumList) where T : System.Enum {
        return EditEnumList<T>(new GUIContent(label), editingEnumList);
    }

    /// <summary>
    /// A convenient enum list editor.
    /// </summary>
    /// <param name="guiContent">An optional title string.</param>
    /// <param name="editingToggleList">The enum list we are editing.</param>
    static public bool EditEnumList<T>(GUIContent guiContent, List<T> editingEnumList) where T : System.Enum {

        bool hasChanged = false;

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
            GUILayout.FlexibleSpace();
            Color tempColor1 = GUI.color;
            GUI.color = Color.yellow;
            if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(66.0f))) {
                editingEnumList.Add(default(T));
                hasChanged = true;
            }

            GUI.color = Color.magenta;
            if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                editingEnumList.Clear();
                hasChanged = true;
            }
            GUI.color = tempColor1;
        }
        EditorGUILayout.EndHorizontal();

        int removeIndex = -1;
        int moveIndexUp = -1;
        int moveIndexDown = -1;

        for (int i = 0; i < editingEnumList.Count; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("[" + i.ToString("00") + "]", GUILayout.Width(CalcLabelWidth("[" + i.ToString("00") + "]")));

                Color tempColor = GUI.color;
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                    moveIndexUp = i;
                }
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↓", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                    moveIndexDown = i;
                }
                GUI.color = tempColor;

                T newEnum = (T)EditorGUILayout.EnumPopup(editingEnumList[i]);
                if (!newEnum.Equals(editingEnumList[i])) {
                    editingEnumList[i] = newEnum;
                    hasChanged = true;
                }

                GUILayout.FlexibleSpace();

                GUI.color = Color.red;
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                    removeIndex = i;
                }
                GUI.color = tempColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex != -1) {
            editingEnumList.RemoveAt(removeIndex);
            hasChanged = true;
        }

        if (moveIndexUp != -1 && moveIndexUp > 0) {
            T moving = editingEnumList[moveIndexUp];
            editingEnumList.RemoveAt(moveIndexUp);
            editingEnumList.Insert(moveIndexUp - 1, moving);
            hasChanged = true;
        }

        if (moveIndexDown != -1 && moveIndexDown < editingEnumList.Count - 1) {
            T moving = editingEnumList[moveIndexDown];
            editingEnumList.RemoveAt(moveIndexDown);
            editingEnumList.Insert(moveIndexDown + 1, moving);
            hasChanged = true;
        }

        return hasChanged;
    }

    /// <summary>
    /// An enum popup UI which support display supplement comment.
    /// </summary>
    /// <param name="selected"></param>
    /// <param name="commentFunc"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    static public Enum EnumPopup(Enum selected, Func<Enum, string> commentFunc, params GUILayoutOption[] options) {
        string[] names = Enum.GetNames(selected.GetType());
        int selectedIndex = new List<string>(names).IndexOf(selected.ToString());
        List<string> displayOptions = new List<string>(names).Select((name) => name + " (" + commentFunc((Enum)Enum.Parse(selected.GetType(), name)) + ")").ToList();
        int newSelectIndex = EditorGUILayout.Popup(selectedIndex, displayOptions.ToArray(), options);
        return Enum.Parse(selected.GetType(), names[newSelectIndex]) as Enum;
    }

    /// <summary>
    /// An alternative version of Vector2 field.
    /// </summary>
    static public void EditExplicitVector2(float x, float y, ref float outX, ref float outY) {
        EditorGUILayout.BeginHorizontal();
        {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                vector2CopyBuffer = new Vector2(x, y);
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                x = vector2CopyBuffer.x;
                y = vector2CopyBuffer.y;
            }
            GUI.color = tempColor;
            EditorGUIUtility.labelWidth = 15;
            outX = EditorGUILayout.FloatField("x", x, GUILayout.MinWidth(30.0f));
            outY = EditorGUILayout.FloatField("y", y, GUILayout.MinWidth(30.0f));
            EditorGUIUtility.labelWidth = 0;
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// Edit Vec2.
    /// </summary>
    static public Vec2 Vec2Field(Vec2 vec2) {
        Vec2 newVec2 = new Vec2(vec2);
        EditorGUILayout.BeginHorizontal();
        {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                vec2CopyBuffer = newVec2;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                newVec2 = new Vec2(vec2CopyBuffer);
            }
            GUI.color = tempColor;
            EditorGUIUtility.labelWidth = 15;
            newVec2.x = EditorGUILayout.FloatField("x", newVec2.x, GUILayout.MinWidth(30.0f));
            newVec2.y = EditorGUILayout.FloatField("y", newVec2.y, GUILayout.MinWidth(30.0f));
            EditorGUIUtility.labelWidth = 0;
        }
        EditorGUILayout.EndHorizontal();
        return newVec2;
    }

    /// <summary>
    /// A convenient Vec2 list editor.
    /// </summary>
    /// <param name="title">An optional title string.</param>
    /// <param name="editingVec2List">The Vec2 list we are editing.</param>
    static public bool EditVec2List(string title, List<Vec2> editingVec2List) {
        return EditVec2List(new GUIContent(title), editingVec2List);
    }

    /// <summary>
    /// A convenient Vec2 list editor.
    /// </summary>
    /// <param name="guiContent">An optional title string.</param>
    /// <param name="editingVec2List">The Vec2 list we are editing.</param>
    static public bool EditVec2List(GUIContent guiContent, List<Vec2> editingVec2List) {

        bool hasChanged = false;

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
            GUILayout.FlexibleSpace();
            Color tempColor1 = GUI.color;
            GUI.color = Color.yellow;
            if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, GUILayout.Width(66.0f))) {
                editingVec2List.Add(Vec2.Zero);
                hasChanged = true;
            }
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                vec2ListCopyBuffer = new List<Vec2>(editingVec2List.ToArray());
            }

            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                if (vec2ListCopyBuffer != null && vec2ListCopyBuffer.Count > 0) {
                    editingVec2List.Clear();
                    for (int i = 0; i < vec2ListCopyBuffer.Count; i++) {
                        editingVec2List.Add(vec2ListCopyBuffer[i]);
                    }
                    hasChanged = true;
                }
            }

            GUI.color = Color.magenta;
            if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                editingVec2List.Clear();
                hasChanged = true;
            }
            GUI.color = tempColor1;
        }
        EditorGUILayout.EndHorizontal();

        int removeIndex = -1;
        int moveIndexUp = -1;
        int moveIndexDown = -1;

        for (int i = 0; i < editingVec2List.Count; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("[" + i.ToString("00") + "]", GUILayout.Width(CalcLabelWidth("[" + i.ToString("00") + "]")));

                Color tempColor = GUI.color;
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                    moveIndexUp = i;
                }
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↓", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                    moveIndexDown = i;
                }
                GUI.color = tempColor;

                Vec2 newVec2 = Vec2Field(editingVec2List[i]);
                if (!newVec2.Equals(editingVec2List[i])) {
                    editingVec2List[i] = newVec2;
                    hasChanged = true;
                }

                GUI.color = Color.red;
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                    removeIndex = i;
                }
                GUI.color = tempColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex != -1) {
            editingVec2List.RemoveAt(removeIndex);
            hasChanged = true;
        }

        if (moveIndexUp != -1 && moveIndexUp > 0) {
            Vec2 moving = new Vec2(editingVec2List[moveIndexUp]);
            editingVec2List.RemoveAt(moveIndexUp);
            editingVec2List.Insert(moveIndexUp - 1, moving);
            hasChanged = true;
        }

        if (moveIndexDown != -1 && moveIndexDown < editingVec2List.Count - 1) {
            Vec2 moving = new Vec2(editingVec2List[moveIndexDown]);
            editingVec2List.RemoveAt(moveIndexDown);
            editingVec2List.Insert(moveIndexDown + 1, moving);
            hasChanged = true;
        }

        return hasChanged;
    }

    /// <summary>
    /// Edit Vec2i.
    /// </summary>
    static public Vec2i Vec2iField(Vec2i vec2) {
        Vec2i newVec2i = new Vec2i(vec2);
        EditorGUILayout.BeginHorizontal();
        {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                vec2CopyBuffer = new Vec2(newVec2i);
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                newVec2i = new Vec2i(vec2CopyBuffer);
            }
            GUI.color = tempColor;
            EditorGUIUtility.labelWidth = 15;
            newVec2i.x = EditorGUILayout.IntField("x", newVec2i.x, GUILayout.MinWidth(30.0f));
            newVec2i.y = EditorGUILayout.IntField("y", newVec2i.y, GUILayout.MinWidth(30.0f));
            EditorGUIUtility.labelWidth = 0;
        }
        EditorGUILayout.EndHorizontal();
        return newVec2i;
    }

    /// <summary>
    /// A convenient Vec2i list editor.
    /// </summary>
    /// <param name="title">An optional title string.</param>
    /// <param name="editingVec2iList">The Vec2i list we are editing.</param>
    static public bool EditVec2iList(string title, List<Vec2i> editingVec2iList) {
        return EditVec2iList(new GUIContent(title), editingVec2iList);
    }

    /// <summary>
    /// A convenient Vec2i list editor.
    /// </summary>
    /// <param name="guiContent">An optional title string.</param>
    /// <param name="editingVec2iList">The Vec2i list we are editing.</param>
    static public bool EditVec2iList(GUIContent guiContent, List<Vec2i> editingVec2iList) {

        bool hasChanged = false;

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
            GUILayout.FlexibleSpace();
            Color tempColor1 = GUI.color;
            GUI.color = Color.yellow;
            if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, GUILayout.Width(66.0f))) {
                editingVec2iList.Add(Vec2i.Zero);
                hasChanged = true;
            }
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                vec2ListCopyBuffer = new List<Vec2>(editingVec2iList.Select(x => new Vec2(x)).ToArray());
            }

            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                if (vec2ListCopyBuffer != null && vec2ListCopyBuffer.Count > 0) {
                    editingVec2iList.Clear();
                    for (int i = 0; i < vec2ListCopyBuffer.Count; i++) {
                        editingVec2iList.Add(new Vec2i(vec2ListCopyBuffer[i]));
                    }
                    hasChanged = true;
                }
            }

            GUI.color = Color.magenta;
            if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                editingVec2iList.Clear();
                hasChanged = true;
            }
            GUI.color = tempColor1;
        }
        EditorGUILayout.EndHorizontal();

        int removeIndex = -1;
        int moveIndexUp = -1;
        int moveIndexDown = -1;

        for (int i = 0; i < editingVec2iList.Count; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("[" + i.ToString("00") + "]", GUILayout.Width(CalcLabelWidth("[" + i.ToString("00") + "]")));

                Color tempColor = GUI.color;
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                    moveIndexUp = i;
                }
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↓", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                    moveIndexDown = i;
                }
                GUI.color = tempColor;

                Vec2i newVec2i = Vec2iField(editingVec2iList[i]);
                if (!newVec2i.Equals(editingVec2iList[i])) {
                    editingVec2iList[i] = newVec2i;
                    hasChanged = true;
                }

                GUI.color = Color.red;
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                    removeIndex = i;
                }
                GUI.color = tempColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex != -1) {
            editingVec2iList.RemoveAt(removeIndex);
            hasChanged = true;
        }

        if (moveIndexUp != -1 && moveIndexUp > 0) {
            Vec2i moving = new Vec2i(editingVec2iList[moveIndexUp]);
            editingVec2iList.RemoveAt(moveIndexUp);
            editingVec2iList.Insert(moveIndexUp - 1, moving);
            hasChanged = true;
        }

        if (moveIndexDown != -1 && moveIndexDown < editingVec2iList.Count - 1) {
            Vec2i moving = new Vec2i(editingVec2iList[moveIndexDown]);
            editingVec2iList.RemoveAt(moveIndexDown);
            editingVec2iList.Insert(moveIndexDown + 1, moving);
            hasChanged = true;
        }

        return hasChanged;
    }

    /// <summary>
    /// Edit Vec3.
    /// </summary>
    static public Vec3 Vec3Field(Vec3 vec3) {
        Vec3 newVec3 = new Vec3(vec3);
        EditorGUILayout.BeginHorizontal();
        {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                vec3CopyBuffer = newVec3;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                newVec3 = new Vec3(vec3CopyBuffer);
            }
            GUI.color = tempColor;
            EditorGUIUtility.labelWidth = 15;
            newVec3.x = EditorGUILayout.FloatField("x", newVec3.x, GUILayout.MinWidth(30.0f));
            newVec3.y = EditorGUILayout.FloatField("y", newVec3.y, GUILayout.MinWidth(30.0f));
            newVec3.z = EditorGUILayout.FloatField("z", newVec3.z, GUILayout.MinWidth(30.0f));
            EditorGUIUtility.labelWidth = 0;
        }
        EditorGUILayout.EndHorizontal();
        return newVec3;
    }

    /// <summary>
    /// A convenient Vec3 list editor.
    /// </summary>
    /// <param name="title">An optional title string.</param>
    /// <param name="editingVec3List">The Vec3 list we are editing.</param>
    static public bool EditVec3List(string title, List<Vec3> editingVec3List) {
        return EditVec3List(new GUIContent(title), editingVec3List);
    }

    /// <summary>
    /// A convenient Vec3 list editor.
    /// </summary>
    /// <param name="guiContent">An optional title string.</param>
    /// <param name="editingVec3List">The Vec3 list we are editing.</param>
    static public bool EditVec3List(GUIContent guiContent, List<Vec3> editingVec3List) {
        bool hasChanged = false;

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
            GUILayout.FlexibleSpace();
            Color tempColor1 = GUI.color;
            GUI.color = Color.yellow;
            if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, GUILayout.Width(66.0f))) {
                editingVec3List.Add(Vec3.Zero);
                hasChanged = true;
            }
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                vec3ListCopyBuffer = new List<Vec3>(editingVec3List.ToArray());
            }

            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                if (vec3ListCopyBuffer != null && vec3ListCopyBuffer.Count > 0) {
                    editingVec3List.Clear();
                    for (int i = 0; i < vec3ListCopyBuffer.Count; i++) {
                        editingVec3List.Add(vec3ListCopyBuffer[i]);
                    }
                    hasChanged = true;
                }
            }

            GUI.color = Color.magenta;
            if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                editingVec3List.Clear();
                hasChanged = true;
            }
            GUI.color = tempColor1;
        }
        EditorGUILayout.EndHorizontal();

        int removeIndex = -1;
        int moveIndexUp = -1;
        int moveIndexDown = -1;

        for (int i = 0; i < editingVec3List.Count; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("[" + i.ToString("00") + "]", GUILayout.Width(CalcLabelWidth("[" + i.ToString("00") + "]")));

                Color tempColor = GUI.color;
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                    moveIndexUp = i;
                }
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↓", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                    moveIndexDown = i;
                }
                GUI.color = tempColor;

                Vec3 newVec3 = Vec3Field(editingVec3List[i]);
                if (!newVec3.Equals(editingVec3List[i])) {
                    editingVec3List[i] = newVec3;
                    hasChanged = true;
                }

                GUI.color = Color.red;
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                    removeIndex = i;
                }
                GUI.color = tempColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex != -1) {
            editingVec3List.RemoveAt(removeIndex);
            hasChanged = true;
        }

        if (moveIndexUp != -1 && moveIndexUp > 0) {
            Vec3 moving = new Vec3(editingVec3List[moveIndexUp]);
            editingVec3List.RemoveAt(moveIndexUp);
            editingVec3List.Insert(moveIndexUp - 1, moving);
            hasChanged = true;
        }

        if (moveIndexDown != -1 && moveIndexDown < editingVec3List.Count - 1) {
            Vec3 moving = new Vec3(editingVec3List[moveIndexDown]);
            editingVec3List.RemoveAt(moveIndexDown);
            editingVec3List.Insert(moveIndexDown + 1, moving);
            hasChanged = true;
        }

        return hasChanged;
    }

    /// <summary>
    /// Edit Vec3i.
    /// </summary>
    static public Vec3i Vec3iField(Vec3i vec3) {
        Vec3i newVec3i = new Vec3i(vec3);
        EditorGUILayout.BeginHorizontal();
        {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                vec3CopyBuffer = new Vec3(newVec3i);
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                newVec3i = new Vec3i(vec3CopyBuffer);
            }
            GUI.color = tempColor;
            EditorGUIUtility.labelWidth = 15;
            newVec3i.x = EditorGUILayout.IntField("x", newVec3i.x, GUILayout.MinWidth(30.0f));
            newVec3i.y = EditorGUILayout.IntField("y", newVec3i.y, GUILayout.MinWidth(30.0f));
            newVec3i.z = EditorGUILayout.IntField("z", newVec3i.z, GUILayout.MinWidth(30.0f));
            EditorGUIUtility.labelWidth = 0;
        }
        EditorGUILayout.EndHorizontal();
        return newVec3i;
    }

    /// <summary>
    /// A convenient Vec3i list editor.
    /// </summary>
    /// <param name="title">An optional title string.</param>
    /// <param name="editingVec3iList">The Vec3i list we are editing.</param>
    static public bool EditVec3iList(string title, List<Vec3i> editingVec3iList) {
        return EditVec3iList(new GUIContent(title), editingVec3iList);
    }

    /// <summary>
    /// A convenient Vec3i list editor.
    /// </summary>
    /// <param name="guiContent">An optional title string.</param>
    /// <param name="editingVec3iList">The Vec3i list we are editing.</param>
    static public bool EditVec3iList(GUIContent guiContent, List<Vec3i> editingVec3iList) {
        bool hasChanged = false;

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
            GUILayout.FlexibleSpace();
            Color tempColor1 = GUI.color;
            GUI.color = Color.yellow;
            if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, GUILayout.Width(66.0f))) {
                editingVec3iList.Add(Vec3i.Zero);
                hasChanged = true;
            }
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                vec3ListCopyBuffer = new List<Vec3>(editingVec3iList.Select(x => new Vec3(x)).ToArray());
            }

            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                if (vec3ListCopyBuffer != null && vec3ListCopyBuffer.Count > 0) {
                    editingVec3iList.Clear();
                    for (int i = 0; i < vec3ListCopyBuffer.Count; i++) {
                        editingVec3iList.Add(new Vec3i(vec3ListCopyBuffer[i]));
                    }
                    hasChanged = true;
                }
            }

            GUI.color = Color.magenta;
            if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                editingVec3iList.Clear();
                hasChanged = true;
            }
            GUI.color = tempColor1;
        }
        EditorGUILayout.EndHorizontal();

        int removeIndex = -1;
        int moveIndexUp = -1;
        int moveIndexDown = -1;

        for (int i = 0; i < editingVec3iList.Count; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("[" + i.ToString("00") + "]", GUILayout.Width(CalcLabelWidth("[" + i.ToString("00") + "]")));

                Color tempColor = GUI.color;
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                    moveIndexUp = i;
                }
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↓", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                    moveIndexDown = i;
                }
                GUI.color = tempColor;

                Vec3i newVec3i = Vec3iField(editingVec3iList[i]);
                if (!newVec3i.Equals(editingVec3iList[i])) {
                    editingVec3iList[i] = newVec3i;
                    hasChanged = true;
                }

                GUI.color = Color.red;
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                    removeIndex = i;
                }
                GUI.color = tempColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex != -1) {
            editingVec3iList.RemoveAt(removeIndex);
            hasChanged = true;
        }

        if (moveIndexUp != -1 && moveIndexUp > 0) {
            Vec3i moving = new Vec3i(editingVec3iList[moveIndexUp]);
            editingVec3iList.RemoveAt(moveIndexUp);
            editingVec3iList.Insert(moveIndexUp - 1, moving);
            hasChanged = true;
        }

        if (moveIndexDown != -1 && moveIndexDown < editingVec3iList.Count - 1) {
            Vec3i moving = new Vec3i(editingVec3iList[moveIndexDown]);
            editingVec3iList.RemoveAt(moveIndexDown);
            editingVec3iList.Insert(moveIndexDown + 1, moving);
            hasChanged = true;
        }

        return hasChanged;
    }

    /// <summary>
    /// Int version of MinMaxSlider.
    /// </summary>
    static public void MinMaxSlider(ref int minValue, ref int maxValue, int minLimit, int maxLimit, params GUILayoutOption[] options) {
        float tempMinValue = minValue;
        float tempMaxValue = maxValue;
        EditorGUILayout.MinMaxSlider(ref tempMinValue, ref tempMaxValue, minLimit, maxLimit, options);
        minValue = (int)tempMinValue;
        maxValue = (int)tempMaxValue;
    }

    /// <summary>
    /// Int version of MinMaxSlider.
    /// </summary>
    static public void MinMaxSlider(string label, ref int minValue, ref int maxValue, int minLimit, int maxLimit, params GUILayoutOption[] options) {
        MinMaxSlider(new GUIContent(label), ref minValue, ref maxValue, minLimit, maxLimit, options);
    }

    /// <summary>
    /// Int version of MinMaxSlider.
    /// </summary>
    static public void MinMaxSlider(GUIContent guiContent, ref int minValue, ref int maxValue, int minLimit, int maxLimit, params GUILayoutOption[] options) {
        float tempMinValue = minValue;
        float tempMaxValue = maxValue;
        EditorGUIUtility.labelWidth = CalcLabelWidth(guiContent.text);
        EditorGUILayout.MinMaxSlider(guiContent, ref tempMinValue, ref tempMaxValue, minLimit, maxLimit, options);
        EditorGUIUtility.labelWidth = 0;
        minValue = (int)tempMinValue;
        maxValue = (int)tempMaxValue;
    }

    /// <summary>
    /// A min max slider with label.
    /// </summary>
    static public void MinMaxSliderLabel(string label, ref int minValue, ref int maxValue, int minLimit, int maxLimit, params GUILayoutOption[] options) {
        MinMaxSliderLabel(new GUIContent(label), ref minValue, ref maxValue, minLimit, maxLimit, options);
    }

    /// <summary>
    /// A min max slider with label.
    /// </summary>
    static public void MinMaxSliderLabel(GUIContent guiContent, ref int minValue, ref int maxValue, int minLimit, int maxLimit, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUIUtility.labelWidth = CalcLabelWidth(guiContent.text);
            MinMaxSlider(guiContent, ref minValue, ref maxValue, minLimit, maxLimit, options);
            EditorGUIUtility.labelWidth = 0;
            string displayMinValue = minValue.ToString("0.00");
            string displayMaxValue = maxValue.ToString("0.00");
            EditorGUILayout.LabelField("[" + displayMinValue + "," + displayMaxValue + "]", GUILayout.Width(70.0f));
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// A min max slider with label.
    /// </summary>
    static public void MinMaxSliderLabel(string label, ref float minValue, ref float maxValue, float minLimit, float maxLimit, params GUILayoutOption[] options) {
        MinMaxSliderLabel(new GUIContent(label), ref minValue, ref maxValue, minLimit, maxLimit, options);
    }

    /// <summary>
    /// A min max slider with label.
    /// </summary>
    static public void MinMaxSliderLabel(GUIContent guiContent, ref float minValue, ref float maxValue, float minLimit, float maxLimit, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUIUtility.labelWidth = CalcLabelWidth(guiContent.text);
            EditorGUILayout.MinMaxSlider(guiContent, ref minValue, ref maxValue, minLimit, maxLimit, options);
            EditorGUIUtility.labelWidth = 0;
            string displayMinValue = minValue.ToString("0.00");
            string displayMaxValue = maxValue.ToString("0.00");
            EditorGUILayout.LabelField("[" + displayMinValue + "," + displayMaxValue + "]", GUILayout.Width(70.0f));
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// A min max slider with int text field.
    /// </summary>
    static public void MinMaxSliderText(string label, ref int minValue, ref int maxValue, int minLimit, int maxLimit, params GUILayoutOption[] options) {
        MinMaxSliderText(new GUIContent(label), ref minValue, ref maxValue, minLimit, maxLimit, options);
    }

    /// <summary>
    /// A min max slider with int text field.
    /// </summary>
    static public void MinMaxSliderText(GUIContent guiContent, ref int minValue, ref int maxValue, int minLimit, int maxLimit, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUIUtility.labelWidth = CalcLabelWidth(guiContent.text);
            MinMaxSlider(guiContent, ref minValue, ref maxValue, minLimit, maxLimit, options);
            EditorGUIUtility.labelWidth = 0;
            minValue = EditorGUILayoutPlus.IntField(minValue, GUILayout.Width(45.0f));
            maxValue = EditorGUILayoutPlus.IntField(maxValue, GUILayout.Width(45.0f));
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// A min max slider with float text field.
    /// </summary>
    static public void MinMaxSliderText(string label, ref float minValue, ref float maxValue, float minLimit, float maxLimit, params GUILayoutOption[] options) {
        MinMaxSliderText(new GUIContent(label), ref minValue, ref maxValue, minLimit, maxLimit, options);
    }

    /// <summary>
    /// A min max slider with float text field.
    /// </summary>
    static public void MinMaxSliderText(GUIContent guiContent, ref float minValue, ref float maxValue, float minLimit, float maxLimit, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUIUtility.labelWidth = CalcLabelWidth(guiContent.text);
            EditorGUILayout.MinMaxSlider(guiContent, ref minValue, ref maxValue, minLimit, maxLimit, options);
            EditorGUIUtility.labelWidth = 0;
            minValue = EditorGUILayoutPlus.FloatField(minValue, GUILayout.Width(45.0f));
            maxValue = EditorGUILayoutPlus.FloatField(maxValue, GUILayout.Width(45.0f));
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// A Rect list editor which can edit rect center directly.
    /// </summary>
    /// <param name="title">An optional title.</param>
    /// <param name="editingRectList">The list we are editing.</param>
    static public void RectCenterFields(string title, List<Rect> editingRectList) {
        RectCenterFields(new GUIContent(title), editingRectList);
    }

    /// <summary>
    /// A Rect list editor which can edit rect center directly.
    /// </summary>
    /// <param name="guiContent">An optional title.</param>
    /// <param name="editingRectList">The list we are editing.</param>
    static public void RectCenterFields(GUIContent guiContent, List<Rect> editingRectList) {

        EditorGUILayout.BeginHorizontal();
        {
            Color tempColor1 = GUI.color;
            GUI.color = Color.yellow;
            if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(66.0f))) {
                editingRectList.Add(new Rect());
            }
            GUI.color = tempColor1;
            EditorGUIUtility.labelWidth = CalcLabelWidth(guiContent.text);
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
            EditorGUIUtility.labelWidth = 0;
        }
        EditorGUILayout.EndHorizontal();

        int removeIndex = -1;
        int moveIndexUp = -1;
        int moveIndexDown = -1;

        for (int i = 0; i < editingRectList.Count; ++i) {
            EditorGUILayout.BeginHorizontal("FrameBox");
            {
                EditorGUILayout.LabelField("[" + i.ToString("00") + "]", GUILayout.Width(CalcLabelWidth("[" + i.ToString("00") + "]")));

                Color tempColor = GUI.color;
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                    moveIndexUp = i;
                }
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↓", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                    moveIndexDown = i;
                }
                GUI.color = tempColor;

                editingRectList[i] = RectCenterField(editingRectList[i]);

                GUI.color = Color.red;
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                    removeIndex = i;
                }
                GUI.color = tempColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex != -1) {
            editingRectList.RemoveAt(removeIndex);
        }

        if (moveIndexUp != -1 && moveIndexUp > 0) {
            Rect moving = editingRectList[moveIndexUp];
            editingRectList.RemoveAt(moveIndexUp);
            editingRectList.Insert(moveIndexUp - 1, moving);
        }

        if (moveIndexDown != -1 && moveIndexDown < editingRectList.Count - 1) {
            Rect moving = editingRectList[moveIndexDown];
            editingRectList.RemoveAt(moveIndexDown);
            editingRectList.Insert(moveIndexDown + 1, moving);
        }
    }

    /// <summary>
    /// A Rect field which can edit rect center directly.
    /// </summary>
    /// <param name="rect">The rect we are editing.</param>
    /// <returns>The editing result.</returns>
    static public Rect RectCenterField(Rect rect) {
        EditorGUILayout.BeginHorizontal("FrameBox");
        {
            //A convient way for copy paste rect data.
            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(50.0f));
            {
                if (GUILayout.Button("Copy", EditorStyles.miniButton)) {
                    rectCopyBuffer = rect;
                }
                if (GUILayout.Button("Paste", EditorStyles.miniButton)) {
                    rect = rectCopyBuffer;
                }
            }
            EditorGUILayout.EndVertical();

            //A hack for not letting width/height changing the center pos.
            Vector2 originalCenter = new Vector2(rect.center.x, rect.center.y);

            rect.center = EditorGUILayout.Vector2Field("Center", rect.center);
            Vector2 newSize = EditorGUILayout.Vector2Field("Size", rect.size);
            if (!Util.VecEqual(newSize, rect.size)) {
                //Size has changed.
                rect.size = newSize;
                //Hack: restore center.
                rect.center = originalCenter;
            }
        }
        EditorGUILayout.EndHorizontal();
        return new Rect(rect);
    }

    /// <summary>
    /// A color field for editing hex string format colors. (without alpha)
    /// </summary>
    /// <param name="hexColor"></param>
    static public bool HexColorField(ref string hexColor) {
        bool hasChanged = false;
        Color editingColor = Util.HexToColor(hexColor);
        EditorGUILayout.BeginHorizontal();
        {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                colorCopyBuffer = editingColor;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                editingColor = colorCopyBuffer;
            }
            GUI.color = tempColor;
            Color newColor = EditorGUILayout.ColorField(editingColor);
            string newHexColor = Util.ColorToHex(newColor);
            if (newHexColor != hexColor) {
                hexColor = newHexColor;
                hasChanged = true;
            }
        }
        EditorGUILayout.EndHorizontal();
        return hasChanged;
    }

    /// <summary>
    /// A color field for editing hex string format colors. (with alpha)
    /// </summary>
    /// <param name="hexColor"></param>
    /// <param name="alpha"></param>
    static public bool HexColorField(ref string hexColor, ref float alpha) {
        bool hasChanged = false;
        Color editingColor = Util.HexToColor(hexColor, alpha);
        EditorGUILayout.BeginHorizontal();
        {
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                colorCopyBuffer = editingColor;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                editingColor = colorCopyBuffer;
            }
            GUI.color = tempColor;
            Color newColor = EditorGUILayout.ColorField(editingColor);
            string newHexColor = Util.ColorToHex(newColor);
            if (newHexColor != hexColor || alpha != newColor.a) {
                hexColor = newHexColor;
                alpha = newColor.a;
                hasChanged = true;
            }
        }
        EditorGUILayout.EndHorizontal();
        return hasChanged;
    }

    /// <summary>
    /// A set of UI for all prime editors to display the ID rename interface.
    /// Also support copy the ID string by pressing the "ID label button".
    /// </summary>
    /// <param name="editingID">Input the editing ID.</param>
    /// <param name="onRename">The delegate been called when user press rename button.</param>
    static public void IDRenameField(ref string editingID, Action<string> onRename) {
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("ID", EditorStyles.miniButton, GUILayout.Width(25.0f))) {
                EditorGUIUtility.systemCopyBuffer = editingID;
            }
            editingID = EditorGUILayoutPlus.TextField(editingID);
            Color tempColor = GUI.color;
            GUI.color = Color.cyan;
            if (GUILayout.Button("Rename", EditorStyles.miniButton, GUILayout.Width(55.0f))) {
                if (onRename != null) {
                    onRename(editingID);
                }
            }
            GUI.color = tempColor;
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// A string option popup.
    /// </summary>
    /// <param name="selectOption"></param>
    /// <param name="displayOptions"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    static public string Popup(string selectOption, string[] displayOptions, params GUILayoutOption[] options) {
        int selectIndex = new List<string>(displayOptions).IndexOf(selectOption);
        int newIndex = EditorGUILayout.Popup(selectIndex, displayOptions, options);
        if (newIndex >= 0 && newIndex < displayOptions.Length) {
            return displayOptions[newIndex];
        }
        return "";
    }

    /// <summary>
    /// An ID selection button from IDCollection.
    /// </summary>
    /// <param name="IDCollectionInput"></param>
    /// <param name="editingID"></param>
    /// <param name="onSelectID"></param>
    static public void IDSelection(string title, IDCollection IDCollectionInput, string editingID, Action<string> onSelectID, Action onOpenEditorClick,
    Action<string> onGreenLitClick = null, Func<string, string> getIDComment = null, params GUILayoutOption[] options) {
        IDSelection(new GUIContent(title), IDCollectionInput, editingID, onSelectID, onOpenEditorClick, onGreenLitClick, getIDComment, options);
    }

    /// <summary>
    /// An ID selection button from IDCollection.
    /// </summary>
    /// <param name="IDCollectionInput"></param>
    /// <param name="editingID"></param>
    /// <param name="onSelectID"></param>
    static public void IDSelection(GUIContent guiContent, IDCollection IDCollectionInput, string editingID, Action<string> onSelectID, Action onOpenEditorClick,
    Action<string> onGreenLitClick = null, Func<string, string> getIDComment = null, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            string displayText = (string.IsNullOrEmpty(editingID)) ? ("[Data ID]") : (editingID + ((getIDComment == null) ? ("") : ("(" + getIDComment(editingID) + ")")));
            if (!string.IsNullOrEmpty(guiContent.text)) {
                EditorGUILayoutPlus.LabelField(guiContent, false);
            }
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                EditorGUIUtility.systemCopyBuffer = editingID;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                if (onSelectID != null) {
                    onSelectID(EditorGUIUtility.systemCopyBuffer);
                }
            }
            GUI.color = tempColor;
            if (GUILayout.Button(displayText, EditorStyles.miniButtonMid)) {
                GenericMenu genericMenu = GenericMenuPlus.Create(IDCollectionInput, (selectedID) => {
                    if (onSelectID != null) {
                        onSelectID((string)selectedID);
                    }
                }, editingID, getIDComment);
                genericMenu.ShowAsContext();
            }
            GUI.color = ColorPlus.Cyan;
            if (GUILayout.Button("e", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                onOpenEditorClick?.Invoke();
            }
            GUI.color = tempColor;
            if (string.IsNullOrEmpty(editingID)) {
                WarningMark();
            } else if (!IDCollectionInput.IDExist(editingID)) {
                ErrorMark();
            } else {
                if (GUILayout.Button(EditorGUIUtility.IconContent("sv_icon_dot3_pix16_gizmo"), EditorStyles.label, GUILayout.Width(18.0f), GUILayout.Height(16.0f))) {
                    onGreenLitClick?.Invoke(editingID);
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// An ID selection button from IDCollection.
    /// </summary>
    /// <param name="IDCollectionInput"></param>
    /// <param name="editingID"></param>
    /// <param name="onSelectID"></param>
    /// <param name="pairedKey">This will be callback as a KeyValuePair key from GenericMenu.MenuFunction2</param>
    static public void IDSelection<T>(string title, IDCollection IDCollectionInput, string editingID, Action<T, string> onSelectID, T pairedKey, Action onOpenEditorClick,
    Action<string> onGreenLitClick = null, Func<string, string> getIDComment = null, params GUILayoutOption[] options) {
        IDSelection(new GUIContent(title), IDCollectionInput, editingID, onSelectID, pairedKey, onOpenEditorClick, onGreenLitClick, getIDComment, options);
    }

    /// <summary>
    /// An ID selection button from IDCollection.
    /// </summary>
    /// <param name="IDCollectionInput"></param>
    /// <param name="editingID"></param>
    /// <param name="onSelectID"></param>
    /// <param name="pairedKey">This will be callback as a KeyValuePair key from GenericMenu.MenuFunction2</param>
    static public void IDSelection<T>(GUIContent guiContent, IDCollection IDCollectionInput, string editingID, Action<T, string> onSelectID, T pairedKey, Action onOpenEditorClick,
    Action<string> onGreenLitClick = null, Func<string, string> getIDComment = null, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            string displayText = (string.IsNullOrEmpty(editingID)) ? ("[Data ID]") : (editingID + ((getIDComment == null) ? ("") : ("(" + getIDComment(editingID) + ")")));
            if (!string.IsNullOrEmpty(guiContent.text)) {
                EditorGUILayoutPlus.LabelField(guiContent, false);
            }
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                EditorGUIUtility.systemCopyBuffer = editingID;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                if (onSelectID != null) {
                    onSelectID(pairedKey, EditorGUIUtility.systemCopyBuffer);
                }
            }
            GUI.color = tempColor;
            if (GUILayout.Button(displayText, EditorStyles.miniButtonMid)) {
                GenericMenu genericMenu = GenericMenuPlus.Create(IDCollectionInput, (kvpObj) => {
                    if (onSelectID != null) {
                        KeyValuePair<object, string> kvp = (KeyValuePair<object, string>)kvpObj;
                        onSelectID((T)kvp.Key, kvp.Value);
                    }
                }, pairedKey, editingID, getIDComment);
                genericMenu.ShowAsContext();
            }
            GUI.color = ColorPlus.Cyan;
            if (GUILayout.Button("e", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                onOpenEditorClick?.Invoke();
            }
            GUI.color = tempColor;
            if (string.IsNullOrEmpty(editingID)) {
                WarningMark();
            } else if (!IDCollectionInput.IDExist(editingID)) {
                ErrorMark();
            } else {
                if (GUILayout.Button(EditorGUIUtility.IconContent("sv_icon_dot3_pix16_gizmo"), EditorStyles.label, GUILayout.Width(18.0f), GUILayout.Height(16.0f))) {
                    onGreenLitClick?.Invoke(editingID);
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// A convient ID list editing interface.
    /// </summary>
    static public bool EditIDList(string title, IDCollection IDCollectionInput, List<string> editingIDList, Action onOpenEditorClick,
    Action<string> onGreenLitClick = null, Func<string, string> getIDComment = null) {
        return EditIDList(new GUIContent(title), IDCollectionInput, editingIDList, onOpenEditorClick, onGreenLitClick, getIDComment);
    }

    /// <summary>
    /// A convient ID list editing interface.
    /// </summary>
    static public bool EditIDList(GUIContent guiContent, IDCollection IDCollectionInput, List<string> editingIDList, Action onOpenEditorClick,
    Action<string> onGreenLitClick = null, Func<string, string> getIDComment = null) {
        bool hasChanged = false;

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
            GUILayout.FlexibleSpace();
            Color tempColor1 = GUI.color;
            GUI.color = Color.yellow;
            if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, GUILayout.Width(66.0f))) {
                editingIDList.Add("");
                hasChanged = true;
            }
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                stringListBuffer = new List<string>(editingIDList);
            }

            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                if (stringListBuffer != null && stringListBuffer.Count > 0) {
                    editingIDList.Clear();
                    editingIDList.AddRange(stringListBuffer);
                    hasChanged = true;
                }
            }

            GUI.color = ColorPlus.SpringGreen;
            if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                List<string> pastedMatrix = Util.StringSplitByNewLineTab(EditorGUIUtility.systemCopyBuffer);
                editingIDList.Clear();
                editingIDList.AddRange(pastedMatrix);
                hasChanged = true;
            }
            GUI.color = Color.magenta;
            if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                editingIDList.Clear();
                hasChanged = true;
            }
            GUI.color = tempColor1;
        }
        EditorGUILayout.EndHorizontal();

        int removeIndex = -1;
        int moveIndexUp = -1;
        int moveIndexDown = -1;

        for (int i = 0; i < editingIDList.Count; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor = GUI.color;
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                    moveIndexUp = i;
                }
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↓", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                    moveIndexDown = i;
                }
                GUI.color = tempColor;

                IDSelection("[" + i.ToString("00") + "]", IDCollectionInput, editingIDList[i], (index, ID) => {
                    if (index >= 0 && index < editingIDList.Count) {
                        editingIDList[index] = ID;
                    }
                }, i, onOpenEditorClick, onGreenLitClick, getIDComment);

                GUI.color = Color.red;
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                    removeIndex = i;
                }
                GUI.color = tempColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex != -1) {
            editingIDList.RemoveAt(removeIndex);
            hasChanged = true;
        }

        if (moveIndexUp != -1 && moveIndexUp > 0) {
            string moving = editingIDList[moveIndexUp];
            editingIDList.RemoveAt(moveIndexUp);
            editingIDList.Insert(moveIndexUp - 1, moving);
            hasChanged = true;
        }

        if (moveIndexDown != -1 && moveIndexDown < editingIDList.Count - 1) {
            string moving = editingIDList[moveIndexDown];
            editingIDList.RemoveAt(moveIndexDown);
            editingIDList.Insert(moveIndexDown + 1, moving);
            hasChanged = true;
        }

        return hasChanged;
    }

    /// <summary>
    /// An ID selection button from IDIndex.
    /// </summary>
    /// <param name="IDIndexInput"></param>
    /// <param name="editingID"></param>
    /// <param name="onSelectID"></param>
    static public void IDSelection(string title, IDIndex IDIndexInput, string editingID, Action<string> onSelectID, Action onOpenEditorClick,
    Action<string> onGreenLitClick = null, Func<string, string> getIDComment = null, params GUILayoutOption[] options) {
        IDSelection(new GUIContent(title), IDIndexInput, editingID, onSelectID, onOpenEditorClick, onGreenLitClick, getIDComment, options);
    }

    /// <summary>
    /// An ID selection button from IDIndex.
    /// </summary>
    /// <param name="IDIndexInput"></param>
    /// <param name="editingID"></param>
    /// <param name="onSelectID"></param>
    static public void IDSelection(GUIContent guiContent, IDIndex IDIndexInput, string editingID, Action<string> onSelectID, Action onOpenEditorClick,
    Action<string> onGreenLitClick = null, Func<string, string> getIDComment = null, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            string displayText = (string.IsNullOrEmpty(editingID)) ? ("[Data ID]") : (editingID + ((getIDComment == null) ? ("") : ("(" + getIDComment(editingID) + ")")));
            if (!string.IsNullOrEmpty(guiContent.text)) {
                EditorGUILayoutPlus.LabelField(guiContent, false);
            }
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                EditorGUIUtility.systemCopyBuffer = editingID;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                if (onSelectID != null) {
                    onSelectID(EditorGUIUtility.systemCopyBuffer);
                }
            }
            GUI.color = tempColor;
            if (GUILayout.Button(displayText, EditorStyles.miniButtonMid)) {
                GenericMenu genericMenu = GenericMenuPlus.Create(IDIndexInput, (selectedID) => {
                    if (onSelectID != null) {
                        onSelectID((string)selectedID);
                    }
                }, editingID, getIDComment);
                genericMenu.ShowAsContext();
            }
            GUI.color = ColorPlus.Cyan;
            if (GUILayout.Button("e", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                onOpenEditorClick?.Invoke();
            }
            GUI.color = tempColor;
            if (string.IsNullOrEmpty(editingID)) {
                WarningMark();
            } else if (!IDIndexInput.IDExist(editingID)) {
                ErrorMark();
            } else {
                if (GUILayout.Button(EditorGUIUtility.IconContent("sv_icon_dot3_pix16_gizmo"), EditorStyles.label, GUILayout.Width(18.0f), GUILayout.Height(16.0f))) {
                    onGreenLitClick?.Invoke(editingID);
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// An ID selection button from IDIndex.
    /// </summary>
    /// <param name="IDIndexInput"></param>
    /// <param name="editingID"></param>
    /// <param name="onSelectID"></param>
    /// <param name="pairedKey">This will be callback as a KeyValuePair key from GenericMenu.MenuFunction2</param>
    static public void IDSelection<T>(string title, IDIndex IDIndexInput, string editingID, Action<T, string> onSelectID, T pairedKey, Action onOpenEditorClick,
    Action<string> onGreenLitClick = null, Func<string, string> getIDComment = null, params GUILayoutOption[] options) {
        IDSelection(new GUIContent(title), IDIndexInput, editingID, onSelectID, pairedKey, onOpenEditorClick, onGreenLitClick, getIDComment, options);
    }

    /// <summary>
    /// An ID selection button from IDIndex.
    /// </summary>
    /// <param name="IDIndexInput"></param>
    /// <param name="editingID"></param>
    /// <param name="onSelectID"></param>
    /// <param name="pairedKey">This will be callback as a KeyValuePair key from GenericMenu.MenuFunction2</param>
    static public void IDSelection<T>(GUIContent guiContent, IDIndex IDIndexInput, string editingID, Action<T, string> onSelectID, T pairedKey, Action onOpenEditorClick,
    Action<string> onGreenLitClick = null, Func<string, string> getIDComment = null, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            string displayText = (string.IsNullOrEmpty(editingID)) ? ("[Data ID]") : (editingID + ((getIDComment == null) ? ("") : ("(" + getIDComment(editingID) + ")")));
            if (!string.IsNullOrEmpty(guiContent.text)) {
                EditorGUILayoutPlus.LabelField(guiContent, false);
            }
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                EditorGUIUtility.systemCopyBuffer = editingID;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                if (onSelectID != null) {
                    onSelectID(pairedKey, EditorGUIUtility.systemCopyBuffer);
                }
            }
            GUI.color = tempColor;
            if (GUILayout.Button(displayText, EditorStyles.miniButtonMid)) {
                GenericMenu genericMenu = GenericMenuPlus.Create(IDIndexInput, (kvpObj) => {
                    if (onSelectID != null) {
                        KeyValuePair<object, string> kvp = (KeyValuePair<object, string>)kvpObj;
                        onSelectID((T)kvp.Key, kvp.Value);
                    }
                }, pairedKey, editingID, getIDComment);
                genericMenu.ShowAsContext();
            }
            GUI.color = ColorPlus.Cyan;
            if (GUILayout.Button("e", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                onOpenEditorClick?.Invoke();
            }
            GUI.color = tempColor;
            if (string.IsNullOrEmpty(editingID)) {
                WarningMark();
            } else if (!IDIndexInput.IDExist(editingID)) {
                ErrorMark();
            } else {
                if (GUILayout.Button(EditorGUIUtility.IconContent("sv_icon_dot3_pix16_gizmo"), EditorStyles.label, GUILayout.Width(18.0f), GUILayout.Height(16.0f))) {
                    onGreenLitClick?.Invoke(editingID);
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// A convient ID list editing interface.
    /// </summary>
    static public bool EditIDList(string title, IDIndex IDIndexInput, List<string> editingIDList, Action onOpenEditorClick,
    Action<string> onGreenLitClick = null, Func<string, string> getIDComment = null) {
        return EditIDList(new GUIContent(title), IDIndexInput, editingIDList, onOpenEditorClick, onGreenLitClick, getIDComment);
    }

    /// <summary>
    /// A convient ID list editing interface.
    /// </summary>
    static public bool EditIDList(GUIContent guiContent, IDIndex IDIndexInput, List<string> editingIDList, Action onOpenEditorClick,
    Action<string> onGreenLitClick = null, Func<string, string> getIDComment = null) {
        bool hasChanged = false;

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
            GUILayout.FlexibleSpace();
            Color tempColor1 = GUI.color;
            GUI.color = Color.yellow;
            if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, GUILayout.Width(66.0f))) {
                editingIDList.Add("");
                hasChanged = true;
            }
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                stringListBuffer = new List<string>(editingIDList);
            }

            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                if (stringListBuffer != null && stringListBuffer.Count > 0) {
                    editingIDList.Clear();
                    editingIDList.AddRange(stringListBuffer);
                    hasChanged = true;
                }
            }

            GUI.color = ColorPlus.SpringGreen;
            if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                List<string> pastedMatrix = Util.StringSplitByNewLineTab(EditorGUIUtility.systemCopyBuffer);
                editingIDList.Clear();
                editingIDList.AddRange(pastedMatrix);
                hasChanged = true;
            }
            GUI.color = Color.magenta;
            if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                editingIDList.Clear();
                hasChanged = true;
            }
            GUI.color = tempColor1;
        }
        EditorGUILayout.EndHorizontal();

        int removeIndex = -1;
        int moveIndexUp = -1;
        int moveIndexDown = -1;

        for (int i = 0; i < editingIDList.Count; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor = GUI.color;

                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                    moveIndexUp = i;
                }
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↓", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                    moveIndexDown = i;
                }
                GUI.color = tempColor;

                IDSelection("[" + i.ToString("00") + "]", IDIndexInput, editingIDList[i], (index, ID) => {
                    if (index >= 0 && index < editingIDList.Count) {
                        editingIDList[index] = ID;
                    }
                }, i, onOpenEditorClick, onGreenLitClick, getIDComment);

                GUI.color = Color.red;
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                    removeIndex = i;
                }
                GUI.color = tempColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex != -1) {
            editingIDList.RemoveAt(removeIndex);
            hasChanged = true;
        }

        if (moveIndexUp != -1 && moveIndexUp > 0) {
            string moving = editingIDList[moveIndexUp];
            editingIDList.RemoveAt(moveIndexUp);
            editingIDList.Insert(moveIndexUp - 1, moving);
            hasChanged = true;
        }

        if (moveIndexDown != -1 && moveIndexDown < editingIDList.Count - 1) {
            string moving = editingIDList[moveIndexDown];
            editingIDList.RemoveAt(moveIndexDown);
            editingIDList.Insert(moveIndexDown + 1, moving);
            hasChanged = true;
        }

        return hasChanged;
    }

    /// <summary>
    /// An asset name selection box.
    /// </summary>
    /// <param name="title">An optional title.</param>
    /// <param name="assetsFolderPath">Start with "Assets/" and "SHOULD NOT" end with "/".</param>
    /// <param name="editingAssetName">The current editing asset name.</param>
    /// <param name="targetTerm">prefab/texture2D/audioClip/font/sprite/script etc.</param>
    /// <param name="onSelectAssetName">GenericMenu callback.</param>
    /// <param name="options">GUILayout options.</param>
    static public void AssetNameSelection(string title, string assetsFolderPath, string editingAssetName, string targetTerm, Action<string> onSelectAssetName, params GUILayoutOption[] options) {
        AssetNameSelection(new GUIContent(title), assetsFolderPath, editingAssetName, targetTerm, onSelectAssetName, options);
    }

    /// <summary>
    /// An asset name selection box.
    /// </summary>
    /// <param name="title">An optional title.</param>
    /// <param name="assetsFolderPath">Start with "Assets/" and "SHOULD NOT" end with "/".</param>
    /// <param name="editingAssetName">The current editing asset name.</param>
    /// <param name="targetTerm">prefab/texture2D/audioClip/font/sprite/script etc.</param>
    /// <param name="onSelectAssetName">GenericMenu callback.</param>
    /// <param name="options">GUILayout options.</param>
    static public void AssetNameSelection(GUIContent guiContent, string assetsFolderPath, string editingAssetName, string targetTerm, Action<string> onSelectAssetName, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            string displayText = (string.IsNullOrEmpty(editingAssetName)) ? ("[" + targetTerm + "]") : (editingAssetName);
            if (!string.IsNullOrEmpty(guiContent.text)) {
                EditorGUILayoutPlus.LabelField(guiContent, false);
            }
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                EditorGUIUtility.systemCopyBuffer = editingAssetName;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                if (onSelectAssetName != null) {
                    onSelectAssetName(EditorGUIUtility.systemCopyBuffer);
                }
            }
            GUI.color = tempColor;
            if (GUILayout.Button(displayText, EditorStyles.miniButtonMid)) {
                GenericMenu genericMenu = GenericMenuPlus.Create(assetsFolderPath, targetTerm, "", (selectedAssetName) => {
                    if (onSelectAssetName != null) {
                        onSelectAssetName((string)selectedAssetName);
                    }
                }, editingAssetName);
                genericMenu.ShowAsContext();
            }
            GUI.color = ColorPlus.Cyan;
            if (GUILayout.Button("f", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetsFolderPath);
            }
            GUI.color = tempColor;
            if (string.IsNullOrEmpty(editingAssetName)) {
                WarningMark();
            } else {
                if (AssetDatabasePlus.IsAssetExistCache(assetsFolderPath, targetTerm, editingAssetName)) {
                    if (GUILayout.Button(EditorGUIUtility.IconContent("sv_icon_dot3_pix16_gizmo"), EditorStyles.label, GUILayout.Width(18.0f), GUILayout.Height(16.0f))) {
                        Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabasePlus.GetAssetPathFromRecordCache(assetsFolderPath, targetTerm, editingAssetName));
                    }
                } else {
                    ErrorMark();
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// An asset name selection box.
    /// </summary>
    /// <param name="title">An optional title.</param>
    /// <param name="assetsFolderPath">Start with "Assets/" and "SHOULD NOT" end with "/".</param>
    /// <param name="editingAssetName">The current editing asset name.</param>
    /// <param name="targetTerm">prefab/texture2D/audioClip/font/sprite/script etc.</param>
    /// <param name="onSelectAssetName">GenericMenu callback.</param>
    /// <param name="pairedKey">This will be callback as a KeyValuePair key from GenericMenu.MenuFunction2</param>
    /// <param name="options">GUILayout options.</param>
    static public void AssetNameSelection<T>(string title, string assetsFolderPath, string editingAssetName, string targetTerm, Action<T, string> onSelectAssetName, T pairedKey, params GUILayoutOption[] options) {
        AssetNameSelection(new GUIContent(title), assetsFolderPath, editingAssetName, targetTerm, onSelectAssetName, pairedKey, options);
    }

    /// <summary>
    /// An asset name selection box.
    /// </summary>
    /// <param name="title">An optional title.</param>
    /// <param name="assetsFolderPath">Start with "Assets/" and "SHOULD NOT" end with "/".</param>
    /// <param name="editingAssetName">The current editing asset name.</param>
    /// <param name="targetTerm">prefab/texture2D/audioClip/font/sprite/script etc.</param>
    /// <param name="onSelectAssetName">GenericMenu callback.</param>
    /// <param name="pairedKey">This will be callback as a KeyValuePair key from GenericMenu.MenuFunction2</param>
    /// <param name="options">GUILayout options.</param>
    static public void AssetNameSelection<T>(GUIContent guiContent, string assetsFolderPath, string editingAssetName, string targetTerm, Action<T, string> onSelectAssetName, T pairedKey, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            string displayText = (string.IsNullOrEmpty(editingAssetName)) ? ("[" + targetTerm + "]") : (editingAssetName);
            if (!string.IsNullOrEmpty(guiContent.text)) {
                EditorGUILayoutPlus.LabelField(guiContent, false);
            }
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                EditorGUIUtility.systemCopyBuffer = editingAssetName;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                if (onSelectAssetName != null) {
                    onSelectAssetName(pairedKey, EditorGUIUtility.systemCopyBuffer);
                }
            }
            GUI.color = tempColor;
            if (GUILayout.Button(displayText, EditorStyles.miniButtonMid)) {
                GenericMenu genericMenu = GenericMenuPlus.Create(assetsFolderPath, targetTerm, "", (kvpObj) => {
                    if (onSelectAssetName != null) {
                        KeyValuePair<object, string> kvp = (KeyValuePair<object, string>)kvpObj;
                        onSelectAssetName((T)kvp.Key, kvp.Value);
                    }
                }, pairedKey, editingAssetName);
                genericMenu.ShowAsContext();
            }
            GUI.color = ColorPlus.Cyan;
            if (GUILayout.Button("f", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetsFolderPath);
            }
            GUI.color = tempColor;
            if (string.IsNullOrEmpty(editingAssetName)) {
                WarningMark();
            } else {
                if (AssetDatabasePlus.IsAssetExistCache(assetsFolderPath, targetTerm, editingAssetName)) {
                    if (GUILayout.Button(EditorGUIUtility.IconContent("sv_icon_dot3_pix16_gizmo"), EditorStyles.label, GUILayout.Width(18.0f), GUILayout.Height(16.0f))) {
                        Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabasePlus.GetAssetPathFromRecordCache(assetsFolderPath, targetTerm, editingAssetName));
                    }
                } else {
                    ErrorMark();
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// A convient asset list editing interface.
    /// </summary>
    /// <param name="title">An optional title.</param>
    /// <param name="assetsFolderPath">Start with "Assets/" and "SHOULD NOT" end with "/".</param>
    /// <param name="editingAssetNameList"></param>
    /// <param name="targetTerm">prefab/texture2D/audioClip/font/sprite/script etc.</param>
    /// <returns></returns>
    static public bool EditAssetNameList(string title, string assetsFolderPath, List<string> editingAssetNameList, string targetTerm) {
        return EditAssetNameList(new GUIContent(title), assetsFolderPath, editingAssetNameList, targetTerm);
    }

    /// <summary>
    /// A convient asset list editing interface.
    /// </summary>
    /// <param name="title">An optional title.</param>
    /// <param name="assetsFolderPath">Start with "Assets/" and "SHOULD NOT" end with "/".</param>
    /// <param name="editingAssetNameList"></param>
    /// <param name="targetTerm">prefab/texture2D/audioClip/font/sprite/script etc.</param>
    /// <returns></returns>
    static public bool EditAssetNameList(GUIContent guiContent, string assetsFolderPath, List<string> editingAssetNameList, string targetTerm) {

        bool hasChanged = false;

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
            GUILayout.FlexibleSpace();
            Color tempColor1 = GUI.color;
            GUI.color = Color.yellow;
            if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, GUILayout.Width(66.0f))) {
                editingAssetNameList.Add("");
                hasChanged = true;
            }
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                stringListBuffer = new List<string>(editingAssetNameList);
            }

            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                if (stringListBuffer != null && stringListBuffer.Count > 0) {
                    editingAssetNameList.Clear();
                    editingAssetNameList.AddRange(stringListBuffer);
                    hasChanged = true;
                }
            }

            GUI.color = ColorPlus.SpringGreen;
            if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                List<string> pastedMatrix = Util.StringSplitByNewLineTab(EditorGUIUtility.systemCopyBuffer);
                editingAssetNameList.Clear();
                editingAssetNameList.AddRange(pastedMatrix);
                hasChanged = true;
            }
            GUI.color = Color.magenta;
            if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                editingAssetNameList.Clear();
                hasChanged = true;
            }
            GUI.color = tempColor1;
        }
        EditorGUILayout.EndHorizontal();

        int removeIndex = -1;
        int moveIndexUp = -1;
        int moveIndexDown = -1;

        for (int i = 0; i < editingAssetNameList.Count; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor = GUI.color;
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                    moveIndexUp = i;
                }
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↓", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                    moveIndexDown = i;
                }
                GUI.color = tempColor;

                AssetNameSelection("[" + i.ToString("00") + "]", assetsFolderPath, editingAssetNameList[i], targetTerm, (index, assetName) => {
                    if (index >= 0 && index < editingAssetNameList.Count) {
                        editingAssetNameList[index] = assetName;
                    }
                }, i);

                GUI.color = Color.red;
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                    removeIndex = i;
                }
                GUI.color = tempColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex != -1) {
            editingAssetNameList.RemoveAt(removeIndex);
            hasChanged = true;
        }

        if (moveIndexUp != -1 && moveIndexUp > 0) {
            string moving = editingAssetNameList[moveIndexUp];
            editingAssetNameList.RemoveAt(moveIndexUp);
            editingAssetNameList.Insert(moveIndexUp - 1, moving);
            hasChanged = true;
        }

        if (moveIndexDown != -1 && moveIndexDown < editingAssetNameList.Count - 1) {
            string moving = editingAssetNameList[moveIndexDown];
            editingAssetNameList.RemoveAt(moveIndexDown);
            editingAssetNameList.Insert(moveIndexDown + 1, moving);
            hasChanged = true;
        }

        return hasChanged;
    }

    /// <summary>
    /// An AudioClip asset name selection box.
    /// </summary>
    /// <param name="title">An optional title.</param>
    /// <param name="assetsFolderPath">Start with "Assets/" and "SHOULD NOT" end with "/".</param>
    /// <param name="editingAssetName">The current editing asset name.</param>
    /// <param name="onSelectAssetName">GenericMenu callback.</param>
    /// <param name="options">GUILayout options.</param>
    static public void AudioClipAssetNameSelection(string title, string assetsFolderPath, string editingAssetName, Action<string> onSelectAssetName, params GUILayoutOption[] options) {
        AudioClipAssetNameSelection(new GUIContent(title), assetsFolderPath, editingAssetName, onSelectAssetName, options);
    }

    /// <summary>
    /// An AudioClip asset name selection box.
    /// </summary>
    /// <param name="title">An optional title.</param>
    /// <param name="assetsFolderPath">Start with "Assets/" and "SHOULD NOT" end with "/".</param>
    /// <param name="editingAssetName">The current editing asset name.</param>
    /// <param name="onSelectAssetName">GenericMenu callback.</param>
    /// <param name="options">GUILayout options.</param>
    static public void AudioClipAssetNameSelection(GUIContent guiContent, string assetsFolderPath, string editingAssetName, Action<string> onSelectAssetName, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            string displayText = (string.IsNullOrEmpty(editingAssetName)) ? ("[AudioClip]") : (editingAssetName);
            if (!string.IsNullOrEmpty(guiContent.text)) {
                EditorGUILayoutPlus.LabelField(guiContent, false);
            }
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                EditorGUIUtility.systemCopyBuffer = editingAssetName;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                if (onSelectAssetName != null) {
                    onSelectAssetName(EditorGUIUtility.systemCopyBuffer);
                }
            }
            GUI.color = tempColor;
            if (GUILayout.Button(displayText, EditorStyles.miniButtonMid)) {
                GenericMenu genericMenu = GenericMenuPlus.Create(assetsFolderPath, "AudioClip", "", (selectedAssetName) => {
                    if (onSelectAssetName != null) {
                        onSelectAssetName((string)selectedAssetName);
                    }
                }, editingAssetName);
                genericMenu.ShowAsContext();
            }

            GUI.color = ColorPlus.LightGreen;
            if (GUILayout.Button("v", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabasePlus.GetAssetPathFromRecordCache(assetsFolderPath, "AudioClip", editingAssetName)); ;
                if (audioClip != null) {
                    EditorUtilPlus.PlayClip(audioClip);
                }
            }

            GUI.color = ColorPlus.LightPink;
            if (GUILayout.Button("s", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                EditorUtilPlus.StopAllClips();
            }

            GUI.color = ColorPlus.Cyan;
            if (GUILayout.Button("f", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetsFolderPath);
            }
            GUI.color = tempColor;
            if (string.IsNullOrEmpty(editingAssetName)) {
                WarningMark();
            } else {
                if (AssetDatabasePlus.IsAssetExistCache(assetsFolderPath, "AudioClip", editingAssetName)) {
                    if (GUILayout.Button(EditorGUIUtility.IconContent("sv_icon_dot3_pix16_gizmo"), EditorStyles.label, GUILayout.Width(18.0f), GUILayout.Height(16.0f))) {
                        Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabasePlus.GetAssetPathFromRecordCache(assetsFolderPath, "AudioClip", editingAssetName));
                    }
                } else {
                    ErrorMark();
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// An AudioClip asset name selection box.
    /// </summary>
    /// <param name="title">An optional title.</param>
    /// <param name="assetsFolderPath">Start with "Assets/" and "SHOULD NOT" end with "/".</param>
    /// <param name="editingAssetName">The current editing asset name.</param>
    /// <param name="onSelectAssetName">GenericMenu callback.</param>
    /// <param name="pairedKey">This will be callback as a KeyValuePair key from GenericMenu.MenuFunction2</param>
    /// <param name="options">GUILayout options.</param>
    static public void AudioClipAssetNameSelection<T>(string title, string assetsFolderPath, string editingAssetName, Action<T, string> onSelectAssetName, T pairedKey, params GUILayoutOption[] options) {
        AudioClipAssetNameSelection(new GUIContent(title), assetsFolderPath, editingAssetName, onSelectAssetName, pairedKey, options);
    }

    /// <summary>
    /// An AudioClip asset name selection box.
    /// </summary>
    /// <param name="title">An optional title.</param>
    /// <param name="assetsFolderPath">Start with "Assets/" and "SHOULD NOT" end with "/".</param>
    /// <param name="editingAssetName">The current editing asset name.</param>
    /// <param name="onSelectAssetName">GenericMenu callback.</param>
    /// <param name="pairedKey">This will be callback as a KeyValuePair key from GenericMenu.MenuFunction2</param>
    /// <param name="options">GUILayout options.</param>
    static public void AudioClipAssetNameSelection<T>(GUIContent guiContent, string assetsFolderPath, string editingAssetName, Action<T, string> onSelectAssetName, T pairedKey, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal(options);
        {
            string displayText = (string.IsNullOrEmpty(editingAssetName)) ? ("[AudioClip]") : (editingAssetName);
            if (!string.IsNullOrEmpty(guiContent.text)) {
                EditorGUILayoutPlus.LabelField(guiContent, false);
            }
            Color tempColor = GUI.color;
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                EditorGUIUtility.systemCopyBuffer = editingAssetName;
            }
            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                if (onSelectAssetName != null) {
                    onSelectAssetName(pairedKey, EditorGUIUtility.systemCopyBuffer);
                }
            }
            GUI.color = tempColor;
            if (GUILayout.Button(displayText, EditorStyles.miniButtonMid)) {
                GenericMenu genericMenu = GenericMenuPlus.Create(assetsFolderPath, "AudioClip", "", (kvpObj) => {
                    if (onSelectAssetName != null) {
                        KeyValuePair<object, string> kvp = (KeyValuePair<object, string>)kvpObj;
                        onSelectAssetName((T)kvp.Key, kvp.Value);
                    }
                }, pairedKey, editingAssetName);
                genericMenu.ShowAsContext();
            }

            GUI.color = ColorPlus.LightGreen;
            if (GUILayout.Button("v", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabasePlus.GetAssetPathFromRecordCache(assetsFolderPath, "AudioClip", editingAssetName)); ;
                if (audioClip != null) {
                    EditorUtilPlus.PlayClip(audioClip);
                }
            }

            GUI.color = ColorPlus.LightPink;
            if (GUILayout.Button("s", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                EditorUtilPlus.StopAllClips();
            }

            GUI.color = ColorPlus.Cyan;
            if (GUILayout.Button("f", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetsFolderPath);
            }
            GUI.color = tempColor;
            if (string.IsNullOrEmpty(editingAssetName)) {
                WarningMark();
            } else {
                if (AssetDatabasePlus.IsAssetExistCache(assetsFolderPath, "AudioClip", editingAssetName)) {
                    if (GUILayout.Button(EditorGUIUtility.IconContent("sv_icon_dot3_pix16_gizmo"), EditorStyles.label, GUILayout.Width(18.0f), GUILayout.Height(16.0f))) {
                        Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabasePlus.GetAssetPathFromRecordCache(assetsFolderPath, "AudioClip", editingAssetName));
                    }
                } else {
                    ErrorMark();
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// A convient AudioClip asset list editing interface.
    /// </summary>
    /// <param name="title">An optional title.</param>
    /// <param name="assetsFolderPath">Start with "Assets/" and "SHOULD NOT" end with "/".</param>
    /// <param name="editingAssetNameList"></param>
    /// <returns></returns>
    static public bool EditAudioClipAssetNameList(string title, string assetsFolderPath, List<string> editingAssetNameList) {
        return EditAudioClipAssetNameList(new GUIContent(title), assetsFolderPath, editingAssetNameList);
    }

    /// <summary>
    /// A convient AudioClip asset list editing interface.
    /// </summary>
    /// <param name="title">An optional title.</param>
    /// <param name="assetsFolderPath">Start with "Assets/" and "SHOULD NOT" end with "/".</param>
    /// <param name="editingAssetNameList"></param>
    /// <returns></returns>
    static public bool EditAudioClipAssetNameList(GUIContent guiContent, string assetsFolderPath, List<string> editingAssetNameList) {

        bool hasChanged = false;

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel, GUILayout.Width(CalcBoldLabelWidth(guiContent.text)));
            GUILayout.FlexibleSpace();
            Color tempColor1 = GUI.color;
            GUI.color = Color.yellow;
            if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, GUILayout.Width(66.0f))) {
                editingAssetNameList.Add("");
                hasChanged = true;
            }
            GUI.color = ColorPlus.LightSalmon;
            if (GUILayout.Button("c", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                stringListBuffer = new List<string>(editingAssetNameList);
            }

            GUI.color = ColorPlus.LightBlue;
            if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                if (stringListBuffer != null && stringListBuffer.Count > 0) {
                    editingAssetNameList.Clear();
                    editingAssetNameList.AddRange(stringListBuffer);
                    hasChanged = true;
                }
            }

            GUI.color = ColorPlus.SpringGreen;
            if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                List<string> pastedMatrix = Util.StringSplitByNewLineTab(EditorGUIUtility.systemCopyBuffer);
                editingAssetNameList.Clear();
                editingAssetNameList.AddRange(pastedMatrix);
                hasChanged = true;
            }
            GUI.color = Color.magenta;
            if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                editingAssetNameList.Clear();
                hasChanged = true;
            }
            GUI.color = tempColor1;
        }
        EditorGUILayout.EndHorizontal();

        int removeIndex = -1;
        int moveIndexUp = -1;
        int moveIndexDown = -1;

        for (int i = 0; i < editingAssetNameList.Count; ++i) {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor = GUI.color;
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                    moveIndexUp = i;
                }
                GUI.color = ColorPlus.Lavender;
                if (GUILayout.Button("↓", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                    moveIndexDown = i;
                }
                GUI.color = tempColor;

                AudioClipAssetNameSelection("[" + i.ToString("00") + "]", assetsFolderPath, editingAssetNameList[i], (index, assetName) => {
                    if (index >= 0 && index < editingAssetNameList.Count) {
                        editingAssetNameList[index] = assetName;
                    }
                }, i);

                GUI.color = Color.red;
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                    removeIndex = i;
                }
                GUI.color = tempColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex != -1) {
            editingAssetNameList.RemoveAt(removeIndex);
            hasChanged = true;
        }

        if (moveIndexUp != -1 && moveIndexUp > 0) {
            string moving = editingAssetNameList[moveIndexUp];
            editingAssetNameList.RemoveAt(moveIndexUp);
            editingAssetNameList.Insert(moveIndexUp - 1, moving);
            hasChanged = true;
        }

        if (moveIndexDown != -1 && moveIndexDown < editingAssetNameList.Count - 1) {
            string moving = editingAssetNameList[moveIndexDown];
            editingAssetNameList.RemoveAt(moveIndexDown);
            editingAssetNameList.Insert(moveIndexDown + 1, moving);
            hasChanged = true;
        }

        return hasChanged;
    }

    /// <summary> 
    /// Draw a horizontal line. 
    /// </summary> 
    static public void HorizontalLine() {
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1.0f));
    }

    /// <summary>
    /// Draw a horizontal rule.
    /// </summary>
    static public void HorizontalRule() {
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3.0f));
    }

    /// <summary>
    /// Draw a seperator.
    /// </summary>
    static public void Separator(float width = 5.0f) {
        GUILayout.Box("", GUILayout.Width(width), GUILayout.Height(13.0f));
    }

    /// <summary>
    /// Draw an error mark.
    /// </summary>
    static public void ErrorMark() {
        EditorGUILayout.LabelField(EditorGUIUtility.IconContent("sv_icon_dot6_pix16_gizmo"), GUILayout.Width(18.0f));
    }

    /// <summary>
    /// Draw a warning mark.
    /// </summary>
    static public void WarningMark() {
        EditorGUILayout.LabelField(EditorGUIUtility.IconContent("sv_icon_dot4_pix16_gizmo"), GUILayout.Width(18.0f));
    }

    /// <summary>
    /// Draw an ok mark.
    /// </summary>
    static public void OkMark() {
        EditorGUILayout.LabelField(EditorGUIUtility.IconContent("sv_icon_dot3_pix16_gizmo"), GUILayout.Width(18.0f));
    }

    //http://answers.unity3d.com/questions/29470/find-width-of-strings-for-guilayoutlablel.html
    public static float CalcLabelWidth(string text) {
        return EditorStyles.label.CalcSize(new GUIContent(text)).x + 5.0f;
    }

    public static float CalcBoldLabelWidth(string text) {
        return EditorStyles.boldLabel.CalcSize(new GUIContent(text)).x + 5.0f;
    }

    public static float CalcMiniLabelWidth(string text) {
        return EditorStyles.centeredGreyMiniLabel.CalcSize(new GUIContent(text)).x + 5.0f;
    }
}

#endif