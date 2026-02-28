using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ShopItemSetting : MonoBehaviour
{
    [Header("Data")]
    private ShopItemData shopItemData;
    [Header("txt,img")]
    public Image Icon;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemDesc;
    public TextMeshProUGUI ItemPrice;
    public Button BuyButton;

    public void SetUpShop(ShopItemData data)
    {
        shopItemData = data;
        if (data.ItemIcon != null)
        {
            Icon.sprite = data.ItemIcon;
        }
        ItemName.text = data.ItemName;
        ItemDesc.text = data.ItemDesc;
        ItemPrice.text = $"{data.basePrice} G";
        BuyButton.onClick.RemoveAllListeners();
        BuyButton.onClick.AddListener(() => OnBuyClick(data.basePrice, data.upgradeValue));
    }

    private void OnBuyClick(int price,float value)//구매시 작동할 로직
    {
        
    }
}
