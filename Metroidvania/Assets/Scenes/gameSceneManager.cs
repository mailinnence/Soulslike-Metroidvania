using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class gameSceneManager : playerStatManager
{
    public move move;
    public energyHp energyHp;
    public SceneMove Background;
    public SceneMove Logo;

    [Header("No_save_location")]
    public float No_save_x;
    public float No_save_y;
    


    [Header("Hp")]
    public hp playerHp;

    [Header("Mp")]
    public mp playerMp;

    [Header("potion")]
    public itemManager itemManager; 


    [Header("Background_Music")]
    public AudioClip death_music;    
    public bool one_var;


    public boss boss;

    void Start()
    {
        // SoundManager.Instance.PlaySound(death_music);
        // SoundManager.Instance.StopSound(death_music);
        save_coordinate();
    }

    // Update is called once per frame
    void Update()
    {   
        resawpon_setting();
        if(!alive && !one_var)
        {
            StartCoroutine(death_music_delay());
        }
    }



    private IEnumerator death_music_delay()
    {
        one_var = true;
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.PlaySound(death_music);
    }


    // 좌표 저장
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
                    float x = playerData.save_coordinate[0];
                    float y = playerData.save_coordinate[1];
                    
                    // move 객체의 위치를 초기화
                    move.transform.position = new Vector3(x, y, move.transform.position.z);
                }

                // Save the updated player data back to the file
                string updatedJson = JsonUtility.ToJson(playerData, true);
                File.WriteAllText(playerPath, updatedJson);
            }
        }
    }



    void resawpon_setting()
    {
        if(!alive)
        {
            StartCoroutine(resawpon());
        }
    }





    private IEnumerator resawpon()
    {
        yield return new WaitForSeconds(4f);

        playerHp.curHp = 100;
        playerMp.curMp = 100;
        
        itemManager.hp_potion = 5;
        itemManager.mp_potion = 5;

        if(boss!=null)
        {boss.progress =0;}

        
        Background.fadeIn_ui();
        Logo.fadeIn_ui();

        string path = Application.persistentDataPath + "/current_player.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            CurrentPlayerData currentPlayerData = JsonUtility.FromJson<CurrentPlayerData>(json);
            int currentPlayer = currentPlayerData.current_player;

            string playerPath = Application.persistentDataPath + $"/player{currentPlayer}.json";
            if (File.Exists(playerPath) && !alive)
            {
                string playerJson = File.ReadAllText(playerPath);
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(playerJson);

                // 좌표 초기화 ---------------------------------------------
                if (playerData.save_coordinate != null && playerData.save_coordinate.Count >= 2)
                {
                    float x = playerData.save_coordinate[0];
                    float y = playerData.save_coordinate[1];
                    
                    // move 객체의 위치를 초기화
                    move.transform.position = new Vector3(x, y, move.transform.position.z);
                }
                // 저장된 곳이 없다면
                else if(playerData.save_coordinate.Count < 2)
                {
                    move.transform.position = new Vector3(No_save_x, No_save_y, move.transform.position.z);
                }

                // Save the updated player data back to the file
                string updatedJson = JsonUtility.ToJson(playerData, true);
                File.WriteAllText(playerPath, updatedJson);
            }
        }

        one_var = false;
        SoundManager.Instance.StopSound(death_music);

        alive = true;
        resawpon_trigger();

    }


    public void resawpon_trigger()
    {
        energyHp.resawpon_action();
    }


}
