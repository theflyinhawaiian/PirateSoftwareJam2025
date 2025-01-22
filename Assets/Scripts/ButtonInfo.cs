using TMPro;
using UnityEngine;

public class ButtonInfo : MonoBehaviour
{
    public int ItemID;
    public TMP_Text PriceTxt;
    public TMP_Text QuantityTxt;
    public ShopManager ShopManager;

    void Update()
    {
        if (ShopManager != null && ShopManager.shopItems.TryGetValue(ItemID, out var item))
        {
            PriceTxt.text = "Price: " + item.Price.ToString();
            QuantityTxt.text = "Quantity: " + item.Quantity.ToString();
        }
        else
        {
            Debug.LogWarning($"Item with ID {ItemID} not found or ShopManager is not assigned.");
        }
    }
}
