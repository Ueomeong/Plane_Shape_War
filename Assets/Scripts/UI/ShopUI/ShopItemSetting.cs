using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ShopItemSetting : MonoBehaviour
{
    [Header("Data")]
    private ShopItemData currentData;
    private ShopManager shopManager;
    [Header("txt,img")]
    public Image Icon;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemDesc;
    public TextMeshProUGUI ItemPrice;
    public Button BuyButton;

    public void SetUpShop(ShopItemData data,ShopManager manager)
    {
        currentData = data;
        shopManager = manager;
        if (data.ItemIcon != null)
        {
            Icon.sprite = data.ItemIcon;
        }
        //ItemName.text = data.ItemName;
        ItemDesc.text = data.ItemDesc;
        RefreshUI();//가격과 레벨은 고정된값이 아님.
        BuyButton.onClick.RemoveAllListeners();
        BuyButton.onClick.AddListener(OnBuyClick);
    }
    public void RefreshUI()//상점 새로고침
    {
        int currentLevel = GameManager.Instance.GetStatLevel(currentData.targetStat);
        int currentPrice = currentData.basePrice + (currentLevel * currentData.priceIncrement);//레벨에 따른 가격 계산

        ItemName.text = $"{currentData.ItemName} (Lv.{currentLevel})";//이름
        ItemPrice.text = $"{currentPrice} G";//가격
        BuyButton.interactable = (GameManager.Instance.money >= currentPrice);
    }
    private void OnBuyClick()//구매시 작동할 로직
    {
        int currentLevel = GameManager.Instance.GetStatLevel(currentData.targetStat);
        int currentPrice = currentData.basePrice + (currentLevel * currentData.priceIncrement);

        if (GameManager.Instance.money >= currentPrice)//구매 가능일때 눌렀을 경우!~
        {
            GameManager.Instance.MoneyChange(-currentPrice);
            GameManager.Instance.IncreaseStatLevel(currentData.targetStat);
            shopManager.UpdateGlobalUI();
        }
    }

}