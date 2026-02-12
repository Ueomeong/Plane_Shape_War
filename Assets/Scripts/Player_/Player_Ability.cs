using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Ability : MonoBehaviour
{
    public PlayerData PlayerData;
    public bool moveable;
    public float rateOfFire => PlayerData.rateOfFire;
    public float coolTime=0f;
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
            shoot(); 
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

    void shoot()
    {
        coolTime=rateOfFire;
        GameManager.Instance.poolmanager.Get(0);
    }
}
