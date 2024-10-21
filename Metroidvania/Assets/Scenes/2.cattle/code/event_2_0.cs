using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public class event_2_0 : MonoBehaviour
{
    public event_background event_background;
    public effectSound effectSound;
    public GameObject player;  // move가 GameObject라고 가정


    void Start()
    {
        save_init();
        save_coordinate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }







    // 진행도 체크 - 처음 씬에서는 이벤트 발생. 후에 추가 
    void save_init()
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

                if (playerData.Progress == 4)
                {
                    event_background.Fade_white_out(1.5f , 1.5f);
                    effectSound.MIRIAM_PORTAL_reverse_function();
                }

                // Save the updated player data back to the file
                string updatedJson = JsonUtility.ToJson(playerData, true);
                File.WriteAllText(playerPath, updatedJson);
            }
        }
    }



    // 이벤트 함수가 추가될 예정
    // Progress를 최종적으로 5로 바꿀 예정



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

                if (playerData.Progress == 5)
                {
                    effectSound.MIRIAM_PORTAL_reverse_function();
                    event_background.Fade_white_out(1.5f , 1.5f);
                    
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
                    playerData.save_coordinate.Add(-1.37f);
                    playerData.save_coordinate.Add(-4.19f);        

                    ChangeMovePosition(new Vector2(-1.37f , -4.19f));  // 예시 좌표        
                }



                // Save the updated player data back to the file
                string updatedJson = JsonUtility.ToJson(playerData, true);
                File.WriteAllText(playerPath, updatedJson);
            }
        }
    }





    void ChangeMovePosition(Vector2 newPosition)
    {
        player.transform.position = new Vector3(newPosition.x, newPosition.y, player.transform.position.z);
    }



}
