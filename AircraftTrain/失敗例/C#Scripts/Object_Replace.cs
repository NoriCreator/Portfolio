using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Replace : MonoBehaviour
{
    // 航空機オブジェクトを複数取得するためのリスト
    public List<GameObject> Aircraft = new List<GameObject>();

    // リスト用の番号
    private int num = 0;

    void Start() 
    {
        // 航空機リストの0番以外を非アクティブ化
        for(num = 1;num < 9;num++)
        {
            Aircraft[num].SetActive(false);
        }
    }

    void Update()
    {
        // spaceキーを押したとき、他の航空機に入れ替える
        if(Input.GetKeyDown("space"))
        {
            num += 1;

            if(num == 0 || num >= 9) 
            {
                Aircraft[8].SetActive(false);
                Aircraft[0].SetActive(true);
                num = 0;
            }
            else if(num >= 1 && num <= 8)
            {
                Aircraft[num - 1].SetActive(false);
                Aircraft[num].SetActive(true);
            }  
        }
    }
}
