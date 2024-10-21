using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class posionFog : MonoBehaviour
{

    // Component
    [HideInInspector] public Rigidbody2D rigid;                  // 물리
    [HideInInspector] public CapsuleCollider2D CapsuleCollider;  // 충돌
    [HideInInspector] public SpriteRenderer spriteRenderer;      // 방향전환
    [HideInInspector] public Animator anim;                      // 애니메이션

    public hp playerHp;
    public energyHp hp;


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

    void Update()
    {
        
    }

    // 색상 변경
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // 색상 변경
            hp.spriteRenderer.color = new Color(1, 0, 0, 0.5f);
        }
    }



    // 데미지 판정 트리거
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // 1초마다 데미지를 입히는 함수 호출
            InvokeRepeating("ApplyDamageToPlayer", 0f, 1f);
            
        }
    }


    // 충돌이 끝났을 때
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // 색상 원상복귀
            hp.spriteRenderer.color = new Color(1, 1, 1, 1);

            // 데미지를 주는 InvokeRepeating 함수 중지
            CancelInvoke("ApplyDamageToPlayer");
        }
    }


    // 플레이어에게 지속적으로 데미지를 입히는 함수
    void ApplyDamageToPlayer()
    {
        playerHp.curHp -= 2;
        hp.blood();
    }
}


// 이와 같은 방식 플레이어한테만 제한되기때문에 멀티게임을 위해서는 코드를 바꿔야 한다.