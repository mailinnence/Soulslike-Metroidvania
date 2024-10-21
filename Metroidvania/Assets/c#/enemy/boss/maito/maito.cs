using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.IO;

public class maito : enemy_move
{


    public energyHp energyHp;

    [Header("카메라 진동")]
    public CameraShake CameraShake;

    [Header("상호작용 구간 , 벡터 , 레이어")]
    public Transform interactionArea;
    public Vector2 interactionArea_;             
    public LayerMask interactionLayer; 
    private Vector3 playerPosition;


    [Header("고스트 공격")]
    public Transform g_area_left;
    public Vector2 g_area_left_;             
    public Transform g_area_right; 
    public Vector2 g_area_right_;
    public int filpx;
    public bool filpx_bool;
    




    [Header("패턴 오브젝트")]
    public GameObject fire_ready;
    public GameObject fire_death;
    public GameObject fire_ball;
    public GameObject spear;
    public GameObject fire_lion;
    public GameObject ghost_king_;
    public GameObject beam;
    public GameObject bolt;


    [Header("페이지2")]
    public GameObject left_ladder;                      
    public GameObject right_ladder;  

    public page2 left_ladder_;
    public page2 right_ladder_;                    
    public page_2_fire fire_1;
    public page_2_fire fire_2;
    public page_2_fire fire_3;
    public page_2_fire fire_4;
    public page2_back fire_5;



    [Header("페이지 관리")]
    public bool page_1_start;
    public bool page_2_start;
    public bool page_1_start_for;


    private bool object_off_;


    // 색깔
    private Renderer objectRenderer;
    private Color originalColor;



    private AudioSource audioSource1;
    public AudioClip clip1;

    
    public bool one_var;
    public bool one_start;

    [Header("win")] 
    public AudioClip PENITENT_CRITICAL_HIT_2;
    public AudioClip Boss_Fight_Ending;
    public event_background event_background;
    public effectSound effectSound;


    // Start is called before the first frame update
    void Start()
    {

        audioSource1 = GetComponent<AudioSource>();
        if (audioSource1 != null) {audioSource1.clip = clip1;}


      // Renderer 컴포넌트를 가져옵니다.
        objectRenderer = GetComponent<Renderer>();

        // 오브젝트의 원래 색상을 저장합니다.
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }


       
        // 생존
        alive = true;

        // pattern_1    불 기둥 공격
        // fire();                             
        
        // pattern_2    불 기둥 즉사 공격
        // fire_death_start();                  
        
        // pattern_3    파이어 볼 + 3스피어
        // fire_ball_type_1();                          
        // spear_type_1();

        // pattern_4    스피어 난사
        // spear_type_2();
       
        // pattern_5    불사자
        // fire_lion_();

        // pattern_6    고스트 킹
        // ghost_king_start();

        // pattern_7    빔  
        // beam_attack_two();

        // page_2       청색 유황 + 사다리 + 지면 불바다
        // page_2_In();
        // page_2_fire_In();
        // bolting();



