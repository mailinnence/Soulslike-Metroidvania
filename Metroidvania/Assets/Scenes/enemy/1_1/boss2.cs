using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;


public class boss2 : playerStatManager
{
    public boss_hp_2 boss_hp_2;

    public TextMeshProUGUI textMesh;
    public Image energe;
    public Image energeBar;
    public Image background;

    public bool see_delay; // 한번만 변화되도록 

    public DialogManager4 DialogManager4;

    [Header("상호작용 구간 , 벡터 , 레이어")]
    public Transform interactionArea;
    public Vector2 interactionArea_;             
    public LayerMask interactionLayer; 
    public LayerMask objectLayer; 

    private bool object_off_;
    
    public event_background event_background;
    public interaction_object interaction_object;


    void Start()
    {
        start();

        event_background.Fade_white_out(1.5f , 1.5f);
        interaction_object.waiting_Anim();



        boss_2_start = false;
        if(!boss_2_start)
        {
            DialogManager4.voice();
        }
    }

    void Update()
    {

        if(boss_2 && !object_off_ && boss_2_start)
        {
            boss_battle();
        }     

        else if(object_off_)
        {
            boss_battle_not();
        }

        if(!boss_2)
        {
            object_off_ = true;
        }
    

    }


    

    public void boss_battle()
    {
        see_delay = true;
        textMesh.enabled = true;
        energe.enabled = true;
        energeBar.enabled = true;
        background.enabled = true;

    }


    public void boss_battle_not()
    {
        see_delay = false;
        textMesh.enabled = false;
        energe.enabled = false;
        energeBar.enabled = false;
        background.enabled = false;

    }





    public void all_dead()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);

        for(int i = 0; i < objectsToHit.Length; i++)
        {
            if(objectsToHit[i].GetComponent<enemy_move>() != null)
            {
                // 피격 데미지 처리
                objectsToHit[i].GetComponent<enemy_move>().all_dead();
            }
        }
    
    }


    public void start()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);

        for(int i = 0; i < objectsToHit.Length; i++)
        {
            if(objectsToHit[i].GetComponent<move>() != null)
            {
                // 피격 데미지 처리
                boss_2 = true;
            }
        }
    
    }





    public void object_off()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, objectLayer);

        for(int i = 0; i < objectsToHit.Length; i++)
        {
            objectsToHit[i].gameObject.SetActive(false);
        }
    }






    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(interactionArea.position , interactionArea_);
    }


}
