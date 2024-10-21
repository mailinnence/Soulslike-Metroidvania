using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic; // 몬스터의 체력을 각기 다르게 배정하기 위해서 딕션너리를 이용한다.

public class enemy_move : playerStatManager
{
    // 각 enemy 마다 파일을 만들어서 관리하는 것은 너무 비효율적이다.
    // 공통적인 부분은 하나의 파일에 만들고 상속시키는 것이 효율적이다.
    // hp 경우 몬스터들마다 다 다르므로 태그를 이용해서 다르게 주는 방식을 택한다.




    [Header("보스전 처리")]
    public string boss_name;
    private boss_hp_1 bossHp;
    public boss_hp_2 bossHp2;


    [Header("체력")]
    public float hp;
    private Dictionary<string, int> monsterName_hp = new Dictionary<string, int>();
    private bool damaged; // 데미지 받았을때 색깔을 바꾸기 위한 변수


    // 레이어 처리 변수
    [HideInInspector] public int platformAndObstacleMask;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }


    void Start()
    {
        // 레이어 처리 변수
        platformAndObstacleMask = LayerMask.GetMask("platform", "ignorePlatform");
        
        // 몬스터 체력 설정
        monsterHp_setting();

        // 몬스터 체력 초기화
        monsterHp_init();



        // 보스전 처리
        if (boss_name.ToLower() == "isabel")
        {
            InitializeBossHp();
        }

      // 보스전 처리
        if (boss_name.ToLower() == "maito")
        {
            InitializeBossHp();
        }
    
    }

    void Update()
    {
        // 체력 관련 로직
        Hp();
        kiki();
 
        // 보스클리어시 색깔 변환
        if(boss_clear)
        {
            spriteRenderer.color = new Color(0, 0, 0, 255f); 
        }
        else if(!damaged && !boss_clear )
        {
            spriteRenderer.color = new Color(255f, 255f, 255f, 255f);
        }


    }





    // 몬스터마다 체력은 각기 달라야 한다.
    // 태그 이름을 이용해서 각 몬스터들의 다르게 체력을 배정한다.
    public void monsterHp_setting()
    {
        // 기본몬스터 (지상) -----------------------------------------------
        // 움직이는 석상
        monsterName_hp.Add("walkingtomb", 150);  
        monsterName_hp.Add("isabel", 400);  
        monsterName_hp.Add("acolite", 50);  
        monsterName_hp.Add("bishop", 100); 
        monsterName_hp.Add("Lionhead", 180); 
        monsterName_hp.Add("menina", 200); 
        monsterName_hp.Add("flagellant", 50); 


        // 기본몬스터 (공중) -----------------------------------------------
        monsterName_hp.Add("flying_head", 99999);  
        monsterName_hp.Add("ghost", 100);  
    }

    // 몬스터 체력 초기화
    public void monsterHp_init()
    {
        // 현재 객체의 태그 이름 가져오기
        string currentTag = gameObject.tag;

        // 태그 이름이 monsterName_hp에 존재하는지 확인
        if (monsterName_hp.ContainsKey(currentTag))
        {
            // 존재하면 해당 체력을 hp 변수에 설정
            hp = monsterName_hp[currentTag];
        }
    }



    // 공격을 받았을때 
    // 공격을 받았을때 데미지와 색깔이 순간 빨간색으로 바뀌는 것은 공통사항이다.
    public void EnemyHit(float _damageDone)
    {
        damaged = true;
        hp -= _damageDone;
        // 오브젝트의 SpriteRenderer 컴포넌트 가져오기
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        // 빨간색으로 색상 변경
        
        renderer.color = new Color(1f, 0.12f, 0.06f); // Hex 코드 FF1F10에 해당하는 색상
        StartCoroutine(ResetColorAfterDelay(0.05f));


        // 보스전
        if (gameObject.CompareTag("isabel"))
        {
            bossHp.boss_damaged(_damageDone);
        }
        
        if (gameObject.CompareTag("maito"))
        {
            bossHp2.boss_damaged(_damageDone);
        }

    }


    // 지정된 지연 후에 색상을 원래대로 되돌리는 코루틴 함수
    IEnumerator ResetColorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 오브젝트의 SpriteRenderer 컴포넌트 가져오기
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        // 초기 색상으로 되돌리기
        renderer.color = Color.white;
        damaged = false;
    }



    // 체력 관련 로직
    public void kiki()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            EnemyHit(100f);
        }
        

    }



    // 체력 관련 로직
    public void Hp()
    {
        if (hp <= 0)
        {
            hp =0;
            gameObject.layer = 15;
            anim.SetTrigger("death");
        }

    }


    public void all_dead()
    {
        hp =0;
        gameObject.layer = 15;
        anim.SetTrigger("death");
    }


    public void AnimationFinished()
    {
        Destroy(gameObject);
    }




    public void AnimationFinished_2()
    {
        hp =0;
        gameObject.layer = 15;
        anim.SetTrigger("death");
    }



    // 데미지를 받았을때
    public void get_hit(bool isFlipped)
    {
        anim.SetTrigger("damaged");

        if (isFlipped) 
        {
            spriteRenderer.flipX = false;
            // rigid.AddForce(new Vector2(-1 * 30f,0) , ForceMode2D.Impulse);
        }
        else
        { 

            spriteRenderer.flipX = true;
            // rigid.AddForce(new Vector2(30f,0) , ForceMode2D.Impulse);
        }
    }






    private void InitializeBossHp()
    {
        // 먼저 같은 게임 오브젝트에서 boss_hp_1 컴포넌트를 찾습니다.
        bossHp = GetComponent<boss_hp_1>();

        // 만약 같은 게임 오브젝트에 없다면, 자식 오브젝트에서 찾습니다.
        if (bossHp == null)
        {
            bossHp = GetComponentInChildren<boss_hp_1>();
        }

        // 그래도 없다면, 씬에서 찾습니다.
        if (bossHp == null)
        {
            bossHp = FindObjectOfType<boss_hp_1>();
        }
    }



}
