using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // TextMeshPro를 사용하기 위해 필요합니다.
using UnityEngine.SceneManagement;
using System.IO;

public class DialogManager4 : MonoBehaviour
{
    public TextMeshProUGUI textComponent;  // TextMeshProUGUI 컴포넌트 참조
    public int currentIndex = 0;
    public int next;
    public playerStatManager playerStatManager;


    public event_background event_background;
    public interaction_object interaction_object;
    public back_sound back_sound;


    [Header("대화")]
    public List<AudioClip> audioClips = new List<AudioClip>();

    // 대화 내용을 담고 있는 리스트
    private List<string> dialogList = new List<string>
    {
        "그대는 누구인가" ,

        "말없는 걸음으로 온 그대여 나에게 무엇을 바라는가",
       
        "그대의 발자취에서 나의 예술을 보았는가\n이곳은 한때 귀족들이 모여들던 곳이지\n그들의 허영과 사치로 나의 예술을 알아보지 못했다네",

        "나는 그들의 살을 자르고 찢고 그들의 아름다운 비명에\n내 몸을 만지며 그들의 피를 얼굴에 발라 황금빛\n정욕으로 예술을 완성하였다네",

        "말없는 걸음으로 온 그대여 나와 함께 춤을 춥시다\n그대의 살을 뜯고 그대의 피를 바닥에 고요히 뿌린 뒤 \n저의 채취로 그대를 엮어 태피스트리로 만들어 드리지요",

    };

    private AudioSource audioSource; // AudioSource 컴포넌트 참조

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // AudioSource 컴포넌트를 추가합니다.
        
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
                    audioSource.Stop();
                    textComponent.text = "";
                    playerStatManager.boss_2_start = true;
                    interaction_object.waiting_end_Anim2();
                    back_sound.Start_music();
                    yield break; // 코루틴 종료
                }
            }

            yield return null; // 프레임 대기
        }
    }

    void PlayAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.volume = 0.7f;
        audioSource.Play();
    }



    IEnumerator next_stage_delay()
    {
        yield return new WaitForSeconds(1.5f);
        next_stage();
    }


    // 게임을 처음 시작했을때
    public void next_stage()
    {
        // Load current_player.json
        string currentPlayerPath = GetSavePath("current_player.json");

        string currentPlayerJson = File.ReadAllText(currentPlayerPath);
        CurrentPlayerData currentPlayerData = JsonUtility.FromJson<CurrentPlayerData>(currentPlayerJson);
        int currentPlayer = currentPlayerData.current_player;

        // Load player{n}.json based on current_player
        string playerPath = GetSavePath($"player{currentPlayer}.json");
        if (File.Exists(playerPath))
        {
            string playerJson = File.ReadAllText(playerPath);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(playerJson);


            // Check if the specified item is in event_Item list
            if (playerData.Progress == 4)
            { 
                
                playerData.Progress = 5;
                SceneManager.LoadScene("2_0");

                // 변경된 데이터를 다시 JSON 형식으로 변환하여 파일에 저장
                string updatedPlayerJson = JsonUtility.ToJson(playerData, true);
                File.WriteAllText(playerPath, updatedPlayerJson);
            }
        }
    }



    string GetSavePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
}
