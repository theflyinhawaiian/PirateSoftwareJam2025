using System.Linq;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour, IGameEventListener
{
    private TMP_Text moneyText;
    private TMP_Text healthText;

    void Start()
    {
        var gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.RegisterListener(this);

        moneyText = gameObject.GetComponentsInChildren<TMP_Text>().Where(x => x.name == "MoneyText").First();
        healthText = gameObject.GetComponentsInChildren<TMP_Text>().Where(x => x.name == "HealthText").First();
    }

    public void HealthUpdated(int currHealth){
        healthText.text = $"Player Health: {currHealth}";
    }

    public void MoneyUpdated(int currMoney){
        moneyText.text = $"Player Money: {currMoney}";
    }
}
