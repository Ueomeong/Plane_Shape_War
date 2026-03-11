using UnityEngine;

public class A_PushAllBullet : MonoBehaviour
{
    private PlayerData PlayerData => GameManager.Instance.runtimePlayerData;
    private Transform A_PushAlltransform;
    private Rigidbody2D rigid;
    private BoxCollider2D collid;
    public float speed = 5.0f;
    public float damage => PlayerData.damage;// µ¥¹ÌÁö
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }


    private void OnEnable()
    {
        if (InGameManager.Instance == null || InGameManager.Instance.player_move == null) return;
        A_PushAlltransform = GetComponent<Transform>();

        A_PushAlltransform.position = InGameManager.Instance.mousepointer.transform.position;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {

            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {

                enemy.TakeDamage(damage, transform.up);
            }
        }
        else if (collision.CompareTag("EnemySpawner"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, transform.up);
            }
        }
    }
    void Deactive()
    {
        rigid.linearVelocity = Vector2.zero;
        gameObject.SetActive(false);
    }
}
