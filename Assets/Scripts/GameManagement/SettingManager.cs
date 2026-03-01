using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public void OnClickHardReset()
    {
        GameManager.Instance.HardReset();
    }
    public void OnClickCheat()
    {
        GameManager.Instance.MoneyChange(100000);
        GameManager.Instance.SaveGameData();
        Debug.Log("¿Ã ªÁ±‚≤€");
    }
}
