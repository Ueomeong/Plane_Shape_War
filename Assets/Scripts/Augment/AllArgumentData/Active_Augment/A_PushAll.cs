using UnityEngine;

public class A_PushAll : ActiveAugment
{
    public override void ExecuteSkill()
    {
        InGameManager.Instance.poolmanager.Get(17);
    }
}
