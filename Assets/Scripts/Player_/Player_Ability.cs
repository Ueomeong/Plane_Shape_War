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
        if(!GameManager.Instance.isLive) { return; }
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
