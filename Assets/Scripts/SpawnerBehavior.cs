using UnityEngine;

public class SpawnerBehavior : MonoBehaviour
{
    public GameObject targetPrefab;
    public float spawnInterval = 3f;

    float lastSpawnTime = 0;

    void Start()
    {
        lastSpawnTime = Time.time;
    }

    void Update()
    {
        var currTime = Time.time;
        if(lastSpawnTime + spawnInterval < Time.time){
            var targetX = Random.value * 10;
            var targetY = Random.value * 10;
            var targetPos = new Vector3(targetX, targetY, gameObject.transform.position.z);
            var obj = Instantiate(targetPrefab);
            obj.transform.position = targetPos;
            lastSpawnTime = currTime;
        }
    }
}
