using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TMP_Text moneyText;
    public TMP_Text healthText;

    public int playerMoney = 0;
    public int playerHealth = 3;
    public ISpawner spawner;

    public void InitializeGame(int currentPlayerMoney){
        playerMoney = currentPlayerMoney;
    }

    void Start()
    {
        spawner = new MixedSpawner(transform, this);

        moneyText.text = $"Player Money: {playerMoney}";
        healthText.text = $"Player Health: {playerHealth}";
    }

    void Update()
    {
        spawner.Spawn();
    }

    public void TargetDestroyed(int value){
        playerMoney += value;
        Debug.Log($"Money: {playerMoney}");
        moneyText.text = $"Player Money: {playerMoney}";
    }

    public void ObstacleImpacted(int damageValue){
        playerHealth -= damageValue;
        Debug.Log($"Health: {playerHealth}");
        healthText.text = $"Player Health: {playerHealth}";

        if(playerHealth <= 0){
            // fuckin die
        }
    }
}
