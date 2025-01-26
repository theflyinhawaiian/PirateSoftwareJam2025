using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;

public class RoomSpawner : ISpawner {
    RoomFileHandler fileHandler;
    float lastSpawnTime;
    float spawnDelay = 6f;
    int roomNumber;

    GameManager manager;
    Transform origin;

    List<GameObject> obstaclePrefabs = new();

    public RoomSpawner(Transform origin, GameManager manager){
        fileHandler = new RoomFileHandler();

        lastSpawnTime = Time.time;

        this.origin = origin;
        this.manager = manager;

        obstaclePrefabs = AssetFinder.GetObstaclePrefabs();
    }

    public void SetRoomNumber(int roomNumber) =>
        this.roomNumber = roomNumber;

    public void Spawn(){
        var currTime = Time.time;
        if(lastSpawnTime + spawnDelay > currTime)
            return;
        
        var roomNum = roomNumber == 0 ? 1 + Random.Range(0, fileHandler.FetchRooms().Length) : roomNumber;
        var numStr = $"{roomNum}".PadLeft(4, '0');
        var roomToSpawn = fileHandler.LoadRoom($"room{numStr}");

        foreach(var entity in roomToSpawn.Entities){
            var gameObj = obstaclePrefabs[(int)entity.Type];
            var instance = Object.Instantiate(gameObj);
            var transform = instance.transform;
            var rb = instance.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
            transform.position = new Vector3(entity.XPosition, entity.YPosition, origin.position.z + entity.ZPosition);
            transform.rotation = new Quaternion(entity.XRotation, entity.YRotation, entity.ZRotation, entity.WValue);
            transform.localScale = new Vector3(entity.XScale, entity.YScale, entity.ZScale);
            var meta = instance.GetComponent<EntityBehavior>();

            meta.SetManager(manager);
            meta.id = entity.Id;
        } 

        lastSpawnTime = currTime;
    }
}