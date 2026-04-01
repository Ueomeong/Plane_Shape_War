using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class Player_Ability : MonoBehaviour
{
    private PlayerData PlayerData;
    public bool moveable;
    public float rateOfFire => PlayerData.rateOfFire;
    public float coolTime=0f;


    public Transform skillHolder;//액티브 스킬이 장착될 위치
    public ActiveAugment currentActiveSkill;
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
            InGameManager.Instance.TogglePauseButton();
        }
        if (!InGameManager.Instance.isLive) { return; }
        moveable = InGameManager.Instance.player_move.isCharging;
        if (Mouse.current.leftButton.isPressed && !moveable && coolTime<=0f) 
        {
            StartCoroutine(shoot());
        }

        //임시 키들
        if(Keyboard.current.tKey.wasPressedThisFrame)
        {
            InGameManager.Instance.poolmanager.Get(6);
        }
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            InGameManager.Instance.poolmanager.Get(7);
        }
        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            InGameManager.Instance.poolmanager.Get(8);
        }
        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            InGameManager.Instance.poolmanager.Get(5);
        }
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            InGameManager.Instance.poolmanager.Get(14);
        }
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            InGameManager.Instance.poolmanager.Get(15);
        }
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            InGameManager.Instance.poolmanager.Get(16);
        }
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            InGameManager.Instance.poolmanager.Get(17);
        }
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            InGameManager.Instance.poolmanager.Get(18);
        }
        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            InGameManager.Instance.poolmanager.Get(4);
        }
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            InGameManager.Instance.player_state.TakeDamage(1);
        }
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            InGameManager.Instance.player_state.Heal(1);
        }

        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            InGameManager.Instance.player_state.UseSP(1);
        }
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            InGameManager.Instance.player_state.GetSP(1);
        }
        //

        //
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            if (currentActiveSkill != null)
            {
                currentActiveSkill.TryUseSkill();
            }
        }
        if (coolTime >= 0f)
        {
            coolTime -= Time.deltaTime;
        }
    }


    IEnumerator shoot()
    {
        int numberOfBullet=0;//초탄
        int maxNumberOfBullet=PlayerData.spread_Bullet;
        coolTime = rateOfFire;
        for(int i=0; i < PlayerData.continuousFire; i++)//연속 발사 총알 개수
        {
            GameObject ShootedBullet = InGameManager.Instance.poolmanager.Get(0);
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }

    public void EquipActiveSkill(GameObject newSkill)
    {
        if(currentActiveSkill!=null)//스킬이 있음
        {
            currentActiveSkill.OnDisable();//기존 스킬 비활성화
            Destroy(currentActiveSkill.gameObject);
        }
        //새 스킬 장착
        Transform parentTransform = skillHolder != null ? skillHolder : transform;
        GameObject skillObj = Instantiate(newSkill, parentTransform);

        // 위치 초기화 (부모 자식 관계가 되었으므로 localPosition)
        skillObj.transform.localPosition = Vector3.zero;

        // 3. 컴포넌트 참조 저장
        currentActiveSkill = skillObj.GetComponent<ActiveAugment>();
    }
}
