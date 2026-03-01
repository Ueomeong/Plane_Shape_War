using TMPro;
using UnityEngine;
using System.Collections.Generic;
public class ShopManager : MonoBehaviour
{
    [Header("UI references")]
    public TextMeshProUGUI topMoneyText;//상단의 돈
    public Transform contentTransform;//스크롤 뷰의 content안에 프리펩들을 넣어야 하므로 content가져오기
    public GameObject shopItemPrefab;//상점의 판매 상품 프리펩

    private List<ShopItemSetting> spawnedItems = new List<ShopItemSetting>();

    private void Start()
    {
        InitializeShop();
        UpdateGlobalUI();
    }

    private void InitializeShop()
    {
        // GameManager에 등록된 모든 상점 데이터(ScriptableObject)를 가져옵니다.
        ShopItemData[] allItems = GameManager.Instance.allShopItems;

        foreach (ShopItemData data in allItems)//상품 목록 생성
        {
            // 프리팹을 Content의 자식으로 생성
            GameObject itemObj = Instantiate(shopItemPrefab, contentTransform);
            ShopItemSetting itemSetting = itemObj.GetComponent<ShopItemSetting>();

            // 세팅 함수 호출 (이때 ShopManager 자신을 넘겨줘서 소통하게 만듦)
            itemSetting.SetUpShop(data, this);

            // 나중에 전체 새로고침을 위해 리스트에 저장해둠
            spawnedItems.Add(itemSetting);
        }
    }

    // 2. 돈 텍스트를 갱신하고, 모든 아이템의 버튼 상태를 새로고침하는 함수
    public void UpdateGlobalUI()
    {
        // 맨 위 돈 UI 텍스트 갱신
        topMoneyText.text = $"{GameManager.Instance.money} G";

        // 돈이 바뀌었으니 모든 아이템에게 "너희 지금 돈으로 살 수 있는지 다시 계산해!" 라고 명령
        foreach (ShopItemSetting item in spawnedItems)
        {
            item.RefreshUI();
        }
    }
}
