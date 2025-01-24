using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ISpawner spawner;
    public List<IGameEventListener> listeners = new();

    public void RegisterListener(IGameEventListener listener){
        listeners.Add(listener);
    }

    void Start()
    {
        spawner = new TargetSpawner(transform);
    }

    void Update()
    {
        spawner.Spawn();
    }
}
