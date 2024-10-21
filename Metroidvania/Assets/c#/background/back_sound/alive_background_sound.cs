using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alive_background_sound : playerStatManager
{
    private AudioSource audioSource1;
    public AudioClip clip1;

    void Start()
    {
        audioSource1 = GetComponent<AudioSource>();
        // Ensure you have assigned clips in the inspector
        if (audioSource1 != null)
        {
            audioSource1.volume = 0.3f;
            audioSource1.clip = clip1;
        }

        sound_1();
    }


    void Update()
    {
        if(!alive || !boss_2) { audioSource1.Stop(); }
    }

    public void sound_1()
    {
        if (audioSource1 != null && clip1 != null)
        {
            audioSource1.loop = true;
            audioSource1.Play();
        }
    }

}
