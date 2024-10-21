using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class book_manager2 : MonoBehaviour
{
    public Transform[] items;  // 아이템 배열


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

                string[] bibleKeys = { "메이사가서(7:14)", "메이사가서(8:22)", "메이사가서(15:3)", "메이사가서(20:1)", "메이사가서(25:6)" };

                for (int i = 0; i < items.Length; i++)
                {
                    if (playerData.bible != null && playerData.bible.Contains(bibleKeys[i]))
                    {
                        if (items[i] != null)
                        {
                            Destroy(items[i].gameObject);
                        }
                    }
                }
            }
        }
    }
}
