using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;//인게임 플레이에 필요한 데이터들
    [Header("GamePlay Data")]
    public int currentStage;//현 스테이지
    public int exp;//경험치
    public int level;//플레이어 레벨
    public bool isLive { get; private set; } = true;//멈추기
    public bool isEnd;
    public PlayerLevelData playerLevelData;// 레벨업에 필요한 경험치
    [Header("UI")]
    public GameObject PauseButton;
    public GameObject PausePanel;
    public GameObject AugmentPanel;
    public GameObject GameOverPanel;
    public GameObject GameWinPanel;
    public EXP_Slider expSlider;
    public GoalText goalText;
    [Header("Scripts")]
    public Player_Move player_move;
    public Player_State player_state;
    public Player_Ability player_ability;
    public mousePointer mousepointer;
    public PoolManager poolmanager;
    public HP_Manager hpManager;
    public CameraShaking camerashaking;
    public SP_Manager spManager;
    [Header("Events")]
    public System.Action<int> OnLevelUp;
    [Header("Stage")]
    StageData stageData;
    public int totalSpawnerCount;
    public int currentSpawnerCount;

    private void Awake()//게임 시작시 초기화 되어야 하는 것들.
    {
        if (Instance == null)
        {
            Instance = this;
        }
        level = 0;
    }
    private void Start()
    {
        isEnd = false;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartStage();
        }
        stageData = GameManager.Instance.currentSelectedStage;
        currentSpawnerCount = 0;// 스포너 개수 초기화
        if (stageData != null)
        {
            totalSpawnerCount = stageData.enemySpawnList.Count;//총 잡아야 하는 적의 수
            // 3. 맵 생성 (MapTile 인스턴스화)
            if (stageData.mapTile != null)
            {
                Instantiate(stageData.mapTile, Vector3.zero, Quaternion.identity);
            }

            // 4. 적 스폰 코루틴 시작
            if (stageData.enemySpawnList != null && stageData.enemySpawnList.Count > 0)
            {
                StartCoroutine(SpawnEnemiesRoutine(stageData.enemySpawnList));
            }
        }
        else
        {
            Debug.LogError("선택된 스테이지 데이터가 없습니다!");
        }
        if (goalText != null)
        {
            goalText.goalTextUpdate();
        }
    }
    private IEnumerator SpawnEnemiesRoutine(List<EnemySpawnData> spawnList)
    {
        // 시간순으로 스폰하기 위해 딜레이 대기 후 풀매니저를 통해 적 소환
        foreach (var spawnData in spawnList)
        {
            // spawnData.spawnDelay가 이전 적 스폰 후 대기 시간이라고 가정
            yield return new WaitForSeconds(spawnData.spawnDelay);

            // 주의: 가지고 계신 poolmanager의 실제 함수명에 맞게 수정하세요.
            // (예: poolmanager.Get((int)spawnData.enemyType, spawnData.spawnPosition))
            if (poolmanager != null)
            {
                GameObject enemy = poolmanager.Get((int)spawnData.enemyType);
                if (enemy != null)
                {
                    enemy.transform.position = spawnData.spawnPosition;
                }
            }
        }
    }
    //게임 정지, 증강 획득, 재시작,게임오버,게임 승리 등 게임의 흐름 제어*******************************************************************
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

    public void AddCurrentSpawnerCount()
    {
        currentSpawnerCount++;
        goalText.goalTextUpdate();
        if (currentSpawnerCount >= totalSpawnerCount)
        {
            isLive = false;
            Time.timeScale = 0.6f;
            player_state.isInvincible = true;
            GameWin();
        }
    }
    public void GameWin()
    {
        GameManager.Instance.StageClear(stageData.stageID);//데이터 저장
        isEnd = true;
        GameWinPanel.SetActive(true);
    }

    public void GameOverProcess()
    {
        isEnd = true;
        GameOverPanel.SetActive(true);
    }
    //레벨업과 경험치------------------------------------------------------------------------
    public void AddExp(int val)
    {
        exp += val;
        if (expSlider != null)
        {
            expSlider.SliderAdjust(exp, playerLevelData.requiredExp[level]);
        }
        if (playerLevelData.requiredExp[level] <= exp)
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
}
