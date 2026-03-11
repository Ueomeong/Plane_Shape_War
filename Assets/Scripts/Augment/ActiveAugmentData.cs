using UnityEngine;
public enum ActiveSkillType { None, Heal, Bullet, Shield } // 스킬 종류
[CreateAssetMenu(fileName = "ActiveAugmentData", menuName = "Scriptable Objects/ActiveAugmentData")]
public class ActiveAugmentData : AugmentData
{
    [Header("Active Skill")]
    public ActiveSkillType grantActiveSkill; // 부여할 액티브 스킬
    public virtual void ApplyAugment()
    {
        // ... 기존 스탯 상승 로직 ...

        // 6. 액티브 스킬 부여 로직
        if (grantActiveSkill != ActiveSkillType.None)
        {
            EquipSkillToPlayer();
        }
    }
    private void EquipSkillToPlayer()
    {
    }
}
