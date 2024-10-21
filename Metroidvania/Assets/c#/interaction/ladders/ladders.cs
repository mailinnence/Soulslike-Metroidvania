using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladders : playerStatManager
{

    [Header("사다리 상하단 구분")]
    public Transform downAnim;
    public Transform upAnim;       
    public Transform laddering;    


    [Header("사다리 벡터")]
    public Vector2 downAnim_;    
    public Vector2 upAnim_;          
    public Vector2 laddering_;       


    [Header("대상")]
    public LayerMask ladderableLayer; 
    public move move;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        laddering_void();
        upAnim_void();
    
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(downAnim.position , downAnim_);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(upAnim.position , upAnim_);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(laddering.position , laddering_);

    }


    
    void laddering_void()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(laddering.position, laddering_, 0, ladderableLayer);
        if (objectsToHit.Length >= 1 && (Input.GetKey(KeyCode.UpArrow)) )
        {
            // 사다리 위치 전송
            move.laddering_position_init(transform.position);
            isLadder = true;
        }

    }
    


    void upAnim_void()
    {
        Collider2D[] objectsToHit1 = Physics2D.OverlapBoxAll(upAnim.position, upAnim_, 0, ladderableLayer);
        if (objectsToHit1.Length >= 1 && (Input.GetKey(KeyCode.UpArrow)) )
        {
            move.laddersUpAnim(transform.position);
        }
    }


}
