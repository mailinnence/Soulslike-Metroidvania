using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // TextMeshPro를 사용하기 위해 필요합니다.
using UnityEngine.SceneManagement;
using System.IO;

public class DialogManager_end : MonoBehaviour
{
 public TextMeshProUGUI textComponent;  // TextMeshProUGUI 컴포넌트 참조
    public int currentIndex = 0;
    public int next;

    public event_background event_background;


    [Header("대화")]
    public List<AudioClip> audioClips = new List<AudioClip>();

    // 대화 내용을 담고 있는 리스트
    private List<string> dialogList = new List<string>
    {
        "해냈군요. 이제 모든 걸 애기 해줄께요" ,
    };

    private AudioSource audioSource; // AudioSource 컴포넌트 참조

    void Start()
    {
        event_background.Fade_white_out(1.5f , 1.5f);
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
                    event_background.Fade_white_In(1.5f , 1.5f);
                    StartCoroutine(next_stage_delay());
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
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("thankYou");
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
