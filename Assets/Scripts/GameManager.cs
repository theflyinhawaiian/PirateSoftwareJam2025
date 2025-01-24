using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ISpawner spawner;

    void Start()
    {
        spawner = new TargetSpawner(transform);
    }

    void Update()
    {
        spawner.Spawn();
    }
}
