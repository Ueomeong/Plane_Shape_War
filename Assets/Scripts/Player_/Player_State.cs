using UnityEngine;

public class Player_State : MonoBehaviour
{
    public PlayerData PlayerData;
    public float InvincibleTime => PlayerData.invincibleTime;
    private float currentInvincibleTime;
    public bool isInvincible;
    public SpriteRenderer spriteRenderer;
  
    private void Awake()
    {
        gameObject.SetActive(true);
        PlayerData.ResetPlayerData();
        isInvincible = false;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    void Update()
    {
        if (!GameManager.Instance.isLive) { return; }
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
        else
        {
           
        }
    }
    public void TakeDamage(int damage)// 일단 int로 하자! 피해 입음!
    {
        if (isInvincible) { return;  }
        GameManager.Instance.camerashaking.ShakeCamera(1.5f, 0.2f);
        isInvincible = true;
        currentInvincibleTime = InvincibleTime;
        PlayerData.currentHP -= damage;
        GameManager.Instance.hpManager.UpdateHP();
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
        GameManager.Instance.spManager.UpdateSP();
    }
    public void GetSP(int value)
    {
        PlayerData.currentSP += value;
        if (PlayerData.currentSP >= PlayerData.maxSP)
        {
            PlayerData.currentSP = PlayerData.maxSP;
        }
        GameManager.Instance.spManager.UpdateSP();
    }

    public void Heal(int heal)// 일단 int로 하자! 힐을 받음!
    {
        PlayerData.currentHP += heal;
        if (PlayerData.currentHP >= PlayerData.maxHP)
        {
            PlayerData.currentHP=PlayerData.maxHP;
        }
        GameManager.Instance.hpManager.UpdateHP();
    }

    private void Die()
    {
        gameObject.SetActive(false);
        GameObject eff = GameManager.Instance.poolmanager.Get(11);
        GameManager.Instance.camerashaking.ShakeCamera(3f, 0.15f);
        if (eff != null)
        {
            eff.transform.position = transform.position;
        }
        Debug.Log("GameOver!");
        //GameManager.Instance.PauseGame();
    }
}
