using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    [Header("ball")]
    public AudioClip GHOST_ENERGY_BALL;       
    public float GHOST_ENERGY_BALL_volum;      

    [Header("True : On , False : Off")]
    public bool echo;

    [Header("상호작용 구간 , 벡터 , 레이어")]
    public Transform interactionArea;
    public Vector2 interactionArea_;
    public LayerMask interactionLayer; 



    void Update()
    {
        sound();
    }


    public void GHOST_ENERGY_BALL_function()
    {
        if(echo) SoundManager.Instance.PlaySound(GHOST_ENERGY_BALL); // 0.6f
        else SoundManager.Instance.StopSound(GHOST_ENERGY_BALL);
    }




    void sound()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);     
        if (objectsToHit.Length >=1) 
        {
            echo = true;
        } 
        else
        { 
            echo = false; 
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(interactionArea.position , interactionArea_);

    }
}
