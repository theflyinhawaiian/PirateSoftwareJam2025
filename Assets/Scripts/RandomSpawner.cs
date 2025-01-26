using System.Collections.Generic;
using System.Linq;
using Assets.Model;
using Assets.Scripts;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class RandomSpawner : ISpawner 
{
    private class PrefabData {
        public GameObject prefab;
        public float extentX;
        public float extentY;
    }

    private List<PrefabData> prefabData = new();
    public float spawnInterval = 3f;

    float lastSpawnTime = 0;

    float leftBounds;
    float rightBounds;
    float topBounds;
    float bottomBounds;

    Transform origin;
    public RandomSpawner(Transform origin, Transform bounds)
    {
        this.origin = origin;
        lastSpawnTime = Time.time;

        var prefabs = AssetFinder.GetObstaclePrefabs();

        leftBounds = bounds.Find("left").position.x;
        rightBounds = bounds.Find("right").position.x;
        topBounds = bounds.Find("top").position.y;
        bottomBounds = bounds.Find("bottom").position.y;

        // there's seemingly no way to do this from a prefab, so we'll spawn 
        // in a sample boi per detected prefab and get its info, destroying it when we're done
        for(var i = 0; i < prefabs.Count; i++){
            var p = prefabs[i];
            var sample = Object.Instantiate(p, new Vector3(1000, 1000, 1000), Quaternion.identity);

            var compositeBounds = 
                sample.transform.GetComponentsInChildren<Renderer>()
                    .Select(x => x.bounds)
                    .Aggregate((intermediateBounds, nextBounds) => {
                        intermediateBounds.Encapsulate(nextBounds);
                        return intermediateBounds;
                    });

            prefabData.Add(new PrefabData { 
                prefab = p,
                extentX = compositeBounds.extents.x,
                extentY = compositeBounds.extents.y
            }); 
            Object.Destroy(sample);
        }
    }

    public void Spawn()
    {
        var currTime = Time.time;
        if(lastSpawnTime + spawnInterval > currTime)
            return;

        var nextObject = Mathf.FloorToInt(Random.Range(0, prefabData.Count));
        var selectedPrefab = prefabData[nextObject];

        Debug.Log($"spawning {selectedPrefab.prefab.name}, x extent: {selectedPrefab.extentX}, left bound: {leftBounds + selectedPrefab.extentX}, bottom bound: {bottomBounds + selectedPrefab.extentY}");

        var spawnX = leftBounds + selectedPrefab.extentX + (Random.value * (rightBounds - selectedPrefab.extentX));
        var spawnY = bottomBounds + selectedPrefab.extentY + (Random.value * (topBounds - selectedPrefab.extentY));
        var targetPos = new Vector3(spawnX, spawnY, origin.position.z);
        var obj = Object.Instantiate(selectedPrefab.prefab);
        obj.transform.position = targetPos;
        var entity = obj.GetComponent<EntityBehavior>();
        entity.moveSpeed = 15f;
        lastSpawnTime = currTime;
    }
}
