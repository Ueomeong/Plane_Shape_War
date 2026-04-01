using UnityEngine;
using UnityEngine.UIElements;

public abstract class ActiveAugment : MonoBehaviour//추상 클래스로 만들어서 각 스킬마다 다른 내용을 구현하도록 합니다.
{
    protected PlayerData PlayerData => GameManager.Instance.runtimePlayerData;
    public string A_skillName;
    public float A_cooldown;
    public int spCost;

    public void TryUseSkill()
    {
        if (PlayerData.currentSP < spCost) return;//SP 부족
        PlayerData.currentSP -= spCost;
        InGameManager.Instance.spManager.UpdateSP();
        ExecuteSkill();
        InGameManager.Instance.hpManager.UpdateHP();
    }
    public abstract void ExecuteSkill();

    public abstract void OnDisable();
}
