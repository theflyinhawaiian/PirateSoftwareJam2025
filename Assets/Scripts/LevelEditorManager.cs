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
        private GameObject selectedObject;
        private Plane dragPlane;          // Plane for object movement
        private Vector3 offset;

        void Start(){
            // 1. Check Screen.width and set panel width accordingly
            var transform = panel.GetComponent<RectTransform>();

            // 2. load & populate obstacle names from the `Assets/Prefabs/Obstacles`
            //   dir
            PopulateObstacles();

            // 3. load & populate room names from the `Assets/Resources/Rooms` dir
            PopulateRooms();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectObject();
            }

            if (Input.GetMouseButton(0) && selectedObject != null)
            {
                DragObject();
            }

            if (Input.GetMouseButtonUp(0) && selectedObject != null)
            {
                ReleaseObject();
            }

            if (selectedObject != null)
            {
                CordsChanged();
            }            
        }

        private void SelectObject()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                selectedObject = hit.collider.gameObject;

                dragPlane = new Plane(Camera.main.transform.forward, selectedObject.transform.position);

                if (dragPlane.Raycast(ray, out float enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);
                    offset = selectedObject.transform.position - hitPoint;
                }

                Debug.Log($"Selected Object: {selectedObject.name}");
            }
        }

        private void DragObject()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector3 newPosition = hitPoint + offset;

                selectedObject.transform.position = newPosition;
            }
        }

        private void ReleaseObject()
        {
            Debug.Log($"Released Object: {selectedObject.name}");
            selectedObject = null;
        }

        private void PopulateObstacles()
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

        private void PopulateRooms()
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

        public void CordsChanged()
        {
            if (selectedObject == null) return;

            Vector3 currentPos = selectedObject.transform.position;

            xInput.text = currentPos.x.ToString("F2");
            yInput.text = currentPos.y.ToString("F2");
            zInput.text = currentPos.z.ToString("F2");
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