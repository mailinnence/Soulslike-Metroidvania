using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maito_white_back : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;

    void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void white_background()
    {
        StartCoroutine(white_background_delay());
    } 




    IEnumerator white_background_delay()
    {
        white_shot_on();
        yield return new WaitForSeconds(1.5f);        
        white_shot_off();
    }





    void white_shot_on()
    {
        spriteRenderer.enabled = true;
    }


    void white_shot_off()
    {
        spriteRenderer.enabled = false;
    }


    


}
