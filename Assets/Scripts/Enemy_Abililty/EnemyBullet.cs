using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Transform bulletTransform;
    private Transform EnemyTransform;
    private Rigidbody2D rigid;
    private BoxCollider2D collid;
    public float speed = 15.0f;
    public GameObject pixelHitEffect;
    public float damage = 1f;// 데미지
                                //public int per 관통?

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        
    }
    public void Init(Vector3 position,Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

        Vector2 fireDirection = transform.right;
        rigid.linearVelocity = fireDirection * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //일단 충돌하면 사라지게
        if (collision.CompareTag("Enemy")| collision.CompareTag("Ignore")) return;
        if (pixelHitEffect != null)
        {
            GameObject effectHit = GameManager.Instance.poolmanager.Get(10);//총알 이펙트
            if (effectHit != null)
            {
                Vector2 hitPoint = collision.ClosestPoint(transform.position);
                effectHit.transform.position = hitPoint;
                effectHit.transform.rotation = transform.rotation;
            }
        }
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.player_state.TakeDamage(1);

        }
        rigid.linearVelocity = Vector2.zero;
        Deactive();
    }
    void Deactive()
    {
        gameObject.SetActive(false);
    }
}
