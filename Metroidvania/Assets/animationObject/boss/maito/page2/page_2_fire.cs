using System.Collections;
using UnityEngine;

public class page_2_fire : playerStatManager
{
    [Header("페이드 인 앤 아웃 ")]

    public bool isFadingOut_; // 페이드 방향 제어
    public bool death;
 


    void Start()
    {
        
    }


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isFadingOut_)
        {
            FadeIn();
            death_fire();
        }
        else
        {
            FadeOut();
        }


        // spriteRenderer의 알파 값을 업데이트
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = fadeFloat_fire;
            spriteRenderer.color = color;
        }
        
    }

    public void FadeOut()
    {
 
        fadeFloat_fire -= fadeSpeed_fire * Time.deltaTime;
        if (fadeFloat_fire <= 0f)
        {
            death = false;
            fadeFloat_fire = 0f;
        }
    }

    public void FadeIn()
    {
  
        fadeFloat_fire += fadeSpeed_fire * Time.deltaTime;
        if (fadeFloat_fire >= 1f)
        {
            fadeFloat_fire = 1f;
        }
    }



    // 레이어가 fire_death이면 즉사
    public void death_fire()
    {
        if (gameObject.layer == LayerMask.NameToLayer("fire_death"))
        {
            StartCoroutine(death_f());
        }  
    }


    IEnumerator death_f()
    {
        yield return new WaitForSeconds(1f);
        death = true;

        
        
    }




    private void OnTriggerStay2D(Collider2D collision)
    {
        
        energyHp energyHpComponent = collision.GetComponent<energyHp>();
        if (energyHpComponent != null && death && alive)
        {
            energyHpComponent.death_trigger_shake();
            energyHpComponent.playerHp.curHp = 0;
        }
    }




}
