using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System;
public class Enemy : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] protected float maxHP;
    [SerializeField] protected float detectRange;
    [SerializeField] protected float AttackDamage;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int moneyDrop;//죽으면 떨어뜨릴 돈의 양
    [SerializeField] protected float willToChase;//플레이어가 근처에 있지 않으면 감소하고 0이되면 더이상 플레이어를 쫓지 않을것
    [SerializeField] protected float reconRange;//정찰 범위
    [SerializeField] protected bool isKnockbackable = true;

    
    public float currentHP;//0이면 사망 판정
    public float attackDamage;//플레이어에게 입힐 피해
    public bool isChasingPlayer;//0이면 정찰모드, 1이면 추적모드
   
    [Header("Knockback Settings")]
    [SerializeField] protected float knockbackForce = 5f; // 기본 밀려나는 힘
    [SerializeField] protected float knockbackTime = 0.1f; // 넉백 되어있는 시간(AI 정지용)
    [Header("Detection Settings")]
    protected Vector3 targetPosition;//정찰중:무작위 위치 ,추적중:플레이어의 위치를 target으로 하자!
    public LayerMask detectionLayer;
    protected Transform playerTransform;

    protected Rigidbody2D rb;
    [SerializeField ]protected SpriteRenderer outlineSpriteRenderer;//외곽선 바꾸기
    protected SpriteRenderer spriteRender;
    protected Vector3 originalScale;
    protected Vector3 spawnPosition;
    protected Coroutine hitRoutine;//중복실행 방지? 할까말까..아직 미사용
    protected Color HpColor;
    protected Color outlineColor;
    protected Color playerChasingColor;
    [Header("Drop ITEMS")]
    [SerializeField] protected float drop_EXP;
    [SerializeField] protected float drop_Money;
    [SerializeField] protected float drop_Items;
    [Header("waiting?")]
    protected bool isWaiting;
    protected float currentWaittime;

    public virtual void Awake()
    {
        spriteRender = GetComponentInChildren<SpriteRenderer>();
        originalScale=spriteRender.transform.localScale;
        rb=GetComponent<Rigidbody2D>();
        HpColor = Color.black;
        outlineColor = Color.black;//원본 색
        playerChasingColor = Color.darkGreen;//추적중일때의 색
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if(playerObj!=null)
        {
            playerTransform = playerObj.transform;
            spawnPosition = transform.localPosition;
        }
    }
    protected virtual void OnEnable()
    {
        HpColor = Color.black;
        currentHP = maxHP;
        attackDamage = AttackDamage;
        isChasingPlayer = false;
        willToChase = 0;
        isWaiting = false;
        if (spriteRender != null)
        {
            spriteRender.transform.localScale = originalScale;//일단 기본 크기로 하고, 나중에 체력에 따른 크기를 조절하자.?
            spriteRender.color = HpColor;
        }
        SetNewReconTargetPosition();
    }
    protected virtual void DetectPlayer()//오버라이딩 가능 함수 플레이어 탐색
    {
        if (playerTransform == null) return;
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectRange)//감지 범위 안에 들어왓다
        {
            Vector2 directionToPlayer = (playerTransform.position-transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer,detectRange,detectionLayer);

            if(hit.collider != null && hit.collider.CompareTag("Player"))
            {
                willToChase = 3;
                isChasingPlayer =true;
                targetPosition = playerTransform.position;
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
    }
    protected virtual void FixedUpdate()//오버라이딩 가능 함수 이동 제어
    {
        if (!InGameManager.Instance.isLive) { return; }
        DetectPlayer();
        outlineSpriteRenderer.color = Color.Lerp(outlineColor,playerChasingColor,willToChase/3);
        if(isChasingPlayer)
        {
            isWaiting = false;
            MoveToTarget(targetPosition,moveSpeed);
        }
        else
        {
            Recon();
        }
    }

    protected virtual void MoveToTarget(Vector3 targetPos,float movespeed)//추적모드 이동
    {
        if (currentHP <= 0) return;
        Vector2 direction = (targetPos - transform.position).normalized;
        rb.AddForce(direction * movespeed);
    }
    protected virtual void Recon()//정찰모드 이동
    {
        if (currentHP <= 0) return;

        if (Vector2.Distance(transform.position, targetPosition) < 0.3f)
        {
            if (!isWaiting)
            {
                currentWaittime = 1f;
            }
            isWaiting = true;//새로운 정찰 지역 찾자
            
        }
        if (isWaiting)
        {
            currentWaittime -= Time.fixedDeltaTime;
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 0.1f);
            if (currentWaittime<=0)
            {
                SetNewReconTargetPosition();
                isWaiting =false;
            }
        }
        else { MoveToTarget(targetPosition, moveSpeed / 2); }
            
    }

    protected void SetNewReconTargetPosition()
    {
        Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * reconRange;
        targetPosition = spawnPosition + (Vector3)randomPoint;
    }






    public virtual void TakeDamage(float damage,Vector3 hitDirection)
    {
        //맞았을대 발끈!
        willToChase = 5;
        isChasingPlayer = true;
        targetPosition = playerTransform.position;
        //
        currentHP -= damage;
        HpColor = Color.Lerp(Color.white, Color.black, currentHP / maxHP);
        InGameManager.Instance.camerashaking.ShakeCamera(0.7f, 0.1f);
        if (gameObject.activeSelf)//총알을 맞았을때, 맞은 방향으로 살짝 수축하게 해보자
        {
            StartCoroutine(EnemyMozzi(originalScale, hitDirection));
        }
        if (currentHP <= 0)
        {
            GameManager.Instance.temporaryMoney += moneyDrop;
            Die();
        }
        KnockBack(hitDirection);
    }

    public virtual void Die()//죽을때 자폭하는 기능을 넣을 수도 있기에 오버라이딩 가능으로 선언함
    {
        GameObject eff = InGameManager.Instance.poolmanager.Get(2);
        InGameManager.Instance.camerashaking.ShakeCamera(3f,0.15f);
        DropLoot();
        if (eff != null)
        {
        eff.transform.position = transform.position;
        }
        gameObject.SetActive(false);
    }

    protected virtual void DropLoot()
    {
        for(int i=0; i<drop_Money;i++)
        {
            GameObject DropObj = InGameManager.Instance.poolmanager.Get(12);
            if(DropObj!=null)
            {
                DropObj.transform.position = transform.position;
                if (DropObj.TryGetComponent<LootItem>(out var loot))
                {
                    // 가치는 1로 설정 (필요시 조절), spawnPosition은 현재 위치, 플레이어 트랜스폼 전달
                    loot.Init(1, transform.position, playerTransform);
                }
            }
        }
        for (int i = 0; i < drop_EXP; i++)
        {
            GameObject DropObj = InGameManager.Instance.poolmanager.Get(13);
            if (DropObj != null)
            {
                DropObj.transform.position = transform.position;
                if(DropObj.TryGetComponent<LootItem>(out var loot))
                {
                    loot.Init(1, transform.position, playerTransform);
                }
            }
        }
    }

    public IEnumerator EnemyMozzi(Vector3 originalSize,Vector3 hitDir)
    {
        if (spriteRender == null)
        {
            yield break;
        }
        spriteRender.color = HpColor * 0.5f;
        Vector3 normalizedHitDir = transform.InverseTransformDirection(hitDir).normalized;
        Vector3 targetScale = originalSize;
        float squashPower = 0.3f;

        targetScale.x -= Mathf.Abs(normalizedHitDir.x) * squashPower * originalSize.x;
        targetScale.y -= Mathf.Abs(normalizedHitDir.y) * squashPower * originalSize.y;
        //찌그러지기
        float duration = 0.05f;
        float elapsed = 0f;
        Vector3 startScale = spriteRender.transform.localScale; // 현재 크기에서 시작

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            spriteRender.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            
            yield return null;
        }
    
        spriteRender.transform.localScale = targetScale;
        //펴지기
        duration = 0.05f;
        elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            spriteRender.transform.localScale = Vector3.Lerp(targetScale, originalSize, elapsed / duration);
            spriteRender.color = Color.Lerp(Color.white, HpColor, elapsed / duration);
            yield return null;
        }

        spriteRender.color = HpColor;
        spriteRender.transform.localScale= originalSize;
    }
    public virtual void KnockBack(Vector3 hitDir)
    {
        if (rb == null | !isKnockbackable) return;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(hitDir.normalized * knockbackForce, ForceMode2D.Impulse);
    }

    /// <summary>

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            InGameManager.Instance.player_state.TakeDamage(1);

            // (선택 사항) 플레이어를 밀어내는 물리적 연출을 더하고 싶다면
            Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
            collision.rigidbody.AddForce(pushDirection * 1f, ForceMode2D.Impulse);
        }
    }
}
