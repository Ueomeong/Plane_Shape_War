using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class MainMenuPanel : MonoBehaviour
{
    [SerializeField]private Image sr;
    private Color start_Color= Color.black;
    private void Start()
    {
        Time.timeScale = 1f;
        sr =GetComponent<Image>();
        sr.color = start_Color;
        StartCoroutine(GameStart());
    }
    IEnumerator GameStart()
    {
        while (sr.color.a>0) {
            Color c= sr.color;
            c.a=c.a-(0.3f)*Time.deltaTime;
            sr.color= c;
            yield return new WaitForFixedUpdate();
        }
    }
}
