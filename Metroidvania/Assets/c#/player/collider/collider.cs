using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collider : playerStatManager
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }


    // 슬라이딩 시 콜라이더 크기 변경
    public void Sliding_Coilder_Size()
    {
        // offset 조절
        CapsuleCollider.offset = new Vector2(0f, 0.05f); // xOffset와 yOffset는 각각 원하는 값으로 대체되어야 함
        CapsuleCollider.size = new Vector2(0.18f, 0.3f); // width와 height는 각각 원하는 값으로 대체되어야 함

        boxCollider.offset = new Vector2(0f, 0.05f);
        boxCollider.size = new Vector2(0.05f, 0.3f);
    }


    // 슬라이딩 시 콜라이더 크기 변경
    public void Down_Coilder_Size()
    {
        // offset 조절
        CapsuleCollider.offset = new Vector2(0f, 0.05f); // xOffset와 yOffset는 각각 원하는 값으로 대체되어야 함
        CapsuleCollider.size = new Vector2(0.18f, 0.3f); // width와 height는 각각 원하는 값으로 대체되어야 함


        boxCollider.offset = new Vector2(0f, 0.05f);
        boxCollider.size = new Vector2(0.05f, 0.3f);
    }


    // 콜라이더 크기 원상복귀
    public void Idle_Coilder_Size()
    {
        // 레이어 원상복귀
        gameObject.layer = 10;
        
        CapsuleCollider.offset = new Vector2(0f, 0.25f); // xOffset와 yOffset는 각각 원하는 값으로 대체되어야 함
        CapsuleCollider.size = new Vector2(0.13f, 0.7f); // width와 height는 각각 원하는 값으로 대체되어야 함

        boxCollider.offset = new Vector2(0f, 0.22f);
        boxCollider.size = new Vector2(0.05f, 0.65f);
    }






}
