//ÀÚÆøº´
using UnityEngine;

public class Capsule : Enemy
{
    [Header("capsule")]
    [SerializeField] private int spawnSquareNum;
    [SerializeField] private float spreadRange;
    public override void Die()
    {
        for (int i = 0; i < spawnSquareNum; i++)
        {
            Vector3 spawnPosition = transform.position;
            Vector3 randomDir = Random.insideUnitCircle.normalized * Random.Range(0.5f, spreadRange);
            Vector3 endPos = spawnPosition + randomDir;
            GameObject baby = InGameManager.Instance.poolmanager.Get(7);
            baby.transform.position = endPos;
        }
        base.Die();
    }
}
