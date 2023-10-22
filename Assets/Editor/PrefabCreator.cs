using UnityEngine;
using UnityEditor;

public class PrefabCreator : MonoBehaviour
{
    [MenuItem("GameObject/Create Prefab From Selected", false, 10)]
    static void CreatePrefab()
    {
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject == null)
        {
            Debug.LogError("No GameObject selected.");
            return;
        }

        string path = EditorUtility.SaveFilePanel("Save Prefab", "Assets/", selectedObject.name, "prefab");
        if (string.IsNullOrEmpty(path))
            return;

        path = FileUtil.GetProjectRelativePath(path);

        Object prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(selectedObject, path, InteractionMode.UserAction);
        if (prefab == null)
        {
            Debug.LogError("Prefab creation failed.");
        }
    }
}
