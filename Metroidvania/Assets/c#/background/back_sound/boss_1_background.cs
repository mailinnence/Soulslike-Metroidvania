using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_1_background : MonoBehaviour
{
   [Header("Background_Music")]
    public string name;                  
    public float volunm_;

    [Header("True : On , False : Off")]
    public bool echo;

    [Header("Background_Music")]
    public AudioClip music;                  
    

    [Header("상호작용 구간 , 벡터 , 레이어")]
    public Transform interactionArea;
    public Vector2 interactionArea_;
    public LayerMask interactionLayer; 

    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sound(); 
    }



    // 배경음악 ---------------------------------------------------------------------------------------------------------
    public void sound_function()
    {
        if (!SoundManager.Instance.IsPlaying(music))
        {
            SoundManager.Instance.PlaySound(music, volume: volunm_);
        }
    }


    public void Stop_sound()
    {
        echo = false;
        SoundManager.Instance.StopSound(music);
    }



    void sound()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);     
        if (objectsToHit.Length >=1 && echo ) // 1_1 보스전 음악만 바꿔야 한다.&& echo 
        {
            echo = true;
            sound_function(); 
        } 
        else
        { 
            Stop_sound(); 
        }
    }
    // -------------------------------------------------------------------------------------------------------------------



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(interactionArea.position , interactionArea_);

    }


}
