using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class TargetSpawner : ISpawner 
{
    private GameObject targetPrefab;
    private GameManager gameManager;
    public float spawnInterval = 3f;

    float lastSpawnTime = 0;

    public float targetX;
    public float targetY;

    float extentX;
    float extentY;

    float leftBounds;
    float rightBounds;
    float topBounds;
    float bottomBounds;

    Transform origin;
    public TargetSpawner(Transform origin, Transform bounds, GameManager manager)
    {
        this.origin = origin;
        gameManager = manager;

        lastSpawnTime = Time.time;

        var targetPrefab = AssetFinder.GetObstaclePrefabs()
                            .Where(x => x.name == "Target")
                            .SingleOrDefault();

        if(targetPrefab == null)
            return;

        this.targetPrefab = targetPrefab;

        leftBounds = bounds.Find("left").position.x;
        rightBounds = bounds.Find("right").position.x;
        topBounds = bounds.Find("top").position.y;
        bottomBounds = bounds.Find("bottom").position.y;

        // there's seemingly no way to do this from a prefab, so we'll spawn 
        // in a sample boi and get its info, destroying it when we're done
        var sample = Object.Instantiate(targetPrefab, new Vector3(1000, 1000, 1000), Quaternion.identity);
        var extents = sample.GetComponent<Renderer>().bounds.extents;
        extentX = extents.x;
        extentY = extents.y;
        Object.Destroy(sample);
    }

    public void SetSpawnCoords(float x, float y){
        targetX = x;
        targetY = y;
    }

    public void Spawn()
    {
        var currTime = Time.time;
        if(lastSpawnTime + spawnInterval > currTime)
            return;

        var spawnX = leftBounds + extentX + (Random.value * (rightBounds - extentX));
        var spawnY = bottomBounds + extentY + (Random.value * (topBounds - extentY));
        var targetPos = new Vector3(spawnX, spawnY, origin.position.z);
        var obj = Object.Instantiate(targetPrefab);
        obj.transform.position = targetPos;
        var behavior = obj.GetComponent<TargetBehavior>();
        behavior.SetManager(gameManager);
        lastSpawnTime = currTime;
    }
}
