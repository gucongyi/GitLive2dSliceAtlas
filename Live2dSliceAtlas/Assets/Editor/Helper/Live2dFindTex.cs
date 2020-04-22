using Live2D.Cubism.Core;
using Live2D.Cubism.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Live2dFindTex : EditorWindow
{
    string textGoName = "nvzhu";
    string pathTexture = "Assets/TexturesSlice/";
    [MenuItem("Tools/图集变贴图")]
    public static void ShowWindow()
    {
        GetWindow(typeof(Live2dFindTex));
    }
    
    private void OnGUI()
    {
        GUILayout.BeginVertical();
        
        textGoName = EditorGUILayout.TextField("GameObject Name", textGoName);
        
        pathTexture = EditorGUILayout.TextField("Path Tex", pathTexture);
        GUILayout.EndVertical();

        if (GUILayout.Button("开始替换"))
        {
            var go=GameObject.Find(textGoName);
            var drawables=go.GetComponent<CubismModel>().Drawables;
            List<string> pngList = GetAllResourcePathByPostFix(pathTexture, ".png", true);
            if (pngList == null || pngList.Count <= 0)
            {
                pngList = GetAllResourcePathByPostFix(pathTexture, ".PNG", true);
            }
            foreach (var eachDrawable in drawables)
            {
                var renderer=eachDrawable.GetComponent<CubismRenderer>();
                var texNamePath = pngList.Find(x => x.Contains(renderer.name));
                if (string.IsNullOrEmpty(texNamePath))
                {
                    Debug.LogError($"renderer.name:{renderer.name}的贴图不存在，请检查！");
                    continue;
                }
                Texture2D texture2d = AssetDatabase.LoadAssetAtPath<Texture2D>(texNamePath);
                renderer.MainTexture = texture2d;
                renderer.isReGenerateMesh = true;
            }
            SavePrefab(go);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            Debug.LogError($"贴图替换完成！");
        }

        
    }

    public static void SavePrefab(GameObject goPrefab)
    {
        string prefabPath = "Assets/Prefabs";
        prefabPath = Path.Combine(prefabPath,$"{goPrefab.name}.prefab");
        PrefabUtility.SaveAsPrefabAsset(goPrefab, prefabPath);
    }

    public static List<string> GetAllResourcePathByPostFix(string srcPath,string postFix ,bool subDire)
    {
        List<string> paths = new List<string>();
        string[] files = Directory.GetFiles(srcPath);
        foreach (string str in files)
        {
            if (str.EndsWith(postFix))
            {
                paths.Add(str);
            }
        }
        if (subDire)
        {
            foreach (string subPath in Directory.GetDirectories(srcPath))
            {
                List<string> subFiles = GetAllResourcePathByPostFix(subPath, postFix,true);
                paths.AddRange(subFiles);
            }
        }
        return paths;
    }
}
