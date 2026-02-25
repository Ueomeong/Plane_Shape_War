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
    public void ResetPlayerData()
    {
        currentHP = maxHP;
        currentSP = maxSP;
        Modifier_rateOfFire = 0f;
        rateOfFire = Base_rateOfFire;
    }
}
