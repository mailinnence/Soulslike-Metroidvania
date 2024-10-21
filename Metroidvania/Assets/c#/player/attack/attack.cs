// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;


// // public class State
// // {

// //     // called at the beginning to set initial variables
// //     virtual void OnEnter();
// //     // called every frame Toupdate the state
// //     virtual void OnUpdate();
// //     // called at the end to clean up any data
// //     virtual void OnExit();

// // }






// public class attack : playerStatManager 
// {

//     [Header("캐릭터 공격")]
//     public Transform sideAttackTransformRight;      // 스탠딩(우) 공격 
//     public Transform sideAttackTransformLeft;       // 스탠딩(좌) 공격 
//     public Transform sideAttackTransformRightBig;   // 스탠딩(우) 마무리 공격 
//     public Transform sideAttackTransformLeftBig;    // 스탠딩(좌) 마무리  공격  

//     public Transform UpAttackTransform;             // 상단 공격
//     public Transform jumpDownAttackTransform;       // 점프 하단 공격
//     public Transform DownAttackTransformRight;      // 하단 우측 공격
//     public Transform DownAttackTransformLeft;       // 하단 좌측 공격

//     public Vector2 sideAttackArea;                  // 스탠딩(좌,우) 공격 범위 
//     public Vector2 sideAttackAreaBig;               // 스탠딩(좌,우) 마무리 공격 범위 
//     public Vector2 UpAttackArea;                    // 상단 공격 범위
//     public Vector2 jumpDownAttackArea;              // 점프 하단 공격
//     public Vector2 DownAttackArea;                  // 숙여서 공격

//     public LayerMask attackableLayer; 
//     private float timeBetweenAttack , timeSinceAttack;  // 초기화 하지 않으면 0 이다.
//     private float damage = 0.01f;                           // 기본 공격 데미지


//     [Header("캐릭터 공격 이펙트")]
//     public GameObject sideAttackEffect;
//     public GameObject sideAttackEffect2;
//     public GameObject sideAttackEffect3;
//     public GameObject UpAttackEffect;
//     public GameObject DownAttackEffect;
//     public GameObject jumpmAttackEffect;


//     [Header("콜라이더 크기 조정")]
//     public collider collider;


//     [Header("공격 콤보 관련 변수")]
//     public int standingCombo;


//     [HideInInspector]
//     private Transform playerTransform;      // 이펙트 위치 변수




//     void Start()
//     {
//         playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  

//         // 공격 콤보
//         standingCombo = 1; 
//     }


//     void Awake()
//     {
//         rigid = GetComponent<Rigidbody2D>();
//         CapsuleCollider = GetComponent<CapsuleCollider2D>();
//         spriteRenderer = GetComponent<SpriteRenderer>();
//         anim = GetComponent<Animator>();
//     }


//     void Update()
//     {
//         anim.SetInteger("attackCounter", standingCombo);
//         if (standingCombo >= 4)
//         {
//             standingCombo = 1;
//         }
//     }




//     // 공격 범위 draw 함수
//     private void OnDrawGizmos()
//     {
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireCube(sideAttackTransformRight.position , sideAttackArea);
//         Gizmos.DrawWireCube(sideAttackTransformLeft.position , sideAttackArea);

//         Gizmos.DrawWireCube(UpAttackTransform.position , UpAttackArea);
//         Gizmos.DrawWireCube(jumpDownAttackTransform.position , jumpDownAttackArea);

//         Gizmos.color = Color.blue;
//         Gizmos.DrawWireCube(DownAttackTransformRight.position , DownAttackArea);
//         Gizmos.DrawWireCube(DownAttackTransformLeft.position , DownAttackArea);

//         Gizmos.color = Color.black;
//         Gizmos.DrawWireCube(sideAttackTransformRightBig.position , sideAttackAreaBig);
//         Gizmos.DrawWireCube(sideAttackTransformLeftBig.position , sideAttackAreaBig);

//     }



//     // 공격 함수
//     public void playerAttackAction()
//     {

