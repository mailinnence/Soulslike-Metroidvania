using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class inven_1_text : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // 텍스트를 바꾸기
    public void ChangeText_1()
    {
        if (textMesh != null)
        {
            textMesh.text = "잊혀진 열쇠\n\n 실패한 이주 계획의 도착지 살레라스의 안뜰 포도창고 열쇠\n\n열쇠의 끝에서 강한 진동이 느껴진다.";
        }
    }


    public void ChangeText_2()
    {
        if (textMesh != null)
        {
            textMesh.text = "이사벨과 이자벨의 증표\n\n 이사벨과 이자벨 자매의 귀품\n 살레라스 왕의 가품이자\n\n 순결의 약조를 의미한다.";
        }
    }

    public void ChangeText_3()
    {
        if (textMesh != null)
        {
            textMesh.text = "메이토의 만든 조형물 \n\n 메이토가 만든 장미모양의 조형문\n\n 꽃잎 사이사이에 광기어린 글귀가 적혀있다.\n\n 지독한 피 냄세가 난다.";
        }
    }



    public void ChangeText_else()
    {
        if (textMesh != null)
        {
            textMesh.text = "";
        }
    }


}
