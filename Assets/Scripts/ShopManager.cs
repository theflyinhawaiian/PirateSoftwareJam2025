using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public int money;
    public TMP_Text MoneyTXT;

    void Start()
    {
        money = GameState.PlayerMoney;
        UpdateMoneyText();
    }

    public void Buy()
    {
        var item = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject.GetComponent<ButtonInfo>();

        if (money >= item.Price && item.Quantity > 0)
        {
            money -= item.Price;
            item.Quantity--;
            GameState.DurabilityLevel += 1;
            UpdateMoneyText();
            item.UpdateButtonInfo();
        }        
    }

    private void UpdateMoneyText()
    {
        MoneyTXT.text = $"Money: {money}";
    }

    public void StartGameTapped(){
        GameState.PlayerMoney = money;
        SceneManager.LoadScene("Main");
    }
}