//         if (Input.GetKeyDown(KeyCode.A) && !Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !isSliding && !acting)
//         {
//             timeSinceAttack += Time.deltaTime;
//             if (timeSinceAttack >= timeBetweenAttack)
//             {
//                 timeSinceAttack = 0;

//                 // 스탠딩 공격
//                 // 우측 공격
//                 if (!spriteRenderer.flipX && !Input.GetKey(KeyCode.UpArrow))  
//                 { 

//                     anim.SetTrigger("attacking");
//                     acting = true;

//                 }

//                 // 좌측 공격
//                 else if (spriteRenderer.flipX && !Input.GetKey(KeyCode.UpArrow)) 
//                 { 

//                     anim.SetTrigger("attacking");
//                     acting = true;

//                 }

//                 // up 공격
//                 else if (Input.GetKey(KeyCode.UpArrow))
//                 {
//                     // 우측 공격
//                     if (!spriteRenderer.flipX) 
//                     { 
//                         acting = true;
//                         anim.SetTrigger("Upattacking");
//                     }
//                     // 좌측 공격
//                     else if (spriteRenderer.flipX ) 
//                     { 
//                         acting = true;
//                         anim.SetTrigger("Upattacking");
//                     }
//                 }
//             }
//         }



//         // 점프 공격
//         else if (Input.GetKeyDown(KeyCode.A) && !Input.GetKey(KeyCode.DownArrow) && anim.GetBool("jump")  && !acting)
//         {
//             timeSinceAttack += Time.deltaTime;
//             if (timeSinceAttack >= timeBetweenAttack)
//             {
//                 timeSinceAttack = 0;
 
//                 // 우측 공격
//                 if (!spriteRenderer.flipX && !Input.GetKey(KeyCode.UpArrow))  
//                 { 
//                     actingButMove = true;
//                     anim.SetTrigger("jumpFrontAttack");
                    
//                 }
//                 // 좌측 공격
//                 else if (spriteRenderer.flipX && !Input.GetKey(KeyCode.UpArrow)) 
//                 { 
//                     actingButMove = true;
//                     anim.SetTrigger("jumpFrontAttack");
                    
//                 }

//                 // up 공격
//                 else if (Input.GetKey(KeyCode.UpArrow))
//                 {
//                     // 우측 공격
//                     if (!spriteRenderer.flipX) 
//                     { 
//                         actingButMove = true;
//                         anim.SetTrigger("jumpUpAttack");
//                     }
//                     // 좌측 공격
//                     else if (spriteRenderer.flipX ) 
//                     { 
//                         actingButMove = true;
//                         anim.SetTrigger("jumpUpAttack");
//                     }
//                 }
//             }
//         }
    

//         // 고개 숙이기
//         if (Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !isSliding && !acting)
//         {

//             // 콜라이더 변경
//             rigid.velocity = new Vector2(0, rigid.velocity.y);
//             rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
//             collider.Down_Coilder_Size();
//             anim.SetBool("crouchDown" , true);

//             // 하단 공격
//             if (Input.GetKeyDown(KeyCode.A))
//             {

//                 // 우측 공격
//                 if (!spriteRenderer.flipX)
//                 {
//                     anim.SetTrigger("crouchDownAttack");
//                 }
//                 // 좌측 공격
//                 else if (spriteRenderer.flipX)
//                 {
//                     anim.SetTrigger("crouchDownAttack");
//                 }
//             }


//         }

//         // 고개 들어올리기
//         else if (!Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !isSliding && !acting)
//         {
//             collider.Idle_Coilder_Size();
//             anim.ResetTrigger("crouchDownAttack"); 
//             anim.ResetTrigger("attacking"); 
//             anim.SetBool("crouchDown" , false);
//             anim.SetBool("crouchUp" , true);
//         }
//     }





