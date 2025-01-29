using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int myValue = 1;
    public int playerMoney = 0;
    public int playerHealth = 3;
    public float moveSpeed = 15f;
    public int roomNumber = 0;
    public RoomSpawner spawner;

    public List<IGameEventListener> listeners = new();

    public Transform bounds;

    int lastRoomNumber;

    public void RegisterListener(IGameEventListener listener){
        listeners.Add(listener);
    }

    public void InitializeGame(int currentPlayerMoney){

    }

    void Start()
    {
        spawner = new RoomSpawner(transform, this);
        playerMoney = GameState.PlayerMoney;
        playerHealth = GameState.DurabilityLevel + 3;
    }

    void Update()
    {
        if(lastRoomNumber != roomNumber){
            spawner.SetRoomNumber(roomNumber);
            lastRoomNumber = roomNumber;
        }
        spawner.Spawn();
    }

    public void TargetDestroyed(int value){
        playerMoney += value;
        Debug.Log($"Money: {playerMoney}");

        foreach(var listener in listeners)
            listener.MoneyUpdated(playerMoney);
    }

    public void ObstacleImpacted(int damageValue){
        playerHealth -= damageValue;
        Debug.Log($"Health: {playerHealth}");

        foreach(var listener in listeners)
            listener.HealthUpdated(playerHealth);

        if(playerHealth <= 0){
            SceneManager.LoadScene();
        }
    }
}
