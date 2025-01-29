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

    void Start()
    {
        spawner = new RoomSpawner(transform, this);
        playerMoney = GameState.PlayerMoney;
        playerHealth = GameState.DurabilityLevel + 3;

        UpdateHealth();
        UpdateMoney();
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

        UpdateMoney();
    }

    public void ObstacleImpacted(int damageValue){
        playerHealth -= damageValue;
        Debug.Log($"Health: {playerHealth}");

        UpdateHealth();

        if(playerHealth <= 0){
            GameState.PlayerMoney = playerMoney;
            SceneManager.LoadScene("Shop");
        }
    }


    private void UpdateMoney() {
        foreach(var listener in listeners)
            listener.MoneyUpdated(playerMoney);
    }

    private void UpdateHealth() {
        foreach(var listener in listeners)
            listener.HealthUpdated(playerHealth);
    }
}
