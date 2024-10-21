using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;  // 추가된 네임스페이스


public class airdash : playerStatManager
{

    public event_background event_background;
    public GameObject[] itemThree;
    public DialogManager3 DialogManager3;
    
    public Vector2 targetPosition = new Vector2(-366.9f, -25.17001f);
    public bool dio;
    public bool stop;
    public playerStatManager playerStatManager;


    public AudioClip wait;
    public bool except;                   
    private bool isDialogRunning = false;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
 
 
    private async void Start()
    {
        // anim.SetTrigger("waiting");
        // event_background.Fade_black_out(5f , 1.5f);

        await airdash_();
        
    }
    // Update is called once per frame
    void Update()
    {
        if(stop)
        {
            playerStatManager.acting = true;
   
        }
    }

 



    private void OnTriggerEnter2D(Collider2D collision)
    {
        // interaction_object가 있을 경우, 이동을 수행합니다.
        if (dio && jumpAble && !isDialogRunning)
        {
            isDialogRunning = true;
            stop = true;
            dio = false;
            StartCoroutine(diolog(collision));
            SoundManager.Instance.PlaySound(wait , volume: 0.5f);
    
        }
        
    }



    IEnumerator diolog(Collider2D collision)
    {
        interaction_object interaction = collision.GetComponent<interaction_object>();

        yield return new WaitForSeconds(1.5f);
        
        event_background.Fade_white_In(1.5f , 1.5f);
        yield return new WaitForSeconds(1.5f);

        Transform objectTransform = collision.transform;   
        objectTransform.position = targetPosition;
        
        event_background.Fade_white_out(1.5f , 1.5f);

        yield return new WaitForSeconds(1.5f);

        stop = false;
        yield return new WaitForSeconds(0.01f);

        interaction.waiting_Anim();
        yield return new WaitForSeconds(0.01f);


        yield return new WaitForSeconds(1f);
        DialogManager3.voice();
        stop = true;


        dio = false;


    }







    // 게임을 처음 시작했을때
    private async Task airdash_()
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
            if (playerData.main_progress == 1 && playerData.candle.Contains(1) && playerData.candle.Contains(2) && playerData.candle.Contains(3))
            { 
               playerData.main_progress = 2;
               dio = true;
                // anim.SetTrigger("waiting");
                    
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