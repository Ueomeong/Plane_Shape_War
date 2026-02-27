//플레이어를 바라본 후, 그대로 벽에 박을때까지 직진
using System.Collections;
using UnityEngine;

public class Triangle : Enemy
{
    [Header("Triangle Specifics")]
    public float maxChargeTime = 1.0f; //돌진준비/충전 시간
    public float attackCoolTime = 1.0f; //돌진후 쿨타임, 잠깐 쉬는 시간
    public float currentChargeTime=0.0f;
    public float maxDashingTime = 3.0f;//최대 돌진 시간
    private Color dashingOutlineColor = Color.blue;//돌진시 외곽선 색
    private Vector2 dashDirection;//돌진 방향
    private float dashSpeed=150.0f;//돌진 속도!
    //private bool isTargetLocked;//목표를 찾았는가?
    private bool isDashing;//돌진 중 인가?
    private bool isCharging;//돌진을 위한 충전 중 인가?

    [Header("Attack Player!")]//플레이어 무적시간 추가로 안쓸꺼임
    [SerializeField] protected float attackCooldown;
    protected float attackTime;
    protected override void OnEnable()
    {
        base.OnEnable();
        //isTargetLocked=false;
        isDashing=false;
        isCharging=false;
        isKnockbackable = true;
        rb.mass = 1f;
    }
    protected override void FixedUpdate()//오버라이딩 가능 함수 이동 제어
    {
        if (isCharging || isDashing) return;
        base.FixedUpdate();
    }
    protected override void DetectPlayer()//오버라이딩 가능 함수 플레이어 탐색
    {
        if (playerTransform == null || isCharging || isDashing) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectRange)//감지 범위 안에 들어왓다
        {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectRange, detectionLayer);

            if (hit.collider != null && hit.collider.CompareTag("Player"))//플레이어 발견
            {
                willToChase = 5;
                isChasingPlayer = true;
                targetPosition = playerTransform.position;
                if(attackCooldown<=0f)//공격 가능 쿨타임
                {
                    StartCoroutine(ChargeAttackSequence());//돌진 공격 과정 시행
                }
            }
        }
        else
        {
            if (willToChase <= 0)
            {
                isChasingPlayer = false;
            }
            else
            {
                willToChase -= Time.fixedDeltaTime;
            }
        }

        if(attackCooldown>0)
        {
            attackCooldown-= Time.fixedDeltaTime;
        }
    }

    private IEnumerator ChargeAttackSequence()//돌진 공격 과정 1. 1초간 충전하면서 플레이어 방향으로 rotate함. 2. 마지막 방향으로 벽에 닿을때 까지 돌진
    {
        isCharging = true;
        currentChargeTime = 0f;
        isKnockbackable = false;
        rb.constraints = RigidbodyConstraints2D.None;
        while (currentChargeTime<=maxChargeTime)//충전중!
        {
            currentChargeTime += Time.fixedDeltaTime;

            Vector2 dir = (playerTransform.position - transform.position).normalized;//플레이어 위치와 tri 의 방향 추출
            float angle = (Mathf.Atan2(dir.y, dir.x) * (Mathf.Rad2Deg))+30;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), currentChargeTime / maxChargeTime);
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 0.1f);

            outlineSpriteRenderer.color = Color.Lerp(outlineColor, dashingOutlineColor,currentChargeTime/maxChargeTime);
            dashDirection = dir;
            yield return new WaitForFixedUpdate();
        }
        //충전 끝! 돌진!
        isCharging =false;
        isDashing = true;
        outlineSpriteRenderer.color = dashingOutlineColor;
        rb.mass = 10f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.linearDamping = 0f;
        rb.AddForce(dashDirection*dashSpeed, ForceMode2D.Impulse);
       
        yield return null;
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing)
        {
            if (collision.gameObject.CompareTag("Wall")) 
            {
                StopDashing();
                attackCooldown = 1f;
            }
            if(collision.gameObject.CompareTag("Player"))
            {
                StopDashing();
                attackCooldown = 1f;
            }
        }
    }

    private void StopDashing()//멈추기
    {
        StopCoroutine(ChargeAttackSequence());
        isDashing=false;
        InGameManager.Instance.camerashaking.ShakeCamera(0.7f, 0.1f);
        rb.linearVelocity= Vector2.zero;
        rb.linearDamping = 5f;//다시 감속받음
        isKnockbackable = true;
        rb.mass = 1f;
    }
    protected override void MoveToTarget(Vector3 targetPos, float movespeed)//추적모드: 돌입 후 1~~2초쯤 기다린후 플레이어의 발견위치 방향를 향해서 빠르게 돌진.
    {
        if (isCharging || isDashing) return;
        base.MoveToTarget(targetPos, movespeed);
    }

    
}
