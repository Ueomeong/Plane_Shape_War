using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Transform bulletTransform;
    private Transform EnemyTransform;
    private Rigidbody2D rigid;
    private BoxCollider2D collid;
    public float speed = 17.0f;
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
}
