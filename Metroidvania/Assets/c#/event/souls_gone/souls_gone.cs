using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public class souls_gone : playerStatManager
{

    public event_background event_background;

    [Header("페이드 인 앤 아웃 ")]
    public float fadeFloat;

    [Header("다음씬으로 ")]
    public Image background;
    public float fadeDuration = 4f;
    public float displayDuration = 4f;


    void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
       
    }


    void Start()
    {
        event_background.Fade_black_out(1f , 1.5f);
        FadeOut();
        save_coordinate();
    }

    void Update()
    {
        spriteRenderer.color = new Color(1, 1, 1, fadeFloat); 
    }

    // 페이드 아웃
    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    // 페이드 아웃 루틴
    private IEnumerator FadeOutRoutine()
    {
        while (fadeFloat > 0f)
        {
            fadeFloat -= 0.5f * Time.deltaTime;
            yield return null; // 한 프레임을 기다립니다.
        }
        fadeFloat = 0f; // fadeFloat이 정확히 0이 되도록 설정합니다.


        // 소울이 사라지면 다음씬으로 이동 
        next_scene_();                         // 배경이 어두워지고
        
    }




    // 로고 백그라운드
    public void alert()
    {
        StartCoroutine(FadeInOutSequence());
    }




    public void next_scene_()
    {
        StartCoroutine(FadeOut_next_scene());
    }

    IEnumerator FadeInOutSequence()
    {
        // 초기 알파값을 0으로 설정
        SetAlpha(0f);

        // 페이드인
        yield return StartCoroutine(Fade(0f, 0.4f)); // 최대 알파값을 0.5로 변경

        // 표시 지속 시간
        yield return new WaitForSeconds(displayDuration);

        // 페이드아웃
        yield return StartCoroutine(Fade(0.4f, 0f));
    }



    // 다음씬으로 넘어가기 위해서 화면이 까맣게 바뀜
    IEnumerator FadeOut_next_scene()
    {
        // 초기 알파값을 0으로 설정
        SetAlpha(0f);

        // 페이드인
        yield return StartCoroutine(Fade(0f, 1f)); // 최대 알파값을 0.5로 변경

        // 표시 지속 시간
        yield return new WaitForSeconds(3f);
        
        SceneManager.LoadScene("event_");      // 다음씬으로 이동
    }



    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(endAlpha);
    }

    void SetAlpha(float alpha)
    {
        Color newColor = background.color;
        newColor.a = alpha;
        background.color = newColor;
    }






    // 다음 씬 정보 수정
    void save_coordinate()
    {
        string path = Application.persistentDataPath + "/current_player.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            CurrentPlayerData currentPlayerData = JsonUtility.FromJson<CurrentPlayerData>(json);
            int currentPlayer = currentPlayerData.current_player;

            string playerPath = Application.persistentDataPath + $"/player{currentPlayer}.json";
            if (File.Exists(playerPath))
            {
                string playerJson = File.ReadAllText(playerPath);
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(playerJson);

                // 좌표 초기화 ---------------------------------------------
                if (playerData.save_coordinate != null && playerData.save_coordinate.Count >= 2)
                {
                    float x = -81.9f;
                    float y = -86.66998f;
                
                }
                
                playerData.save_Scene = "event_";
                playerData.save_Location = "약속의 기원";
                playerData.Progress = 4;
                playerData.save_activate.Clear();
                
                
                // 좌표 초기화 ---------------------------------------------
                if (playerData.save_coordinate == null)
                {
                    playerData.save_coordinate = new List<float>();
                }
                else
                {
                    playerData.save_coordinate.Clear();
                }

                // Add the new coordinates
                playerData.save_coordinate.Add(-81.9f);
                playerData.save_coordinate.Add( -86.66998f);

                // Save the updated player data back to the file
                string updatedJson = JsonUtility.ToJson(playerData, true);
                File.WriteAllText(playerPath, updatedJson);
            }
        }
    }



}