using TMPro;
using UnityEngine;

public class ButtonInfo : MonoBehaviour
{
    public int Price;
    public int Quantity;
    public TMP_Text PriceTxt;
    public TMP_Text QuantityTxt;
    public ShopManager ShopManager;

    void Start()
    {
        UpdateButtonInfo();
    }

    public void UpdateButtonInfo()
    {
        PriceTxt.text = $"Price: {Price}";
        QuantityTxt.text = $"Quantity: {Quantity}";
    }
}