    //    StartCoroutine(FireRoutine());       // 코루틴 확인

    }


    
    IEnumerator start_attack_boss()
    {
        yield return new WaitForSeconds(2f);
        page_1_start = true;
    }



    // Update is called once per frame
    void Update()
    {

        if(boss_2_start && !one_start)
        {
            one_start = true;
            StartCoroutine(start_attack_boss());
        }


        if(!inventory && page_1_start)
        {
            StartCoroutine(page_1());
        }
        if(!inventory && page_2_start)
        {
            StartCoroutine(page_2());
        } 



        if (!boss_2) 
        {
            object_off_ = true;
            if(!one_var)
            {
                boss_clear_2 = true;
                one_var = true;
                SoundManager.Instance.PlaySound(PENITENT_CRITICAL_HIT_2 , volume: 0.8f , pitch : 1f);
                StartCoroutine(ChangeColorCoroutine());
            }
        }


        if(object_off_)
        {
            page_1_start = false;
            page_1_start_for = false;
        }

        
    }


    public void page_2_pattern()
    {
        page_1_start = false;
        page_1_start_for = false;
        page_2_start = true;
    }



    IEnumerator page_1()
    {
        page_1_start = false;
        page_1_start_for = true;
        while (page_1_start_for)
        {
            float pattern_delay_time = 0f;
            int randomNumber = Random.Range(1, 8);

            // 1에서 7까지의 숫자로 리스트 생성
            List<int> numbers = Enumerable.Range(1, 7).ToList();

            // 리스트를 랜덤하게 섞기
            numbers = numbers.OrderBy(x => Random.Range(0, numbers.Count)).ToList();

            foreach (int number in numbers)
            {
                if(!page_1_start_for)
                {
                    break;
                }
    
                if(number == 1)       { fire();                               pattern_delay_time = 4.5f;  }
                else if(number == 2)  { fire_death_start();                   pattern_delay_time = 8f;    }
                else if(number == 3)  { fire_ball_type_1(); spear_type_1();   pattern_delay_time = 10f;   }
                else if(number == 4)  { spear_type_2();                       pattern_delay_time = 8f;    }
                else if(number == 5)  { fire_lion_();                         pattern_delay_time = 5f;    }
                else if(number == 6)  { ghost_king_start();                   pattern_delay_time = 6f;    }
                else if(number == 7)  { beam_attack_two();                    pattern_delay_time = 5f;    }       
                yield return new WaitForSeconds(pattern_delay_time);
            }
        }
    }



    IEnumerator page_2()
    {
        
        page_2_start = false;
        page_1_start_for = false;
  
        page_2_In();
        yield return new WaitForSeconds(8f);
        page_2_fire_In();

        yield return new WaitForSeconds(3f);
        bolting();

        yield return new WaitForSeconds(15f); 
        page_2_fire_Out();
        
        yield return new WaitForSeconds(5f); 
        page_2_Out();

        page_1_start = true;
    }








    IEnumerator FireRoutine()
    {
        while (true)
        {
            //fire();
            ghost_king();
            // 3초 대기
            yield return new WaitForSeconds(3f);
        }
    }







    void patter_list()
    {
        // 1에서 7까지의 숫자로 리스트 생성
        List<int> numbers = Enumerable.Range(1, 7).ToList();

        // 리스트를 랜덤하게 섞기
        numbers = numbers.OrderBy(x => Random.Range(0, numbers.Count)).ToList();

    }



    // 보스 사망시 공격 오브젝트 생성 제거
    public void SetAllPatternsToNull()
    {
        fire_ready = null;
        fire_death = null;
        fire_ball = null;
        spear = null;
        fire_lion = null;
        ghost_king_ = null;
        beam = null;
        bolt = null;
    }




    // 불 기둥 공격 ------------------------------------------------------

    // 불 기둥 함수
    void fire()
    {
        StartCoroutine(Fire_pattern());
    }


    IEnumerator Fire_pattern()
    {
        fire1();
        yield return new WaitForSeconds(1.5f);
        fire2();     
    }



    // 종류.1
    void fire1()
    {
        float[] xOffsets = {  -5f, -10f, -15f, 0f, 5f, 10f, 15f };
        fire_ready_function(xOffsets);
    }

    // 종류.2
    void fire2()
    {
        float[] xOffsets = { -2.5f, -7.5f, -12.5f, 2.5f, 7.5f, 12.5f };
        fire_ready_function(xOffsets);
    }



    // 불 기둥 준비 위치
    void fire_ready_function(float[] xOffsets)
    {
        if(!object_off_)
        {
            foreach (float xOffset in xOffsets)
            {
                Vector3 offset = new Vector3(xOffset, -0.5f, 0f);
                GameObject effectInstance = Instantiate(fire_ready, transform.position + offset, transform.rotation);
            }

        }
       
    }




    // 불 기둥 즉사 공격 ------------------------------------------------------

    void fire_death_start()
    {
        StartCoroutine(FireRoutine_death());
    }
        

    IEnumerator FireRoutine_death()
    {
        for (int i = 0; i < 3; i++)
        {
            // fire();
            fire_death_();
            // 5초 대기
            yield return new WaitForSeconds(2.5f);
        }
    }



    // 불 기둥 함수
    void fire_death_()
    {
        // 경고음
        Vector3 currentPosition = transform.position;
        energyHp.Instant_Death_Detection(currentPosition);

        float[] xOffsets = {  -1.3f, -2.6f, 0f, 1.3f, 2.6f };
        fire_death_function(xOffsets);
    }



    // // 불 기둥 시작
    void fire_death_function(float[] xOffsets)
    {
        // 플레이어 탐지
        transform_function();

        foreach (float xOffset in xOffsets)
        {
            Vector3 playerPositionX = new Vector3(playerPosition.x, -47.28f, 0f);

            Vector3 offset = new Vector3(xOffset, -0.5f, 0f);
            GameObject effectInstance = Instantiate(fire_death, playerPositionX + offset, transform.rotation);
        }
    }







    // 파이어 볼 , 스피어 -----------------------------------------------------------------------------------------------------
    // 불 기둥 준비 위치


    void fire_ball_type_1()
    {
        StartCoroutine(fire_ball_delay());
    }




    IEnumerator fire_ball_delay()
    {
        if(!object_off_)
        {
            for (int i = 0; i < 16; i++)
            {
                fire_ball_(i*20f);
            
                yield return new WaitForSeconds(0.5f);
            }
        }

    }






    void fire_ball_(float angle)
    {
        Vector3 offset = new Vector3(-10f, 9f, 0f);

        // 예시: z축을 기준으로 45도 회전
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject effectInstance = Instantiate(fire_ball, transform.position + offset, rotation);
    }





    // 스피어 -----------------------------------------------------------------------------------------------
    void spear_type_1()
    {
        StartCoroutine(spear_type_1_());
    }


    IEnumerator spear_type_1_()
    {
  
        Vector3 offset1 = new Vector3(10, 9f, 0f);
        GameObject effectInstance1 = Instantiate(spear, transform.position + offset1, transform.rotation);
        yield return new WaitForSeconds(0.4f);
        
        Vector3 offset2 = new Vector3(12, 6f, 0f);
        GameObject effectInstance2 = Instantiate(spear, transform.position + offset2, transform.rotation);
        yield return new WaitForSeconds(0.2f);

        Vector3 offset3 = new Vector3(8, 8f, 0f);
        GameObject effectInstance3 = Instantiate(spear, transform.position + offset3, transform.rotation);


    }



    void spear_type_2()
    {
        StartCoroutine(spear_type_2_());
    }


    IEnumerator spear_type_2_()
    {
        // 오프셋 값을 배열에 저장
        Vector3[] offsets = new Vector3[]
        {
            new Vector3(-15, 8f, 0f),
            new Vector3(-12, 9f, 0f),
            new Vector3(-8, 8f, 0f),
            new Vector3(-6, 8f, 0f),
            new Vector3(-3, 9f, 0f),
            new Vector3(0, 9f, 0f),
            new Vector3(3, 8f, 0f),
            new Vector3(6, 8f, 0f),
            new Vector3(9, 9f, 0f),
            new Vector3(12, 8f, 0f)
        };

        // 반복문을 사용하여 오프셋과 대기 시간 적용
        foreach (Vector3 offset in offsets)
        {
            Instantiate(spear, transform.position + offset, transform.rotation);
            yield return new WaitForSeconds(0.4f);
        }
    }






    // 불사자 -----------------------------------------------------------------------------------------------

    void fire_lion_()
    {
        Vector3 currentPosition = new Vector3(-510, 0f, 0f);
        energyHp.Instant_Death_Detection(currentPosition);
        StartCoroutine(fire_lion_delay());
    }


    IEnumerator fire_lion_delay()
    {
        // 오프셋 값을 배열에 저장
        Vector3[] offsets = new Vector3[]
        {
            new Vector3(-15, 7f, 0f),
            new Vector3(-12, 6f, 0f),
            new Vector3(-8, 5f, 0f),
            new Vector3(-6, 4f, 0f),
            new Vector3(-3, 3f, 0f),
            new Vector3(0, 2f, 0f),
            new Vector3(3, 3f, 0f),
            new Vector3(6, 4f, 0f),
            new Vector3(9, 5f, 0f),
            new Vector3(12, 6f, 0f)
        };

        // 반복문을 사용하여 오프셋과 대기 시간 적용
        foreach (Vector3 offset in offsets)
        {
            Instantiate(fire_lion, transform.position + offset, transform.rotation);
            yield return new WaitForSeconds(0.4f);
            StartCoroutine(CameraShake_());
        }
    }


    
    IEnumerator CameraShake_()
    {
        yield return new WaitForSeconds(0.02f);
        CameraShake.TriggerShake(7f, 5.5f, 0.16f);
    }









    // beam -----------------------------------------------------------------------------------------------------------------
    // 불 기둥 시작
    
    
    void beam_attack_two()
    {
        StartCoroutine(beam_attack_two_delay());
    }


    IEnumerator beam_attack_two_delay()
    {
        for (int i = 0; i < 2; i++)
        {
            // 경고음
            Vector3 currentPosition = transform.position;
            energyHp.Instant_Death_Detection(currentPosition);

            beam_attack();
            yield return new WaitForSeconds(2f);

        }
    }

    void beam_attack()
    {

        Vector3 offset = new Vector3(-14.5f, 3f, 0f);
        GameObject effectInstance = Instantiate(beam, transform.position + offset, transform.rotation);

        Vector3 offset1 = new Vector3(13f, 3f, 0f);
        GameObject effectInstance1 = Instantiate(beam, transform.position + offset1, transform.rotation);

        // 생성된 오브젝트의 SpriteRenderer 컴포넌트를 가져옵니다.
        SpriteRenderer spriteRenderer_e = effectInstance1.GetComponent<SpriteRenderer>();
        spriteRenderer_e.flipX = true;

    }










    // 고스트 킹 공격 -----------------------------------------------------------------------------------------------------


    void ghost_king_start()
    {
        StartCoroutine(ghost_king_three());
    }
        
    // 3초 간격으로 3번 반복
    IEnumerator ghost_king_three()
    {
        for (int i = 0; i < 3; i++)
        {
            ghost_king();
            yield return new WaitForSeconds(3f);
        }
    }


    // 고스트 킹 공격 위치 - 측면일떄는 카메라를 넘어가지 않게 측면의 반대쪽으로 측면이 아닐경우 번갈아가면서 공격
    void ghost_king()
    {
        // 현재 플레이어의 위치 선정
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);
        if (objectsToHit.Length >=1)
        {
            playerPosition = objectsToHit[0].transform.position;
        }

        // 측면 처리
        Collider2D[] objectsToHit_left = Physics2D.OverlapBoxAll(g_area_left.position, g_area_left_, 0, interactionLayer);
        Collider2D[] objectsToHit_right = Physics2D.OverlapBoxAll(g_area_right.position, g_area_left_, 0, interactionLayer);


        // 위치 설정
        if (objectsToHit_left.Length >=1)
        {
            filpx_bool = false;
            filpx = 0;
        }
   
        else if (objectsToHit_right.Length >=1)
        {
            filpx_bool = false;
            filpx = 1;
        }
    
        else if(objectsToHit_left.Length == 0 && objectsToHit_right.Length == 0)
        {
            if(!filpx_bool) 
            { 
                filpx = Random.Range(0, 2);
            }
            
            filpx_bool = true;
            
        }

        // 생성
        if(filpx == 0)
        {
            Vector3 offset = new Vector3(4f, -0.5f, 0f);
            GameObject effectInstance = Instantiate(ghost_king_, playerPosition + offset, transform.rotation);

            SpriteRenderer spriteRenderer_e = effectInstance.GetComponent<SpriteRenderer>();
            spriteRenderer_e.flipX = true;
            filpx = 1;

        }

        else if(filpx == 1)
        {
            Vector3 offset = new Vector3(-4f, -0.5f, 0f);
            GameObject effectInstance = Instantiate(ghost_king_, playerPosition + offset, transform.rotation);
            filpx = 0;
        }



    }










    // page_2 번개 ------------------------------------------------------------------------------------------
    void bolting()
    {
        StartCoroutine(bolting_three());

    }


        
    // 3초 간격으로 3번 반복
    IEnumerator bolting_three()
    {

        Vector3 offset1 = new Vector3(-4.3f, 00f, 0f);
        GameObject effectInstance1 = Instantiate(bolt, transform.position + offset1, transform.rotation);
        
        yield return new WaitForSeconds(4f);        
        Vector3 offset2 = new Vector3(4.3f, 0f, 0f);
        GameObject effectInstance2 = Instantiate(bolt, transform.position + offset2, transform.rotation);

        yield return new WaitForSeconds(4f);
        Vector3 offset3 = new Vector3(-4.3f, 0f, 0f);
        GameObject effectInstance3 = Instantiate(bolt, transform.position + offset3, transform.rotation);                

        yield return new WaitForSeconds(4f);
        Vector3 offset4 = new Vector3(4.3f, 0f, 0f);
        GameObject effectInstance4 = Instantiate(bolt, transform.position + offset4, transform.rotation);
    }



    // page_2 ----------------------------------------------------------------------------------------------------------

    public void page_2_In()
    {
      
        left_ladder.SetActive(true);
        right_ladder.SetActive(true);

        fadeFloat = 0f;
        fadeSpeed = 0.1f; 


        left_ladder_.isFadingOut = true;
        right_ladder_.isFadingOut= true;  


    }
        
    


    public void page_2_fire_In()
    {

        fadeFloat_fire = 0f;
        fadeSpeed_fire = 0.05f; 
      
        fire_1.isFadingOut_ = true;
        fire_2.isFadingOut_ = true;
        fire_3.isFadingOut_ = true;
        fire_4.isFadingOut_ = true;


        fadeFloat_fire_back = 0f;
        fadeSpeed_fire_back = 0.05f; 

        fire_5.isFadingOut_ = true;

        audioSource1.loop = true;
        audioSource1.Play();
    }



   





    



    public void page_2_Out()
    {
        left_ladder_.isFadingOut = false;
        right_ladder_.isFadingOut = false;
        audioSource1.Stop();
        StartCoroutine(page_2_Out_delay());
    }


   // 3초 간격으로 3번 반복
    IEnumerator page_2_Out_delay()
    {
        yield return new WaitForSeconds(4f);       
        energyHp.ladder_damage_or_death(); 
        left_ladder.SetActive(false);
        right_ladder.SetActive(false);
    }





    public void page_2_fire_Out()
    {
        fire_1.isFadingOut_ = false;
        fire_2.isFadingOut_ = false;
        fire_3.isFadingOut_ = false;
        fire_4.isFadingOut_ = false;

    }



    




    // 보스 사망 콜라이더 제거 ----------------------------------------------------------------------------------------
    public void collider_off()
    {
        CapsuleCollider.enabled= false;
    }





    // 플레이어 탐지 함수 -------------------------------------------------------------------------------------------------------
    void transform_function()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);

        if (objectsToHit.Length >=1)
        {
            playerPosition = objectsToHit[0].transform.position;
        }
   
    }




    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        // 플레이어 감지 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(interactionArea.position , interactionArea_);
   
        // 플레이어 감지 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(g_area_left.position , g_area_left_);
        Gizmos.DrawWireCube(g_area_right.position , g_area_right_);

    }

  



    // 색상을 변경하는 Coroutine
    private IEnumerator ChangeColorCoroutine()
    {
        if (objectRenderer != null)
        {
            // boss_clear_2 = false;
            // 색상을 검은색으로 변경합니다.
            objectRenderer.material.color = Color.black;

            // 1초 동안 대기합니다.
            yield return new WaitForSeconds(0.01f);

            // 원래 색상으로 복원합니다.
            objectRenderer.material.color = originalColor;

            yield return new WaitForSeconds(7f);
            SoundManager.Instance.PlaySound(Boss_Fight_Ending , volume: 0.8f , pitch : 1f);
        
            yield return new WaitForSeconds(10f);
            event_background.Fade_white(1.5f , 1.5f);

            yield return new WaitForSeconds(3f);
            effectSound.MIRIAM_PORTAL_CHALLENGE_function();
            SceneManager.LoadScene("event_ 2");
            player_setting_init();
        }
    }






    public void player_setting_init()
    {
        damaged = false;              
        damagedMove = false;          
        isSlope = false;                       
        jumpAble = true;                       
        jump_verticalattack = false;       
        jump_ghost = false;        
        jump_high = 0;          
        gravity_anim_ = false;          
        gravity_hit = false;
        high_landing_ = false;   
        slidingExcept = false;        
        isSliding = false;            
        isSliding_stabbing = false;            
        stabbing_ = false;   
        airdooring = false;
        acting = false;
        actingButMove = false;
        alive = true;      
        hurt = false;            
        attacking = false;                             
        attackAble = true;                                        
        itemUsingState = false;                        
        textAble = false;                                      
        isLadder = false;             
        isLadderDown = false;           
        DownKeyLimit = false;            
        hangAble = false;
        parrying_action = false;
        parrying_counter = false;
    }




}
