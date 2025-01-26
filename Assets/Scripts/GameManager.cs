using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int myValue = 1;
    public int playerMoney = 0;
    public int playerHealth = 3;
    public float moveSpeed = 15f;
    public ISpawner spawner;
    public List<IGameEventListener> listeners = new();

    public Transform bounds;

    public void RegisterListener(IGameEventListener listener){
        listeners.Add(listener);
    }

    public void InitializeGame(int currentPlayerMoney){
        playerMoney = currentPlayerMoney;
    }

    void Start()
    {
        spawner = new RoomSpawner(transform, this);
    }

    void Update()
    {
        spawner.Spawn();
    }

    public void TargetDestroyed(int value){
        playerMoney += value;
        Debug.Log($"Money: {playerMoney}");
    }

    public void ObstacleImpacted(int damageValue){
        playerHealth -= damageValue;
        Debug.Log($"Health: {playerHealth}");

        if(playerHealth <= 0){
            // fuckin die
        }
    }
}
