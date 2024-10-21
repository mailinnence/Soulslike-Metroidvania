using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // TextMeshPro를 사용하기 위해 필요합니다.
using UnityEngine.SceneManagement;
using System.IO;


public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI textComponent;  // TextMeshProUGUI 컴포넌트 참조
    private int currentIndex = 0;
    public int next;



    [Header("대화")]
    public List<AudioClip> audioClips = new List<AudioClip>();

    // 대화 내용을 담고 있는 리스트
    private List<string> dialogList = new List<string>
    {
        "당신은 나의 주 나의 방패시오 나의 영광이시오\n나의 머리를 드시는 자니이다",
        "내게 구하라 하셨으니\n당신께서 열방을 열어\n나를 지키는 자의 어깨를 드소서",
        "그로 하여 나의 고통을 우리의 고통을\n끝낼 수 있도록 인도하소서",
        "당신께서 의로우사 의로운 일을 좋아하시니\n이제 당신의 얼굴을 구하나이다",
        "저희를 눈동자같이 지키시고 \n당신의 장막 안에 감추소서",
        "그로 하여 그를 이끌고\n그의 검이 무뎌지지 않게 하시며\n그의 무릎이 고통 중에 꿇리지 않게 하소서",
        "이 고통으로부터 구원하여 주소서"
    };

    private AudioSource audioSource; // AudioSource 컴포넌트 참조

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // AudioSource 컴포넌트를 추가합니다.
        StartCoroutine(DialogStart());
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
                    game_start_first();
                    SceneManager.LoadScene("1_0");
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






    // 게임을 처음 시작했을때
    public void game_start_first()
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
            if (playerData.main_progress == 0)
            { 
                playerData.main_progress = 1;
                playerData.save_Scene = "1_0";

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
