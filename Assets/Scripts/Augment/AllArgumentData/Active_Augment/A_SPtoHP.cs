using UnityEngine;

public class A_SPtoHP : ActiveAugment
{
    [SerializeField] int healAmount = 1;
    public override void ExecuteSkill()
    {
        PlayerData.currentHP += healAmount;
        if (PlayerData.currentHP > PlayerData.maxHP)
        {
            PlayerData.currentHP = PlayerData.maxHP;
        }
    }
    public override void OnDisable()
    {

    }
}
