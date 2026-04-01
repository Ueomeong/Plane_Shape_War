using UnityEngine;
using System.Collections;
public class A_SPtoDamage : ActiveAugment
{
    [SerializeField] float damageAmount = 0.5f;//버프 증가 배율 ex) 0.5f면 공격력이 50% 증가
    [SerializeField] float buffDuration = 5f;//버프 지속시간
    [SerializeField] float buffTimer;
    private float tempDamage;// 증가할 공격력 값
    private Coroutine buffCoroutine;//버프 타이머 코루틴 참조

    private GameObject auraEffect;// 버프 이펙트 오브젝트;

    private void Awake()
    {
        // 시작할 때 내 자식들 중에서 "DamageAura"라는 이름을 가진 녀석을 찾아서 저장해둡니다.
        Transform auraTransform = transform.Find("DamageAura");
        if (auraTransform != null)
        {
            auraEffect = auraTransform.gameObject;
        }
    }

    public override void ExecuteSkill()
    {
        if (buffCoroutine != null)
        {
            // 기존 버프 타이머를 정지시킵니다.
            StopCoroutine(buffCoroutine);

            // 기존에 올려줬던 데미지를 일단 원상복구 시킵니다. (중첩 방지)
            PlayerData.damage -= tempDamage;
        }

        buffCoroutine = StartCoroutine(BuffSequence());
    }

    private IEnumerator BuffSequence()
    {
        tempDamage = PlayerData.damage * damageAmount;
        PlayerData.damage += tempDamage;
        buffTimer = buffDuration;
        while (buffTimer>=0f)
        {
            buffTimer -= Time.deltaTime;
            yield return null;
        }

        PlayerData.damage -= tempDamage;
        buffCoroutine = null;
    }

    public override void OnDisable()
    {
        if (buffCoroutine != null)
        {
            StopCoroutine(buffCoroutine);
            PlayerData.damage -= tempDamage;
            buffCoroutine = null;
        }
    }
}
