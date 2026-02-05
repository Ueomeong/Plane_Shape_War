using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform bullet_transform;
    private Transform mousepointer_transform;
    private Rigidbody2D rigid;
    private BoxCollider2D collid;
    public float speed = 30.0f;
    public GameObject pixelHitEffect;
    public float damage=10.0f;// 데미지
    //public int per 관통?
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        if (GameManager.Instance == null || GameManager.Instance.player_move == null) return;

        bullet_transform = GetComponent<Transform>();
        mousepointer_transform = GameManager.Instance.player_move.GetComponent<Transform>();

        bullet_transform.position = GameManager.Instance.mousepointer.transform.position;
        bullet_transform.rotation = GameManager.Instance.mousepointer.transform.rotation;

        rigid.linearVelocity = transform.up * speed;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //일단 충돌하면 사라지게
        if (collision.CompareTag("Ignore")) return;
        if(pixelHitEffect != null)
        {
            GameObject effectHit = GameManager.Instance.poolmanager.Get(1);//총알 이펙트
            if(effectHit != null)
            {
                Vector2 hitPoint = collision.ClosestPoint(transform.position);
                effectHit.transform.position = hitPoint;
                effectHit.transform.rotation = transform.rotation;
            }
        }
        if(collision.CompareTag("Enemy"))
        {

            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy!=null)
            {
 
                enemy.TakeDamage(damage,transform.up);
            }
        }
        rigid.linearVelocity = Vector2.zero;
        Deactive();
    }
    void Deactive()
    {
        gameObject.SetActive(false);
    }
}
