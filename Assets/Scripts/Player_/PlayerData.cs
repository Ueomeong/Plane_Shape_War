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
    public float maxShooingForce;
    public float minShooingForce;
    public float maxChargeTime;
    [Header("*Charge Attack")]
    public float chargeDamage;
    [Header("*Bullet")]
    public float damage;
    public float rateOfFire;
    public int per;
    public void ResetPlayerData()
    {
        currentHP = maxHP;
        currentSP = maxSP;
    }
}
