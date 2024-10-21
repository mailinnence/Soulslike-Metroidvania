using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class event_sound : MonoBehaviour
{

    [Header("Background_Music")]
    public AudioClip MIRIAM_SHARD;   
    public AudioClip ARCHIDIACONO_DISAPPEAR;



    public void MIRIAM_SHARD_function()
    {
        SoundManager.Instance.PlaySound(MIRIAM_SHARD);
    }


    public void ARCHIDIACONO_DISAPPEAR_function()
    {
        SoundManager.Instance.PlaySound(ARCHIDIACONO_DISAPPEAR);
    }


}
