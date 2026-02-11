using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.LowLevelPhysics2D.PhysicsShape;
public class Player_Move : MonoBehaviour
{
    public PlayerData PlayerData;
    [Header("Componenets")]
    public Rigidbody2D rigid;//플레이어의 rigidbody2D
    public SpriteRenderer Player_renderer;//플레이어 렌더러
    public Transform Player_transform;//플레이어 크기 조절하기 위해서

    [Header("Basic Movement Settings")]
    public Vector2 inputVec;//input방향 벡터로 저장
    public Vector2 currentVelocity;//?
    public float moveSpeed => PlayerData.moveSpeed;
    public float smoothTime = 0.2f;
    [Header("Charge Movement Settings")]
    public Vector2 ShootingDir;//첫 발사될 방향 저장

    public Vector2 RealShootingDir;//실제 발사 방향
    public float maxAngleDegree = 50f;//충전중 방향전환의 한계치
    public float ShootingSpeed;//발사될 힘?가속의 크기
    public Boolean isCharging=false;//충전중인가
    public float maxShooingForce => PlayerData.maxShooingForce;
    public float minShooingForce =>PlayerData.minShooingForce;
    public float maxChargeTime => PlayerData.maxChargeTime;
    public float currentChargeTime = 0.0f;
    public float ShootingTime = 0.0f;//감속을 받지 않고 날라가는 시간
    public Boolean isShooting =false;//돌진 중 인가?
    public float last_chargedTime = 0.0f;//마지막에 충전했던 시간
    public Color colorOnShoot;

    public float offsetMultiplier = 0.5f;//크기에 따른 벽에 붙이기 위한 값
    public Transform Visual_transform;
    private Vector3 scaleOnShoot;//발사 순간의 크기


    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        Player_transform = GetComponent<Transform>();//미사용
        Player_renderer =GetComponentInChildren<SpriteRenderer>();
        Visual_transform = Player_renderer.transform;
    }
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    private void FixedUpdate()//기본 이동 & shoot 호출
    {
        if (!GameManager.Instance.isLive) { return; }
        if (ShootingTime > 0)//쏘는 동안 발동됨
        {
            
            ShootingTime -= Time.deltaTime;

            float t = 1.0f - (ShootingTime/ last_chargedTime);
            Player_renderer.color = Color.Lerp(colorOnShoot, Color.white, t);
            mochi_shoot(t);
            //colorOnShoot= Player_renderer.color;
            return;
        }
        else
        {
            isShooting=false;
        }
        if (isCharging)//충전 로직
        {
            rigid.linearVelocity = Vector2.zero;//일단 정지

            ///마우스 방향
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
            Vector2 mouseDir = ((Vector2)mouseWorldPos - (Vector2)transform.position).normalized;

            float angle = Vector2.SignedAngle(ShootingDir, mouseDir);
            angle = Mathf.Clamp(angle, -maxAngleDegree, maxAngleDegree);
            RealShootingDir = Quaternion.Euler(0, 0, angle) * ShootingDir;
            ///

            if (Vector2.Dot(inputVec, ShootingDir) < -0.5f)//dot product값
            {
                currentChargeTime += Time.fixedDeltaTime;//충전
                Player_renderer.color = Color.Lerp(Color.white, Color.orange, currentChargeTime / maxChargeTime);//색바꾸기
                colorOnShoot = Player_renderer.color;
                if (currentChargeTime > maxChargeTime)
                {
                    currentChargeTime = maxChargeTime;//최고 충전시간 1.5초임
                }
                last_chargedTime = currentChargeTime;
                mochi(currentChargeTime / maxChargeTime);
            }
            else
            {
                Shoot();
            }
        }
        else
        {
            ShootingDir = Vector2.zero;
            Visual_transform.localScale = Vector3.one;
            Vector2 targetVelocity = inputVec * moveSpeed;
            rigid.linearVelocity = Vector2.SmoothDamp(rigid.linearVelocity, targetVelocity, ref currentVelocity, smoothTime);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)//충돌 판정
    {
        if (isCharging) { return; }
        ContactPoint2D contact = collision.GetContact(0);
        if (Vector2.Dot(inputVec, contact.normal) < -0.7f)
        {
            isCharging = true;
            ShootingDir = contact.normal; // 발사될 방향 저장

        }
    }
    private void Shoot()
    {
        //발사!
        isShooting = true;
        float ChargeRatio = currentChargeTime / maxChargeTime;
        float finalShootingForce = Mathf.Lerp(minShooingForce, maxShooingForce, ChargeRatio);//최종힘
        rigid.AddForce(RealShootingDir*finalShootingForce, ForceMode2D.Impulse);
        ShootingTime = (Mathf.Lerp(1, 10, ChargeRatio))/15;
        //비충전 상태로 전환
        isCharging =false;
        currentChargeTime = 0.0f;
        
    }
    private void mochi(float ratio) {
        float squash = Mathf.Lerp(1.0f, 0.6f, ratio);//눌리는 
        float stretch = Mathf.Lerp(1.0f, 1.3f, ratio);//늘어나는
        Vector3 targetScale=Vector3.one;

        if (Mathf.Abs(ShootingDir.x) > Mathf.Abs(ShootingDir.y))
        {
           targetScale.x= squash;
           targetScale.y= stretch;
        }
        else
        {
            targetScale.y = squash;
            targetScale.x = stretch;
        }
        //빈공간 없애기
        float offset = (1.0f - squash) * offsetMultiplier;//비는 공간의 크기
        Vector3 targetPos = -ShootingDir * offset;
        
        Visual_transform.localPosition = targetPos;

        Visual_transform.localScale = targetScale;
    }
    private void mochi_shoot(float ratio)
    {
        float squash = Mathf.Lerp(0.4f, 1.0f, ratio);//눌리는 
        float stretch = Mathf.Lerp(1.6f, 1.0f, ratio);//늘어나는
        Vector3 targetScale = Vector3.one;
        if (Mathf.Abs(ShootingDir.x) > Mathf.Abs(ShootingDir.y))
        {
            targetScale.x = stretch;
            targetScale.y = squash;
        }
        else
        {
            targetScale.x = squash;
            targetScale.y = stretch;
        }

        Visual_transform.localPosition = Vector3.zero;
        Visual_transform.localScale = targetScale;
        
    }
}