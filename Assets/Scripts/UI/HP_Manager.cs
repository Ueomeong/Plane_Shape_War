using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class HP_Manager : MonoBehaviour
{
    public PlayerData PlayerData;
    public GameObject HPPrefab;
    public Transform HPContainer;
    private Color HPColor;
    private Color HPColorDark;

    private List<GameObject> hearts = new List<GameObject>();
    private void Start()
    {
        HPColor = new Color(190/255f, 15/255f, 0f);
        HPColorDark = new Color(37 / 255f, 37 / 255f, 37 / 255f);
        InitHP();
    }

    public void InitHP()
    {
        //초기화
        foreach (var i in hearts)
        {
            Destroy(i);
        }
        hearts.Clear();
        //재배치
        for(int i =0; i<PlayerData.maxHP;i++)
        {
            GameObject newHeart = Instantiate(HPPrefab,HPContainer);
            hearts.Add(newHeart);
        }
        UpdateHP();
    }

    public void UpdateHP()
    {
        for(int i=0; i<hearts.Count;i++)
        {
            Image heartImage = hearts[i].GetComponent<Image>();
            if (i < PlayerData.currentHP)
            {
                heartImage.color = HPColor;
            }
            else 
            {
                heartImage.color = HPColorDark;
            }
        }
    }
}
