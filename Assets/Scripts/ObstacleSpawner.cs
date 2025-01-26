using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class ObstacleSpawner : ISpawner 
{
    private GameObject obstaclePrefab;
    private GameManager gameManager;
    public float spawnInterval = 3f;

    float lastSpawnTime = 0;

    Transform origin;
    public ObstacleSpawner(Transform origin, GameManager manager)
    {
        this.origin = origin;
        gameManager = manager;

        lastSpawnTime = Time.time;

        var prefabs = AssetFinder.GetObstaclePrefabs();

        var obstaclePrefab = prefabs
                            .Where(x => x.name == "TunnelObstacle")
                            .SingleOrDefault();

        if(obstaclePrefab == null)
            return;

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
        var obj = GameObject.Instantiate(obstaclePrefab);
        obj.transform.position = targetPos;
        var behavior = obj.GetComponent<ObstacleBehavior>();
        behavior.SetManager(gameManager);
        lastSpawnTime = currTime;
    }
}
