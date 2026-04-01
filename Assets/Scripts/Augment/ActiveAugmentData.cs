using UnityEngine;
[CreateAssetMenu(fileName = "ActiveAugmentData", menuName = "Scriptable Objects/ActiveAugmentData")]
public class ActiveAugmentData : AugmentData
{
    [Header("Active Skill")]
    public GameObject skillPrefab;
    public override void ApplyAugment()
    {
        base.ApplyAugment();
        if (skillPrefab != null)
        {
            EquipSkillToPlayer();
        }
        else
        {
            Debug.Log($"error active skill");
        }

    }
    private void EquipSkillToPlayer()
    {
        Player_Ability player_Ability = InGameManager.Instance.player_ability;
        if(player_Ability!=null)
        {
            player_Ability.EquipActiveSkill(skillPrefab);
            GameManager.Instance.AddAcquiredAugment(augmentName);
        }
    }
}
