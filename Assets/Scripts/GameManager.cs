using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Data")]
    public static GameManager Instance;
    public int stage;//클리어한 스테이지
    public int money;//돈 보유량
    public int temporaryMoney;//저장했다가 게임 클리어시 이 값을 매개변수로 넣어 MoneyChage호출
    [Header("GamePlay Data")]
    public bool isLive { get; private set; } = true;
    public Player_Move player_move;
    public Player_State player_state;
    public mousePointer mousepointer;
    public PoolManager poolmanager;
    public PlayerData PlayerData;
    public CameraShaking camerashaking;


    private void Awake()
    {
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

    public void PauseGame()
    {
        isLive = false;
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        isLive = true;
        Time.timeScale = 1;
    }



    //데이터 저장과 불러오기
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
