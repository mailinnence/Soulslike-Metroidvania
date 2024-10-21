using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class acolite_sound : MonoBehaviour
{
    [Header("기모으기")]
    public AudioClip _ACOLYTE_CHARGE_ATTACK_DEFAULT;       
    public float _ACOLYTE_CHARGE_ATTACK_DEFAULT_volums;      
    
    [Header("공격 방출")]
    public AudioClip ACOLYTE_RELEASE_ATTACK_DEFAULT;
    public float ACOLYTE_RELEASE_ATTACK_DEFAULT_volums;   
    
    [Header("죽음")]
    public AudioClip ACOLYTE_DEATH_DEFAULT;
    public float ACOLYTE_DEATH_DEFAULT_volums; 
    
    [Header("발소리_1")]
    public AudioClip _ACOLYTE_FOOTSTEPS_DEFAULT_1;
    public float _ACOLYTE_FOOTSTEPS_DEFAULT_1_volums;     
    
    [Header("발소리_2")]
    public AudioClip _ACOLYTE_FOOTSTEPS_DEFAULT_2;  
    public float _ACOLYTE_FOOTSTEPS_DEFAULT_2_volums;  
                           

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


    // 기모으기
    public void _ACOLYTE_CHARGE_ATTACK_DEFAULT_function()
    {
        if(echo) SoundManager.Instance.PlaySound(_ACOLYTE_CHARGE_ATTACK_DEFAULT , volume: _ACOLYTE_CHARGE_ATTACK_DEFAULT_volums); 
        else SoundManager.Instance.StopSound(_ACOLYTE_CHARGE_ATTACK_DEFAULT);
    }


    // 공격 방출
    public void ACOLYTE_RELEASE_ATTACK_DEFAULT_function()
    {
        if(echo) SoundManager.Instance.PlaySound(ACOLYTE_RELEASE_ATTACK_DEFAULT , volume: ACOLYTE_RELEASE_ATTACK_DEFAULT_volums); 
        else SoundManager.Instance.StopSound(ACOLYTE_RELEASE_ATTACK_DEFAULT);
    }


    // 죽음
    public void ACOLYTE_DEATH_DEFAULT_function()
    {
        if(echo) SoundManager.Instance.PlaySound(ACOLYTE_DEATH_DEFAULT , volume: ACOLYTE_DEATH_DEFAULT_volums); 
        else SoundManager.Instance.StopSound(ACOLYTE_DEATH_DEFAULT);
    }


    // 발소리_1
    public void _ACOLYTE_FOOTSTEPS_DEFAULT_1_function()
    {
        if(echo) SoundManager.Instance.PlaySound(_ACOLYTE_FOOTSTEPS_DEFAULT_1 , volume: _ACOLYTE_FOOTSTEPS_DEFAULT_1_volums); 
        else SoundManager.Instance.StopSound(_ACOLYTE_FOOTSTEPS_DEFAULT_1);
    }


    // 발소리_2
    public void _ACOLYTE_FOOTSTEPS_DEFAULT_2_function()
    {
        if(echo) SoundManager.Instance.PlaySound(_ACOLYTE_FOOTSTEPS_DEFAULT_2 , volume: _ACOLYTE_FOOTSTEPS_DEFAULT_2_volums); 
        else SoundManager.Instance.StopSound(_ACOLYTE_FOOTSTEPS_DEFAULT_2);
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
