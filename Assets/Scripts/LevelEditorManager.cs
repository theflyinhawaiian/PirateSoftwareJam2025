using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Model;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts {
    enum ViewMode {
        HeadOn,
        Isometric
    }

    public class LevelEditorManager : MonoBehaviour {
        public TMP_Dropdown obstacleSelector;
        public TMP_Dropdown roomsDropdown;
        public TMP_InputField saveRoomInput;
        private readonly string obstaclesPath = "Assets/Prefabs/Obstacles";
        private readonly string roomsPath = "Assets/Resources/Rooms";
        private List<GameObject> obstacleGameObjects = new();
        private GameObject selectedObject;
        private List<GameObject> roomContents = new();
        private int nextId = 0;

        private RoomFileHandler roomSerializer;

        void Start()
        {
            roomSerializer = new RoomFileHandler();
            PopulateObstacles();

            PopulateRooms();
        }

        void Update()
        {
            if(Selection.activeGameObject != selectedObject){
                selectedObject = Selection.activeGameObject;
                Debug.Log(selectedObject);
            }
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
                Vector3 position = new(0, 0, 0);
                var obj = Instantiate(selectedObstacle, position, Quaternion.identity);
                InitializeEntity(obj);
                Debug.Log($"Instantiated: {selectedObstacle.name}");
                roomContents.Add(obj);
            }
            else
            {
                Debug.LogError("No obstacle selected!");
            }
        }

        private void InitializeEntity(GameObject entity){
            var behavior = entity.GetComponent<EntityBehavior>();
            // TODO: Give the behavior a place to read moveSpeed value from so we can control it more dynamically
            behavior.moveSpeed = 0;
            behavior.id = nextId;

            // increment nextId so the next entity initialized has a unique Id
            nextId++;
        }

        public void DeleteObject()
        {
            if (selectedObject == null)
            {
                Debug.LogWarning("No object selected to delete.");
                return;
            }

            Debug.Log($"Deleting Object: {selectedObject.name}");

            roomContents.Remove(selectedObject);

            Destroy(selectedObject);

            selectedObject = null;
        }

        public void ListItemSelected()
        {
        }

        public void LoadRoom()
        {
            var room = roomSerializer.LoadRoom("myRoom");

            foreach(var entity in room.Entities){
                var gameObj = obstacleGameObjects[(int)entity.Type];
                var instance = Instantiate(gameObj);
                var transform = instance.transform;
                transform.position = new Vector3(entity.XPosition, entity.YPosition, entity.ZPosition);
                transform.rotation = new Quaternion(entity.XRotation, entity.YRotation, entity.ZRotation, 1);
                transform.localScale = new Vector3(entity.XScale, entity.YScale, entity.ZScale);
                var meta = instance.GetComponent<EntityBehavior>();

                meta.moveSpeed = 0;
                meta.id = entity.Id;
            }
        }

        public void SaveRoom()
        {
            var room = new Room {
                Entities = roomContents.Select(x => {
                    var meta = x.GetComponent<EntityBehavior>();
                    var objTransform = x.transform;
                    return new Entity {
                            // Need to strip the '(Clone)' portion of the name
                            Type = Entity.ToEntityType(x.name.Substring(0, x.name.IndexOf('('))),
                            XPosition = objTransform.position.x,
                            YPosition = objTransform.position.y,
                            ZPosition = objTransform.position.z,
                            XRotation = objTransform.rotation.x,
                            YRotation = objTransform.rotation.y,
                            ZRotation = objTransform.rotation.z,
                            XScale = objTransform.localScale.x,
                            YScale = objTransform.localScale.y,
                            ZScale = objTransform.localScale.z,
                            Id = meta.id
                        };
                    }).ToList()
                };
            roomSerializer.SaveRoom(room, "myRoom");
        }
    }
}