#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class IDPairEditor : EditorWindow {

    static public bool EditIDPair(IDPair idPair, IDCollection IDCollectionInput1, IDCollection IDCollectionInput2, Action onOpenEditorClick1, Action onOpenEditorClick2, Action<string> onGreenLitClick1 = null, Action<string> onGreenLitClick2 = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection("ID1", IDCollectionInput1, idPair.ID1, (ID) => {
            idPair.ID1 = ID;
            hasChanged = true;
        }, onOpenEditorClick1, onGreenLitClick1);

        EditorGUILayoutPlus.IDSelection("ID2", IDCollectionInput2, idPair.ID2, (ID) => {
            idPair.ID2 = ID;
            hasChanged = true;
        }, onOpenEditorClick2, onGreenLitClick2);
        return hasChanged;
    }

    static public bool EditIDPair(string IDLabel1, string IDLabel2, IDPair idPair, IDCollection IDCollectionInput1, IDCollection IDCollectionInput2, Action onOpenEditorClick1, Action onOpenEditorClick2, Action<string> onGreenLitClick1 = null, Action<string> onGreenLitClick2 = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection(IDLabel1, IDCollectionInput1, idPair.ID1, (ID) => {
            idPair.ID1 = ID;
            hasChanged = true;
        }, onOpenEditorClick1, onGreenLitClick1);

        EditorGUILayoutPlus.IDSelection(IDLabel2, IDCollectionInput2, idPair.ID2, (ID) => {
            idPair.ID2 = ID;
            hasChanged = true;
        }, onOpenEditorClick2, onGreenLitClick2);
        return hasChanged;
    }

    //--

    static public bool EditIDPair(IDPair idPair, IDIndex IDIndex1, IDIndex IDIndex2, Action onOpenEditorClick1, Action onOpenEditorClick2, Action<string> onGreenLitClick1 = null, Action<string> onGreenLitClick2 = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection("ID1", IDIndex1, idPair.ID1, (ID) => {
            idPair.ID1 = ID;
            hasChanged = true;
        }, onOpenEditorClick1, onGreenLitClick1);

        EditorGUILayoutPlus.IDSelection("ID2", IDIndex2, idPair.ID2, (ID) => {
            idPair.ID2 = ID;
            hasChanged = true;
        }, onOpenEditorClick2, onGreenLitClick2);
        return hasChanged;
    }

    static public bool EditIDPair(string IDLabel1, string IDLabel2, IDPair idPair, IDIndex IDIndex1, IDIndex IDIndex2, Action onOpenEditorClick1, Action onOpenEditorClick2, Action<string> onGreenLitClick1 = null, Action<string> onGreenLitClick2 = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection(IDLabel1, IDIndex1, idPair.ID1, (ID) => {
            idPair.ID1 = ID;
            hasChanged = true;
        }, onOpenEditorClick1, onGreenLitClick1);

        EditorGUILayoutPlus.IDSelection(IDLabel2, IDIndex2, idPair.ID2, (ID) => {
            idPair.ID2 = ID;
            hasChanged = true;
        }, onOpenEditorClick2, onGreenLitClick2);
        return hasChanged;
    }

    //--

    static public bool EditIDPair(IDPair idPair, IDCollection IDCollection1, IDIndex IDIndex2, Action onOpenEditorClick1, Action onOpenEditorClick2, Action<string> onGreenLitClick1 = null, Action<string> onGreenLitClick2 = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection("ID1", IDCollection1, idPair.ID1, (ID) => {
            idPair.ID1 = ID;
            hasChanged = true;
        }, onOpenEditorClick1, onGreenLitClick1);

        EditorGUILayoutPlus.IDSelection("ID2", IDIndex2, idPair.ID2, (ID) => {
            idPair.ID2 = ID;
            hasChanged = true;
        }, onOpenEditorClick2, onGreenLitClick2);
        return hasChanged;
    }

    static public bool EditIDPair(string IDLabel1, string IDLabel2, IDPair idPair, IDCollection IDCollection1, IDIndex IDIndex2, Action onOpenEditorClick1, Action onOpenEditorClick2, Action<string> onGreenLitClick1 = null, Action<string> onGreenLitClick2 = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection(IDLabel1, IDCollection1, idPair.ID1, (ID) => {
            idPair.ID1 = ID;
            hasChanged = true;
        }, onOpenEditorClick1, onGreenLitClick1);

        EditorGUILayoutPlus.IDSelection(IDLabel2, IDIndex2, idPair.ID2, (ID) => {
            idPair.ID2 = ID;
            hasChanged = true;
        }, onOpenEditorClick2, onGreenLitClick2);
        return hasChanged;
    }

    //--

    static public bool EditIDPair(IDPair idPair, IDIndex IDIndex1, IDCollection IDCollection2, Action onOpenEditorClick1, Action onOpenEditorClick2, Action<string> onGreenLitClick1 = null, Action<string> onGreenLitClick2 = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection("ID1", IDIndex1, idPair.ID1, (ID) => {
            idPair.ID1 = ID;
            hasChanged = true;
        }, onOpenEditorClick1, onGreenLitClick1);

        EditorGUILayoutPlus.IDSelection("ID2", IDCollection2, idPair.ID2, (ID) => {
            idPair.ID2 = ID;
            hasChanged = true;
        }, onOpenEditorClick2, onGreenLitClick2);
        return hasChanged;
    }

    static public bool EditIDPair(string IDLabel1, string IDLabel2, IDPair idPair, IDIndex IDIndex1, IDCollection IDCollection2, Action onOpenEditorClick1, Action onOpenEditorClick2, Action<string> onGreenLitClick1 = null, Action<string> onGreenLitClick2 = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection(IDLabel1, IDIndex1, idPair.ID1, (ID) => {
            idPair.ID1 = ID;
            hasChanged = true;
        }, onOpenEditorClick1, onGreenLitClick1);

        EditorGUILayoutPlus.IDSelection(IDLabel2, IDCollection2, idPair.ID2, (ID) => {
            idPair.ID2 = ID;
            hasChanged = true;
        }, onOpenEditorClick2, onGreenLitClick2);
        return hasChanged;
    }

    //== Edit List ==

    static public bool EditIDPairs(string title, List<IDPair> idPairs, IDCollection IDCollectionInput1, IDCollection IDCollectionInput2, Action onOpenEditorClick1, Action onOpenEditorClick2, Action<string> onGreenLitClick1 = null, Action<string> onGreenLitClick2 = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(66.0f))) {
                    hasChanged = true;
                    idPairs.Add(new IDPair());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idPairs.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                string ID1 = words[0];
                                string ID2 = words[1];
                                if (!string.IsNullOrEmpty(ID1) && !string.IsNullOrEmpty(ID2)) {
                                    idPairs.Add(new IDPair(ID1, ID2));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idPairs.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idPairs.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idBoolChanged = EditIDPair(idPairs[i], IDCollectionInput1, IDCollectionInput2, onOpenEditorClick1, onOpenEditorClick2, onGreenLitClick1, onGreenLitClick2);
                    if (idBoolChanged) {
                        hasChanged = true;
                    }

                    Color tempColor2 = GUI.color;
                    GUI.color = Color.red;
                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                        hasChanged = true;
                        deleteIndex = i;
                    }
                    GUI.color = tempColor2;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (deleteIndex != -1) {
                idPairs.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditIDPairs(string title, string IDLabel1, string IDLabel2, List<IDPair> idPairs, IDCollection IDCollectionInput1, IDCollection IDCollectionInput2, Action onOpenEditorClick1, Action onOpenEditorClick2, Action<string> onGreenLitClick1 = null, Action<string> onGreenLitClick2 = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(66.0f))) {
                    hasChanged = true;
                    idPairs.Add(new IDPair());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idPairs.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                string ID1 = words[0];
                                string ID2 = words[1];
                                if (!string.IsNullOrEmpty(ID1) && !string.IsNullOrEmpty(ID2)) {
                                    idPairs.Add(new IDPair(ID1, ID2));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idPairs.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idPairs.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idBoolChanged = EditIDPair(IDLabel1, IDLabel2, idPairs[i], IDCollectionInput1, IDCollectionInput2, onOpenEditorClick1, onOpenEditorClick2, onGreenLitClick1, onGreenLitClick2);
                    if (idBoolChanged) {
                        hasChanged = true;
                    }

                    Color tempColor2 = GUI.color;
                    GUI.color = Color.red;
                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                        hasChanged = true;
                        deleteIndex = i;
                    }
                    GUI.color = tempColor2;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (deleteIndex != -1) {
                idPairs.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    //--

    static public bool EditIDPairs(string title, List<IDPair> idPairs, IDIndex IDIndexInput1, IDIndex IDIndexInput2, Action onOpenEditorClick1, Action onOpenEditorClick2, Action<string> onGreenLitClick1 = null, Action<string> onGreenLitClick2 = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(66.0f))) {
                    hasChanged = true;
                    idPairs.Add(new IDPair());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idPairs.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                string ID1 = words[0];
                                string ID2 = words[1];
                                if (!string.IsNullOrEmpty(ID1) && !string.IsNullOrEmpty(ID2)) {
                                    idPairs.Add(new IDPair(ID1, ID2));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idPairs.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idPairs.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idBoolChanged = EditIDPair(idPairs[i], IDIndexInput1, IDIndexInput2, onOpenEditorClick1, onOpenEditorClick2, onGreenLitClick1, onGreenLitClick2);
                    if (idBoolChanged) {
                        hasChanged = true;
                    }

                    Color tempColor2 = GUI.color;
                    GUI.color = Color.red;
                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                        hasChanged = true;
                        deleteIndex = i;
                    }
                    GUI.color = tempColor2;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (deleteIndex != -1) {
                idPairs.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditIDPairs(string title, string IDLabel1, string IDLabel2, List<IDPair> idPairs, IDIndex IDIndexInput1, IDIndex IDIndexInput2, Action onOpenEditorClick1, Action onOpenEditorClick2, Action<string> onGreenLitClick1 = null, Action<string> onGreenLitClick2 = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(66.0f))) {
                    hasChanged = true;
                    idPairs.Add(new IDPair());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idPairs.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                string ID1 = words[0];
                                string ID2 = words[1];
                                if (!string.IsNullOrEmpty(ID1) && !string.IsNullOrEmpty(ID2)) {
                                    idPairs.Add(new IDPair(ID1, ID2));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idPairs.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idPairs.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idBoolChanged = EditIDPair(IDLabel1, IDLabel2, idPairs[i], IDIndexInput1, IDIndexInput2, onOpenEditorClick1, onOpenEditorClick2, onGreenLitClick1, onGreenLitClick2);
                    if (idBoolChanged) {
                        hasChanged = true;
                    }

                    Color tempColor2 = GUI.color;
                    GUI.color = Color.red;
                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                        hasChanged = true;
                        deleteIndex = i;
                    }
                    GUI.color = tempColor2;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (deleteIndex != -1) {
                idPairs.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    //--

    static public bool EditIDPairs(string title, List<IDPair> idPairs, IDCollection IDCollectionInput1, IDIndex IDIndexInput2, Action onOpenEditorClick1, Action onOpenEditorClick2, Action<string> onGreenLitClick1 = null, Action<string> onGreenLitClick2 = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(66.0f))) {
                    hasChanged = true;
                    idPairs.Add(new IDPair());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idPairs.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                string ID1 = words[0];
                                string ID2 = words[1];
                                if (!string.IsNullOrEmpty(ID1) && !string.IsNullOrEmpty(ID2)) {
                                    idPairs.Add(new IDPair(ID1, ID2));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idPairs.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idPairs.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idBoolChanged = EditIDPair(idPairs[i], IDCollectionInput1, IDIndexInput2, onOpenEditorClick1, onOpenEditorClick2, onGreenLitClick1, onGreenLitClick2);
                    if (idBoolChanged) {
                        hasChanged = true;
                    }

                    Color tempColor2 = GUI.color;
                    GUI.color = Color.red;
                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                        hasChanged = true;
                        deleteIndex = i;
                    }
                    GUI.color = tempColor2;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (deleteIndex != -1) {
                idPairs.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditIDPairs(string title, string IDLabel1, string IDLabel2, List<IDPair> idPairs, IDCollection IDCollectionInput1, IDIndex IDIndexInput2, Action onOpenEditorClick1, Action onOpenEditorClick2, Action<string> onGreenLitClick1 = null, Action<string> onGreenLitClick2 = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(66.0f))) {
                    hasChanged = true;
                    idPairs.Add(new IDPair());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idPairs.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                string ID1 = words[0];
                                string ID2 = words[1];
                                if (!string.IsNullOrEmpty(ID1) && !string.IsNullOrEmpty(ID2)) {
                                    idPairs.Add(new IDPair(ID1, ID2));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idPairs.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idPairs.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idBoolChanged = EditIDPair(IDLabel1, IDLabel2, idPairs[i], IDCollectionInput1, IDIndexInput2, onOpenEditorClick1, onOpenEditorClick2, onGreenLitClick1, onGreenLitClick2);
                    if (idBoolChanged) {
                        hasChanged = true;
                    }

                    Color tempColor2 = GUI.color;
                    GUI.color = Color.red;
                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                        hasChanged = true;
                        deleteIndex = i;
                    }
                    GUI.color = tempColor2;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (deleteIndex != -1) {
                idPairs.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    //--

    static public bool EditIDPairs(string title, List<IDPair> idPairs, IDIndex IDIndexInput1, IDCollection IDCollectionInput2, Action onOpenEditorClick1, Action onOpenEditorClick2, Action<string> onGreenLitClick1 = null, Action<string> onGreenLitClick2 = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(66.0f))) {
                    hasChanged = true;
                    idPairs.Add(new IDPair());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idPairs.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                string ID1 = words[0];
                                string ID2 = words[1];
                                if (!string.IsNullOrEmpty(ID1) && !string.IsNullOrEmpty(ID2)) {
                                    idPairs.Add(new IDPair(ID1, ID2));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idPairs.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idPairs.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idBoolChanged = EditIDPair(idPairs[i], IDIndexInput1, IDCollectionInput2, onOpenEditorClick1, onOpenEditorClick2, onGreenLitClick1, onGreenLitClick2);
                    if (idBoolChanged) {
                        hasChanged = true;
                    }

                    Color tempColor2 = GUI.color;
                    GUI.color = Color.red;
                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                        hasChanged = true;
                        deleteIndex = i;
                    }
                    GUI.color = tempColor2;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (deleteIndex != -1) {
                idPairs.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditIDPairs(string title, string IDLabel1, string IDLabel2, List<IDPair> idPairs, IDIndex IDIndexInput1, IDCollection IDCollectionInput2, Action onOpenEditorClick1, Action onOpenEditorClick2, Action<string> onGreenLitClick1 = null, Action<string> onGreenLitClick2 = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(66.0f))) {
                    hasChanged = true;
                    idPairs.Add(new IDPair());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idPairs.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                string ID1 = words[0];
                                string ID2 = words[1];
                                if (!string.IsNullOrEmpty(ID1) && !string.IsNullOrEmpty(ID2)) {
                                    idPairs.Add(new IDPair(ID1, ID2));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idPairs.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idPairs.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idBoolChanged = EditIDPair(IDLabel1, IDLabel2, idPairs[i], IDIndexInput1, IDCollectionInput2, onOpenEditorClick1, onOpenEditorClick2, onGreenLitClick1, onGreenLitClick2);
                    if (idBoolChanged) {
                        hasChanged = true;
                    }

                    Color tempColor2 = GUI.color;
                    GUI.color = Color.red;
                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                        hasChanged = true;
                        deleteIndex = i;
                    }
                    GUI.color = tempColor2;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (deleteIndex != -1) {
                idPairs.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    //--

}

#endif