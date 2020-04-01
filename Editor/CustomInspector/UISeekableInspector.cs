using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UISeekable))]
[CanEditMultipleObjects]
public class UISeekableInspector : Editor {
    public override void OnInspectorGUI() {
        serializedObject.Update();

        SerializedProperty toggleOnProperty = serializedObject.FindProperty("canBeSeeked");

        SerializedProperty useSpecifiedSeekTargetProperty = serializedObject.FindProperty("useSpecifiedSeekTarget");

        SerializedProperty specifiedSeekNeighborOnLeftProperty = serializedObject.FindProperty("specifiedSeekNeighborOnLeft");
        SerializedProperty specifiedSeekNeighborOnRightProperty = serializedObject.FindProperty("specifiedSeekNeighborOnRight");
        SerializedProperty specifiedSeekNeighborOnDownProperty = serializedObject.FindProperty("specifiedSeekNeighborOnDown");
        SerializedProperty specifiedSeekNeighborOnUpProperty = serializedObject.FindProperty("specifiedSeekNeighborOnUp");

        toggleOnProperty.boolValue = GUILayout.Toggle(toggleOnProperty.boolValue, "Can Be Seeked", EditorStyles.miniButton);
        useSpecifiedSeekTargetProperty.boolValue = GUILayout.Toggle(useSpecifiedSeekTargetProperty.boolValue, "Specify Seek Target", EditorStyles.miniButton);

        if (useSpecifiedSeekTargetProperty.boolValue) {
            EditorGUILayout.BeginVertical("FrameBox");
            {
                specifiedSeekNeighborOnLeftProperty.objectReferenceValue = EditorGUILayout.ObjectField("Left", specifiedSeekNeighborOnLeftProperty.objectReferenceValue, typeof(UISeekable), true);
                specifiedSeekNeighborOnRightProperty.objectReferenceValue = EditorGUILayout.ObjectField("Right", specifiedSeekNeighborOnRightProperty.objectReferenceValue, typeof(UISeekable), true);
                specifiedSeekNeighborOnDownProperty.objectReferenceValue = EditorGUILayout.ObjectField("Down", specifiedSeekNeighborOnDownProperty.objectReferenceValue, typeof(UISeekable), true);
                specifiedSeekNeighborOnUpProperty.objectReferenceValue = EditorGUILayout.ObjectField("Up", specifiedSeekNeighborOnUpProperty.objectReferenceValue, typeof(UISeekable), true);
            }
            EditorGUILayout.EndVertical();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
