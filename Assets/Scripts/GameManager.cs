using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int playerMoney = 0;
    public int playerHealth = 3;
    public ISpawner spawner;

    public void InitializeGame(int currentPlayerMoney){
        playerMoney = currentPlayerMoney;
    }

    void Start()
    {
        spawner = new MixedSpawner(transform, this);
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
