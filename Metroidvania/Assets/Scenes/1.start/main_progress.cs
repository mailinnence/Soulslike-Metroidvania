using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;  // 추가된 네임스페이스

public class main_progress : playerStatManager
{
    public event_background event_background;


    // Start is called before the first frame update
    private async void Start()
    {
        anim.SetTrigger("first_rise");
        event_background.Fade_black_out(5f , 1.5f);

        // await game_start_first();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }



    // 게임을 처음 시작했을때
    private async Task game_start_first()
    {
        // Load current_player.json
        string currentPlayerPath = GetSavePath("current_player.json");

        string currentPlayerJson = await ReadFileAsync(currentPlayerPath);
        CurrentPlayerData currentPlayerData = JsonUtility.FromJson<CurrentPlayerData>(currentPlayerJson);
        int currentPlayer = currentPlayerData.current_player;

        // Load player{n}.json based on current_player
        string playerPath = GetSavePath($"player{currentPlayer}.json");
        if (File.Exists(playerPath))
        {
            string playerJson = await ReadFileAsync(playerPath);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(playerJson);

            // Check if the specified item is in event_Item list
            if (playerData.main_progress == 0)
            { 
                playerData.main_progress = 1;
                playerData.save_Scene = "1_0";

                // 변경된 데이터를 다시 JSON 형식으로 변환하여 파일에 저장
                string updatedPlayerJson = JsonUtility.ToJson(playerData, true);
                await WriteFileAsync(playerPath, updatedPlayerJson);
            }
        }
    }

    private Task<string> ReadFileAsync(string path)
    {
        return Task.Run(() => File.ReadAllText(path));
    }

    private Task WriteFileAsync(string path, string content)
    {
        return Task.Run(() => File.WriteAllText(path, content));
    }

    string GetSavePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
}
