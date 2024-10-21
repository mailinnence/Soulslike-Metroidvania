using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class VerticalPlungeDoor : playerStatManager
{

    [HideInInspector] public BoxCollider2D boxCollider2D; 
    public string name;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // 한번 파괴된 적이 있다면 시작시 삭제
        reset();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void VerticalAttack(string type)
    {
        if(type == "shake")
        {
            anim.SetTrigger("shake");
        }
        else if (type == "destory")
        {
            anim.SetTrigger("destory");
            change_progress();
        }
    }

    public void destory_anim()
    {
        Destroy(gameObject);
    }






    // 한번 파괴한 적이 있다면 다시는 없어야 한다.
    public void reset()
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

            // 오브젝트의 위치로 설명 텍스트 판단
            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);

            // Check if the specified item is in event_Item list
            if (playerData.skill.Contains(name))
            {
                destory_anim();

                // 변경된 데이터를 다시 JSON 형식으로 변환하여 파일에 저장
                string updatedPlayerJson = JsonUtility.ToJson(playerData, true);
                File.WriteAllText(playerPath, updatedPlayerJson);
            }
        }
    }








    // 한번 파괴한 적이 있다면 다시는 없어야 한다.
    public void change_progress()
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

            // 오브젝트의 위치로 설명 텍스트 판단
            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);

            // Check if the specified item is in event_Item list
            if (!playerData.skill.Contains(name))
            {
                
                playerData.skill.Add(name);

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


