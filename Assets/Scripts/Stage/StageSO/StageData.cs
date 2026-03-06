using UnityEngine;
using System.Collections.Generic;

// 적의 종류를 구분하는 Enum (object pooling에서 적의 index) 모음
public enum EnemyType
{
    capsule = 4,
    diamond =5,
    hexagon =6,
    square =7,
    triangle =8,
    EnemySpawner_square=14,
    EnemySpawner_triangle=15,
    EnemySpawner_hexagon=16,
}

// 개별 적의 스폰 타이밍과 위치를 담는 구조체
[System.Serializable]
public struct EnemySpawnData
{
    [Tooltip("스폰할 적의 종류")]
    public EnemyType enemyType;

    [Tooltip("적 스폰 위치")]
    public Vector2 spawnPosition; // 2D 환경이므로 Vector2 사용

    [Tooltip("게임 시작(또는 웨이브 시작) 후 몇 초 뒤에 등장할지")]
    public float spawnDelay;
}

// 유니티 프로젝트 창에서 우클릭으로 쉽게 생성할 수 있도록 메뉴 추가
[CreateAssetMenu(fileName = "New Stage Data", menuName = "Scriptable Objects/StageData", order = 1)]
public class StageData : ScriptableObject
{
    [Header("UI 정보 (스테이지 선택 씬용)")]
    public int stageID;             // 스테이지 고유 번호 (잠금 해제 로직 등에 활용)
    public string stageName;        // UI 버튼에 표시될 텍스트
    public Sprite stageThumbnail;   // UI 버튼에 들어갈 썸네일 이미지

    [Header("맵 정보 (게임 플레이 씬용)")]
    public GameObject mapTile;    // 맵의 바닥
    [Header("전투 정보 (적 스폰 데이터)")]
    // 이 리스트를 인스펙터에서 채워주면, GamePlayScene에서 순차적으로 읽어와 적을 스폰합니다.
    public List<EnemySpawnData> enemySpawnList;
}