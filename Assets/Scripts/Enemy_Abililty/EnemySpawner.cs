using UnityEngine;

public class EnemySpawner : Enemy
{
    [Header("Spawner Settings")]
    [Tooltip("오브젝트 풀에서 가져올 적의 ID")]
    [SerializeField] private int enemyPoolId;
    [SerializeField] private float spawnInterval; // 적 생성 주기
    [SerializeField] private int maxSpawnCount;    // 최대 생성량
    [SerializeField] private float spawnRadiusf; // 스폰될 때 스포너 주변 반경
    [SerializeField] private float ShootingForce; //스폰시 던지는 힘
    [SerializeField] private float initDropMoney;// 돈 드롭 양
    [SerializeField] private float initDropEXP; //exp드롭 양
    private int currentSpawnCount;
    private float spawnTimer;
    private bool isDepleted; // 생성량을 모두 소모했는지 여부
    [Header("state Outline")]
    public SpriteRenderer StateOutLine;
    private Color FullChargedColor;
    private Color NoneChargedColor;

    protected override void OnEnable()
    {
        base.OnEnable();
        drop_Money = initDropMoney;
        drop_EXP = initDropEXP;
        FullChargedColor = new Color(1f, 1f, 1f);
        NoneChargedColor = new Color(0, 0, 0);
       // 스포너 기본 설정 덮어쓰기
       isKnockbackable = false; // 넉백 무시
        attackDamage = 0;        // 공격력 0 (혹시 모를 예외 방지)
        moveSpeed = 0;//이속도 0
        // 스폰 관련 변수 초기화
        currentSpawnCount = maxSpawnCount;
        spawnTimer = 0f;
        isDepleted = false;
    }

    protected override void FixedUpdate()
    {
        // 게임 오버 상태이거나, 죽었거나, 생성량을 다 소모했다면 작동 중지
        if (!InGameManager.Instance.isLive || currentHP <= 0 || isDepleted) return;

        // 부모 클래스의 탐지 로직 실행 
        // (플레이어가 detectRange 안에 들어오면 isChasingPlayer가 true로 바뀜)
        DetectPlayer();

        // 외곽선 색상 변경 효과는 부모의 것을 그대로 사용 (선택 사항)
        if (outlineSpriteRenderer != null)
        {
            outlineSpriteRenderer.color = Color.Lerp(outlineColor, playerChasingColor, willToChase / 3);
        }
        if(StateOutLine!=null)
        {
            StateOutLine.color = Color.Lerp(FullChargedColor, NoneChargedColor,spawnTimer/spawnInterval);
        }

        // 플레이어를 감지했을 때만 적을 스폰!
        if (isChasingPlayer)
        {
            spawnTimer += Time.fixedDeltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnEnemy();
                spawnTimer = 0f;
            }
        }
        else
        {
            spawnTimer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        if (currentSpawnCount <= 0) return;

        // 풀매니저를 통해 적 생성
        GameObject enemyObj = InGameManager.Instance.poolmanager.Get(enemyPoolId);
        if (enemyObj != null)
        {
            // 1. 스포너의 위치에 적 생성
            enemyObj.transform.position = transform.position;

            // 2. 플레이어를 향해 발사!
            if (enemyObj.TryGetComponent<Rigidbody2D>(out Rigidbody2D erb))
            {
                // 스포너에서 플레이어(targetPosition)를 향하는 방향 벡터 계산
                Vector2 shootDirection = (targetPosition - transform.position).normalized;

                // 해당 방향으로 힘을 가함
                erb.AddForce(shootDirection * ShootingForce, ForceMode2D.Impulse);
            }
        }

        currentSpawnCount--;

        // 최대 생성량을 모두 소모했을 때의 처리
        if (currentSpawnCount <= 0)
        {
            HandleDepletion();
        }
    }
    private void HandleDepletion()
    {
        isDepleted = true;

        // 기능을 상실했다는 시각적 피드백 (어둡게 만들기)
        if (spriteRender != null)
        {
            HpColor = Color.gray; // 데미지를 입었을 때도 회색을 유지하도록 베이스 컬러 변경
            spriteRender.color = Color.gray;
        }
    }

    protected override void DropLoot()
    {
        if (isDepleted)
        {
            // 이미 다 스폰했다면 아무것도 드롭하지 않음
            drop_Money = 0;
            drop_EXP = 0;
        }
        else
        {
            // 스포너를 파괴했을 때, 남은 스폰 횟수에 비례하여 재화 드롭량 증가
            // (예: 기본 드롭량 + (기본 드롭량 * 남은 횟수))
            drop_Money = drop_Money + (drop_Money * currentSpawnCount);
            drop_EXP = drop_EXP + (drop_EXP * currentSpawnCount);
        }

        // 재화 드롭량을 재계산한 후 부모의 드롭 로직 실행
        base.DropLoot();
    }

    // 부모의 충돌 로직을 빈 함수로 덮어써서 플레이어에게 피해를 주지 않게 함
    protected override void OnCollisionStay2D(Collision2D collision)
    {
        // 아무것도 하지 않음! (피해를 주거나 밀어내지 않습니다)
    }
}
