using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgound_layer2 : playerStatManager
{
    // 캐릭터의 위치를 받아오기 위해
    public move move;


    // Start is called before the first frame update
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

      

    

    }
}
