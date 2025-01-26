using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        private class RoomInfo {
            public string Name;
            public Room Data;
        }

        public TMP_Dropdown obstacleSelector;
        public TMP_Dropdown roomsDropdown;
        public TMP_InputField saveRoomInput;
        private readonly string roomsPath = "Assets/Resources/Rooms";
        private List<GameObject> obstacleGameObjects = new();
        private List<RoomInfo> savedRooms = new();
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
            if(Selection.activeGameObject != selectedObject)
            {
                if (Selection.activeGameObject == null)
                {
                    selectedObject = null;
                    return;
                }

                Transform asf = Selection.activeGameObject.transform;

                while (asf.parent != null)
                {
                    asf = asf.parent;
                } 

                selectedObject = asf.gameObject;
                Selection.activeGameObject = selectedObject;
                
                Debug.Log(selectedObject);
            }
        }

        private void PopulateObstacles()
        {
            obstacleSelector.ClearOptions();
            obstacleGameObjects.Clear(); 

            obstacleGameObjects = AssetFinder.GetObstaclePrefabs();

            obstacleSelector.AddOptions(obstacleGameObjects.Select(x => x.name).ToList());
        }

        private void PopulateRooms()
        {
            roomsDropdown.ClearOptions();

            var roomData = AssetFinder.GetText(roomsPath);

            savedRooms = roomData.Select(x => new RoomInfo {
                Name = x.name,
                Data = JsonUtility.FromJson<Room>(x.text)
            }).ToList();

            roomsDropdown.AddOptions(savedRooms.Select(x => x.Name).ToList());
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
            var rb = entity.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
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
            foreach(var entity in roomContents){
                Destroy(entity);
            }

            roomContents.Clear();

            var roomName = roomsDropdown.options[roomsDropdown.value].text;
            var room = roomSerializer.LoadRoom(roomName);

            foreach(var entity in room.Entities){
                var gameObj = obstacleGameObjects[(int)entity.Type];
                var instance = Instantiate(gameObj);
                var rb = instance.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
                var transform = instance.transform;
                transform.position = new Vector3(entity.XPosition, entity.YPosition, entity.ZPosition);
                transform.rotation = new Quaternion(entity.XRotation, entity.YRotation, entity.ZRotation, entity.WValue);
                transform.localScale = new Vector3(entity.XScale, entity.YScale, entity.ZScale);
                var meta = instance.GetComponent<EntityBehavior>();

                meta.moveSpeed = 0;
                meta.id = entity.Id;

                roomContents.Add(instance);
            }

            nextId = room.Entities.Last().Id + 1;
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
                            WValue = objTransform.rotation.w,
                            XScale = objTransform.localScale.x,
                            YScale = objTransform.localScale.y,
                            ZScale = objTransform.localScale.z,
                            Id = meta.id
                        };
                    }).ToList()
                };
            var regex = new Regex(@"\d+");
            
            var lastRoom = savedRooms.LastOrDefault();
            var numStr = "";
            if(lastRoom == null){
                numStr = "0001";
            }else{
                var num = int.Parse(regex.Match(lastRoom.Name).Value) + 1;
                numStr = $"{num}".PadLeft(4, '0');
            }

            roomSerializer.SaveRoom(room, $"room{numStr}");
        }
    }
}