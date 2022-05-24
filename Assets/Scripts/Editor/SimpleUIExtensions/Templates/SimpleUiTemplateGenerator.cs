/*
 * Last Update: 04.28.2021
 * Generating a template for SimpleUi
*/
#if UNITY_EDITOR

using System;
using System.IO;
using JokerGhost;
using UnityEditor;
using UnityEngine;

namespace SimpleUIExtensions.Templates 
{
    /// <summary>
    /// Generates templates of simpleUI user classes.
    /// </summary>
    internal sealed class SimpleUiTemplateGenerator 
    {
        private const string WindowTemplate = "Window.cs.txt";
        private const string OnlyWindowTemplate = "OnlyWindow.cs.txt";
        private const string ViewControllerTemplate = "ViewController.cs.txt";
        private const string ViewTemplate = "View.cs.txt";
        
        private static string PathTemplates => Application.dataPath + "/Scripts/Editor/SimpleUIExtensions/Templates/";

        [MenuItem ("Assets/Create/SimpleUiExtensions/Create new Window", false, -200)]
        private static void CreateWindow () 
        {
            var assetPath = GetAssetPath ();
            var nameClass = assetPath.Substring(assetPath.LastIndexOf("/", StringComparison.Ordinal) + 1);

            CreateTemplateInternal($"{assetPath}/{nameClass}Window.cs", 
                                     nameClass, PathTemplates + WindowTemplate);
            CreateTemplateInternal($"{assetPath}/{nameClass}ViewController.cs", 
                                     nameClass, PathTemplates + ViewControllerTemplate);
            CreateTemplateInternal($"{assetPath}/{nameClass}View.cs", 
                                     nameClass, PathTemplates + ViewTemplate);
        }
        
        [MenuItem ("Assets/Create/SimpleUiExtensions/Create only Window template", false, -200)]
        private static void CreateWindowTemplate () 
        {
            var assetPath = GetAssetPath ();
            var nameClass = assetPath.Substring(assetPath.LastIndexOf("/", StringComparison.Ordinal) + 1);
            
            CreateTemplateInternal($"{assetPath}/{nameClass}Window.cs", 
                                     nameClass, PathTemplates + OnlyWindowTemplate);
        }
        
        [MenuItem ("Assets/Create/SimpleUiExtensions/Create new Controller", false, -200)]
        private static void CreateController () 
        {
            var assetPath = GetAssetPath ();
            var nameClass = assetPath.Substring(assetPath.LastIndexOf("/", StringComparison.Ordinal) + 1);
            
            CreateTemplateInternal($"{assetPath}/{nameClass}ViewController.cs", 
                                     nameClass, PathTemplates + ViewControllerTemplate);
            CreateTemplateInternal($"{assetPath}/{nameClass}View.cs", 
                                     nameClass, PathTemplates + ViewTemplate);
        }
        
        private static void CreateTemplateInternal (string pathName, string nameClass, string pathTemplate) 
        {
            var res = TemplateGenerator.Create(pathName, nameClass, pathTemplate);
            if (res != null) 
            {
                EditorUtility.DisplayDialog ("Template generator", res, "Close");
            }
            else
            {
                AssetDatabase.Refresh();
            }
        }
        
        private static string GetAssetPath()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!string.IsNullOrEmpty(path) && AssetDatabase.Contains(Selection.activeObject))
            {
                if (!AssetDatabase.IsValidFolder(path))
                {
                    path = Path.GetDirectoryName(path);
                }
            }
            else
            {
                path = "Assets";
            }

            return path;
        }
    }
}

#endif