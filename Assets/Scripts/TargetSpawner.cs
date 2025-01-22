using UnityEngine;

public class TargetSpawner : ISpawner 
{
    private GameObject targetPrefab;
    public float spawnInterval = 3f;

    float lastSpawnTime = 0;

    Transform origin;
    public TargetSpawner(Transform origin, GameObject targetPrefab)
    {
        this.origin = origin;
        this.targetPrefab = targetPrefab;
    }

    void Start()
    {
        lastSpawnTime = Time.time;
    }

    public void Spawn()
    {
        
        var currTime = Time.time;
        if(lastSpawnTime + spawnInterval < Time.time){
            var targetX = Random.value * 10;
            var targetY = Random.value * 10;
            var targetPos = new Vector3(targetX, targetY, origin.position.z);
            var obj = GameObject.Instantiate(targetPrefab);
            obj.transform.position = targetPos;
            lastSpawnTime = currTime;
        }

        Debug.Log("Hello");


    }
}
