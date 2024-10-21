using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class item_manager : MonoBehaviour
{
    public Transform item_1;    // 잊혀진 열쇠
    public Transform item_2;
    public Transform item_3;
    public Transform item_4;
    public Transform item_5;


    void Start()
    {
        item_management();
    }

    void Update()
    {

    }


    void item_management()
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
                CollectableNamespace.PlayerData playerData = JsonUtility.FromJson<CollectableNamespace.PlayerData>(playerJson);
                
                // playerData.event_Item에 "잊혀진 열쇠"가 있는지 확인
                if (playerData.event_Item != null && playerData.event_Item.Contains("잊혀진 열쇠"))
                {
                    // item_1 오브젝트가 존재하면 삭제
                    if (item_1 != null)
                    {
                        Destroy(item_1.gameObject);
                    }
                }

                if (playerData.event_Item != null && playerData.event_Item.Contains("간음의 순결"))
                {
                    // item_1 오브젝트가 존재하면 삭제
                    if (item_1 != null)
                    {
                        Destroy(item_2.gameObject);
                    }
                }

                if (playerData.event_Item != null && playerData.event_Item.Contains("지식의 무지"))
                {
                    // item_1 오브젝트가 존재하면 삭제
                    if (item_1 != null)
                    {
                        Destroy(item_3.gameObject);
                    }
                }

                if (playerData.event_Item != null && playerData.event_Item.Contains("찬가의 귀곡조"))
                {
                    // item_1 오브젝트가 존재하면 삭제
                    if (item_1 != null)
                    {
                        Destroy(item_4.gameObject);
                    }
                }

                if (playerData.event_Item != null && playerData.event_Item.Contains("처음 몸을 준 사랑"))
                {
                    // item_1 오브젝트가 존재하면 삭제
                    if (item_1 != null)
                    {
                        Destroy(item_5.gameObject);
                    }
                }
            }
        }
    }



}
