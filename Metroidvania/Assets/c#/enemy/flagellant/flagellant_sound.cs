using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flagellant_sound : MonoBehaviour
{
    [Header("걷기_1")]
    public AudioClip _FLAGELLANT_FOOTSTEPS_DEFAULT_1;       
    public float _FLAGELLANT_FOOTSTEPS_DEFAULT_1_volums;      
    
    [Header("걷기_2")]
    public AudioClip _FLAGELLANT_FOOTSTEPS_DEFAULT_2;
    public float _FLAGELLANT_FOOTSTEPS_DEFAULT_2_volums;   
    
    [Header("뛰기_1")]
    public AudioClip _FLAGELLANT_RUNNING_2;
    public float _FLAGELLANT_RUNNING_2_volums; 
 
    [Header("뛰기_1")]
    public AudioClip _FLAGELLANT_RUNNING_3;  
    public float _FLAGELLANT_RUNNING_3_volums;  


    [Header("공격 준비")]
    public AudioClip FLAGELLANT_ATTACK;  
    public float FLAGELLANT_ATTACK_volums;      

    [Header("공격")]
    public AudioClip FLAGELLANT_BASIC_ATTACK_2;  
    public float FLAGELLANT_BASIC_ATTACK_2_volums;  

    [Header("죽음")]
    public AudioClip FLAGELLANT_DEATH_VANISH;  
    public float FLAGELLANT_DEATH_VANISH_volums;  
                    
    [Header("idle")]
    public AudioClip FLAGELLANT_SELFHIT;  
    public float FLAGELLANT_SELFHIT_volums;  


    [Header("True : On , False : Off")]
    public bool echo;


    [Header("상호작용 구간 , 벡터 , 레이어")]
    public Transform interactionArea;
    public Vector2 interactionArea_;
    public LayerMask interactionLayer; 



    void Update()
    {
        sound();
    }


    // 걷기_1
    public void _FLAGELLANT_FOOTSTEPS_DEFAULT_1_function()
    {
        if(echo) SoundManager.Instance.PlaySound(_FLAGELLANT_FOOTSTEPS_DEFAULT_1 , volume : _FLAGELLANT_FOOTSTEPS_DEFAULT_1_volums ); 
        else SoundManager.Instance.StopSound(_FLAGELLANT_FOOTSTEPS_DEFAULT_1);
    }


    // 걷기_2
    public void _FLAGELLANT_FOOTSTEPS_DEFAULT_2_function()
    {
        if(echo) SoundManager.Instance.PlaySound(_FLAGELLANT_FOOTSTEPS_DEFAULT_2 , volume : _FLAGELLANT_FOOTSTEPS_DEFAULT_2_volums); 
        else SoundManager.Instance.StopSound(_FLAGELLANT_FOOTSTEPS_DEFAULT_2);
    }


    // 뛰기_1
    public void _FLAGELLANT_RUNNING_2_function()
    {
        if(echo) SoundManager.Instance.PlaySound(_FLAGELLANT_RUNNING_2 , volume : _FLAGELLANT_RUNNING_2_volums); 
        else SoundManager.Instance.StopSound(_FLAGELLANT_RUNNING_2);
    }


    // 뛰기_2
    public void _FLAGELLANT_RUNNING_3_function()
    {
        if(echo) SoundManager.Instance.PlaySound(_FLAGELLANT_RUNNING_3 , volume : _FLAGELLANT_RUNNING_3_volums); 
        else SoundManager.Instance.StopSound(_FLAGELLANT_RUNNING_3);
    }

    // 공격 준비
    public void FLAGELLANT_ATTACK_function()
    {
        if(echo) SoundManager.Instance.PlaySound(FLAGELLANT_ATTACK , volume : FLAGELLANT_ATTACK_volums); 
        else SoundManager.Instance.StopSound(FLAGELLANT_ATTACK);
    }

    // 공격
    public void FLAGELLANT_BASIC_ATTACK_2_function()
    {
        if(echo) SoundManager.Instance.PlaySound(FLAGELLANT_BASIC_ATTACK_2 , volume : FLAGELLANT_BASIC_ATTACK_2_volums); 
        else SoundManager.Instance.StopSound(FLAGELLANT_BASIC_ATTACK_2);
    }

    // 죽음
    public void FLAGELLANT_DEATH_VANISH_function()
    {
        if(echo) SoundManager.Instance.PlaySound(FLAGELLANT_DEATH_VANISH , volume : FLAGELLANT_DEATH_VANISH_volums); 
        else SoundManager.Instance.StopSound(FLAGELLANT_DEATH_VANISH);
    }


    // idle
    public void FLAGELLANT_SELFHIT_function()
    {
        if(echo) SoundManager.Instance.PlaySound(FLAGELLANT_SELFHIT , volume : FLAGELLANT_SELFHIT_volums); 
        else SoundManager.Instance.StopSound(FLAGELLANT_SELFHIT);
    }



    void sound()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);     
        if (objectsToHit.Length >=1) 
        {
            echo = true;
        } 
        else
        { 
            echo = false; 
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(interactionArea.position , interactionArea_);

    }
}
