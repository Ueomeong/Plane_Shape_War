using UnityEngine;

public class Bullet : MonoBehaviour
{
    private PlayerData PlayerData => GameManager.Instance.runtimePlayerData;
    private Transform bullet_transform;
    private Transform mousepointer_transform;
    private Rigidbody2D rigid;
    private BoxCollider2D collid;
    public float speed = 30.0f;
    public GameObject pixelHitEffect;
    public float damage=> PlayerData.damage;// 데미지
    public int totalPer => PlayerData.per;//관통가능 적
    private int per;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }


    private void OnEnable()
    {
        if (InGameManager.Instance == null || InGameManager.Instance.player_move == null) return;
        
        bullet_transform = GetComponent<Transform>();
        mousepointer_transform = InGameManager.Instance.player_move.GetComponent<Transform>();

        bullet_transform.position = InGameManager.Instance.mousepointer.transform.position;
        bullet_transform.rotation = InGameManager.Instance.mousepointer.transform.rotation;
        
        rigid.linearVelocity = transform.up * speed;
        per = totalPer;
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //일단 충돌하면 사라지게
        if (collision.CompareTag("Ignore")) return;
        if(pixelHitEffect != null)
        {
            GameObject effectHit = InGameManager.Instance.poolmanager.Get(1);//총알 이펙트
            if(effectHit != null)
            {
                Vector2 hitPoint = collision.ClosestPoint(transform.position);
                effectHit.transform.position = hitPoint;
                effectHit.transform.rotation = transform.rotation;
            }
        }
        if (collision.CompareTag("Enemy"))
        {

            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {

                enemy.TakeDamage(damage, transform.up);
                per -= 1;
                if (per <= 0)
                {
                    Deactive();
                }
            }
        }
        else
        {
            Deactive();
        }
    }
    void Deactive()
    {
        rigid.linearVelocity = Vector2.zero;
        gameObject.SetActive(false);
    }
}
