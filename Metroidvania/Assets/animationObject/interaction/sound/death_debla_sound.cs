using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death_debla_sound : MonoBehaviour
{
    private AudioSource audioSource1;
    private AudioSource audioSource2;

    // Start is called before the first frame update
    void Start()
    {
        // 이 오브젝트에 있는 첫 번째와 두 번째 AudioSource 컴포넌트를 가져옵니다.
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length >= 2)
        {
            audioSource1 = audioSources[0];
            audioSource2 = audioSources[1];
        }
    }

    // 첫 번째 사운드를 재생하는 함수
    public void PlayFirstAudio()
    {
        if (audioSource1 != null)
        {
            audioSource1.Play();
        }
    }

    // 두 번째 사운드를 재생하는 함수
    public void PlaySecondAudio()
    {
        if (audioSource2 != null)
        {
            audioSource2.Play();
        }
    }
}
