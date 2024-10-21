using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class inven_2_text : MonoBehaviour
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
            textMesh.text = "테레아서(3:1)\n\n 이것은 살레라스 국왕의 계보니라\n\n살레가 실리오를 낳고 실리오가\n이작을 낳고 \n이작은 메다하와 그의 자매들을 낳고\n\n메다하는 두말에게서 시오와\n셀루를 낳은 즉 \n\n셀루의 쌍둥이 자매가 곧 \n\n이사벨과 이자벨이라.";
        }
    }


    public void ChangeText_2()
    {
        if (textMesh != null)
        {
            textMesh.text = "테레아서(3:6)\n\n 살레라스의 국왕 시드오로 \n\n 이사벨과 이자벨을 아끼니 \n 그 자매에게 금와 옥 \n\n 먹을 것과 입을 것이 \n부족함이 없더라";
        }
    }


    public void ChangeText_3()
    {
        if (textMesh != null)
        {
            textMesh.text = "테레아서(3:11)\n\n 왕가의 포도밭 관리자 나리씨에게 \n\n 나디오라는 아들이 있으매 \n\n 그의 용모가 단정하고 아름다워 \n\n 뭇 자매들이 그를 꿈꾸었더라";
        }
    }

    public void ChangeText_4()
    {
        if (textMesh != null)
        {
            textMesh.text = "테레아서(3:17)\n\n 별의 셋째가 지나는 날 \n\n 이사벨과 이자벨이 나디오를 보매 \n\n 그의 손을 잡고 포도창고로가\n\n 형제여 우리가 너를 탐하자\n\n우리에게로와\n너가 우리를 채우길 원하노라";
        }
    }


    public void ChangeText_5()
    {
        if (textMesh != null)
        {
            textMesh.text = "테레아서(3:22)\n\n 나디오가 말하매\n\n 아름다운 자매여 이러지 마소서하매\n 자매가 그를 돌로쳐 죽이고 \n그를 범한지라\n\n 뭇 날 왕에게 고하길 \n우리에게 부족함이 없이 주사\n사내 목숨하나 우리가 \n가졌나이다 하니\n\n 왕이 자매를 저주한지라.";
        }
    }



    public void ChangeText_6()
    {
        if (textMesh != null)
        {
            textMesh.text = "메이사가서(7:14)\n\n 동쪽 국가 메이사 가의 집안은 \n예술가의 집안이요\n\n 그들의 해안선을 지나면 \n뒤집힌 성채아래로\n\n 금으로 채워진 섬이 있으매 \n\n 산벽이 가문을 지킨지라.";
        }
    }
 

    public void ChangeText_7()
    {
        if (textMesh != null)
        {
            textMesh.text = "메이사가서(8:22)\n\n 메이사가의 13대손\n\n 메이토는 탐욕한 자요\n 뒤틀림에 눈이 먼 자니라\n\n 그가 자신의 형제의 눈을 뽑으매\n\n말하길\n 형제여 그대는 아름답노라 하더라 ";
        }
    }


    public void ChangeText_8()
    {
        if (textMesh != null)
        {
            textMesh.text = "메이사가서(15:3)\n\n 메이토가 그림을 그리매\n산 것의 피로 그리고 \n\n 마법을 부리매\n 무언가를 가두기를 원하더라";
        }
    }


    public void ChangeText_9()
    {
        if (textMesh != null)
        {
            textMesh.text = "메이사가서(20:1)\n\n 그의 주위로 아끼는 촛불이 있어 \n\n 하나는 아무타시 죽음이요 \n\n 둘은 에메토우 정욕이매\n\n 셋은 메마다라 원함이라" ;
        }
    }

    public void ChangeText_10()
    {
        if (textMesh != null)
        {
            textMesh.text = "메이사가서(25:6)\n\n 뒤집힌 성채에 달이 여덟째 뜨던 밤\n\n 화방에 비명이 들리매\n\n 오직 메이토만이 웃더라\n형제여 자매여\n 나를 위해 살을 잘라 달라\n 그대의 눈을 뽑아달라 하더라\n\n 비명이 멈춘 즉 \n아무타시 , 에메토우 , 메마다라만 \n남았더라";
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