//    void attackEffect()
//     {
//         // 스탠딩 좌측 우측 공격
//         if (!Input.GetKey(KeyCode.UpArrow) && !anim.GetBool("crouchDown"))
//         {
//             if (!spriteRenderer.flipX)
//             {
//                 StartCoroutine(attackEffecter("attackingRight"));
//             }
//             else if (spriteRenderer.flipX)
//             {
//                 StartCoroutine(attackEffecter("attackingLeft"));    
//             }    
//         }
//         // 스탠딩 상단 공격
//         else if (Input.GetKey(KeyCode.UpArrow))
//         {
//             if (!spriteRenderer.flipX)
//             {
//                 StartCoroutine(attackEffecter("UpattackingRight")); 
//             }
//             else if (spriteRenderer.flipX)
//             {
//                 StartCoroutine(attackEffecter("UpattackingLeft")); 
//             }
//         }
//         // 숙이고 좌우 공격
//         else if (anim.GetBool("crouchDown"))
//         {
//             if (!spriteRenderer.flipX)
//             {
//                 StartCoroutine(attackEffecter("crouchDownAttackRight")); 
//             }
//             else if (spriteRenderer.flipX)
//             {
//                 StartCoroutine(attackEffecter("crouchDownAttackLeft")); 
//             }
//         }
//     }



//     void jumpAttackEffect()
//     {
//         // 우측 공격
//         if (!spriteRenderer.flipX && !Input.GetKey(KeyCode.UpArrow))  
//         { 
//             StartCoroutine(attackEffecter("jumpmAttackRight")); 
//         }
//         // 좌측 공격
//         else if (spriteRenderer.flipX && !Input.GetKey(KeyCode.UpArrow)) 
//         { 
//             StartCoroutine(attackEffecter("jumpmAttackLeft")); 
//         }

//         // up 공격
//         else if (Input.GetKey(KeyCode.UpArrow))
//         {
//             // 우측 공격
//             if (!spriteRenderer.flipX) 
//             { 
//                 StartCoroutine(attackEffecter("UpattackingRight")); 
//             }
//             // 좌측 공격
//             else if (spriteRenderer.flipX ) 
//             { 
//                 StartCoroutine(attackEffecter("UpattackingLeft")); 
//             }
//         }
//     }





//     IEnumerator attackEffecter(string type)
//     {
//         // 스탠딩 공격
//         if (type == "attackingRight")
//         {
//             if (standingCombo == 1)
//             {
//                 Hit(sideAttackTransformRight , sideAttackArea);      
//                 // 이펙트 위치
//                 Vector3 offset = new Vector3(-1.7f, -1.2f, 0f); 
//                 GameObject effectInstance = Instantiate(sideAttackEffect, sideAttackTransformRight.position+offset, sideAttackTransformRight.rotation);
//                 effectInstance.transform.SetParent(playerTransform);
//                 yield return new WaitForSeconds(0.18f);    
//                 acting = false;
//             }
//             else if (standingCombo == 2)
//             {
//                 Hit(sideAttackTransformRight , sideAttackArea);      
//                 // 이펙트 위치
//                 Vector3 offset = new Vector3(-1.4f, -1.1f, 0f); 
//                 GameObject effectInstance = Instantiate(sideAttackEffect2, sideAttackTransformRight.position+offset, sideAttackTransformRight.rotation);
//                 effectInstance.transform.SetParent(playerTransform);
//                 yield return new WaitForSeconds(0.18f);    
//                 acting = false;
//             }
//             else if (standingCombo == 3)
//             {
//                 Hit(sideAttackTransformRightBig , sideAttackAreaBig);      
//                 // 이펙트 위치
//                 Vector3 offset = new Vector3(-2.4f, -1.2f, 0f); 
//                 GameObject effectInstance = Instantiate(sideAttackEffect3, sideAttackTransformRightBig.position+offset, sideAttackTransformRightBig.rotation);
//                 effectInstance.transform.SetParent(playerTransform);
//                 yield return new WaitForSeconds(0.18f);    
//                 acting = false;
//             }
           
//         }

//         else if (type == "attackingLeft")
//         {
            
//             if (standingCombo == 1)
//             {
//                 Hit(sideAttackTransformLeft , sideAttackArea);            
//                 // 이펙트 위치
//                 Vector3 offset = new Vector3(1.7f, -1.2f, 0f); 
//                 GameObject effectInstance =  Instantiate(sideAttackEffect, sideAttackTransformLeft.position+offset, sideAttackTransformLeft.rotation);
//                 effectInstance.transform.SetParent(playerTransform);
//                 effectInstance.GetComponent<SpriteRenderer>().flipX = true;
//                 yield return new WaitForSeconds(0.18f);
//                 acting = false;
//             }
//             else if (standingCombo == 2)
//             {
           
