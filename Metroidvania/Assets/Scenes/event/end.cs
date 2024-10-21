using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class end : playerStatManager
{
    public event_background event_background;
    public GameObject[] itemThree;
    public DialogManager_end DialogManager_end;
    
    private async void Start()
    {
        // anim.SetTrigger("waiting");
        // event_background.Fade_black_out(5f , 1.5f);

        // await game_start_first();
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 객체에서 interaction_object 컴포넌트를 가져옵니다.
        interaction_object interaction = collision.GetComponent<interaction_object>();

        // interaction_object가 있을 경우, waiting_Anim 메서드를 호출합니다.
        if (interaction != null)
        {

            DialogManager_end.voice();
            interaction.waiting_Anim();


            foreach (GameObject item in itemThree)
            {
                if (item != null)
                {
                    item.SetActive(false);
                }
            }
        }
    }
}
