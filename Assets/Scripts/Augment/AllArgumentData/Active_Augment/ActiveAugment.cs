using UnityEngine;
using UnityEngine.UIElements;

public abstract class ActiveAugment : MonoBehaviour//추상 클래스로 만들어서 각 스킬마다 다른 내용을 구현하도록 합니다.
{
    public string A_skillName;
    public float A_cooldown;
    public float A_ManaUseage;
    public Image icon;
    public abstract void ExecuteSkill();
}
