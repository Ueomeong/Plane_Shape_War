//sniper
using UnityEngine;
using System.Collections;
using System;
public class Diamond : Enemy
{
    [Header("Diamond Specifics")]
    public float maxChargeTime=1.5f;//총알 발사 주기
    public float attackCoolTime = 1.0f;//공격 사이에 쿨타임
    public float currentChargeTime = 0.0f;
    public float stopDistance = 5.0f;//플레이어와의 적정 거리  

    private bool isMoveable = true;
    private bool isCharging = false; //충전중인가?
    private bool isShootable = false;//쏠 수 있는가?
    private int RandNumToMove;//-1,1 중 하나의 값을 가질것. 이에 따라 조준중 움직임 방향을 정하자.
    private float RandMoveTimeCount;
    private Color ChargeColor = Color.blue;//충전시 색

    private Vector3 originalSize = Vector3.one;//기본 크기
    private Vector3 fullChargedSize = new Vector3(0.65f,1.75f,1);//풀 충전 크기
    private Vector3 ShootingSize = new Vector3(1.12f, 0.9f, 1);//쏘는 순간의 크기

    private Transform ts;

    public override void Awake()
    {

        base.Awake();
        ts= GetComponentInChildren<Transform>();
        isMoveable = true;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        currentChargeTime = 0.0f;
        ts.localScale = originalSize;
        isCharging = false;
        isShootable = true;
        attackDamage = 1;//공격력
        isKnockbackable = true;
        RandNumToMove = 1;
        RandMoveTimeCount = 0f;
    }

    protected override void FixedUpdate()
    {
        DetectPlayer();
        outlineSpriteRenderer.color = Color.Lerp(outlineColor, playerChasingColor, willToChase / 3);
        RandMoveTimeCount-=Time.fixedDeltaTime;
        if (isChasingPlayer)
        {
            isWaiting = false;
            if (isMoveable)
            {
                ShootingMove(targetPosition, moveSpeed * 1.1f);
            }
            if(isShootable)
            {
                isShootable=false;
                currentChargeTime = 0f;
                StartCoroutine(ChargeShootingSequence());
            }
        }
        else
        {

            Recon();
        }
    }

    private IEnumerator ChargeShootingSequence()
    {
        isCharging=true;
        Vector2 shootingDirection;
        rb.constraints = RigidbodyConstraints2D.None;
        while (currentChargeTime <= maxChargeTime)//충전중!
        {
            currentChargeTime += Time.fixedDeltaTime;
            ts.localScale = Vector3.Lerp(originalSize,fullChargedSize,currentChargeTime/maxChargeTime);
            Vector2 dir = (playerTransform.position - transform.position).normalized;//플레이어 위치와 tri 의 방향 추출
            float angle = (Mathf.Atan2(dir.y, dir.x) * (Mathf.Rad2Deg));
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), currentChargeTime / maxChargeTime);
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 0.1f);

            outlineSpriteRenderer.color = Color.Lerp(outlineColor, ChargeColor, currentChargeTime / maxChargeTime);
            shootingDirection = dir;
            yield return new WaitForFixedUpdate();
        }

        currentChargeTime = 0f;
        rb.linearVelocity=Vector2.zero;
        while (currentChargeTime <= 0.08f)
        {
            currentChargeTime += Time.fixedDeltaTime;
            ts.localScale = Vector3.Lerp(fullChargedSize, ShootingSize, currentChargeTime / 0.08f);
            rb.AddForce((playerTransform.position - transform.position).normalized * -1 * 50f);
            yield return new WaitForFixedUpdate();
        }
        Fire();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isMoveable = false;
        //rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        currentChargeTime = 0f;
        while (currentChargeTime <= 0.3f)
        {
            currentChargeTime += Time.fixedDeltaTime;
            ts.localScale = Vector3.Lerp(ShootingSize, originalSize, currentChargeTime / 0.3f);
            yield return new WaitForFixedUpdate();
        }

        ts.localScale = originalSize;
        isMoveable = true;
        isCharging = false;
        isShootable = true;
        currentChargeTime = 0f;
        yield return null;
    }

    protected void ShootingMove(Vector3 targetPos, float movespeed)//조준 상태에서의 움직임
    {
        if (currentHP <= 0) return;

        Vector2 dir = (targetPos - transform.position).normalized;
        float distanceToPlayer= Vector2.Distance(transform.position, targetPos);//플레이어와의 거리
        Vector2 correctionDir = Vector2.zero;
        if(distanceToPlayer>stopDistance + 1f)//플레이어보다 너무 먼경우 다가가자
        {
            correctionDir = dir * 0.5f;
        }
        else if(distanceToPlayer<stopDistance - 1f)//플레이어와 너무 가까운 경우 멀어지자
        {
            correctionDir = -dir * 0.5f;
        }

        if(RandMoveTimeCount<=0)
        {
            GetNewRandTime();
        }
        Vector2 perpendicularMoveDir = new Vector2(dir.y * RandNumToMove, -dir.x * RandNumToMove);

        Vector2 finalDir = (correctionDir+perpendicularMoveDir).normalized;//최종 이동 방향

        rb.AddForce(finalDir * movespeed);

        //이동
    }

    protected void GetNewRandTime()
    {
        RandMoveTimeCount = UnityEngine.Random.Range(3f,10f);
        RandNumToMove = (UnityEngine.Random.value > 0.5f) ? 1 : -1;
    }

    private void Fire()
    {
        GameObject bulletObj = GameManager.Instance.poolmanager.Get(9);

        EnemyBullet bullet = bulletObj.GetComponent<EnemyBullet>();
        if(bullet!=null)
        {
            bullet.Init(transform.position, transform.rotation);
        }
    }
}
