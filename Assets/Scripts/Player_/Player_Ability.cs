using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Ability : MonoBehaviour
{
    public bool moveable;
    public float rateOfFire = 0.15f;
    public float coolTime=0f;
    void Update()
    {
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
    }
    private void FixedUpdate()
    {
        if (coolTime > 0f)
        {
            coolTime -= Time.fixedDeltaTime;
        }
    }
    void shoot()
    {
        coolTime=rateOfFire;
        GameManager.Instance.poolmanager.Get(0);
    }
}
