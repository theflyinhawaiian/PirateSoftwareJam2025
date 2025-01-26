using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class MixedSpawner : ISpawner 
{
    private GameObject obstaclePrefab;
    private GameObject targetPrefab;
    private GameManager gameManager;
    public float spawnInterval = 3f;

    int spawnCount = 0;
    float lastSpawnTime = 0;

    Transform origin;
    public MixedSpawner(Transform origin, GameManager manager)
    {
        this.origin = origin;
        gameManager = manager;

        lastSpawnTime = Time.time;

        var prefabs = AssetFinder.GetObstaclePrefabs();

        var obstaclePrefab = prefabs
                            .Where(x => x.name == "TunnelObstacle")
                            .SingleOrDefault();
        var targetPrefab = prefabs.Where(x => x.name == "Target")
                            .SingleOrDefault();

        if(obstaclePrefab == null || targetPrefab == null)
            return;

        this.targetPrefab = targetPrefab;
        this.obstaclePrefab = obstaclePrefab;
    }

    public void Spawn()
    {
        var currTime = Time.time;
        if(lastSpawnTime + spawnInterval > currTime)
            return;

        var targetX = Random.value * 10;
        var targetY = Random.value * 10;
        var targetPos = new Vector3(targetX, targetY, origin.position.z);
        if(spawnCount % 2 == 0)
            SpawnTarget(targetPos);
        else
            SpawnObstacle(targetPos);
        
        spawnCount++;
        lastSpawnTime = currTime;
    }

    void SpawnTarget(Vector3 targetPos){
        var obj = GameObject.Instantiate(targetPrefab);
        obj.transform.position = targetPos;
        var behavior = obj.GetComponent<TargetBehavior>();
        behavior.SetManager(gameManager);

    }

    void SpawnObstacle(Vector3 targetPos){
        var obj = GameObject.Instantiate(obstaclePrefab);
        obj.transform.position = targetPos;
        var behavior = obj.GetComponent<ObstacleBehavior>();
        behavior.SetManager(gameManager);
    }
}
