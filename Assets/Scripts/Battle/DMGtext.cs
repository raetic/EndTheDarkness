﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DMGtext : MonoBehaviour
{
    int type; //0>데미지 ->빨강 1>행동력 파랑 2>방어도 노랑 3>체력 회복>초록 etc)
    TextMeshProUGUI t;

    public void GetType(int i,float value)
    {
        t = GetComponent<TextMeshProUGUI>();
        t.text = value + "";
        type = i;
        if (i == 0) //피격
        {
            t.color = Color.red;
        }
        if (i == 1) //속도 증가
        {
            t.color = Color.blue;
        }
        if (i == 2) //방어도 획득
        {
            t.color = Color.yellow;
        }
        if (i == 3) //체력 증가
        {
            t.color = Color.green;
        }
        if(i == 4) //공격력 획득
        {
            t.color = Color.magenta;
        }
        StartCoroutine("TextChange");
    }

    IEnumerator TextChange()
    {
        int c = 0;
        while (c < 120)
        {
           
            t.color = new Color(t.color.r,t.color.g,t.color.b,t.color.a-0.01f);
            transform.position += new Vector3(0, 0.02f);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }
    // Update is called once per frame
    
}
