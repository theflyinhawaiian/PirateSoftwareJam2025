using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    // YO, IF YOU'RE HAVING TROUBLE SERIALIZING, YOU MIGHT BE TRYING TO SERIALIZE PROPS INSTEAD OF FIELDS
    public static class FileHandler
    {
        public static void SaveToJSON<T>(List<T> toSave, string relPath)
        {
            var content = JsonHelper.ToJson(toSave.ToArray());
            WriteFile(GetPath(relPath), content);
        }

        public static void SaveToJSON<T>(T toSave, string relPath)
        {
            var content = JsonUtility.ToJson(toSave, true);
            Debug.Log(content);
            WriteFile(GetPath(relPath), content);
        }

        public static List<T> ReadListFromJSON<T>(string relPath)
        {
            var content = ReadFile(GetPath(relPath));

            if (string.IsNullOrEmpty(content) || content == "{}")
                return new List<T>();

            var res = JsonHelper.FromJson<T>(content).ToList();

            return res;

        }

        public static T ReadFromJSON<T>(string relPath)
        {
            var content = ReadFile(GetPath(relPath));

            if (string.IsNullOrEmpty(content) || content == "{}")
                return default;

            return JsonUtility.FromJson<T>(content);

        }

        public static void CreateDir(string relativePath) => Directory.CreateDirectory(GetPath(relativePath));

        public static string[] GetFiles(string directoryName) => Directory.GetFiles(GetPath(directoryName));

        private static string GetPath(string relativePath) => Path.Combine(Application.dataPath, relativePath);

        private static void WriteFile(string path, string content)
        {
            var fileStream = new FileStream(path, FileMode.Create);

            using (var writer = new StreamWriter(fileStream)) {
                writer.Write(content);
            }
        }

        private static string ReadFile(string path)
        {
            if (!File.Exists(path))
                return string.Empty;

            using (var reader = new StreamReader(path)) {
                string content = reader.ReadToEnd();
                return content;
            }
        }
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            var wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            var wrapper = new Wrapper<T>
            {
                Items = array
            };
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            var wrapper = new Wrapper<T>
            {
                Items = array
            };
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}