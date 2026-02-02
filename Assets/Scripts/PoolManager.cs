using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;//프리펩 종류마다 저장
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        for(int i=0;i< pools.Length; i++)
        {
            pools[i]=new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject selected_object=null;
        foreach(GameObject item in pools[index])
        {
            if(!item.activeSelf)//비활성화되어있음
            {
                selected_object = item;
                item.SetActive(true);
                break;
            }
        }
        if (selected_object == null)//못찾음
        {
            selected_object= Instantiate(prefabs[index],transform);
            pools[index].Add(selected_object);
        }
        return selected_object;
    }
}
