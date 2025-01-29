using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour
{
    public float money;
    public TMP_Text MoneyTXT;

    void Start()
    {
        UpdateMoneyText();
    }

    public void Buy()
    {
        var item = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject.GetComponent<ButtonInfo>();

        if (money >= item.Price && item.Quantity > 0)
        {
            money -= item.Price;
            item.Quantity--;
            UpdateMoneyText();
            item.UpdateButtonInfo();
        }        
    }

    private void UpdateMoneyText()
    {
        MoneyTXT.text = $"Money: {money}";
    }
}
