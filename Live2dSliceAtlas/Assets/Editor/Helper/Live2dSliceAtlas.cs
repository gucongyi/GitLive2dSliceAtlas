using Live2D.Cubism.Core;
using Live2D.Cubism.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Live2dSliceAtlas : Editor
{
    [MenuItem("Assets/将模型的图集切成贴图")]
    public static void SliceAtlas()
    {
        try
        {
            GameObject goPrefab = Selection.activeGameObject;
            string modelName = goPrefab.name;
            string savePath = EditorUtility.OpenFolderPanel("选择要保持图片的路径", "", "");
            EditorUtility.DisplayProgressBar("开始切图", $"savePath:{savePath}", 0.5f);

            goPrefab.GetComponent<CubismRenderController>().SliceAtlas(savePath);
            Debug.Log($"<color=green>savePath:{savePath} 图片保存成功</color>");
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            EditorUtility.ClearProgressBar();
        }
    }
}
