using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class mousePointer : MonoBehaviour
{
    public float radius = 1f; // 플레이어 중심에서 화살표까지의 거리
    private Camera mainCamera;
    public Player_Move Player_Move;//import
    private SpriteRenderer render;
    void Start()
    {
        mainCamera = Camera.main;
        Player_Move=GetComponentInParent<Player_Move>();
        render=GetComponent<SpriteRenderer>();
       
    }

    void Update()
    {
        if (Player_Move == null) return;//오류 방지

        if (Player_Move.isCharging == false)//기본 조준 상태
        {
            // 1. 마우스 위치 가져오기
            render.color = new Color(1f, 1f, 0f, 0.5f);
            transform.localScale = new Vector3(0.7f, 0.3f, 1);
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, -mainCamera.transform.position.z));

            // 2. 플레이어(부모) 중심에서 마우스까지의 방향 벡터 계산
            // transform.parent.position은 플레이어의 위치입니다.
            Vector2 direction = (mouseWorldPos - transform.parent.position).normalized;

            // 3. 화살표의 위치 결정 (플레이어 중심 + 방향 * 거리)
            // 로컬 좌표계를 사용하면 부모를 기준으로 한 상대적 위치를 잡기 쉽습니다.
            transform.localPosition = direction * radius;

            // 4. 화살표의 회전 결정
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else//충전상태
        {
            render.color = new Color(1f, 0f, 0f, 0.5f);
            Vector3 shootingDir = Player_Move.RealShootingDir;
            float chargeRatio = Player_Move.currentChargeTime / Player_Move.maxChargeTime;
            transform.localScale = new Vector3(0.7f, 0.3f*(1f+chargeRatio * 3f), 1);
            transform.localPosition = shootingDir * radius;
            float angle = Mathf.Atan2(shootingDir.y, shootingDir.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
