using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackEffect : playerStatManager
{

    [HideInInspector]
    private Transform playerTransform;      // 이펙트 위치 변수



	[Header("스탠딩 3콤보 피격 효과 오브젝트")]
    public GameObject combo1;                   // 피격1
    public GameObject combo2;                   // 피격2
    public GameObject combo3;                   // 피격3
    public GameObject comboManager;             // 피격3

    private SpriteRenderer combo1Flipx1;        // 피격1 Flipx
    private SpriteRenderer combo1Flipx2;        // 피격2 Flipx
    private SpriteRenderer combo1Flipx3;        // 피격3 Flipx   
    private SpriteRenderer comboManagerFlipx;   // 피격1 Flipx 매니저



    [Header("스탠딩 상단 피격 효과 오브젝트")]
    public GameObject UpCombo;                   // 상단 피격
    private SpriteRenderer UpComboFlipx;        // 피격1 Flipx





    void Start()
    {
        // 자식 오브젝트 생성
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  


    }


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();


        // 스탠딩 3콤보 좌우 처리 
        combo1Flipx1 = combo1.GetComponent<SpriteRenderer>();
        combo1Flipx2 = combo2.GetComponent<SpriteRenderer>();
        combo1Flipx3 = combo3.GetComponent<SpriteRenderer>();
        comboManagerFlipx = comboManager.GetComponent<SpriteRenderer>();


        // 스탠딩 상단 좌우 처리
        UpComboFlipx = UpCombo.GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {

    }




    // 스탠딩 피격 이펙트 ------------------------------------------------------------------------------------------------------------
    public void standingHitEffect(int combo ,  Vector3 hitPosition)
    {
        if (isSlope)
        {
            standingHitEffect_Slope(combo  , hitPosition);
        }
        else
        {
            standingHitEffect_flat(combo  , hitPosition);
        }
    }



    // 스탠딩 피격-평지 이펙트
    public void standingHitEffect_flat(int combo ,  Vector3 hitPosition )
    {

        // 콤보 매니저
        if (combo == 1)     {comboManager = combo1;}
        else if(combo == 2) {comboManager = combo2;}
        else if(combo3)     {comboManager = combo3;}


        // 정면
        Vector2 offsetLeft = new Vector2(0.0f, 0.9f); 
        Vector2 offsetRight = new Vector2(0.0f, 0.9f);


        RaycastHit2D rayHitLeft = Physics2D.Raycast(rigid.position  + offsetLeft  , Vector3.right, 3f ,LayerMask.GetMask("enemy"));
        RaycastHit2D rayHitRight = Physics2D.Raycast(rigid.position + offsetRight , Vector3.left, 3f ,LayerMask.GetMask("enemy"));

   
        Debug.DrawRay(rigid.position + offsetLeft, Vector2.right * 2.57f, Color.green);
        Debug.DrawRay(rigid.position + offsetLeft, Vector2.left * 2.48f, Color.green);


        // 우측
        if (rayHitLeft.collider != null) 
        {            
            combo1Flipx1.flipX = false;
            combo1Flipx2.flipX = false;
            combo1Flipx3.flipX = false;

            if (rayHitLeft.distance >= 1.5f)
            {
                Vector3 offset = new Vector3(-0.7f, 1.1f, 0f); 
                Instantiate(comboManager, hitPosition + offset, transform.rotation);
            }
            else if(rayHitLeft.distance < 1.5f  )
            {
                Vector3 offset = new Vector3(-1f, 1.1f, 0f); 
                Instantiate(comboManager, hitPosition + offset, transform.rotation);
            }
            else
            {
                Vector3 offset = new Vector3(-1f, 1.1f, 0f); 
                Instantiate(comboManager, hitPosition + offset, transform.rotation);
            }
        }


        // 좌측
        else if (rayHitRight.collider != null) 
        {            
            combo1Flipx1.flipX = true;
            combo1Flipx2.flipX = true;
            combo1Flipx3.flipX = true;
            
            if (rayHitRight.distance >= 1.5f)
            {
                Vector3 offset = new Vector3(0.7f, 1.1f, 0f); 
                Instantiate(comboManager, hitPosition + offset, transform.rotation);
            }
            else if(rayHitLeft.distance < 1.5f  )
            {
                Vector3 offset = new Vector3(1f, 1.1f, 0f); 
                Instantiate(comboManager, hitPosition + offset, transform.rotation);
            }
            else
            {
                Vector3 offset = new Vector3(1f, 1.1f, 0f); 
                Instantiate(comboManager, hitPosition + offset, transform.rotation);
            }
        }

    }


    // 스탠딩 피격-경사로 이펙트
    public void standingHitEffect_Slope(int combo  , Vector3 hitPosition)
    {

        RaycastHit2D SlidingRayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("platform"));
        if (SlidingRayHit.collider != null)
        {
            Transform hitTransform = SlidingRayHit.collider.transform;

            // 해당 객체의 z 축 기울기를 출력합니다.
            float zRotation = hitTransform.eulerAngles.z;

            // 만약 각도가 180도보다 크면, 보정합니다.
            if (zRotation > 180f) { zRotation -= 360f; }

            float SlidingAngle = zRotation;

            if (spriteRenderer.flipX) { SlidingAngle += 180; }

            // SlidingAngle을 라디안으로 변환합니다.
            float slidingAngleInRadians = SlidingAngle * Mathf.Deg2Rad;

            // 레이캐스트의 방향을 설정합니다.
            Vector2 direction = new Vector2(Mathf.Cos(slidingAngleInRadians), Mathf.Sin(slidingAngleInRadians));

            // SlidingAngle 방향으로 레이캐스트를 쏩니다.
            Vector2 offsetUP = new Vector2(0.0f, 0.9f);
            RaycastHit2D slidingRayHit = Physics2D.Raycast(rigid.position + offsetUP, direction, 3, LayerMask.GetMask("platform"));

            // 레이캐스트 결과를 시각적으로 표시합니다.
            Debug.DrawRay(rigid.position + offsetUP, direction * 3, Color.magenta);
        }


        // 콤보 매니저
        if (combo == 1)     {comboManager = combo1;}
        else if(combo == 2) {comboManager = combo2;}
        else if(combo3)     {comboManager = combo3;}


        // 우측
        if (SlidingRayHit.collider != null && !spriteRenderer.flipX) 
        {            
            combo1Flipx1.flipX = false;
            combo1Flipx2.flipX = false;
            combo1Flipx3.flipX = false;

            if (SlidingRayHit.distance >= 1.5f)
            {
                Vector3 offset = new Vector3(-1.8f, 1.1f, 0f); 
                Instantiate(comboManager, hitPosition + offset, transform.rotation);
            }
            else if(SlidingRayHit.distance < 1.5f  )
            {
                Vector3 offset = new Vector3(-1f, 1.1f, 0f); 
                Instantiate(comboManager, hitPosition + offset, transform.rotation);
            }
            else
            {
                Vector3 offset = new Vector3(-1.8f, 1.1f, 0f); 
                Instantiate(comboManager, hitPosition + offset, transform.rotation);
            }
        }

        // 좌측
        else if (SlidingRayHit.collider != null && spriteRenderer.flipX)  
        {            
            combo1Flipx1.flipX = true;
            combo1Flipx2.flipX = true;
            combo1Flipx3.flipX = true;
            
            if (SlidingRayHit.distance >= 1.5f)
            {
                Vector3 offset = new Vector3(1.8f, 1.1f, 0f); 
                Instantiate(comboManager, hitPosition + offset, transform.rotation);
            }
            else if(SlidingRayHit.distance < 1.5f  )
            {
                Vector3 offset = new Vector3(1f, 1.1f, 0f); 
                Instantiate(comboManager, hitPosition + offset, transform.rotation);
            }
            else
            {
                Vector3 offset = new Vector3(1.8f, 1.1f, 0f); 
                Instantiate(comboManager, hitPosition + offset, transform.rotation);
            }
        }

    }

    // ------------------------------------------------------------------------------------------------------------------------------










    // 스탠딩 상단 피격 이펙트 -------------------------------------------------------------------------------------------------------
    public void UpStandingHitEffect(Vector3 hitPosition)
    {
        if (spriteRenderer.flipX)
        {
            UpComboFlipx.flipX = true;
            Vector3 offset = new Vector3(0f,  1.1f, 0f); 
            Instantiate(UpComboFlipx, hitPosition + offset, transform.rotation);
        }
        else if (!spriteRenderer.flipX)
        {
            UpComboFlipx.flipX = false;
            Vector3 offset = new Vector3(0f,  1.1f, 0f); 
            Instantiate(UpComboFlipx, hitPosition + offset, transform.rotation);
        }
        
    }




    // ------------------------------------------------------------------------------------------------------------------------------




}





// 몬스터 마다 크기가 다 다르다 
// 단순 거리로 가서는 안된다. 비율로 가야 한다.