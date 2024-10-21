using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maito_blck_object : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }



    public void Set_black()
    {
        // Color.black을 사용하여 검정색으로 설정
        spriteRenderer.color = Color.black;
    }

    public void Set_white()
    {
        // Color.white을 사용하여 흰색으로 설정
        spriteRenderer.color = Color.white;
    }
}
