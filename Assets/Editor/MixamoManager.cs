 
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
 
public class MixamoManager : EditorWindow
{
    private static MixamoManager editor;
    private static int width = 350;
    private static int height = 300;
    private static int x = 0;
    private static int y = 0;
    private static List<string> allFiles = new List<string>();
   
    [MenuItem("Window/Mixamo Manager")]
    static void ShowEditor()
    {
        editor = EditorWindow.GetWindow<MixamoManager>();
        CenterWindow();
    }
 
    private void OnGUI()
    {
        if (GUILayout.Button("Rename"))
        {
            Rename();
        }
    }
   
    public void Rename()
    {
        DirSearch();
 
        if (allFiles.Count > 0)
        {
            for (int i = 0; i < allFiles.Count; i++)
            {
                int idx = allFiles[i].IndexOf("Assets");
                string filename = Path.GetFileName(allFiles[i]);
                string asset = allFiles[i].Substring(idx);
                AnimationClip orgClip = (AnimationClip)AssetDatabase.LoadAssetAtPath(
                    asset, typeof(AnimationClip));
 
                var fileName = Path.GetFileNameWithoutExtension(allFiles[i]);
                var importer = (ModelImporter)AssetImporter.GetAtPath(asset);
 
                RenameAndImport(importer, fileName);
            }
        }
    }
 
    private void RenameAndImport(ModelImporter asset, string name)
    {
        ModelImporter modelImporter = asset as ModelImporter;
        ModelImporterClipAnimation[] clipAnimations = modelImporter.defaultClipAnimations;
 
        for (int i = 0; i < clipAnimations.Length; i++)
        {
            clipAnimations[i].name = name;
        }
       
        modelImporter.clipAnimations = clipAnimations;
        modelImporter.SaveAndReimport();
    }
 
    private static void CenterWindow()
    {
        editor = EditorWindow.GetWindow<MixamoManager>();
        x = (Screen.currentResolution.width - width) / 2;
        y = (Screen.currentResolution.height - height) / 2;
        editor.position = new Rect(x, y, width, height);
        editor.maxSize = new Vector2(width, height);
        editor.minSize = editor.maxSize;
    }
 
    static void DirSearch()
    {
        string info = Application.dataPath+ "/MixamoAnimations/";
        string[] fileInfo = Directory.GetFiles(info, "*.fbx", SearchOption.AllDirectories);
        foreach (string file in fileInfo)
        {
            if (file.EndsWith(".fbx"))
                allFiles.Add(file);
        }
    }
}