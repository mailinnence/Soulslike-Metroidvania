using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill_Manager : playerStatManager
{





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
}
