using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flying_head : enemy_move
{

    [Header("방향")]
    public int nextMove;


    [Header("즉사기")]
    public bool oneKill;


    [Header("감지 상태")]
    public bool detection_player;
    public bool detection_attack;
    Vector2 position;           // 플레이어 위치


    [Header("공격")]
    public int damage; 
    public bool attacking;
    public bool get_parrying;


    [Header("공격 대상 플레이어")]
    public LayerMask attackableLayer1;



    [Header("플레이어 감지")]
    public Transform playerDetection;          
    public Vector2 playerDetection_;


    [Header("공격 범위")]
    public Transform attack;          
    public Vector2 attack_;


    [Header("체력 생존 여부 ")]
    public bool alive; // 생존 여부

    


    // Start is called before the first frame update
    void Start()
    {
        // 생존
        alive = true;

        // 데미지 초기화
        damage = 99999;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!inventory)
        {            
            playerDetection_function();
            attackPlayer_detection();
            if (detection_player && !detection_attack) walk();
        }
        
    }



   // 플레이어 탐지
    public void playerDetection_function()
    {
        // 플레이어 위치 판단 ----------------------------------------------------------------------------------------------------------------------------------------
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(playerDetection.position, playerDetection_, 0, attackableLayer1);
  
        // 좌측
        if(objectsToHit.Length >= 1)  
        { 
            detection_player = true;  
            position = objectsToHit[0].transform.position;
            // Debug.Log((transform.position.x - position.x) + " , " + (transform.position.y - position.y));
        }
    }


    // 이동
    public void walk()
    {
        float maxspeedX = 3f;
        // 최고 속도
        if(rigid.velocity.x >= maxspeedX)
        {
            rigid.velocity = new Vector2(maxspeedX, rigid.velocity.y);
        }
        else if(rigid.velocity.x <= -1 * maxspeedX)
        {
            rigid.velocity = new Vector2(-1 * maxspeedX , rigid.velocity.y);
        }
        
        if(rigid.velocity.y >= maxspeedX)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, maxspeedX);
        }
        else if(rigid.velocity.y <= -1 * maxspeedX)
        {
            rigid.velocity = new Vector2(rigid.velocity.x , -1 * maxspeedX );
        }


        // 관성 잡기
        // 우측
        if(spriteRenderer.flipX && rigid.velocity.x >0)
        {
            rigid.velocity = new Vector2(0f , rigid.velocity.y);
        }
        // 좌측
        else if(!spriteRenderer.flipX && rigid.velocity.x < 0)
        {
            rigid.velocity = new Vector2(0f , rigid.velocity.y);
        }
        // 위에서 아래로 
        if (transform.position.y - position.y > 2.5f && rigid.velocity.y > 0)
        {
            rigid.velocity = new Vector2(rigid.velocity.x , 0f);
        }
        // 아래에서 위로 
        if (transform.position.y - position.y < 2.5f && rigid.velocity.y < 0)
        {
            rigid.velocity = new Vector2(rigid.velocity.x , 0f);
        }


        // 왼쪽으로
        if(transform.position.x - position.x > 2.5f)
        {
            spriteRenderer.flipX = true;
            rigid.AddForce(new Vector2(-1 * 0.5f,0) , ForceMode2D.Impulse);
        }
        // 오른쪽으로
        else if(position.x - transform.position.x > 2.5f)
        {
            spriteRenderer.flipX = false;
            rigid.AddForce(new Vector2(1 * 0.5f,0) , ForceMode2D.Impulse);
        }
    
    
        // 위쪽으로
        if(transform.position.y - position.y > 2.5f)
        {
            rigid.AddForce(new Vector2(0 , -1 * 0.5f) , ForceMode2D.Impulse);
        }
        // 아래쪽으로
        else if(transform.position.y - position.y < 2.5f)
        {
            rigid.AddForce(new Vector2(0 , 1 * 0.5f) , ForceMode2D.Impulse);
        }
    
    }




    // 플레이어 공격 탐지
    public void attackPlayer_detection()
    {
  
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(attack.position, attack_, 0, attackableLayer1);
        if (objectsToHit.Length >= 1)
        {
            Vector3 currentPosition = transform.position;
            if(!oneKill) objectsToHit[0].GetComponent<energyHp>().Instant_Death_Detection(currentPosition);
            
            detection_attack = true;
            oneKill = true;
            anim.SetTrigger("explode");
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation; 
        }
    }




    // 자폭
    public void attackPlayer_anim()
    {
        HashSet<GameObject> attackedObjects = new HashSet<GameObject>();

     
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(attack.position, attack_, 0, attackableLayer1);
        foreach (Collider2D collider in objectsToHit)
        {
            // 레이어 비교 코드
            if (collider.gameObject.layer == LayerMask.NameToLayer("parrying"))
            {
                collider.GetComponent<parrying>().parrying_interaction(spriteRenderer.flipX, "guard" , 10);
            }

            else if (collider.gameObject.layer != LayerMask.NameToLayer("parrying") && attackedObjects.Add(collider.gameObject))
            {
                collider.GetComponent<energyHp>().monster_attack_lv1(spriteRenderer.flipX, damage);
            }
        }
        

    }


    // 즉사기 알림 해체
    public void onekill_off_anim()
    {
        oneKill = false;
    }


    

    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        // 플레이어 감지 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(playerDetection.position , playerDetection_);
   

        // 공격 범뮈
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attack.position , attack_);

    }

    public void Single_object_alive_anim()
    {
        alive = false;
    }


}
