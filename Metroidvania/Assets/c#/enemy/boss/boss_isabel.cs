using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic; // 몬스터의 체력을 각기 다르게 배정하기 위해서 딕션너리를 이용한다.

public class boss_isabel : playerStatManager
{
    // 각 enemy 마다 파일을 만들어서 관리하는 것은 너무 비효율적이다.
    // 공통적인 부분은 하나의 파일에 만들고 상속시키는 것이 효율적이다.
    // hp 경우 몬스터들마다 다 다르므로 태그를 이용해서 다르게 주는 방식을 택한다.

    public boss_hp_1 boss_hp_1;



    // 체력
    public float hp;
    private Dictionary<string, int> monsterName_hp = new Dictionary<string, int>();

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
    }

    void Update()
    {
        // 체력 관련 로직
        Hp();

 

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
        hp -= _damageDone;
        // 오브젝트의 SpriteRenderer 컴포넌트 가져오기
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        // 빨간색으로 색상 변경
        renderer.color = new Color(1f, 0.12f, 0.06f); // Hex 코드 FF1F10에 해당하는 색상
        StartCoroutine(ResetColorAfterDelay(0.05f));



        if (gameObject.CompareTag("isabel"))
        {
     
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


    public void AnimationFinished()
    {
        Destroy(gameObject);
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



}
