using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beam : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;    
    private Animator anim;
    private bool start;



    public float targetX_1 = 310.7f; // 목표 X 위치
    public float targetX_2 = 284.01f; // 목표 X 위치
    public float speed = 5.0f; // 이동 속도





    void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    { 


    }


    // Update is called once per frame
    void Update()
    {
        if(start && !spriteRenderer.flipX)
        {
            beam_1();
        }
        else if(start && spriteRenderer.flipX)
        {
            beam_2();
        }
    }

    void beam_1()
    {
        Vector3 currentPosition = transform.position;;

        // 목표 위치를 설정합니다.
        Vector3 targetPosition = new Vector3(targetX_1, currentPosition.y, currentPosition.z);

        // 현재 위치에서 목표 위치로 천천히 이동합니다.
        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        // 목표 위치에 도달했을 때 이동을 멈춥니다.
        if (transform.position.x == targetX_1)
        {
            anim.SetTrigger("end");
        }

    }



    void beam_2()
    {
        // 현재 위치를 가져옵니다.
        Vector3 currentPosition = transform.position;

        // 목표 위치를 설정합니다.
        Vector3 targetPosition = new Vector3(targetX_2, currentPosition.y, currentPosition.z);

        // 현재 위치에서 목표 위치로 천천히 이동합니다.
        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        // 목표 위치에 도달했을 때 이동을 멈춥니다.
        if (transform.position.x == targetX_2)
        {
            anim.SetTrigger("end");
        }

    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        energyHp energyHpComponent = collision.GetComponent<energyHp>();
        move moveComponent = collision.GetComponent<move>();

  
        // 에어대쉬 일떄는 회피가능
        if (energyHpComponent != null && !moveComponent.non_collider)
        {
            energyHpComponent.death_trigger_shake();
            energyHpComponent.playerHp.curHp = 0;
            // energyHpComponent.monster_attack_lv1(spriteRenderer.flipX, 9999);
        }
    }





    public void start_()
    {
        start = true;
    }



    public void out_()
    {
        Destroy(gameObject);
    }


    public void boxCollider_out()
    {
        boxCollider.enabled = false;
    }


  


}
