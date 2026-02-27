using System.Collections;
using UnityEngine;

public class Hexagon : Enemy
{
    [SerializeField]public SpriteRenderer shieldRenderer;
    [SerializeField]private bool isShieldOn;
    [SerializeField] private float maxShieldHP;
    [SerializeField] private float currentShieldHP;
    [SerializeField] private float shieldDisplayTime;
    public float elapsedTime = 0f;
    private Coroutine shieldEffectCoroutine;
    protected override void OnEnable()
    {
        HpColor = Color.black;
        currentHP = maxHP;
        attackDamage = AttackDamage;
        isChasingPlayer = false;
        willToChase = 0;
        isWaiting = false;
        if (spriteRender != null)
        {
            spriteRender.transform.localScale = originalScale;//일단 기본 크기로 하고, 나중에 체력에 따른 크기를 조절하자.
            spriteRender.color = HpColor;
        }
        SetNewReconTargetPosition();
        isShieldOn = true;
        currentShieldHP = maxShieldHP;
        shieldRenderer.color = new Color(0f,0.95f,1f,0f);
    }
    public override void TakeDamage(float damage, Vector3 hitDirection)
    {
        //맞았을대 발끈!
        willToChase = 5;
        isChasingPlayer = true;
        targetPosition = playerTransform.position;
        //
        if (!isShieldOn)//쉴드가 없다면.
        {
            knockbackForce = 5.0f;
            currentHP -= damage;
        }
        else//쉴드가 있다면
        {
            knockbackForce = 1.5f;
            currentShieldHP-= damage;//총알의 피해도 막지만, 박치기를 했을때 대량의 데미지를 입게 해보자.
            if(currentShieldHP<=0)
            {
                isShieldOn = false;
                if (shieldEffectCoroutine != null)
                {
                    StopCoroutine(shieldEffectCoroutine);
                    shieldEffectCoroutine = null;
                }

                // 쉴드 이미지 끄기
                if (shieldRenderer != null)
                {
                    Color c = shieldRenderer.color;
                    c.a = 0f;
                    shieldRenderer.color = c;
                }

                // 파괴 이펙트
                GameObject eff = InGameManager.Instance.poolmanager.Get(3);
                if (eff != null) eff.transform.position = transform.position;
            }
            else
            {
                if(shieldEffectCoroutine != null)
                { StopCoroutine(shieldEffectCoroutine); }

                elapsedTime = 0f;
                shieldEffectCoroutine=StartCoroutine(ShieldAbsorbDamage());
            }
               
        }
        HpColor = Color.Lerp(Color.white, Color.black, currentHP / maxHP);
        InGameManager.Instance.camerashaking.ShakeCamera(0.7f, 0.1f);
        if (gameObject.activeSelf && !isShieldOn)//총알을 맞았을때, 맞은 방향으로 살짝 수축하게 해보자, 쉴드가 없어야함!
        {
            StartCoroutine(EnemyMozzi(originalScale, hitDirection));
        }
        if (currentHP <= 0)
        {
            GameManager.Instance.temporaryMoney += moneyDrop;
            Die();
        }
        KnockBack(hitDirection);
    }

    private IEnumerator ShieldAbsorbDamage()
    {
        if (shieldRenderer == null) { yield break; }

        Color color = shieldRenderer.color;
        float startAlpha = (currentShieldHP / maxShieldHP)+0.2f;
        color.a = startAlpha;
        shieldRenderer.color = color;

        

        while(elapsedTime < shieldDisplayTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / shieldDisplayTime;
            float currentAlpha = Mathf.Lerp(startAlpha, 0,t);
            color.a = currentAlpha;
            shieldRenderer.color = color;
            yield return null;
        }
        color.a = 0;

        shieldRenderer.color = color;
    }

}
