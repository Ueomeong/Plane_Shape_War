using UnityEngine;
public class Diamond_LaserPoint : MonoBehaviour
{
    SpriteRenderer sr;
    BoxCollider2D bc;
    public LayerMask layerMask;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();

        // 코드로 Draw Mode를 강제할 수도 있습니다.
        sr.drawMode = SpriteDrawMode.Sliced;
    }

    private void FixedUpdate()
    {
        // 1. 레이저 쏘기 (마스크에 Player 추가 권장)
       
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.up, 100f, layerMask);

        // 2. 거리 계산
        float distanceLazer = (ray.collider != null) ? ray.distance : 100f;

        // 3. 스프라이트 길이 조정 (Pivot이 Bottom이라 위로만 커짐)
        sr.size = new Vector2(sr.size.x, distanceLazer);

        // [중요] 박스 콜라이더 조정
        // 크기는 레이저 길이만큼 설정
        bc.size = new Vector2(bc.size.x, distanceLazer);

        // 위치(Offset)를 길이의 절반만큼 위(+Y)로 이동시켜
        // 아래쪽 끝이 0(발사 위치)에 고정된 것처럼 보이게 함
        bc.offset = new Vector2(0, distanceLazer / 2f);

        //
        Debug.DrawRay(transform.position, transform.up * distanceLazer, Color.red);

        // 2. 만약 무언가에 맞았다면 콘솔에 이름 출력 (디버깅용)
        if (ray.collider != null)
        {
            Debug.Log("레이저가 맞은 것: " + ray.collider.name);
        }
    }
}