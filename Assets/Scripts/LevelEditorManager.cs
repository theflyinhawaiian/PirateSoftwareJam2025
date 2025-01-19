using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private readonly string obstaclesPath = "Assets/Prefabs/Obstacles";
        private readonly string roomsPath = "Assets/Resources/Rooms";
        private List<GameObject> obstacleGameObjects = new();

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
            obstacleGameObjects.Clear(); 

            if (!Directory.Exists(obstaclesPath))
            {
                Directory.CreateDirectory(obstaclesPath);
                Debug.Log($"Created directory: {obstaclesPath}");
            }

            string[] assetPaths = AssetDatabase.FindAssets("t:Prefab", new[] { obstaclesPath });

            foreach (var guid in assetPaths)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject obstacle = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (obstacle != null)
                {
                    obstacleGameObjects.Add(obstacle);
                }
            }

            obstacleSelector.AddOptions(obstacleGameObjects.Select(x => x.name).ToList());
        }

        void PopulateRooms()
        {
            roomsDropdown.ClearOptions();

            if (!Directory.Exists(roomsPath))
            {
                Directory.CreateDirectory(roomsPath);
                Debug.Log($"Created directory: {roomsPath}");
            }

            TextAsset[] rooms = Resources.LoadAll<TextAsset>("Rooms");

            var roomNames = new List<string>();
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
            GameObject selectedObstacle = obstacleGameObjects[obstacleSelector.value];

            if (selectedObstacle != null)
            {
                Vector3 position = new(50, 50, 0);
                Instantiate(selectedObstacle, position, Quaternion.identity);
                Debug.Log($"Instantiated: {selectedObstacle.name}");
            }
            else
            {
                Debug.LogError("No obstacle selected!");
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