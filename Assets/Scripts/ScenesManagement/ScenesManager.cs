using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public void OnClickStartGame()
    {
        SceneManager.LoadScene("GamePlayScene");
    }
    public void OnClickStageSelect()
    {
        SceneManager.LoadScene("StageScene");
    }
    public void OnClickShop()
    {
        SceneManager.LoadScene("ShopScene");
    }

    public void OnClickSettings()
    {
        SceneManager.LoadScene("SettingScene");
    }
    public void OnClickMainMenu()
    {
        
        SceneManager.LoadScene("MainMenuScene");
    }
}
