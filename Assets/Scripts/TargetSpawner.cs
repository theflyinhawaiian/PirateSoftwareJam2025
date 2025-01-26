using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class TargetSpawner : ISpawner 
{
    private GameObject targetPrefab;
    private GameManager gameManager;
    public float spawnInterval = 3f;

    float lastSpawnTime = 0;

    Transform origin;
    public TargetSpawner(Transform origin, GameManager manager)
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
    }

    public void Spawn()
    {
        var currTime = Time.time;
        if(lastSpawnTime + spawnInterval > currTime)
            return;

        var targetX = Random.value * 10;
        var targetY = Random.value * 10;
        var targetPos = new Vector3(targetX, targetY, origin.position.z);
        var obj = GameObject.Instantiate(targetPrefab);
        obj.transform.position = targetPos;
        var behavior = obj.GetComponent<TargetBehavior>();
        behavior.SetManager(gameManager);
        lastSpawnTime = currTime;
    }
}
