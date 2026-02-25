using UnityEngine;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    [Header("Game Data")]
    public static GameManager Instance;
    public int stage;//클리어한 스테이지
    public int money;//돈 보유량
    public int exp;//경험치
    public int level;//플레이어 레벨
    public int temporaryMoney;//저장했다가 게임 클리어시 이 값을 매개변수로 넣어 MoneyChage호출
    [Header("GamePlay Data")]
    public bool isLive { get; private set; } = true;
    public PlayerLevelData playerLevelData;
    private HashSet<string> acquiredAugments = new HashSet<string>();//획득한 증강 저장
    [Header("UI")]
    public GameObject PauseButton;
    public GameObject PausePanel;
    public GameObject AugmentPanel;
    public EXP_Slider expSlider;
    [Header("Scripts")]
    public Player_Move player_move;
    public Player_State player_state;
    public mousePointer mousepointer;
    public PoolManager poolmanager;
    public PlayerData PlayerData;
    public HP_Manager hpManager;
    public CameraShaking camerashaking;
    public SP_Manager spManager;
    [Header("Events")]
    public System.Action<int> OnLevelUp;
    private void Awake()
    {
        level = 0;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGameData();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //게임 정지, 레벨업, 재시작 *************************************************************************
    public void TogglePauseButton()
    {
        if (isLive)
        {
            PauseGame();
        }
        else 
        { 
            ResumeGame();
        }
    }
    public void PauseGame()
    {
        isLive = false;
        Time.timeScale = 0;
        PausePanel.SetActive(true);
        PauseButton.SetActive(false);
    }
    public void LevelUPPauseGame()
    {
        isLive = false;
        Time.timeScale = 0;

        PauseButton.SetActive(false);
        AugmentPanel.SetActive(true);
    }
    public void ResumeGame()
    {
        isLive = true;
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        PauseButton.SetActive(true);
        AugmentPanel.SetActive(false);
        if (expSlider != null)
        {
            expSlider.SliderAdjust(exp, playerLevelData.requiredExp[level]);
        }
    }

    //레벨업과 경험치*************************************************************************
    public void AddExp(int val)
    {
        exp += val;
        if(expSlider!=null)
        {
            expSlider.SliderAdjust(exp, playerLevelData.requiredExp[level]);
        }
        if (playerLevelData.requiredExp[level]<=exp)
        {
            exp -= playerLevelData.requiredExp[level];
            LevelUp();
        }
        
    }
    public void LevelUp()
    {
        level++;
        OnLevelUp?.Invoke(level);
        LevelUPPauseGame();//임시
    }
   
    //증강 관리 (Set으로)*************************************************************************

    public void AddAcquiredAugment(string augmentName)
    {
        if(!acquiredAugments.Contains(augmentName))
        {
            acquiredAugments.Add(augmentName);//획득한 증강 목록에 추가
        }
    }

    public bool HasAcquired(string augmentName)
    {
        return acquiredAugments.Contains(augmentName);
    }
    
    public void ResetAugments()
    {
        acquiredAugments.Clear();
    }

    //데이터 저장과 불러오기*************************************************************************
    public void StageClear()//스테이지 클리어시 호출
    {
        stage++;
        MoneyChange(temporaryMoney);
        SaveGameData();
    }
    public void MoneyChange(int cost)//돈을 벌거나 쓸때 호출
    {
        money += cost;
        temporaryMoney = 0;
        SaveGameData();
    }
    public void SaveGameData()
    {
        PlayerPrefs.SetInt("CurrentStage", stage);
        PlayerPrefs.SetInt("CurrentMoney", money);
        PlayerPrefs.Save();
    }
    public void LoadGameData()
    {
        stage = PlayerPrefs.GetInt("CurrentStage",0);
        money = PlayerPrefs.GetInt("CurrentMoney",0);
    }
}
