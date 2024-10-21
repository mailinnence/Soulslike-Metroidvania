using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pattern_1 : MonoBehaviour
{
    

    [Header("패턴_1,2")]         
    public AudioClip ISIDORA_FIRECOLUMN_ANTICIPATION_2;
    public AudioClip BURNT_FACE_FIRE_BALL_SHOT;

    [Header("패턴_3")] 
    public AudioClip FIRE_BALL_BOUNCE_3;
    public AudioClip FIRE_BALL_EXPLODE;

    [Header("패턴_5")] 
    public AudioClip ElmFireTrap_LIGHT;
    public AudioClip _ElmFireTrap_LIGHT;



    [Header("패턴_6")] 
    public AudioClip GUARDIAN_APPEAR;
    public AudioClip GUARDIAN_ATTACK;
    public AudioClip GUARDIAN_DISAPPEAR;

    [Header("page_2")] 
    public AudioClip SNAKE_THUNDERBOLT;
    public AudioClip CAULDRON_FIRE;




    // 패턴_1 -----------------------------------------------------------------------------------------------------
    public void ISIDORA_FIRECOLUMN_ANTICIPATION_2_function()
    {
        SoundManager.Instance.PlaySound(ISIDORA_FIRECOLUMN_ANTICIPATION_2 , volume: 0.8f , pitch : 0.6f);
    }

    public void BURNT_FACE_FIRE_BALL_SHOT_function()
    {
        SoundManager.Instance.PlaySound(BURNT_FACE_FIRE_BALL_SHOT , volume: 0.3f);
    }

    

    // 패턴_2 -----------------------------------------------------------------------------------------------------
    public void ISIDORA_FIRECOLUMN_ANTICIPATION_2_row_function()
    {
        SoundManager.Instance.PlaySound(ISIDORA_FIRECOLUMN_ANTICIPATION_2 , volume: 0.8f , pitch : 0.8f);
    }

    public void BURNT_FACE_FIRE_BALL_SHOT_row_function()
    {
        SoundManager.Instance.PlaySound(BURNT_FACE_FIRE_BALL_SHOT , volume: 0.5f , pitch : 1f);
    }




    // 패턴_3 -----------------------------------------------------------------------------------------------------
    public void FIRE_BALL_BOUNCE_3_function()
    {
        SoundManager.Instance.PlaySound(FIRE_BALL_BOUNCE_3 , volume: 1f , pitch : 1f);
    }


    public void FIRE_BALL_EXPLODE_function()
    {
        SoundManager.Instance.PlaySound(FIRE_BALL_EXPLODE , volume: 1f , pitch : 1f);
    }





    // 패턴_5 -----------------------------------------------------------------------------------------------------
    public void ElmFireTrap_LIGHT_function()
    {
        SoundManager.Instance.PlaySound(ElmFireTrap_LIGHT , volume: 0.1f , pitch : 0.8f);
    }


    public void _ElmFireTrap_LIGHT_function()
    {
        SoundManager.Instance.PlaySound(_ElmFireTrap_LIGHT , volume: 0.8f , pitch : 1f);
    }




    // page_2 ----------------------------------------------------------------------------------------------------
    public void SNAKE_THUNDERBOLT_function()
    {
        SoundManager.Instance.PlaySound(SNAKE_THUNDERBOLT , volume: 0.2f , pitch : 1.2f);
    }

    public void CAULDRON_FIRE_function_1()
    {
        SoundManager.Instance.PlaySound(CAULDRON_FIRE , volume: 1f , pitch : 1f);
    }
    
    public void CAULDRON_FIRE_function_2()
    {
        SoundManager.Instance.PlaySound(CAULDRON_FIRE , volume: 1f , pitch : 0.6f);
    }
    

    public void CAULDRON_FIRE_function_3()
    {
        SoundManager.Instance.PlaySound(CAULDRON_FIRE , volume: 1f , pitch : 1.3f);
    }
    



    public void GUARDIAN_APPEAR_function_3()
    {
        SoundManager.Instance.PlaySound(GUARDIAN_APPEAR , volume: 0.7f , pitch : 1f);
    }

    public void GUARDIAN_ATTACK_function_3()
    {
        SoundManager.Instance.PlaySound(GUARDIAN_ATTACK , volume: 0.7f , pitch : 1f);
    }

    public void GUARDIAN_DISAPPEAR_function_3()
    {
        SoundManager.Instance.PlaySound(GUARDIAN_DISAPPEAR , volume: 0.7f , pitch : 1f);
    }


}
