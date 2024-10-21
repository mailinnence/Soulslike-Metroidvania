using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class chair_man2 : MonoBehaviour
{

    [HideInInspector] public SpriteRenderer spriteRenderer;      
    [HideInInspector] public Animator anim;   

    public collectable collectable;

    private bool one_var;  // 한번만 작동하도록 도와주는 변수

    [Header("다음씬으로 ")]
    public Image background;
    public float fadeDuration = 4f;
    public float displayDuration = 4f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        next();
    }

    public void put_down()
    {
        anim.SetTrigger("put_down");
    }

    public void collectable_see()
    {
        collectable.spriteRenderer.enabled = true;
        collectable.enabled = true;
    }



    void next()
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
                
                if (playerData.event_Item.Contains("피로 그린 장미") && !one_var)
                {
                    one_var = true;
                    StartCoroutine(next_scene());
                } 


                string updatedJson = JsonUtility.ToJson(playerData, true);
                File.WriteAllText(playerPath, updatedJson);
            }
        }
    }


    IEnumerator next_scene()
    {
        
        yield return new WaitForSeconds(5f);
        // 베경을 어둡게 만듬
        next_scene_();
        
        yield return new WaitForSeconds(5f);
        save_coordinate();
        SceneManager.LoadScene("end");
     
    }










    // 다음씬으로 이동 기능 및 백그라운드

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
                
                playerData.save_Scene = "2_0";
                playerData.save_Location = "슬픔의 기억";
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
                playerData.save_coordinate.Add(-76.08f);
                playerData.save_coordinate.Add(-4.19f);

                // Save the updated player data back to the file
                string updatedJson = JsonUtility.ToJson(playerData, true);
                File.WriteAllText(playerPath, updatedJson);
            }
        }
    }






}
