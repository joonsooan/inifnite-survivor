using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리팹들을 보관할 변수
    public GameObject[] prefabs;

    // 풀을 담당하는 리스트 배열
    private List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // 선택한 풀의 비활성화된 게임오브젝트에 접근
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            // 발견하면 -> select 변수에 할당
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // 발견하지 못하면 (select가 null일 때)
        if (!select)
        {
            // 새롭게 생성하고 select 변수에 할당
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}