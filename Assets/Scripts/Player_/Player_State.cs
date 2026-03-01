using UnityEngine;

public class Player_State : MonoBehaviour
{
    public PlayerData PlayerData;
    public float InvincibleTime => PlayerData.invincibleTime;
    private float currentInvincibleTime;
    public bool isInvincible;
    public SpriteRenderer spriteRenderer;
    public float regenTimeRemainSP;//sp리젠까지 남은 시간

    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.runtimePlayerData != null)
        {
            PlayerData = GameManager.Instance.runtimePlayerData;
        }
        gameObject.SetActive(true);
        PlayerData.ResetPlayerData();
        isInvincible = false;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    void Update()
    {
        if (!InGameManager.Instance.isLive) { return; }
        //무적시간 ********************
        if(currentInvincibleTime>=0)
        {
            currentInvincibleTime -= Time.deltaTime;
        }
        else
        {
            if (isInvincible)
            {
                spriteRenderer.color = Color.white;
                isInvincible = false;
            }
        }
        if (isInvincible)
        {
            // 시간에 따라 0.2~1.0 사이를 빠르게 왔다갔다 함
            float alpha = Mathf.SmoothStep(0.2f, 1.0f, Mathf.PingPong(Time.time * 5f, 1f));
            Color color = Color.black;
            color.a = alpha;
            spriteRenderer.color = color;
        }
        //SP리젠**************************
        if (PlayerData.maxSP > PlayerData.currentSP)
        {
            regenTimeRemainSP += Time.deltaTime;
        }
        else
        {
            regenTimeRemainSP = 0;
        }
        if (regenTimeRemainSP>=PlayerData.regenTimeSP)
        {
            GetSP(1);
            regenTimeRemainSP = 0;
        }
    }
    public void TakeDamage(int damage)// 일단 int로 하자! 피해 입음!
    {
        if (isInvincible) { return;  }
        InGameManager.Instance.camerashaking.ShakeCamera(1.5f, 0.2f);
        isInvincible = true;
        currentInvincibleTime = InvincibleTime;
        PlayerData.currentHP -= damage;
        InGameManager.Instance.hpManager.UpdateHP();
        if (PlayerData.currentHP <= 0)
        {
            Die();
        }
    }

    public void UseSP(int value)
    {
        PlayerData.currentSP -= value;
        if (PlayerData.currentSP <=0)
        {
            PlayerData.currentSP = 0;
        }
        InGameManager.Instance.spManager.UpdateSP();
    }
    public void GetSP(int value)
    {
        PlayerData.currentSP += value;
        if (PlayerData.currentSP >= PlayerData.maxSP)
        {
            PlayerData.currentSP = PlayerData.maxSP;
        }
        InGameManager.Instance.spManager.UpdateSP();
    }

    public void Heal(int heal)// 일단 int로 하자! 힐을 받음!
    {
        PlayerData.currentHP += heal;
        if (PlayerData.currentHP >= PlayerData.maxHP)
        {
            PlayerData.currentHP=PlayerData.maxHP;
        }
        InGameManager.Instance.hpManager.UpdateHP();
    }

    private void Die()
    {
        gameObject.SetActive(false);
        GameObject eff = InGameManager.Instance.poolmanager.Get(11);
        InGameManager.Instance.camerashaking.ShakeCamera(3f, 0.15f);
        if (eff != null)
        {
            eff.transform.position = transform.position;
        }
        Debug.Log("GameOver!");
        InGameManager.Instance.GameOverProcess();
        //GameManager.Instance.PauseGame();
    }

}
