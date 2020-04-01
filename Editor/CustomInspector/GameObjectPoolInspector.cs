using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameObjectPool))]
public class GameObjectPoolInspector : Editor {
    public override void OnInspectorGUI() {
        //Display the open editor button.
        GameObjectPool gameObjectPool = (GameObjectPool)target;
        if (gameObjectPool != null) {
            GUI.color = Color.cyan;
            if (GUILayout.Button("Open Editor", EditorStyles.miniButton)) {
                GameObjectPoolEditor.OpenWindow();
            }
            GUI.color = Color.white;
            //Display GameObjectPool info.
            EditorGUILayout.HelpBox(gameObjectPool.PoolInfoStr(), MessageType.Info);
        } else {
            EditorGUILayout.HelpBox("OOps! Target is null?!", MessageType.Warning);
        }
    }
}
