using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // TextMeshPro를 사용하기 위해 필요합니다.
using UnityEngine.SceneManagement;
using System.IO;

public class DialogManager3 : MonoBehaviour
{

    public TextMeshProUGUI textComponent;  // TextMeshProUGUI 컴포넌트 참조
    private int currentIndex = 0;
    public int next;
    public GameObject player;
    public event_background event_background;
    public airdash airdash;
    public playerStatManager playerStatManager;


   [Header("대화")]
    public List<AudioClip> audioClips = new List<AudioClip>();

    public Vector2 targetPosition = new Vector2(-306.6562f, -25.16999f);

    // 대화 내용을 담고 있는 리스트
    private List<string> dialogList = new List<string>
    {
        "잠시만요. 지금 당신이 만나게 될 자는 \n저의 반절의 힘을 가져간 메이토라는 자에요.\n 그 자는 각별히 주의해야 해요.",
        "검을 단단히 쥐세요\n 그는 지금까지와의 적과는 \n차원이 다른 힘을 가지고 있어요.",
        "제 마지막 힘을 드릴께요",
    };

    private AudioSource audioSource; // AudioSource 컴포넌트 참조

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // AudioSource 컴포넌트를 추가합니다.
        // StartCoroutine(DialogStart());
    }


    public void wait()
    {
        textComponent.text = "잠시만요";
    }

    public void wait_off()
    {
        textComponent.text = "";
    }


    private Coroutine dialogCoroutine;
    public void voice()
    {
        if (dialogCoroutine != null)
        {
            StopCoroutine(dialogCoroutine);
        }
        dialogCoroutine = StartCoroutine(DialogStart());
    }


    IEnumerator DialogStart()
    {
        yield return new WaitForSeconds(3f);
        textComponent.text = dialogList[currentIndex];

        if (currentIndex == 0 && next == 0) 
        { 
            next++;
            PlayAudioClip(audioClips[0]);
        }

        while (true)
        {
            // 엔터키를 누르면 다음 대화로 넘어갑니다.
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (currentIndex < dialogList.Count - 1)
                {
                    // 현재 대화와 오디오를 종료합니다.
                    audioSource.Stop();
                    
                    // 다음 대화로 이동합니다.
                    currentIndex++;
                    textComponent.text = dialogList[currentIndex];
                    
                    if (currentIndex < audioClips.Count)
                    {
                        PlayAudioClip(audioClips[currentIndex]);
                    }

                    yield return new WaitForSeconds(0.1f); // 조금 기다리기 (필요에 따라 조절)
                }
                else
                {
                    StartCoroutine(return_());
                    yield break; // 코루틴 종료
                }
            }

            yield return null; // 프레임 대기
        }
    }

    void PlayAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }



    IEnumerator return_()
    {
        wait_off();
        event_background.Fade_white_In(1.5f , 1.5f);
        yield return new WaitForSeconds(1.5f);

        airdash.stop = false;
        yield return new WaitForSeconds(0.01f);
        player.transform.position = targetPosition;
        
        event_background.Fade_white_out(1.5f , 1.5f);

        playerStatManager.acting = false;
        interaction_object interaction = player.GetComponent<interaction_object>();
        interaction.waiting_end_Anim();
        
    }



}
