using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_sound : MonoBehaviour
{
    // --------------------------------------------------------------------------------------------------------------------------------
    [Header("player")] 
    public AudioClip PENITENT_HEAVY_DAMAGE; 
    public AudioClip GHOSTKNIGHT_DAMAGE;

    // 둔기
    public void PENITENT_HEAVY_DAMAGE_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_HEAVY_DAMAGE); // , volume: 0.6f
    }

    // 칼날
    public void GHOSTKNIGHT_DAMAGE_function()
    {
        SoundManager.Instance.PlaySound(GHOSTKNIGHT_DAMAGE); // , volume: 0.6f
    }




    // --------------------------------------------------------------------------------------------------------------------------------
    [Header("flying_head")]
    public AudioClip HEAD_EXPLODE;                  
    public AudioClip HEAD_THROWER_DEATH;                   


    public void HEAD_EXPLODE_function()
    {
        SoundManager.Instance.PlaySound(HEAD_EXPLODE , volume: 0.7f); // , volume: 0.6f
    }

    public void HEAD_THROWER_DEATH_function()
    {
        SoundManager.Instance.PlaySound(HEAD_THROWER_DEATH , volume: 0.7f);
    }





    [Header("Lionhead")]
    public AudioClip LEON_DEATH;                  
    public AudioClip LEON_HIT;                   
    public AudioClip LEON_PREATTACK;                  
    public AudioClip LEON_START_ATTACK;  
    public AudioClip LEON_step1;  
    public AudioClip LEON_step2;           


    public void LEON_DEATH_function()
    {
        SoundManager.Instance.PlaySound(LEON_DEATH); // , volume: 0.6f
    }

    public void LEON_HIT_function()
    {
        SoundManager.Instance.PlaySound(LEON_HIT , volume: 0.6f);
    }

    public void LEON_PREATTACK_function()
    {
        SoundManager.Instance.PlaySound(LEON_PREATTACK); // , volume: 0.6f
    }

    public void LEON_START_ATTACK_function()
    {
        SoundManager.Instance.PlaySound(LEON_START_ATTACK , volume: 1f);
    }

    public void LEON_step1_function()
    {
        SoundManager.Instance.PlaySound(LEON_step1); // , volume: 0.6f
    }

    public void LEON_step2_function()
    {
        SoundManager.Instance.PlaySound(LEON_step2);
    }







    [Header("menina")]
    public AudioClip MENINA_IDLE_3;                  
    public AudioClip MENINA_IDLE_4;                   
    public AudioClip MENINA_PREATTACK;                  
    public AudioClip MENINA_ATTACK_MOVE_1;  
    public AudioClip MENINA_ATTACK_MOVE_2;  
    public AudioClip MENINA_DEATH;
   


    public void MENINA_IDLE_3_function()
    {
        SoundManager.Instance.PlaySound(MENINA_IDLE_3); // , volume: 0.6f
    }

    public void MENINA_IDLE_4_function()
    {
        SoundManager.Instance.PlaySound(MENINA_IDLE_4);
    }

    public void MENINA_PREATTACK_function()
    {
        SoundManager.Instance.PlaySound(MENINA_PREATTACK); // , volume: 0.6f
    }

    public void MENINA_ATTACK_MOVE_1_function()
    {
        SoundManager.Instance.PlaySound(MENINA_ATTACK_MOVE_1);
    }

    public void MENINA_ATTACK_MOVE_2_function()
    {
        SoundManager.Instance.PlaySound(MENINA_ATTACK_MOVE_2);
    }

    public void MENINA_DEATH_function()
    {
        SoundManager.Instance.PlaySound(MENINA_DEATH);
    }





    [Header("bishop")]
    public AudioClip BISHOP_ATTACK;                  
    public AudioClip BISHOP_DEATH;                   
    public AudioClip BISHOP_PRE_ATTACK;                  

   


    public void BISHOP_ATTACK_function()
    {
        SoundManager.Instance.PlaySound(BISHOP_ATTACK); // , volume: 0.6f
    }

    public void BISHOP_DEATH_function()
    {
        SoundManager.Instance.PlaySound(BISHOP_DEATH);
    }

    public void BISHOP_PRE_ATTACK_function()
    {
        SoundManager.Instance.PlaySound(BISHOP_PRE_ATTACK); // , volume: 0.6f
    }








    [Header("ghost")]
    public AudioClip BELLGHOST_APPEARING;                  
    public AudioClip BELLGHOST_DEATH_DEFAULT;                   
    public AudioClip GHOST_HURT_DEFAULT;
    public AudioClip GHOST_SHOOT;

   


    public void BELLGHOST_APPEARING_function()
    {
        SoundManager.Instance.PlaySound(BELLGHOST_APPEARING); // , volume: 0.6f
    }

    public void BELLGHOST_DEATH_DEFAULT_function()
    {
        SoundManager.Instance.PlaySound(BELLGHOST_DEATH_DEFAULT);
    }

    public void GHOST_HURT_DEFAULT_function()
    {
        SoundManager.Instance.PlaySound(GHOST_HURT_DEFAULT); // , volume: 0.6f
    }

    public void GHOST_SHOOT_function()
    {
        SoundManager.Instance.PlaySound(GHOST_SHOOT);
    }







    [Header("energy_ball")]
    public AudioClip GHOST_ENERGY_BALL;                  
    public AudioClip TOXIC_BALL_EXPLODE;                   
         


    public void GHOST_ENERGY_BALL_function()
    {
        SoundManager.Instance.PlaySound(GHOST_ENERGY_BALL); // , volume: 0.6f
    }

    public void TOXIC_BALL_EXPLODE_function()
    {
        SoundManager.Instance.PlaySound(TOXIC_BALL_EXPLODE);
    }






    [Header("isabel")]
    public AudioClip Reverse_ISABEL_DEATH;
    public AudioClip _ISABEL_STEP_ROCKS_1;                  
    public AudioClip _ISABEL_STEP_ROCKS_2;                   
    public AudioClip ISABEL_ATTACK;     
    public AudioClip ISABEL_DEATH;
    public AudioClip ISABEL_IDLE_1;
    public AudioClip ISABEL_IDLE_4;



    public void Reverse_ISABEL_DEATH_function()
    {
        SoundManager.Instance.PlaySound(Reverse_ISABEL_DEATH); // , volume: 0.6f
    }

    public void _ISABEL_STEP_ROCKS_1_function()
    {
        SoundManager.Instance.PlaySound(_ISABEL_STEP_ROCKS_1);
    }

    public void _ISABEL_STEP_ROCKS_2_function()
    {
        SoundManager.Instance.PlaySound(_ISABEL_STEP_ROCKS_2);
    }

    public void ISABEL_ATTACK_function()
    {
        SoundManager.Instance.PlaySound(ISABEL_ATTACK);
    }

    public void ISABEL_DEATH_function()
    {
        SoundManager.Instance.PlaySound(ISABEL_DEATH);
    }

    public void ISABEL_IDLE_1_function()
    {
        SoundManager.Instance.PlaySound(ISABEL_IDLE_1);
    }

    public void ISABEL_IDLE_4_function()
    {
        SoundManager.Instance.PlaySound(ISABEL_IDLE_4);
    }

}
