using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectSound : MonoBehaviour
{


    [Header("사다리 ")]
    public AudioClip _PENITENT_CLIMB_LADDER_1;                  
    public AudioClip _PENITENT_CLIMB_LADDER_2;                   
    public AudioClip _PENITENT_CLIMB_LADDER_3;                   



    [Header("부서지는 바닥")]
    public AudioClip FLOOR_STONE_BREAK;                  
    public AudioClip GET_FLOOR_ITEM;                   


    [Header("벽에 칼 꽂기")]
    public AudioClip WALL_CLIMB_GRAB;                  
    public AudioClip WALL_CLIMB_OFF;                  


    [Header("함정 즉사")]
    public AudioClip PENITENT_SPIKES_DEATH;                  


    [Header("절벽")]
    public AudioClip Penitent_EdgeGrab;                
    public AudioClip Penitent_EdgeClimb;                  


    [Header("내려찍기")]
    public AudioClip VERTICAL_ATTACK_START;                   
    public AudioClip VERTICAL_ATTACK_FALL;                   
    public AudioClip VERTICAL_ATTACK_HIT_LV3;                 
    
  


    [Header("기모아서 공격 관련 효과음")]
    public AudioClip Charging_start;                   // 기 모으기
    public AudioClip Charging_ready;                   // 기 모으기 완료
    public AudioClip Charging_shot;                    // 기 모으기 날리기
    
  
    
    [Header("데미지를 받았을 때 관련 효과음")]
    public AudioClip PENITENT_DEATH_DEFAULT;            // 죽는 애니메이션
    public AudioClip PENITENT_SIMPLE_DAMAGE_DEFAULT;    // 데미지를 받았을때


    [Header("기본 칼날 효과음")]
    public AudioClip PENITENT_SLASH_AIR_1;  // 콤비1
    public AudioClip PENITENT_SLASH_AIR_2;  // 콤비2
    public AudioClip PENITENT_SLASH_AIR_3;  // 콤비3
    public AudioClip AMANECIDA_SWORD_ATTACK;  // 찌르기 피격

    [Header("기본 칼날 피격 효과음")]
    public AudioClip PENITENT_ENEMY_HIT_1;  // 피격1
    public AudioClip PENITENT_ENEMY_HIT_2;  // 피격2
    public AudioClip PENITENT_ENEMY_HIT_3;  // 피격3
    
    



    [Header("이동 관련 효과음")]
    public AudioClip PENITENT_RUN_MARBLE_6;     // 걷기1
    public AudioClip PENITENT_RUN_MARBLE_8;     // 걷기2
    public AudioClip PENITENT_JUMP;             // 점프
    public AudioClip PENITENT_DASH;             // 대쉬
    public AudioClip PENITENT_LANDING_MARBLE;   // 착지 
    public AudioClip jump2effectSound;          // 2단 점프 이펙트
    public AudioClip airdash;                   // 에어대쉬
    public AudioClip HARD_LANDING;              // 높은 곳에서 떨어졌을때

    [Header("회복 관련 효과음")]
    public AudioClip flask_sound;               // 회복 
    public AudioClip flask_lack_sound;          // 아이템 부족




    [Header("기도대 관련 효과음")]
    public AudioClip prayTable_Charging;               // 기도대 충전
    public AudioClip prayTable_knee_down;              // 기도대 휴식
    public AudioClip prayTable_knee_up;                // 기도대 휴식종류
    

    [Header("패링 관련 효과음")]
    public AudioClip parrying_start;              
    public AudioClip parrying_success_short;
    public AudioClip parrying_success_counter;              
    public AudioClip parrying_counter;                
    

    [Header("아이템 관련 효과음")]         
    public AudioClip Prayer_collected;




    [Header("장소 관련 효과음")]         
    public AudioClip ZONE_INFO;
    public AudioClip PENITENT_KNEEL_DOWN;
    public AudioClip MIRIAM_PORTAL_CHALLENGE;
    public AudioClip MIRIAM_PORTAL_reverse;


    [Header("기다리기 관련 효과음")]         
    public AudioClip PENITENT_START_TALK;
    public AudioClip PENITENT_END_TALK;



    [Header("공격 받았을떄")]         
    public AudioClip PENITENT_SIMPLE_DAMAGE_damaged;
    public AudioClip GHOSTKNIGHT_DAMAGE;






    [Header("스킬 관련 효과음")]
    public AudioClip skill_ready;                   // 스킬 시전 준비
    // 데보라
    public AudioClip debla_explode;                 // 데보라 폭발음
    public AudioClip debla_hit;                     // 데보라 피격효과음


    // 이동 관련 효과음 --------------------------------------------------------------------------------------
    // 걷기 효과음
    public void PENITENT_RUN_MARBLE_6_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_RUN_MARBLE_6 , volume: 4f);
    }

    public void PENITENT_RUN_MARBLE_8_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_RUN_MARBLE_8 , volume: 4f);
    }

    // 점프 효과음
    public void PENITENT_JUMP_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_JUMP , volume: 0.4f);
    }

    // 2단 점프 이펙트 효과음
    public void jump2effectSound_function()
    {
        SoundManager.Instance.PlaySound(jump2effectSound, volume: 0.3f, pitch: 0.5f); 
    }

    // 에어대쉬 효과음
    public void airdash_function()
    {
        SoundManager.Instance.PlaySound(airdash, volume: 0.3f); 
    }
 
    // 착지 효과음
    public void PENITENT_LANDING_MARBLE_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_LANDING_MARBLE );
    }

    // 높은 곳에서 떨여졌을때 효과음
    public void HARD_LANDING_function()
    {
        SoundManager.Instance.PlaySound(HARD_LANDING );
    }



    // 대쉬 효과음
    public void PENITENT_DASH_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_DASH);
    }


    // -------------------------------------------------------------------------------------------------------




    // 데미지를 받았을 때 관련 효과음 ---------------------------------------------------------------------------
    // 죽음 애니메이션 효과음
    public void PENITENT_DEATH_DEFAULT_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_DEATH_DEFAULT);
    }


    // 데미지 받았을때 효과음
    public void PENITENT_SIMPLE_DAMAGE_DEFAULT_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_SIMPLE_DAMAGE_DEFAULT , volume: 0.4f);
    }
    // -------------------------------------------------------------------------------------------------------



    

    // 기본 칼날 효과음 --------------------------------------------------------------------------------------
    public void PENITENT_SLASH_AIR_1_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_SLASH_AIR_1);
    }

    public void PENITENT_SLASH_AIR_2_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_SLASH_AIR_2);
    }

    public void PENITENT_SLASH_AIR_3_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_SLASH_AIR_3);
    }

    // 찌르기
    public void AMANECIDA_SWORD_ATTACK_function()
    {
        SoundManager.Instance.PlaySound(AMANECIDA_SWORD_ATTACK  );
    }
    // -------------------------------------------------------------------------------------------------------

  


      // 기본 칼날 피격 효과음 --------------------------------------------------------------------------------------
    public void PENITENT_ENEMY_HIT_1_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_ENEMY_HIT_1);
    }

    public void PENITENT_ENEMY_HIT_2_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_ENEMY_HIT_2);
    }

    public void PENITENT_ENEMY_HIT_3_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_ENEMY_HIT_3);
    }
    // -------------------------------------------------------------------------------------------------------





    // 회복 효과음 --------------------------------------------------------------------------------------
    public void recovery_function()
    {
        SoundManager.Instance.PlaySound(flask_sound);
    }

    public void recoveryFail_function()
    {
        SoundManager.Instance.PlaySound(flask_lack_sound);
    }

    // -------------------------------------------------------------------------------------------------------




    // 기도대 관련 효과음 --------------------------------------------------------------------------------------
    public void prayTable_Charging_function()
    {
        SoundManager.Instance.PlaySound(prayTable_Charging);
    }

    public void prayTable_knee_down_function()
    {
        SoundManager.Instance.PlaySound(prayTable_knee_down);
    }

    public void prayTable_knee_up_function()
    {
        SoundManager.Instance.PlaySound(prayTable_knee_up);
    }
    // -------------------------------------------------------------------------------------------------------





    // 기 모아서 공격 효과음 --------------------------------------------------------------------------------------
    public void Charging_start_function()
    {
        SoundManager.Instance.PlaySound(Charging_start);
    }

    public void Charging_ready_function()
    {
        SoundManager.Instance.PlaySound(Charging_ready);
    }

    public void Charging_shot_function()
    {
        SoundManager.Instance.PlaySound(Charging_shot);
    }
 
 
    // -------------------------------------------------------------------------------------------------------






    // 점프 내려찍기 공격 효과음 --------------------------------------------------------------------------------------
    public void VERTICAL_ATTACK_START_function()
    {
        SoundManager.Instance.PlaySound(VERTICAL_ATTACK_START);
    }

    public void VERTICAL_ATTACK_FALL_function()
    {
        SoundManager.Instance.PlaySound(VERTICAL_ATTACK_FALL);
    }

    public void VERTICAL_ATTACK_HIT_function()
    {
        SoundManager.Instance.PlaySound(VERTICAL_ATTACK_HIT_LV3);
    }
    // -------------------------------------------------------------------------------------------------------









    // 절벽 상호작용 공격 효과음 --------------------------------------------------------------------------------------
    public void Penitent_EdgeGrab_function()
    {
        SoundManager.Instance.PlaySound(Penitent_EdgeGrab);
    }

    public void Penitent_EdgeClimb_function()
    {
        SoundManager.Instance.PlaySound(Penitent_EdgeClimb);
    }
    // -------------------------------------------------------------------------------------------------------




    // 함정 즉사 효과음 --------------------------------------------------------------------------------------
    public void PENITENT_SPIKES_DEATH_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_SPIKES_DEATH);
    }
    // -------------------------------------------------------------------------------------------------------




    // 벽에 칼 꽂기 즉사 효과음 --------------------------------------------------------------------------------------
    public void WALL_CLIMB_GRAB_function()
    {
        SoundManager.Instance.PlaySound(WALL_CLIMB_GRAB);
    }

    public void WALL_CLIMB_OFF_function()
    {
        SoundManager.Instance.PlaySound(WALL_CLIMB_OFF);
    }
    // -------------------------------------------------------------------------------------------------------




    // 부서지는 바닥 효과음 --------------------------------------------------------------------------------------
    public void FLOOR_STONE_BREAK_function()
    {
        SoundManager.Instance.PlaySound(FLOOR_STONE_BREAK , volume: 0.1f);
    }

    public void GET_FLOOR_ITEM_function()
    {
        SoundManager.Instance.PlaySound(GET_FLOOR_ITEM , volume: 0.5f);
    }
    // -------------------------------------------------------------------------------------------------------






    // 사다리 효과음 --------------------------------------------------------------------------------------
    public void _PENITENT_CLIMB_LADDER_1_function()
    {
        SoundManager.Instance.PlaySound(_PENITENT_CLIMB_LADDER_1 , volume: 0.1f);
    }

    public void  _PENITENT_CLIMB_LADDER_function()
    {
        SoundManager.Instance.PlaySound(_PENITENT_CLIMB_LADDER_2 , volume: 0.5f);
    }

    public void  _PENITENT_CLIMB_LADDER_3_function()
    {
        SoundManager.Instance.PlaySound(_PENITENT_CLIMB_LADDER_3 , volume: 0.5f);
    }
    // -------------------------------------------------------------------------------------------------------




    // 패링 효과음 --------------------------------------------------------------------------------------
    public void parrying_start_function()
    {
        SoundManager.Instance.PlaySound(parrying_start );
    }

    public void  parrying_success_short_function()
    {
        SoundManager.Instance.PlaySound(parrying_success_short );
    }

    public void  parrying_success_counter_function()
    {
        SoundManager.Instance.PlaySound(parrying_success_counter );
    }

    public void  parrying_counter_function()
    {
        SoundManager.Instance.PlaySound(parrying_counter );  //, volume: 0.5f
    }
    // -------------------------------------------------------------------------------------------------------



    // 아이템 효과음 --------------------------------------------------------------------------------------
    public void GET_ITEM_function()
    {
        SoundManager.Instance.PlaySound(GET_FLOOR_ITEM , volume: 0.8f);
    }

    public void Prayer_collected_function()
    {
        SoundManager.Instance.PlaySound(Prayer_collected );
    }
    // -------------------------------------------------------------------------------------------------------






    // 장소 이동 효과음 --------------------------------------------------------------------------------------
    public void ZONE_INFO_function()
    {
        SoundManager.Instance.PlaySound(ZONE_INFO );
    }

    public void PENITENT_KNEEL_DOWN_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_KNEEL_DOWN );
    }

    public void MIRIAM_PORTAL_CHALLENGE_function()
    {
        SoundManager.Instance.PlaySound(MIRIAM_PORTAL_CHALLENGE );
    }

    public void MIRIAM_PORTAL_reverse_function()
    {
        SoundManager.Instance.PlaySound(MIRIAM_PORTAL_reverse );
    }
    
    // -------------------------------------------------------------------------------------------------------





    // 기다리기 이동 효과음 --------------------------------------------------------------------------------------
    public void PENITENT_START_TALK_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_START_TALK );
    }

    public void PENITENT_END_TALK_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_END_TALK );
    }
    // -------------------------------------------------------------------------------------------------------






    // 공격 받았을떄 효과음 -----------------------------------------------------------------------------------
    public void PENITENT_SIMPLE_DAMAGE_damaged_function()
    {
        SoundManager.Instance.PlaySound(PENITENT_SIMPLE_DAMAGE_damaged );
    }

    public void GHOSTKNIGHT_DAMAGE_function()
    {
        SoundManager.Instance.PlaySound(GHOSTKNIGHT_DAMAGE );
    }
    // -------------------------------------------------------------------------------------------------------







    // 스킬 관련 효과음 --------------------------------------------------------------------------------------
    public void skill_ready_function()
    {
        SoundManager.Instance.PlaySound(skill_ready);
    }

 
    // 데보라
    public void debla_explode_function()
    {
        SoundManager.Instance.PlaySound(debla_explode);
    }

    public void debla_hit_function()
    {
        SoundManager.Instance.PlaySound(debla_hit);
    }
    // -------------------------------------------------------------------------------------------------------






}