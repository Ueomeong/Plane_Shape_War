using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Button))]
public class StageSelectButton : MonoBehaviour
{
    public StageData myStageData;
    private Button myButton;

    private void Start()
    {
        myButton = GetComponent<Button>();
        CheckStageUnlock();
    }
    private void CheckStageUnlock()
    {
        if (GameManager.Instance != null && myStageData != null)
        {
            if (GameManager.Instance.stageID >= myStageData.stageID)
            {
                myButton.interactable = true;
            }
            else
            {
                myButton.interactable = false;
            }
        }
    }
    public void OnClickStage()
    {
         GameManager.Instance.currentSelectedStage = myStageData;
         SceneManager.LoadScene("GamePlayScene");
    }
}
