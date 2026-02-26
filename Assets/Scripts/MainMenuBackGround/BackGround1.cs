using UnityEngine;

public class BackGround1 : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float limit = 15f;
    public Vector3 moveVector;

    private Transform tf;
    private void Awake()
    {
        tf = GetComponent<Transform>();
        moveVector = Random.insideUnitCircle.normalized;
    }
    private void FixedUpdate()
    {
        tf.position += moveVector * moveSpeed * Time.fixedDeltaTime;
        Vector3 pos = tf.position;
        bool bounced = false;

        if (Mathf.Abs(pos.x) > limit)
        {
            moveVector.x *= -1; // X축 반전
            bounced = true;
        }
        if (Mathf.Abs(pos.y) > limit)
        {
            moveVector.y *= -1; // Y축 반전
            bounced = true;
        }

        // 끼임 방지: 경계를 벗어났다면 즉시 위치를 한계치 안으로 고정
        if (bounced)
        {
            pos.x = Mathf.Clamp(pos.x, -limit, limit);
            pos.y = Mathf.Clamp(pos.y, -limit, limit);
            tf.position = pos;
        }
    }
}
