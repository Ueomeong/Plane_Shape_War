using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("*HP/Shield")]
    public int maxHP;
    public int currentHP;
    public float maxShieldHP;
    public float currentShieldHP;
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
        currentShieldHP = maxShieldHP;
    }
}
