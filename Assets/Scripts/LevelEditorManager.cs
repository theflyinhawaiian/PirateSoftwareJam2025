using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

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
        float screenWidth;
        RectTransform panelTransform;

        void Start()
        { 
            panelTransform = panel.GetComponent<RectTransform>();

            PopulateObstacles();

            PopulateRooms();
        }

        void Update()
        {
            if(screenWidth != Screen.width)
                AdjustUISize();

            if (Input.GetMouseButtonDown(0))
                HandleMouseClick();                            

            if (selectedObject != null)
                UpdateCoordinateText();
        }

        private void AdjustUISize()
        {
            
            screenWidth = Screen.width;
            var offsetMin = panelTransform.offsetMin;
            offsetMin.x = Screen.width * .8f;
            panelTransform.offsetMin = offsetMin;
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

        public void DeleteObject()
        {
            if (selectedObject != null)
            {
                Debug.Log($"Deleting Object: {selectedObject.name}");

                Destroy(selectedObject);

                selectedObject = null;

                xInput.text = "";
                yInput.text = "";
                zInput.text = "";
            }
            else
            {
                Debug.LogWarning("No object selected to delete.");
            }
        }

        public void ToggleViewMode() 
        {

        }

        #region Object-Movement

        private void HandleMouseClick()
        {
            if (EventSystem.current.IsPointerOverGameObject()) { return; }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (clickedObject != selectedObject)
                {
                    SelectObject(clickedObject);
                }
            }
            else
            {
                DeselectObject();
            }
        }

        void SelectObject(GameObject obj)
        {
            selectedObject = obj;

            Debug.Log($"Selected Object: {selectedObject.name}");

            UpdateCoordinateText();
        }

        private void DeselectObject()
        {
            if (selectedObject != null)
            {
                Debug.Log($"Deselected Object: {selectedObject.name}");
            }

            selectedObject = null;

            xInput.text = "";
            yInput.text = "";
            zInput.text = "";
        }

        public void UpdateCoordinateText()
        {
            if (selectedObject == null) return;

            Vector3 currentPos = selectedObject.transform.position;

            xInput.text = currentPos.x.ToString("F2");
            yInput.text = currentPos.y.ToString("F2");
            zInput.text = currentPos.z.ToString("F2");
        }

        #endregion

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