using UnityEngine;

public class ExpOrb : LootItem
{
    protected override void playerGetThisItem(int val)//플레이어가 이 아이템을 획득함
    {
        GameManager.Instance.exp += val;
        gameObject.SetActive(false);
    }
}
