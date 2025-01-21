using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts {
    public class AssetFinder {
        private static void EnsurePathExists(string path){
            if (Directory.Exists(path))
                return;

            Directory.CreateDirectory(path);
            Debug.Log($"Created directory: {path}");
        }

        public static List<GameObject> GetPrefabs(string path){
            return Get<GameObject>("t:prefab", path);
        }

        public static List<TextAsset> GetText(string path){
            return Get<TextAsset>("t:TextAsset", path);
        }

        public static List<T> Get<T>(string searchFilter, string path) where T : Object {
            EnsurePathExists(path);
            string[] assetPaths = AssetDatabase.FindAssets(searchFilter, new[] { path });

            return assetPaths.Select(x => {
                var assetPath = AssetDatabase.GUIDToAssetPath(x);
                return AssetDatabase.LoadAssetAtPath<T>(assetPath);
            }).Where(x => x != null).ToList();
        }
    }
}