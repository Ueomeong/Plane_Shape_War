using UnityEngine;

public class BackGround2 : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float limit = 15f;
    public BackGround1 backGround1;
    private Transform tf;
    private void Awake()
    {
        tf = GetComponent<Transform>();
    }
    private void FixedUpdate()
    {
        if (backGround1 == null) return; // 에러 방지

        // 배경의 방향을 그대로 가져와 이동
        tf.position += backGround1.moveVector * moveSpeed * Time.fixedDeltaTime;

        // 화면 밖으로 나갔을 때 반대편에서 나타나게 함 (Pac-man 효과)
        Vector3 pos = tf.position;

        if (pos.x > limit) pos.x = -limit;
        else if (pos.x < -limit) pos.x = limit;

        if (pos.y > limit) pos.y = -limit;
        else if (pos.y < -limit) pos.y = limit;

        tf.position = pos;
    }
}
