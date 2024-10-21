using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class candle : playerStatManager
{

    public candle_camera candle_camera;

    public GameObject candle_off_out;
    public GameObject candle_off_in;

    public int location;

    public enemy_controll_2 enemy_controll_2;
    public airdash airdash;


    // Start is called before the first frame update
    void Start()
    {
        candle_init();
    }

    void Awake()
    {
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }    

    // Update is called once per frame
    void Update()
    {
        
    }


    public void end()
    {
        anim.SetTrigger("end");
        CapsuleCollider.enabled = false;

        candle_camera.move_camera();
        candle_event();
        
        if (location == 1)      { change_candle(1); }
        else if (location == 2) { change_candle(2); }
        else if (location == 3) { change_candle(3); }
    } 


    // 이벤트 발생
    public void candle_event()
    {

        SpriteRenderer outRenderer = candle_off_out.GetComponent<SpriteRenderer>();
        SpriteRenderer inRenderer = candle_off_in.GetComponent<SpriteRenderer>();

        outRenderer.enabled = false;
        inRenderer.enabled = true;
    }






    public void change_candle(int candle_)
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


            if (!playerData.candle.Contains(candle_))
            {
                if(candle_ == 1){enemy_controll_2.reset_enemy_1();}
                else if(candle_ == 2){enemy_controll_2.reset_enemy_2();}
                else if(candle_ == 3){enemy_controll_2.reset_enemy_3();}
            }

            playerData.candle.Add(candle_);

            // 변경된 데이터를 다시 JSON 형식으로 변환하여 파일에 저장
            string updatedPlayerJson = JsonUtility.ToJson(playerData, true);
            File.WriteAllText(playerPath, updatedPlayerJson);

            if(playerData.candle.Contains(1) && playerData.candle.Contains(2) && playerData.candle.Contains(3))
            {
                airdash.dio = true;
            }
        }


    }


    string GetSavePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }





    // 이미 끝 촛불은 시작시 꺼져야 하고 해당되는 그림은 바뀌어야 한다.
    public void candle_init()
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
            
            
            if (playerData.candle.Contains(location))
            {
                anim.SetTrigger("end");
                candle_event();
                CapsuleCollider.enabled = false;
            }



            // 변경된 데이터를 다시 JSON 형식으로 변환하여 파일에 저장
            string updatedPlayerJson = JsonUtility.ToJson(playerData, true);
            File.WriteAllText(playerPath, updatedPlayerJson);
        }
    }


}
