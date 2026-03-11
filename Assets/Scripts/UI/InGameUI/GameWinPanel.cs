using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameWinPanel : MonoBehaviour
{
    public GameObject GameWinMenu;
    [SerializeField] private Image sr;
    [SerializeField] private float showSpeed; // 인스펙터에서 1 이상의 충분한 값(예: 1~3)인지 꼭 확인하세요!
    private Color startColor = new Color(0, 0, 0, 0);

    private void Awake()
    {
        // 혹시 Image 할당을 까먹었을 때를 대비해 가져옵니다.
        if (sr == null) sr = GetComponent<Image>();
    }

    private void OnEnable()
    {
        GameWinMenu.SetActive(false);
        sr.color = startColor;
        StartCoroutine(Show());
    }

    IEnumerator Show()
    {
        // 알파값이 1(완전 불투명)이 될 때까지 반복
        while (sr.color.a < 1f)
        {
            Color tmp = sr.color;
            // 게임 속도에 영향받지 않도록 unscaledDeltaTime 사용
            tmp.a += showSpeed * Time.unscaledDeltaTime;
            sr.color = tmp;

            // FixedUpdate 대신 다음 프레임(Update)까지 대기
            yield return null;
        }

        // 혹시 알파값이 1을 초과해버렸다면 깔끔하게 1로 맞춰주기
        Color finalColor = sr.color;
        finalColor.a = 1f;
        sr.color = finalColor;

        // 대기 시간도 현실 시간 기준으로 0.1초 대기
        yield return new WaitForSecondsRealtime(0.1f);

        // 마침내 승리 메뉴 등장!
        GameWinMenu.SetActive(true);
    }
}