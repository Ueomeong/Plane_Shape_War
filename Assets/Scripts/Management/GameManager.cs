using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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
    public PlayerData PlayerData;//원본
    public PlayerData runtimePlayerData;//복사본
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
        StartStage(1);//임시
    }
    //스테이지 시작 로직*************************************************************************
    public void StartStage(int stageNum)
    {
        // 1. 원본 데이터를 복제하여 "이번 판"용 데이터를 만듭니다.
        // 이렇게 하면 이전 판의 데이터는 사라지고, 깔끔한 새 상태로 시작합니다.
        runtimePlayerData = Instantiate(PlayerData);

        // 2. 증강 목록 초기화
        acquiredAugments.Clear();

        // 3. 스테이지 설정
        //currentStageIndex = stageNum;

        // 4. 첫 번째 라운드 씬 로드 (씬 이름은 프로젝트에 맞게 관리)
        //SceneManager.LoadScene($"Stage{stageNum}_Round1");

        // 5. 게임 상태 재개
        ResumeGame();
    }
    /*
    public void NextRound(string nextSceneName)
    {
        // runtimePlayerData는 GameManager가 들고 있으므로
        // 씬이 바뀌어도 체력, 증강, 공격력이 그대로 유지됩니다.
        SceneManager.LoadScene(nextSceneName);
    }
    public void EndStage(bool isWin)
    {
        if(isWin)
        {
            // 보상 지급 등 로직
            StageClear();
        }
        
        // 스테이지 선택 화면으로 돌아가기
        SceneManager.LoadScene("StageSelectScene");
        
        // 주의: 여기서 runtimePlayerData를 굳이 null로 만들 필요는 없지만,
        // 다음번 StartStage()가 호출될 때 어차피 덮어씌워지므로 초기화됩니다.
    }
     */
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
