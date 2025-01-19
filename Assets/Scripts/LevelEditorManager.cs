using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts {
    enum ViewMode {
        HeadOn,
        Isometric
    }

    public class LevelEditorManager : MonoBehaviour {
        public GameObject panel;
        public TMP_Text viewModeText;
        public TMP_Dropdown obstacleSelector;
        public TMP_InputField xInput;
        public TMP_InputField yInput;
        public TMP_InputField zInput;
        public TMP_Dropdown roomsDropdown;
        public TMP_InputField saveRoomInput;

        private string obstaclesPath = "Assets/Prefabs/Obstacles";
        private string roomsPath = "Assets/Resources/Rooms";

        void Start(){
            // 1. Check Screen.width and set panel width accordingly
            var transform = panel.GetComponent<RectTransform>();

            // 2. load & populate obstacle names from the `Assets/Prefabs/Obstacles`
            //   dir
            PopulateObstacles();

            // 3. load & populate room names from the `Assets/Resources/Rooms` dir
            PopulateRooms();
        }

        void PopulateObstacles()
        {
            obstacleSelector.ClearOptions();

            if (!Directory.Exists(obstaclesPath))
            {
                Directory.CreateDirectory(obstaclesPath);
                Debug.Log($"Created directory: {obstaclesPath}");
            }

#if UNITY_EDITOR
            // Editor-only: Use AssetDatabase to load obstacles
            string[] assetPaths = AssetDatabase.FindAssets("t:Prefab", new[] { obstaclesPath });
            var obstacleNames = new System.Collections.Generic.List<string>();

            foreach (var guid in assetPaths)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject obstacle = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (obstacle != null)
                {
                    obstacleNames.Add(obstacle.name);
                }
            }
            obstacleSelector.AddOptions(obstacleNames);
#else
            // Runtime: Use Resources.LoadAll for obstacles
            GameObject[] obstacles = Resources.LoadAll<GameObject>(runtimeObstaclesPath);
            var obstacleNames = new System.Collections.Generic.List<string>();

            foreach (var obstacle in obstacles) {
                obstacleNames.Add(obstacle.name);
            }

            obstacleSelector.AddOptions(obstacleNames);
#endif
        }

        void PopulateRooms()
        {
            roomsDropdown.ClearOptions();

            if (!Directory.Exists(roomsPath))
            {
                Directory.CreateDirectory(roomsPath);
                Debug.Log($"Created directory: {roomsPath}");
            }

            // Load all rooms from "Assets/Resources/Rooms"
            TextAsset[] rooms = Resources.LoadAll<TextAsset>("Rooms");

            // Extract names and add them to the dropdown
            var roomNames = new System.Collections.Generic.List<string>();
            foreach (var room in rooms)
            {
                roomNames.Add(room.name);
            }

            roomsDropdown.AddOptions(roomNames);
        }

        public void DeleteObject()
        {

        }

        public void CreateObject()
        {
            string selectedObstacleName = obstacleSelector.options[obstacleSelector.value].text;

            GameObject prefab = null;

#if UNITY_EDITOR
            // Editor-only: Load the prefab using AssetDatabase
            string[] assetPaths = AssetDatabase.FindAssets(selectedObstacleName, new[] { obstaclesPath });
            if (assetPaths.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(assetPaths[0]);
                prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            }
#else
            // Runtime: Load the prefab from Resources
            prefab = Resources.Load<GameObject>($"{runtimeObstaclesPath}/{selectedObstacleName}");
#endif

            if (prefab != null)
            {
                // Parse position input fields
                float.TryParse(xInput.text, out float x);
                float.TryParse(yInput.text, out float y);
                float.TryParse(zInput.text, out float z);

                // Instantiate the prefab
                Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
                Debug.Log($"Created object: {selectedObstacleName} at position ({x}, {y}, {z})");
            }
            else
            {
                Debug.LogError($"Prefab not found: {selectedObstacleName}");
            }
        }

        public void ToggleViewMode() 
        {

        }

        public void CoordsChanged()
        {

        }

        public void ListItemSelected()
        {

        }

        public void LoadRoom()
        {

        }

        public void SaveRoom()
        {

        }
    }
}