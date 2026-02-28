using UnityEngine;
public enum StatType
{
    MaxHP,//최대체력
    MaxSp,//최대마나
    InvicibleTime,//무적시간
    MoveSpeed,//이동속도
    MaxShootingForce,
    MinShootingForce,
    MaxChargeTime,
    Damage,//데미지
    Modifier_rateOfFire,//추가된 공격속도의 총 합 //1이면 공속 2배
    Per,//관통력
    ContinuousFire,//연속 발사하는 총알의 개수
    Spread_Bullet//총알을 동시에 여러개 발사하는 개수(약 90도 각도로 흩뿌를것?)
}
[CreateAssetMenu(fileName = "ShopItemData", menuName = "Scriptable Objects/ShopItemData")]
public class ShopItemData : ScriptableObject
{
    public string ItemName;//이름
    public string ItemDesc;//설명
    public Sprite ItemIcon;//아이콘
    public int basePrice;//초기 가격
    public int priceIncreasement;//레벨당 증가하는 가격
    public float upgradeValue;//증가하는 능력치의 값
    [Header("Upgrade Target")]
    public StatType targetStat;
}
