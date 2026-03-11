using UnityEngine;
using System.Collections;
public class A_PushAllBullet : MonoBehaviour
{
    private PlayerData PlayerData => GameManager.Instance.runtimePlayerData;
    private Transform PullAllTransform;
    private Rigidbody2D rigid;
    private SpriteRenderer Render;
    [Header("Skill Settings")]
    public float range;         // 최종적으로 커질 최대 크기
    public float expandDuration;// 커지고 사라지는 데 걸리는 시간
    public float knockbackForce; // 넉백 힘
    public float damage => PlayerData.damage*1.5f; // 플레이어 데미지
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Render = GetComponent<SpriteRenderer>();
    }


    private void OnEnable()
    {
        if (InGameManager.Instance == null || InGameManager.Instance.player_move == null) return;
        PullAllTransform = GetComponent<Transform>();
        PullAllTransform.position = InGameManager.Instance.player_move.transform.position;
        PullAllTransform.localScale = Vector3.zero;
        if (Render != null)
        {
            Color c = Render.color;
            c.a = 1f;
            Render.color = c;
        }
        StartCoroutine(A_PushAllBulletShoot());
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("EnemySpawner"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                float originalForce = enemy.knockbackForce;
                enemy.knockbackForce = knockbackForce;
                enemy.TakeDamage(damage, pushDirection);
                enemy.knockbackForce = originalForce;
            }
        }
    }

    IEnumerator A_PushAllBulletShoot()//발사 되었을 시 진행될 로직
    {
        float timer = 0f;
        Vector3 targetScale = new Vector3(range, range, 1f);//최대 크기
        while(timer<expandDuration)
        {
            timer += Time.deltaTime;
            PullAllTransform.position = InGameManager.Instance.player_move.transform.position;
            float progress = timer / expandDuration;
            PullAllTransform.localScale = Vector3.Lerp(Vector3.zero, targetScale, progress);

            if(Render!=null)
            {
                Color c = Render.color;
                c.a = Mathf.Lerp(1f, 0f, progress);
                Render.color = c;
            }
            yield return null;
        }

        Deactive();
    }
    void Deactive()
    {
        rigid.linearVelocity = Vector2.zero;
        gameObject.SetActive(false);
    }
}
