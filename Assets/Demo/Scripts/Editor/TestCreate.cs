using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class TestCreate : MonoBehaviour
{


    [MenuItem("Prefabs/Test_EditPrefabContentsScope")]
    public static void Test()
    {

        GameObject assetRoot = Selection.activeObject as GameObject;
        string assetPath = AssetDatabase.GetAssetPath(assetRoot);


        using (var editingScope = new PrefabUtility.EditPrefabContentsScope(assetPath))
        {
            var prefabRoot = editingScope.prefabContentsRoot;

            var guids = AssetDatabase.FindAssets("t:texture2D", new[] { "Assets/Demo/Resources/Textures/img" });
            foreach (var guid in guids)
            {
                var newGO = new GameObject();
                newGO.transform.SetParent(prefabRoot.transform);
                newGO.AddComponent<RawImage>().texture = AssetDatabase.LoadAssetAtPath<Texture>(AssetDatabase.GUIDToAssetPath(guid));
                Object.Destroy(newGO);
            }

        }

        AssetDatabase.OpenAsset(assetRoot);
        SceneView.FrameLastActiveSceneView();
    }

}
