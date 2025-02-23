using System.Linq;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour, IGameEventListener
{
    private TMP_Text moneyText;
    private TMP_Text healthText;

    void Start()
    {
        moneyText = gameObject.GetComponentsInChildren<TMP_Text>().Where(x => x.name == "MoneyText").First();
        healthText = gameObject.GetComponentsInChildren<TMP_Text>().Where(x => x.name == "HealthText").First();

        var gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.RegisterListener(this);
    }

    public void HealthUpdated(int currHealth){
        healthText.text = $"Health: {currHealth}";
    }

    public void MoneyUpdated(int currMoney){
        moneyText.text = $"Money: {currMoney}";
    }
}
