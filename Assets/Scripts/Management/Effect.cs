using UnityEngine;
using System.Collections;
public class Effect : MonoBehaviour
{
    // 이펙트가 보여질 시간 (애니메이션 길이에 맞춰 조절하세요)
    public float activeTime = 3f;
    private ParticleSystem ps;
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {
        if (ps != null)
        {
            ps.Stop();
            ps.Play();
        }
        Invoke("Deactive", activeTime);

    }

    private void OnDisable()
    {
        // 비활성화될 때 Invoke가 남아있지 않도록 취소 (안전장치)
        CancelInvoke("Deactive");
    }

    void Deactive()
    {
        // 스스로를 비활성화하면 PoolManager가 나중에 다시 재사용할 수 있게 됨
        gameObject.SetActive(false);
    }
}
