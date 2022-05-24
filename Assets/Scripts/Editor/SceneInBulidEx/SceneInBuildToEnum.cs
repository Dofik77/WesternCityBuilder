/*
 * Last Update: 04.28.2021
 * Generating an enumeration by scenes in the build
*/
#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace JokerGhost
{
    public static class SceneInBuildToEnum
    {
        private static string EnumPath => Path.Combine(Application.dataPath, "Resources/EScene.cs");
        
        [MenuItem("Assets/Create Enum from scenes in build", false, 1000000)]
        private static void RefreshEnumScene()
        {
            var allNamesForEnum = new List<string>();

            EditorBuildSettings.scenes.ForEach(scene =>
            {
                var index = scene.path.LastIndexOf("/", StringComparison.Ordinal);
                allNamesForEnum.Add(scene.path.Substring(index + 1, scene.path.Length - index - 7));
            });

            GenerateEnum.Create(EnumPath, allNamesForEnum, "EScene");
            
            AssetDatabase.Refresh();
        }
    }
}

#endif