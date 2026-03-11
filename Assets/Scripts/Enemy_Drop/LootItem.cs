using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class LootItem : MonoBehaviour
{
    [Header("Dropping Settings")]
    public float spreadRange;//퍼지는 범위
    public float popDuration;//튀어오르는 시간
    public float waitDuration;//바닥에서 잠시 대기하는 시간
    public float originMoveSpeed;//플레이어에게 날아가는 속도
    private float moveSpeed;
    private int value;//이 아이템의 가치
    private bool isMagnetized;
    private Transform targetPlayer;
    private void OnEnable()
    {
       moveSpeed=originMoveSpeed;
    }
    public void Init(int amount, Vector3 spawnPosition, Transform player)
    {
        value = amount;
        targetPlayer = player;
        transform.position = spawnPosition;
        isMagnetized = false;

        StartCoroutine(DropProcess());
    }

    IEnumerator DropProcess()
    {
        Vector3 spawnPosition = transform.position;
        Vector3 randomDir = Random.insideUnitCircle.normalized * Random.Range(0.5f, spreadRange);
        Vector3 endPos = spawnPosition + randomDir;
        float timer = 0f;
        while(timer<=popDuration)
        {
            timer += Time.deltaTime;
            float t = timer / popDuration;//진행과정
            float easeOut = 1f - Mathf.Pow(1f - t, 3);
            transform.position = Vector3.Lerp(spawnPosition , endPos, easeOut);
            yield return null;
        }
        yield return new WaitForSeconds(waitDuration);
        isMagnetized = true;//플레이어에게 다가가자
    }

    protected void Update()
    {
        if (!isMagnetized || targetPlayer == null) return;
        transform.position = Vector3.MoveTowards(transform.position, targetPlayer.position, moveSpeed * Time.deltaTime);
        moveSpeed += 10f * Time.deltaTime;

        if(Vector3.Distance(transform.position,targetPlayer.position)<0.5f | InGameManager.Instance.isEnd)
        {
            playerGetThisItem(value);
        }
    }
    protected virtual void playerGetThisItem(int val)//플레이어가 이 아이템을 획득함
    {
      
    }
}
