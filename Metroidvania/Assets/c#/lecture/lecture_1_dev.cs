using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lecture_1_dev : MonoBehaviour
{

    [Header("Horizontal Movement Settings")]
    // 
    public float walkSpeed;

    // Jump detection 
    public float jumpForce;
    public Transform groundCheck;
    public float groundCheckY = 0.2f;
    public float groundCheckX = 0.5f;
    public LayerMask whatIsGround;

    // sliding
    private bool canSliding;
    private bool Slided; 
    private float slidingSpeed;
    private float slidingTime;
    private float slidingColdown;
    private float gravity; 
    GameObject sliding_dust_effect;



    private Rigidbody2D rb;
    private float xAxis , yAxis;
    Animator anim;


    // import PlayerStateList  
    PlayerStateList pState;
    private int jumpBufferCounter = 0 ;
    private int jumpBufferFrames;
    private float coyoteTimeCounter = 0;
    private float coyoteTime;

    private int airJumpCounter = 0;
    private int maxAirJumps;
  

    // attack
    bool attack = false;
    float timeBetweenAttack , timeSinceAttack;
    Transform sideAttackTransform , JumpUpAttackTransform , JumpFrontAttackTransform;
    Vector2 sideAttackArea , JumpUpAttackArea , JumpFrontAttackArea;
    LayerMask attackableLayer;
    float damage; 
    GameObject SlashEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        pState = GetComponent<PlayerStateList>();
        gravity = rb.gravityScale;
    }

    void Update()
    {
        GetInputs();
        UpdateJumpVariables();
        if (pState.sliding) return;
        Flip();
        Move();
        Jump(); 
        Attack();

    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");

        // 여기서 0은 마우스 버튼의 인덱스를 나타냅니다. 
        // 보통 왼쪽 버튼은 0, 오른쪽 버튼은 1, 중간 버튼(휠 클릭)은 2입니다.
        attack = Input.GetMouseButtonDown(0);

    }




    void Flip()
    {

        if(xAxis < 0)
        {
            transform.localScale = new Vector2(-2 , transform.localScale.y);
        }
        else if(xAxis > 0)
        {
            transform.localScale = new Vector2(2 , transform.localScale.y);
        }


    }



    private void Move()
    {
        rb.velocity = new Vector2(walkSpeed * xAxis , rb.velocity.y);
        anim.SetBool("walking" , rb.velocity.x != 0 && Grounded());
    }



    void sliding()
    {
        if(Input.GetButtonDown("sliding") && canSliding && !Slided)
        {
            StartCoroutine(Slide());
            Slided = true;
        }
        if(Grounded())
        {
            Slided = false;
        }

    }

    IEnumerator Slide()
    {
        canSliding = false;
        pState.sliding = true;
        anim.SetTrigger("sliding");
        rb.gravityScale = 0;
        rb.velocity = new Vector2(transform.localScale.x * slidingSpeed , 0);
        if (Grounded()) Instantiate(sliding_dust_effect , transform);
        yield return new WaitForSeconds(slidingTime);
        rb.gravityScale = gravity;
        pState.sliding = false;
        yield return new WaitForSeconds(slidingColdown);
        canSliding = true;

    }


    void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if (attack && timeSinceAttack >= timeBetweenAttack)
        {
            timeSinceAttack = 0;
            anim.SetTrigger("attacking");
        
            if(yAxis == 0 || yAxis < 0 && Grounded())
            {
                Hit(sideAttackTransform , sideAttackArea);
                Instantiate(SlashEffect , sideAttackTransform);
            }
            else if (yAxis > 0)
            {
                Hit(JumpUpAttackTransform , sideAttackArea);
                SlashEffectAtAngle(SlashEffect , 90 , JumpUpAttackTransform);

            }
            else if (yAxis < 0 && !Grounded())
            {
                Hit(JumpFrontAttackTransform , sideAttackArea);
                SlashEffectAtAngle(SlashEffect , -90 , JumpUpAttackTransform);
            }
        }
    }


    void Hit(Transform _attackTransform, Vector2 _attackArea)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);

        if (objectsToHit.Length > 0)
        {
            Debug.Log(333);
        }
    
        for(int i = 0; i < objectsToHit.Length; i++)
        {
            if(objectsToHit[i].GetComponent<enemy_dev>() != null)
            {
                objectsToHit[i].GetComponent<enemy_dev>().EnemyHit(damage);
            }
        }
    }

    // 각도를 조절하기 위해 만들었는데 이 프로젝트에서는 필요가 없다.
    void SlashEffectAtAngle(GameObject _slashEffect , int _effectAngle , Transform _attackTransform)
    {
        _slashEffect = Instantiate(_slashEffect , _attackTransform);
        _slashEffect.transform.eulerAngles = new Vector3(0,0, _effectAngle);
        _slashEffect.transform.localScale = new Vector2(transform.localScale.x , transform.localScale.y);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(sideAttackTransform.position , sideAttackArea);
        Gizmos.DrawWireCube(JumpUpAttackTransform.position , JumpUpAttackArea);
        Gizmos.DrawWireCube(JumpFrontAttackTransform.position , JumpFrontAttackArea);
    }





    public bool Grounded()
    {
        if (Physics2D.Raycast(groundCheck.position , Vector2.down , groundCheckY , whatIsGround)
            || Physics2D.Raycast(groundCheck.position + new Vector3(groundCheckX , 0 , 0) , Vector2.down , groundCheckY ,whatIsGround)
            || Physics2D.Raycast(groundCheck.position + new Vector3(-groundCheckX , 0 , 0) , Vector2.down , groundCheckY ,whatIsGround))

        {
      
            return true;
        }
        else
        {
            return false;
        }

    }

    void Jump()
    {

        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x , 0);
            pState.jumping = false;
        }

        if (!pState.jumping)
        {
            // if(Input.GetButtonDown("Jump") && Grounded())
            if(jumpBufferCounter > 0 && coyoteTimeCounter > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x , jumpForce);
                pState.jumping = true;
            }
            else if(!Grounded() && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump")) // 일반적인 2단 점프 원리
            {
                pState.jumping = true;
                airJumpCounter++;
                rb.velocity = new Vector3(rb.velocity.x , jumpForce);
            }
        }
 
    anim.SetBool("jump" , !Grounded());
    }
        
    
    void UpdateJumpVariables()
    {
        if (Grounded())
        {
            pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;
        }
        
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
 
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferFrames;
        }

        else
        {
            jumpBufferCounter--;
        }


    }


}
