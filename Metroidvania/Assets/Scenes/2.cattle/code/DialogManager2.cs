using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // TextMeshPro를 사용하기 위해 필요합니다.
using UnityEngine.SceneManagement;
using System.IO;

public class DialogManager2 : MonoBehaviour
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
        "해냈군요. 언젠가 당신이 깨어나 이 고통에서 꺼내주길 기다렸어요" ,

        "이주 계획 , 일곱 거인의 전쟁 , \n불시착 , 당신과 나의 관계\n슬프게도 당신은 아무것도 기억하지 못하고 있군요",
       
        "당신이 간음의 자매로부터 저의 힘을 되찾아 주었지만 \n지금의 힘으로 얼마 애기할 수 없어요",

        "간략히 애기할께요 제 이름은 미암 이라고 해요.",

        "우리는 우리의 멸망한 세상에서 \n다른 세계로 이주하던 중\n기습으로 인헤 불시착했어요",

        "불시착한 곳의 저주받은 자들은 우리를 반겨주지 않았고\n우리는 모든걸 다해 싸웠지만 결국 패배하여 \n부상당한 당신을 데리고 도망가던 중 \n마지막 힘을 다해 이 공간을 만들어 도망쳤어요.",

        "저의 힘은 이 세상의 저주받은 자들에게 \n반절씩 빼았겼고\n그 중 하나를 당신이 찾아준거에요.",

        "마지막 반절의 제 힘을 되찾아주세요.\n그러면 모든 걸 설명해드릴께요",

        "얼마없지만 제게 남은 마지막 힘을 드릴께요",

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
