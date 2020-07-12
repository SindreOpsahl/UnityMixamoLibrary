 
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
            //set clip name to same as file name
            clipAnimations[i].name = name;
            clipAnimations[i].takeName = modelImporter.clipAnimations[i].takeName;

            //clip length
            clipAnimations[i].firstFrame = modelImporter.clipAnimations[i].firstFrame;
            clipAnimations[i].lastFrame= modelImporter.clipAnimations[i].lastFrame;
            
            //loop settings
            clipAnimations[i].loopTime = modelImporter.clipAnimations[i].loopTime;
            clipAnimations[i].loopPose = modelImporter.clipAnimations[i].loopPose;
            clipAnimations[i].cycleOffset = modelImporter.clipAnimations[i].cycleOffset;

            //root motion settings
            clipAnimations[i].lockRootRotation = modelImporter.clipAnimations[i].lockRootRotation;
            clipAnimations[i].keepOriginalOrientation = modelImporter.clipAnimations[i].keepOriginalOrientation;
            clipAnimations[i].rotationOffset = modelImporter.clipAnimations[i].rotationOffset;

            clipAnimations[i].lockRootHeightY = modelImporter.clipAnimations[i].lockRootHeightY;
            clipAnimations[i].keepOriginalPositionY = modelImporter.clipAnimations[i].keepOriginalPositionY;
            clipAnimations[i].heightFromFeet = modelImporter.clipAnimations[i].heightFromFeet;
            clipAnimations[i].heightOffset = modelImporter.clipAnimations[i].heightOffset;

            clipAnimations[i].lockRootPositionXZ = modelImporter.clipAnimations[i].lockRootPositionXZ;
            clipAnimations[i].keepOriginalPositionXZ = modelImporter.clipAnimations[i].keepOriginalPositionXZ;

            //mirror setting
            clipAnimations[i].mirror = modelImporter.clipAnimations[i].mirror;

            //additive reference pose settings
            clipAnimations[i].hasAdditiveReferencePose = modelImporter.clipAnimations[i].hasAdditiveReferencePose;
            clipAnimations[i].additiveReferencePoseFrame = modelImporter.clipAnimations[i].additiveReferencePoseFrame;

            //other settings
            clipAnimations[i].curves = modelImporter.clipAnimations[i].curves;
            clipAnimations[i].events = modelImporter.clipAnimations[i].events;
            clipAnimations[i].maskSource = modelImporter.clipAnimations[i].maskSource;
            clipAnimations[i].maskType= modelImporter.clipAnimations[i].maskType;
            clipAnimations[i].wrapMode = modelImporter.clipAnimations[i].wrapMode;
            
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
        string info = UnityEditor.EditorUtility.SaveFolderPanel("Select folder", Application.dataPath, "") + "/";
        string[] fileInfo = Directory.GetFiles(info, "*.fbx", SearchOption.AllDirectories);
        foreach (string file in fileInfo)
        {
            if (file.EndsWith(".fbx"))
                allFiles.Add(file);
        }
    }
}