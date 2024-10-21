using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sound_walkingtomb : MonoBehaviour
{

    [Header("walkingtomb")]
    public AudioClip walk;       
    public float walk_volum;      
    public AudioClip attack;
    public float attack_volum;   
    public AudioClip attackReady;
    public float attackReady_volum; 
    public AudioClip hit;
    public float hit_volum;     
    public AudioClip death;  
    public float death_volum;    
                           
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


    public void walk_function()
    {
        if(echo) SoundManager.Instance.PlaySound(walk , volume: walk_volum); // 0.6f
        else SoundManager.Instance.StopSound(walk);
    }

    public void attack_function()
    {
        if(echo) SoundManager.Instance.PlaySound(attack);
        else SoundManager.Instance.StopSound(attack);
    }

    public void attackReady_function()
    {
        if(echo) SoundManager.Instance.PlaySound(attackReady);
        else SoundManager.Instance.StopSound(attackReady);
    }

    public void death_function()
    {
        if(echo) SoundManager.Instance.PlaySound(death);
        else SoundManager.Instance.StopSound(death);
    }


    public void hit_function()
    {
        if(echo) SoundManager.Instance.PlaySound(hit , volume: death_volum); // 0.4f
        else SoundManager.Instance.StopSound(hit);
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
