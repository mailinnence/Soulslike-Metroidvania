using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakable_platform : MonoBehaviour
{
    // Component
    [HideInInspector] public Rigidbody2D rigid;                  // 물리
    [HideInInspector] public BoxCollider2D boxCollider2D; // 충돌
    [HideInInspector] public SpriteRenderer spriteRenderer;      // 방향전환
    [HideInInspector] public Animator anim;                      // 애니메이션
 

    public move move;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();        
    }

    // Update is called once per frame
    void Update()
    {

    }
            

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer("player") ||
        collision.gameObject.layer == LayerMask.NameToLayer("parrying") ||
        collision.gameObject.layer == LayerMask.NameToLayer("NonColider") || 
        collision.gameObject.layer == LayerMask.NameToLayer("playerDameged"))
        && move.rigid.velocity.y == 0f)
        {
            anim.SetTrigger("break");
        }
    }


    public void repair()
    {
        StartCoroutine(repairDelay());
    }
 


    private IEnumerator repairDelay()
    {
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("repair");
    }


    public void boxCollider2D_enabled()
    {
        boxCollider2D.enabled = true;
    } 
        

    public void boxCollider2D_disabled()
    {
        boxCollider2D.enabled = false;
    } 
  
}
