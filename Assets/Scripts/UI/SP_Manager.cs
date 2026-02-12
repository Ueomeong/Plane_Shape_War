using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class SP_Manager : MonoBehaviour
{
    public PlayerData PlayerData;
    public GameObject SPPrefab;
    public Transform SPContainer;
    private Color SPColor;
    private Color SPColorDark;

    private List<GameObject> SP = new List<GameObject>();
    private void Start()
    {
        SPColor = new Color(0f, 124/255f, 255f);
        SPColorDark = new Color(0 / 255f, 47 / 255f, 77 / 255f);
        InitSP();
    }

    public void InitSP()
    {
        //초기화
        foreach (var i in SP)
        {
            Destroy(i);
        }
        SP.Clear();
        //재배치
        for(int i =0; i<PlayerData.maxSP;i++)
        {
            GameObject newSP = Instantiate(SPPrefab,SPContainer);
            SP.Add(newSP);
        }
        UpdateSP();
    }

    public void UpdateSP()
    {
        for(int i=0; i<SP.Count;i++)
        {
            Image SPImage = SP[i].GetComponent<Image>();
            if (i < PlayerData.currentSP)
            {
                SPImage.color = SPColor;
            }
            else 
            {
                SPImage.color = SPColorDark;
            }
        }
    }
}
