using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectButton : MonoBehaviour
{
    public StageData myStageData;
    public void OnClickStage()
    {
         GameManager.Instance.currentSelectedStage = myStageData;
         SceneManager.LoadScene("GamePlayScene");
    }
}
