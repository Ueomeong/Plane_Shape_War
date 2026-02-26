using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("*HP/SkillPoint")]
    public int maxHP;
    public int currentHP;
    public int maxSP;
    public int currentSP;
    public float invincibleTime;
    [Header("*Movement")]
    public float moveSpeed;
    [Header("*Charge Movement")]
    public float maxShootingForce;
    public float minShootingForce;
    public float maxChargeTime;
    [Header("*Charge Attack")]
    public float chargeDamage;
    [Header("*Bullet")]
    public float damage;
    public float Base_rateOfFire;//기본 공격속도
    public float Modifier_rateOfFire;//추가된 공격속도의 총 합 //1이면 공속 2배
    public float rateOfFire;//실제 적용되는 공격속도
    public int per;
    public int continuousFire;//연속 발사하는 총알의 개수
    public int spread_Bullet;//총알을 동시에 여러개 발사하는 개수(약 90도 각도로 흩뿌를것?)
    public void ResetPlayerData()
    {
        currentHP = maxHP;
        currentSP = maxSP;
        Modifier_rateOfFire = 0f;
        rateOfFire = Base_rateOfFire;
    }
}
