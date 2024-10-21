using System.Collections;
using UnityEngine;

public class page2 : playerStatManager
{
    [Header("페이드 인 앤 아웃 ")]

    public bool isFadingOut; // 페이드 방향 제어
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
        if (isFadingOut)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }


        // spriteRenderer의 알파 값을 업데이트
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = fadeFloat;
            spriteRenderer.color = color;
        }
        
    }

    public void FadeOut()
    {
 
        fadeFloat -= fadeSpeed * Time.deltaTime;
        if (fadeFloat <= 0f)
        {
            fadeFloat = 0f;
            gameObject.SetActive(false);
        }
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);
        fadeFloat += fadeSpeed * Time.deltaTime;
        if (fadeFloat >= 1f)
        {
            fadeFloat = 1f;
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




    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        energyHp energyHpComponent = collision.GetComponent<energyHp>();
        if (energyHpComponent != null && death)
        {
            energyHpComponent.death_trigger_shake();
            energyHpComponent.playerHp.curHp = 0;
        }
    }




}
