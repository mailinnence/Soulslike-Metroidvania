using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface damage
{
    // public void OnCollisionEnter2D(Collision2D collision);
    public void OnDamaged(Vector2 targetPos);
    public void NoneDamged();
    public void energe(string type);
}




public class energyHp : playerStatManager 
{

    [Header("Hp")]
    public hp playerHp;

    [Header("Mp")]
    public mp playerMp;



    [Header("출혈 효과 오브젝트")]
    public Blood Blood;

    [Header("즉사기 감지")]
    public GameObject Instant_Death_Detection_;


    [Header("효과음 변수")]
    public effectSound effectSound;

    // 1.적은 데미지로 이동에 문제가 없는 데미지를 받을 경우
    // 2.적은 데미지로 조금 밀릴 정도만 데미지를 받을 경우
    // 3.큰 데미지를 받아서 멀리 날아가서 눕게 되는 경우


    [Header("공격 피격시 진동 관련 변수")]
    public CameraShake CameraShake;

    public Transform playerTransform;      // 이펙트 위치 변수


    [Header("사망 판정")]
    public SceneMove SceneMove;
    public SceneMove death_ui;





    void Start()
    {
        
    }


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        
    }


    // Update is called once per frame
    void Update()
    {
        // 보스클리어시 색깔 변환
 
        if (boss_clear || boss_clear_2)
        {
            spriteRenderer.color = new Color(0, 0, 0, 255f); 
            spriteRenderer.sortingOrder = 10;          
        }
        else
        {
            spriteRenderer.color = new Color(255f, 255f, 255f, 255f);
            spriteRenderer.sortingOrder = 5;
        }





        // 캐릭터 사망
        if(playerHp.curHp <= 0)
        {
            boss_1 = false;
            gameObject.layer = 15;
        }


        if (playerHp.curHp <= 0 && alive && !boss_clear)
        {
            ladder_damage_or_death();

            // 색깔
            spriteRenderer.color = new Color(1, 1, 1, 1f); // 4번째 값은 알파값

            // 상태
            hurt = false;
            acting = true;
            alive = false;
            anim.SetBool("jump" ,false);
            anim.SetBool("jump2" ,false);
            anim.SetBool("walk" ,false);
           
            gameObject.layer = 15;

            // 효과음
            effectSound.PENITENT_DEATH_DEFAULT_function();

            // 애니메이션
            rigid.velocity = new Vector2(0, 0);
            anim.SetTrigger("death");
            
            // ui 처리
            Invoke("death_ui_change" , 1.5f); 

           
        }
        if (alive == false && gameObject.layer != 15)
        {
            spriteRenderer.sortingOrder = 30;
            gameObject.layer = 15;


        }


        // 캐릭터 충돌 시 더해지는 힘을 방지하기 위해서 최대 속도를 정해놓음
        if (damaged)
        {
            if (rigid.velocity.x > 15f)
            {
                rigid.velocity = new Vector2(15f, rigid.velocity.y);
            }
            if (rigid.velocity.y > 15f)
            {
                rigid.velocity = new Vector2(rigid.velocity.y, 15f);
            }
        }
        AnimActingManager();

        // 체력 무한회복 방지
        PreventingInfiniteRecovery();


        // 무적시간 
        if(hurt)
        {
            gameObject.layer = 11;
        }
    }






    // 1.적은 데미지로 이동에 문제가 없는 데미지를 받을 경우
    //  총돌 없음 >> 출혈만 구현되면 됨
    public void blood()
    {
        if (anim.GetBool("walk") && !anim.GetBool("jump") && !isSliding || !Input.anyKey )
        {
            // effectSound.PENITENT_SIMPLE_DAMAGE_DEFAULT_function();
            Blood.ActivateBloodEffect(1.3f , 1.3f);
        }
    }





    // 데미지 판정
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 생존 상태일때 와 enemy 레이어에 닿았을때만 작동하도록 설정해야 한다.
        if(collision.gameObject.layer == LayerMask.NameToLayer("enemy") && alive && !hurt)
        {
            acting = false;
            actingButMove = false; 
            slidingExcept = false;

            // 데미지를 입은 경우 물리법칙과 애니메이션을 설정한다. 접촉한 적의 태그를 같이 보낸다. 
            OnDamaged(collision.transform.position , collision.gameObject.tag);
        }
    }


    // 데미지 입은 경우 (무적상태)
    void OnDamaged(Vector2 targetPos , string tag)
    {
        // 무적시간 관련 변수
        hurt = true;
        // 데미지 상태로 전환
        damaged = true;

        // 출혈 효과
        // Blood.ActivateBloodEffect(1.3f , 1.3f);

        // change layer
        gameObject.layer = 11;
        // or
        // gameObject.layer = LayerMask.NameToLayer("playerDamaged");
        
        // 효과음
        effectSound.PENITENT_SIMPLE_DAMAGE_DEFAULT_function();

        // 색깔
        // spriteRenderer.color = new Color(1, 0, 0, 1f); // 4번째 값은 알파값
        // Invoke("colorTurn" , 0.08f);



        // reaction force
        // 부딪힌 목표물에서 오른쪽에 있으면 +1 왼쪽이면 -1
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        int dirc = transform.position.x - targetPos.x  > 0 ? 1 : -1;

        rigid.AddForce(new Vector2(dirc,1) * 10 , ForceMode2D.Impulse);
       
        // anim
        anim.SetTrigger("damaged");
        CameraShake.TriggerShake(3f, 3f, 0.13f);
        energe(tag);
    }



    // 데미지 상태 해체 >> 이동 상태로 만듬
    // 경사로 속도처리로 인해서 다시 false 처리를 해주어야한다.
    // true 동안에는 이동할 수 없으므로 무적처리보다 더 빨리 처리해주어야 한다.
    void NoneDamged() 
    { 
        Invoke("ResetDamageState", 0.28f);   // 애니메이션이 좀 더 빠르게 끝난다는 점을 반영하여 조금 늦게 실행하게 함
    }

    // 데미지 상태 벗어난 경우 (무적 해제)
    void OffDamaged()
    {
        Invoke("TurnOffImmunity", 0.8f);
    } 

    void ResetDamageState()
    {
        damaged = false;
        anim.SetBool("jump", false);
    }

    void TurnOffImmunity()
    {
        hurt = false;
    }

    void colorTurn()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }







    // 체력 / 에너지 관리
    void energe(string tag)
    {
        // hp ---------------------------------
        // 충돌
        if (tag == "walkingtomb" || tag == "isabel" || tag == "Lionhead" 
        || tag == "bishop" || tag == "acolite" || tag == "flying_head" 
        || tag == "ghost" || tag == "menina" || tag == "flagellant")
        {
            playerHp.curHp -= 10;
            attacking = false;
            acting = false;
            isSliding_stabbing = false;
            high_landing_ = false;

            anim.SetBool("walk" , false);
            anim.SetBool("jump" , false);
            anim.SetBool("jump2" , false);
            anim.SetBool("hangon" , false);
            anim.SetBool("wallclimbing" , false);
            
        }
        
    



        // 0일 경우 게임오버
        if(playerHp.curHp < 0)
        {
            ladder_damage_or_death();
            boss_1 = false;
            playerHp.curHp = 0;

        }
    }



    // energy 한도를 넘어가는 것을 방지
    public void PreventingInfiniteRecovery()
    {
        // 위로
        if(playerHp.curHp >= 100) { playerHp.curHp = 100; }
        if(playerMp.curMp >= 100) { playerMp.curMp = 100; }

        // 아래로
        if(playerHp.curHp <= 0) { playerHp.curHp = 0; }
        if(playerMp.curMp <= 0) { playerMp.curMp = 0; }
    }




    // 애니메이션 관리
    public void AnimActingManager()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("death"))
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
        }

    }



    // soundType > 0 이면 둔탁한 소리 , 1 칼날 소리 
    public void monster_attack_lv1(bool isFlipped , int M_damage , int soundType = 0)
    {
        anim.SetBool("laddering" , false);
        // 무적시간 관련 변수
        hurt = true;
        // 데미지 상태로 전환
        damaged = true;
        acting = false;
        attacking = false;
        anim.SetBool("walk" , false);
        

        // 출혈 효과
        // Blood.ActivateBloodEffect(1.3f , 1.3f);

        // change layer
        gameObject.layer = 11;

        playerHp.curHp -= M_damage;

        // 공격에 맞으면 카메라 쉐이크
        CameraShake.TriggerShake(5f, 5f, 0.13f);

        if(isFlipped && !anim.GetBool("wallclimbing"))
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigid.AddForce(new Vector2(-10,5)  , ForceMode2D.Impulse);
            anim.SetTrigger("damaged");

            if(soundType == 0)       {effectSound.PENITENT_SIMPLE_DAMAGE_damaged_function();}
            else if(soundType == 1)  {effectSound.GHOSTKNIGHT_DAMAGE_function();}

        }
        else if (!isFlipped && !anim.GetBool("wallclimbing"))
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigid.AddForce(new Vector2(10,5) , ForceMode2D.Impulse);
            anim.SetTrigger("damaged");

            if(soundType == 0)       {effectSound.PENITENT_SIMPLE_DAMAGE_damaged_function();}
            else if(soundType == 1)  {effectSound.GHOSTKNIGHT_DAMAGE_function();}

        }
        if (anim.GetBool("wallclimbing"))
        {
            anim.SetBool("wallclimbing" , false);
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigid.AddForce(new Vector2(0,0) , ForceMode2D.Impulse);
            hurt = false;
            damaged = false;

            if(soundType == 0)       {effectSound.PENITENT_SIMPLE_DAMAGE_damaged_function();}
            else if(soundType == 1)  {effectSound.GHOSTKNIGHT_DAMAGE_function();}
        }

        ladder_damage_or_death();

    }





    public void monster_attack_lv2(bool isFlipped , int M_damage)
    {
        // 무적시간 관련 변수
        hurt = true;
        // 데미지 상태로 전환
        damaged = true;

        attacking = false;

        // 출혈 효과
        // Blood.ActivateBloodEffect(1.3f , 1.3f);

        // change layer
        gameObject.layer = 11;

        // Debug.Log(M_damage);
        playerHp.curHp -= M_damage;

        if(isFlipped )
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigid.AddForce(new Vector2(-15,5)  , ForceMode2D.Impulse);
            anim.SetTrigger("damaged2");
        }
        else if (!isFlipped )
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigid.AddForce(new Vector2(15,5) , ForceMode2D.Impulse);
            anim.SetTrigger("damaged2");
        }
    }






    // 즉사기 감지
    public void Instant_Death_Detection(Vector3 position)
    {
     
        if(transform.position.x < position.x)
        {
            Vector3 offset = new Vector3(1f, 1.8f, 0f); 
            GameObject effectInstance = Instantiate(Instant_Death_Detection_, transform.position+offset, transform.rotation);
            effectInstance.transform.SetParent(playerTransform);
        }
        else if (transform.position.x > position.x)
        {
            Vector3 offset = new Vector3(-1f, 1.8f, 0f); 
            GameObject effectInstance = Instantiate(Instant_Death_Detection_, transform.position+offset, transform.rotation);
            effectInstance.transform.SetParent(playerTransform);
        }
    }






    // 트랩을 밟았을 때
    public void death_trap()
    {
        boss_1 = false;
        anim.SetTrigger("death_trap");
        playerHp.curHp = 0;
        acting = true;
        alive = false;
        high_landing_ = false;
        gameObject.layer = 15;

        Invoke("death_ui_change" , 1f); 
    }


    public void death_trigger()
    {
        boss_1 = false;
        anim.SetTrigger("death");
        gameObject.layer = 15;
        ladder_damage_or_death();
    }


    public void death_trigger_shake()
    {
        if(alive)
        {
            CameraShake.TriggerShake(7f, 5.5f, 0.16f);
            anim.SetTrigger("death");
            gameObject.layer = 15;
            ladder_damage_or_death();
        }

    }




    public void death_ui_change()
    {
        SceneMove.fadeIn_ui_slow();
        death_ui.fadeIn_ui_slow();      
    }



    public void resawpon_action()
    {
        anim.SetTrigger("respawn");
 
    }



    // 사다리 중 맞거나 죽었을떄
    public void ladder_damage_or_death()
    {
        isLadder = false;
        anim.speed = 1f;
        rigid.gravityScale = 2.2f;
        rigid.velocity = new Vector2( rigid.velocity.x, rigid.velocity.y);
        anim.SetBool("laddering", false);
    }







}


