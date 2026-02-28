using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class GameOverPanel : MonoBehaviour
{
    public GameObject GameOverMenu;
    [SerializeField] private Image sr;
    [SerializeField] private float showSpeed;
    private Color startColor = new Color(0, 0, 0, 0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        sr = GetComponent<Image>();
    }
    private void OnEnable()
    {
        GameOverMenu.SetActive(false);
        sr.color = startColor;
        StartCoroutine(Show());
    }
    IEnumerator Show()
    {
        while(sr.color.a<1f)
        {
            Color tmp = sr.color;
            tmp.a += showSpeed * Time.deltaTime;
            sr.color = tmp;
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.5f);
        GameOverMenu.SetActive(true);
    }
}
