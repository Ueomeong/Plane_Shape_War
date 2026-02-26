using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class Player_Ability : MonoBehaviour
{
    private PlayerData PlayerData;
    public bool moveable;
    public float rateOfFire => PlayerData.rateOfFire;
    public float coolTime=0f;
    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.runtimePlayerData != null)
        {
            PlayerData = GameManager.Instance.runtimePlayerData;
        }
    }
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame)
        {
            GameManager.Instance.TogglePauseButton();
        }
        if (!GameManager.Instance.isLive) { return; }
        moveable = GameManager.Instance.player_move.isCharging;
        if (Mouse.current.leftButton.isPressed && !moveable && coolTime<=0f) 
        {
            StartCoroutine(shoot());
        }
        if(Keyboard.current.tKey.wasPressedThisFrame)
        {
            GameManager.Instance.poolmanager.Get(6);
        }
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            GameManager.Instance.poolmanager.Get(7);
        }
        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            GameManager.Instance.poolmanager.Get(8);
        }
        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            GameManager.Instance.poolmanager.Get(5);
        }
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            GameManager.Instance.player_state.TakeDamage(1);
        }
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            GameManager.Instance.player_state.Heal(1);
        }

        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            GameManager.Instance.player_state.UseSP(1);
        }
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            GameManager.Instance.player_state.GetSP(1);
        }

        if (coolTime >= 0f)
        {
            coolTime -= Time.deltaTime;
        }
    }


    IEnumerator shoot()
    {
        int numberOfBullet=0;//√ ≈∫
        int maxNumberOfBullet=PlayerData.spread_Bullet;
        coolTime = rateOfFire;
        for(int i=0; i < PlayerData.continuousFire; i++)//ø¨º” πﬂªÁ √—æÀ ∞≥ºˆ
        {
            GameObject ShootedBullet = GameManager.Instance.poolmanager.Get(0);
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }
}
