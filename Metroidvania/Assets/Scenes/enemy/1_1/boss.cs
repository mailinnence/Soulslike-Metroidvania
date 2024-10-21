using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;


public class boss : playerStatManager
{
    public boss_hp_1 boss_hp_1;

    public TextMeshProUGUI textMesh;
    public Image energe;
    public Image energeBar;
    public Image background;

    public bool see_delay; // 한번만 변화되도록 

    public enemy_controll_1 enemy_controll_1;
    public boss_1_background back_sound_stage_1;


    [Header("상호작용 구간 , 벡터 , 레이어")]
    public Transform interactionArea;
    public Vector2 interactionArea_;             
    public LayerMask interactionLayer; 


    [Header("Background_Music")]
    public AudioClip boss_death;  
    public AudioClip boss_clear;    
    public int progress;



    void Start()
    {
        progress = 0;
    }

    void Update()
    {
 
        if(boss_1 && !see_delay)
        {
            see_delay = true;           // 한번만 발생해야 하기떄문!!
            Invoke("boss_battle" , 4.5f);
        }
        else if(!boss_1)
        {
            boss_battle_not();
            boss_hp_1.init();
        }

        if(!alive)
        {
            progress = 3;
        }
     
    }


    

    public void boss_battle()
    {
        if(progress == 0) {progress = 1; }

        StartCoroutine(boss_music_start_delay());
        enemy_controll_1.reset_enemy_except_boss();
        see_delay = true;
        textMesh.enabled = true;
        energe.enabled = true;
        energeBar.enabled = true;
        background.enabled = true;

    }


    public void boss_battle_not()
    {
        if(alive && progress == 1)
        {
            StartCoroutine(boss_music_clear_delay());
            SoundManager.Instance.PlaySound(boss_death);
            progress = 2;
        }
        back_sound_stage_1.echo = false;
        see_delay = false;
        textMesh.enabled = false;
        energe.enabled = false;
        energeBar.enabled = false;
        background.enabled = false;

    }


    IEnumerator boss_music_start_delay()
    {
        yield return new WaitForSeconds(3f);
        back_sound_stage_1.echo = true;
    }





    IEnumerator boss_music_clear_delay()
    {
        yield return new WaitForSeconds(2.5f);
        SoundManager.Instance.PlaySound(boss_clear);
        
    }




    // 설명 텍스트 여부
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




    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(interactionArea.position , interactionArea_);
    }


}
