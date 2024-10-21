using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class get_hit : playerStatManager
{


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // 모든 공격에 피격반응
    public void get_hit_1(Collider2D enemyObject , bool isFlipped)
    {
        string[] enemy_hit = {"bishop" , "ghost" , "flagellant" , "" , "" , "" , ""};
        string enemyTag = enemyObject.tag;

        foreach (string i in enemy_hit)
        {
            if (i == enemyTag)
            {
                enemyObject.GetComponent<enemy_move>().get_hit(isFlipped);
            }
        }
    }

    // 콤보3 , 차징공격 , 찌르기 , 내려찍기 >> 피격 반응
    public void get_hit_2(Collider2D enemyObject , bool isFlipped)
    {
        string[] enemy_hit = { "acolite" , "" , "" , "" , "" , "" , ""};
        string enemyTag = enemyObject.tag;

        foreach (string i in enemy_hit)
        {
            if (i == enemyTag)
            {
                enemyObject.GetComponent<enemy_move>().get_hit(isFlipped);
            }
        }
    }

}
