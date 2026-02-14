using UnityEngine;

public class Coin : LootItem
{
    protected override void playerGetThisItem(int val)//«√∑π¿ÃæÓ∞° ¿Ã æ∆¿Ã≈€¿ª »πµÊ«‘// µ∑ »πµÊ!
    {
        GameManager.Instance.temporaryMoney += val;
        gameObject.SetActive(false);
    }
}