//                 Hit(sideAttackTransformLeft , sideAttackArea);            
//                 // 이펙트 위치
//                 Vector3 offset = new Vector3(1.4f, -1.1f, 0f); 
//                 GameObject effectInstance =  Instantiate(sideAttackEffect2, sideAttackTransformLeft.position+offset, sideAttackTransformLeft.rotation);
//                 effectInstance.transform.SetParent(playerTransform);
//                 effectInstance.GetComponent<SpriteRenderer>().flipX = true;
//                 yield return new WaitForSeconds(0.18f);
//                 acting = false;
//             }
//             else if (standingCombo == 3)
//             {
           
//                 Hit(sideAttackTransformLeftBig , sideAttackAreaBig);            
//                 // 이펙트 위치
//                 Vector3 offset = new Vector3(2.4f, -1.2f, 0f); 
//                 GameObject effectInstance =  Instantiate(sideAttackEffect3, sideAttackTransformLeftBig.position+offset, sideAttackTransformLeftBig.rotation);
//                 effectInstance.transform.SetParent(playerTransform);
//                 effectInstance.GetComponent<SpriteRenderer>().flipX = true;
//                 yield return new WaitForSeconds(0.18f);
//                 acting = false;
//             }

//         }


//         // 스탠딩 상단 공격
//         else if (type == "UpattackingRight")
//         {
//             Hit(UpAttackTransform , UpAttackArea);   
//             // 이펙트 위치
//             Vector3 offset = new Vector3(0f, -3.0f, 0f);    
//             GameObject effectInstance = Instantiate(UpAttackEffect, UpAttackTransform.position + offset, UpAttackTransform.rotation);
//             effectInstance.transform.SetParent(playerTransform);
//             yield return new WaitForSeconds(0.25f);
//             acting = false;
//             actingButMove = false;
//         }

//         else if (type == "UpattackingLeft")
//         {
//             Hit(UpAttackTransform , UpAttackArea);
//             // 이펙트 위치
//             Vector3 offset = new Vector3(0f, -3.0f, 0f);    
//             GameObject effectInstance = Instantiate(UpAttackEffect, UpAttackTransform.position+offset, UpAttackTransform.rotation);
//             effectInstance.transform.SetParent(playerTransform);
//             effectInstance.GetComponent<SpriteRenderer>().flipX = true;
//             yield return new WaitForSeconds(0.25f);
//             acting = false;
//             actingButMove = false;
//         }

//         // 하단 공격 
//         else if (type =="crouchDownAttackRight")
//         {
//             acting = true;
//             Hit(DownAttackTransformRight , DownAttackArea);
//             // 이펙트 위치
//             Vector3 offset = new Vector3(-1.1f, -0.7f, 0f);   
//             GameObject effectInstance = Instantiate(DownAttackEffect, DownAttackTransformRight.position + offset, DownAttackTransformRight.rotation);
//             effectInstance.transform.SetParent(playerTransform);
//             yield return new WaitForSeconds(0.5f);
//             acting = false;
//         }
        
//         else if (type =="crouchDownAttackLeft")
//         {
//             acting = true;
//             Hit(DownAttackTransformLeft , DownAttackArea);

//             // 이펙트 위치
//             Vector3 offset = new Vector3(1.1f, -0.7f, 0f);   
//             GameObject effectInstance = Instantiate(DownAttackEffect, DownAttackTransformLeft.position+offset, DownAttackTransformLeft.rotation);
//             effectInstance.transform.SetParent(playerTransform);
            
//             effectInstance.GetComponent<SpriteRenderer>().flipX = true;
//             yield return new WaitForSeconds(0.15f);
//             acting = false;

//         }


