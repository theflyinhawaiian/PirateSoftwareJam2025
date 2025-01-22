using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour
{
    public float money;
    public TMP_Text MoneyTXT;

    public Dictionary<int, Item> shopItems = new();

    void Start()
    {
        MoneyTXT.text = "Money: " + money.ToString();

        shopItems[1] = new () { Id = 1, Price = 10, Quantity = 0 };
        shopItems[2] = new () { Id = 2, Price = 20, Quantity = 0 };
        shopItems[3] = new () { Id = 3, Price = 30, Quantity = 0 };
        shopItems[4] = new () { Id = 4, Price = 40, Quantity = 0 };
    }

    public void Buy()
    {
        GameObject buttonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ButtonInfo buttonInfo = buttonRef.GetComponent<ButtonInfo>();
        int itemId = buttonInfo.ItemID;

        if (shopItems.TryGetValue(itemId, out Item item))
        {
            if (money >= item.Price)
            {
                money -= item.Price;
                item.Quantity++;
                MoneyTXT.text = $"Money: {money.ToString()}";
                buttonInfo.QuantityTxt.text = item.Quantity.ToString();
            }
        }
    }

    public class Item
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
