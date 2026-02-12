using UnityEngine;

public enum AugmentTier { Silver, Gold, Platinum }

[CreateAssetMenu(fileName = "New Augment", menuName = "Scriptable Objects/AugmentData")]
public class AugmentData : ScriptableObject
{
    [Header("Basic Info")]
    public string augmentName;
    [TextArea] public string description;
    public Sprite icon;
    public AugmentTier tier;

    [Space(10)]
    [Header("--- Stat Modifiers (0 = No Change) ---")]

    [Header("HP & SP")]
    public int stat_MaxHP;       // 최대 체력 증가량
    public int stat_CurrentHP;   // 즉시 회복량 (또는 감소량)
    public int stat_MaxSP;       // 최대 SP 증가량
    public int stat_CurrentSP;   // SP 즉시 회복량

    [Header("Defensive")]
    public float stat_InvincibleTime; // 무적 시간 증가량

    [Header("Movement")]
    public float stat_MoveSpeed;      // 이동 속도 증가량

    [Header("Charge Ability")]
    public float stat_MaxShootingForce; // 최대 발사 힘
    public float stat_MinShootingForce; // 최소 발사 힘
    public float stat_MaxChargeTime;    // 충전 속도 (줄어들면 더 빨리 풀충전)
    public float stat_ChargeDamage;     // 돌진 데미지 (별도 구현 필요 시)

    [Header("Bullet Ability")]
    public float stat_BulletDamage;     // 총알 데미지
    public float stat_RateOfFire;       // 연사 속도 (쿨타임 감소면 음수 입력, 속도 증가면 양수)
    public int stat_BulletPerShot;    // 한 번에 발사되는 총알 수

    public virtual void ApplyAugment()
    {
        PlayerData pd = GameManager.Instance.PlayerData;

        // 1. HP & SP 적용
        pd.maxHP += stat_MaxHP;
        pd.currentHP += stat_CurrentHP;
        if (pd.currentHP > pd.maxHP) pd.currentHP = pd.maxHP;

        pd.maxSP += stat_MaxSP;
        pd.currentSP += stat_CurrentSP;
        if (pd.currentSP > pd.maxSP) pd.currentSP = pd.maxSP;

        pd.invincibleTime += stat_InvincibleTime;
        pd.moveSpeed += stat_MoveSpeed;

        // 3. 돌진(Charge) 스탯 적용
        pd.maxShootingForce += stat_MaxShootingForce;
        pd.minShootingForce += stat_MinShootingForce;
        pd.maxChargeTime += stat_MaxChargeTime;
        // pd.chargeDamage += stat_ChargeDamage; // PlayerData에 변수가 있다면 주석 해제

        // 4. 사격(Bullet) 스탯 적용
        pd.damage += stat_BulletDamage; // PlayerData 변수명 확인 필요
        pd.rateOfFire += stat_RateOfFire; // 쿨타임 방식이면 -0.1 처럼 음수를 넣어야 빨라짐
        pd.per += stat_BulletPerShot; // PlayerData에 변수가 있다면 주석 해제

        // 5. UI 및 매니저 갱신 (필수!)
        // HP나 SP 최대치가 변했으니 UI를 다시 그려줍니다.
        if (stat_MaxHP != 0 || stat_CurrentHP != 0)
        {
            GameManager.Instance.hpManager.InitHP();
        }

        if (stat_MaxSP != 0 || stat_CurrentSP != 0)
        {
            GameManager.Instance.spManager.InitSP();
        }
    }
}