//         // 점프 정면 공격
//         else if (type == "jumpmAttackRight")
//         {
//             Hit(sideAttackTransformRight , sideAttackArea);      
//             // 이펙트 위치
//             Vector3 offset = new Vector3(-1.7f, -1.2f, 0f); 
//             GameObject effectInstance = Instantiate(jumpmAttackEffect, sideAttackTransformRight.position+offset, sideAttackTransformRight.rotation);
//             effectInstance.transform.SetParent(playerTransform);
//             yield return new WaitForSeconds(0.1f);
//             actingButMove = false;
//         }


//         else if (type == "jumpmAttackLeft")
//         {
//             Hit(sideAttackTransformLeft , sideAttackArea);            
//             // 이펙트 위치
//             Vector3 offset = new Vector3(1.7f, -1.2f, 0f); 
//             GameObject effectInstance =  Instantiate(jumpmAttackEffect, sideAttackTransformLeft.position+offset, sideAttackTransformLeft.rotation);
//             effectInstance.transform.SetParent(playerTransform);
//             effectInstance.GetComponent<SpriteRenderer>().flipX = true;
//             yield return new WaitForSeconds(0.1f);
//             actingButMove = false;
//         }



//         // 하단 공격 
//         else if (type =="crouchDownAttackRight")
//         {
//             acting = true;
//             Hit(DownAttackTransformRight , DownAttackArea);
//             // 이펙트 위치
//             Vector3 offset = new Vector3(-1.1f, -0.7f, 0f);   
//             GameObject effectInstance = Instantiate(DownAttackEffect, DownAttackTransformRight.position + offset, DownAttackTransformRight.rotation);
//             effectInstance.transform.SetParent(playerTransform);
//             yield return new WaitForSeconds(0.15f);
//             acting = false;
//         }
        
//         else if (type =="crouchDownAttackLeft")
//         {
//             acting = true;
//             Hit(DownAttackTransformLeft , DownAttackArea);

//             // 이펙트 위치
//             Vector3 offset = new Vector3(1.1f, -0.7f, 0f);   
//             GameObject effectInstance = Instantiate(DownAttackEffect, DownAttackTransformLeft.position+offset, DownAttackTransformLeft.rotation);
//             effectInstance.transform.SetParent(playerTransform);
            
//             effectInstance.GetComponent<SpriteRenderer>().flipX = true;
//             yield return new WaitForSeconds(0.15f);
//             acting = false;

//         }


//         // 점프 정면 공격
//         else if (type == "jumpmAttackRight")
//         {
//             Hit(sideAttackTransformRight , sideAttackArea);      
//             // 이펙트 위치
//             Vector3 offset = new Vector3(-1.7f, -1.2f, 0f); 
//             GameObject effectInstance = Instantiate(jumpmAttackEffect, sideAttackTransformRight.position+offset, sideAttackTransformRight.rotation);
//             effectInstance.transform.SetParent(playerTransform);
//             yield return new WaitForSeconds(0.1f);
//             actingButMove = false;
//         }


//         else if (type == "jumpmAttackLeft")
//         {
//             Hit(sideAttackTransformLeft , sideAttackArea);            
//             // 이펙트 위치
//             Vector3 offset = new Vector3(1.7f, -1.2f, 0f); 
//             GameObject effectInstance =  Instantiate(jumpmAttackEffect, sideAttackTransformLeft.position+offset, sideAttackTransformLeft.rotation);
//             effectInstance.transform.SetParent(playerTransform);
//             effectInstance.GetComponent<SpriteRenderer>().flipX = true;
//             yield return new WaitForSeconds(0.1f);
//             actingButMove = false;
//         }
//     }


//     // 공격 판정 함수
//     void Hit(Transform _attackTransform, Vector2 _attackArea)
//     {
//         Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);

//         for(int i = 0; i < objectsToHit.Length; i++)
//         {
//             if(objectsToHit[i].GetComponent<enemy_move>() != null)
//             {
//                 // Debug.Log(objectsToHit[i].GetComponent<enemy_move>());
//                 objectsToHit[i].GetComponent<enemy_move>().EnemyHit(damage);
//                 // standingCombo +=1 ;
//             }


//         }
//     }


// }














