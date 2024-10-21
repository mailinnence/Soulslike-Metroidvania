using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class collectable2 : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer spriteRenderer;      
    [HideInInspector] public Animator anim;   



    // 이벤트 아이템 , 도서 , 스킬(이동기)  
    /*
    event_Item
    bible
    skill
    move_skill

    */





    [Header("상호작용 구간 , 벡터 , 레이어")]
    public Transform interactionArea;
    public Vector2 interactionArea_;             
    public LayerMask interactionLayer; 


    [Header("상호작용 설명 텍스트")]
    public textBackground textBackground;
    public text text;
    public bool textAble;                      // 설명 텍스트 문 동작 여부


    [Header("상호작용 player 애니메이션")]
    public interaction_object interaction_object;
    public chair_man chair_man;



    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        textAble = true;
    }


    // Update is called once per frame
    void Update()
    {
        DescriptionText(interactionArea,  interactionArea_);
        pickUp(interactionArea, interactionArea_ );
    }




    // 설명 텍스트 여부
    void DescriptionText(Transform interactionArea, Vector2 interactionArea_ )
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);

        if(textAble)
        {

            if (objectsToHit.Length >=1)
            {
                textBackground.Activate();
                text.Activate();
            }
            else
            {
                textBackground.Deactivate();
                text.Deactivate();
            }
        }
        else
        {
            textBackground.Deactivate();
            text.Deactivate();
        }
    }

    // 애니메이션 중에는 설명 텍스트가 뜰 필요가 없음
    void textAble_on()
    {
        textAble = true;
    }






    // 아이템 픽업 애니메이션 
    void pickUp(Transform interactionArea, Vector2 interactionArea_ )
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);


        if (Input.GetKey(KeyCode.C) && objectsToHit.Length >=1 && !playerStatManager.acting)
        {
            textAble = false;
            Vector3 currentPosition = transform.position;
            interaction_object.pickUp_Anim2(currentPosition);

            StartCoroutine(DestroyObjectAfterDelay());
        }
    }


   
    // 2초 후 객체를 삭제하는 코루틴
    IEnumerator DestroyObjectAfterDelay()
    {
        yield return new WaitForSeconds(0.8f);
     
        anim.SetTrigger("take");
        chair_man.put_down();
        
    }

    
    public void destroy_Anim()
    {
        Destroy(gameObject);
    }



    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(interactionArea.position , interactionArea_);
    }
}
