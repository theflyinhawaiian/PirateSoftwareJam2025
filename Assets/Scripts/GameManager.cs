using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ISpawner spawner;
    public GameObject targetPrefab;

    void Start()
    {
        spawner = new TargetSpawner(transform, gameObject);
    }

    void Update()
    {
        spawner.Spawn();
    }
}
