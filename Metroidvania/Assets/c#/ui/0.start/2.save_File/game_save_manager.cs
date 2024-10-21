using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class StageEntry
{
    public string key;
    public int value;
}

[System.Serializable]
public class PlayerData
{
    public int current_player;
    public string last_play_time;
    public string save_Scene;
    public string save_Location;
    public List<string> save_activate;
    public List<float> save_coordinate;
    public int Progress;
    public List<string> Progress_Obstacle;
    public int hp;
    public int mp;
    public int money;
    public int main_progress;
    public List<string> event_Item;
    public List<string> bible;
    public List<string> skill;
    public List<string> move_skill;
    public string current_skill;
    public List<string> current_Item;
    public ConsumableItem consumable_Item;
    public List<StageEntry> stage_list;

    public List<int> candle;

    public void SetStageList(Dictionary<string, int> dict)
    {
        stage_list = dict.Select(kvp => new StageEntry { key = kvp.Key, value = kvp.Value }).ToList();
    }

    public Dictionary<string, int> GetStageList()
    {
        return stage_list.ToDictionary(entry => entry.key, entry => entry.value);
    }
}

[System.Serializable]
public class ConsumableItem
{
    public int hpPotion;
    public int mpPotion;
}

public class game_save_manager : MonoBehaviour
{
    [Header("UI")]
    public choice_save choice_save;
    public SceneMove SceneMove;
    public ui_Sound ui_Sound;

    [Header("menu")]
    public int current;
    public List<int> choiceList = new List<int> { 1, 2, 3 };
    private int currentIndex = 0;

    [Header("image")]
    public Image no_player1;
    public Image no_player2;
    public Image no_player3;

    public Image player1;
    public Image player2;
    public Image player3;

    public TextMeshProUGUI player1_location_text;
    public TextMeshProUGUI player1_time_text;

    public TextMeshProUGUI player2_location_text;
    public TextMeshProUGUI player2_time_text;

    public TextMeshProUGUI player3_location_text;
    public TextMeshProUGUI player3_time_text;

    void Start()
    {
        current = 1;
        UpdateCurrent();

        UpdatePlayerUI(1);
        UpdatePlayerUI(2);
        UpdatePlayerUI(3);
        // Debug.Log("Persistent Data Path: " + Application.persistentDataPath);
    }

    void Update()
    {
        changeUi();
        esc();
        sound_Manager();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveToPrevious();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveToNext();
        }
    }

    void MoveToPrevious()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateCurrent();
        }
    }

    void MoveToNext()
    {
        if (currentIndex < choiceList.Count - 1)
        {
            currentIndex++;
            UpdateCurrent();
        }
    }

    void UpdateCurrent()
    {
        current = choiceList[currentIndex];
    }

    void changeUi()
    {
        if (current == 1)
        {
            choice_save.MoveTo1();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                current_player_change(current);
                if (!player1.enabled)
                {
                    player_creation(current);
                    Game_Start(current);
                }
                else
                {
                    Game_Start(current);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                DeletePlayerData(1);
            }
        }

        if (current == 2)
        {
            choice_save.MoveTo2();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                current_player_change(current);

                if (!player2.enabled)
                {
                    player_creation(current);
                    Game_Start(current);
                }
                else
                {
                    Game_Start(current);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                DeletePlayerData(2);
            }
        }

        if (current == 3)
        {
            choice_save.MoveTo3();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                current_player_change(current);

                if (!player3.enabled)
                {
                    player_creation(current);
                    Game_Start(current);
                }
                else
                {
                    Game_Start(current);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                DeletePlayerData(3);
            }
        }
    }

    void current_player_change(int num)
    {
        string path = GetSavePath("current_player.json");

        PlayerData playerData = new PlayerData { current_player = num };
        string updatedJsonString = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(path, updatedJsonString);
    }

    void UpdatePlayerUI(int num)
    {
        PlayerData data = LoadPlayerData(num);
        Image playerImage = num == 1 ? player1 : num == 2 ? player2 : player3;
        TextMeshProUGUI locationText = num == 1 ? player1_location_text : num == 2 ? player2_location_text : player3_location_text;
        TextMeshProUGUI timeText = num == 1 ? player1_time_text : num == 2 ? player2_time_text : player3_time_text;
        Image noPlayerImage = num == 1 ? no_player1 : num == 2 ? no_player2 : no_player3;

        if (data != null)
        {
            playerImage.enabled = true;
            locationText.enabled = true;
            timeText.enabled = true;
            noPlayerImage.enabled = false;

            locationText.text = data.save_Location;
            timeText.text = data.last_play_time;
        }
        else
        {
            playerImage.enabled = false;
            locationText.enabled = false;
            timeText.enabled = false;
            noPlayerImage.enabled = true;
        }
    }

    void player_creation(int num)
    {
        string currentDateTime = DateTime.Now.ToString("yyyy년 M월 d일 : HH시 mm분");

        PlayerData newPlayerData = new PlayerData
        {
            last_play_time = $"기억이 이어진 날 : {currentDateTime}",
            main_progress = 0,
            save_Scene = "first",
            save_Location = "슬픔의 기억",
            save_activate = new List<string> {  },
            save_coordinate = new List<float> {  },
            Progress = 1,
            Progress_Obstacle = new List<string> {  },
            hp = 100,
            mp = 100,
            money=0,
            event_Item = new List<string> {  },
            bible = new List<string> {  },
            skill = new List<string> {  },
            move_skill = new List<string> {  },
            current_skill = "",
            current_Item = new List<string> { },
            consumable_Item = new ConsumableItem { hpPotion = 5, mpPotion = 5 },
            stage_list = new List<StageEntry> { new StageEntry { key = "startZone", value = 1 } },
            candle = new List<int> {  },
        };

        SavePlayerData(newPlayerData, num);
        UpdatePlayerUI(num);
    }

    void DeletePlayerData(int num)
    {
        string path = GetSavePath($"player{num}.json");

        if (File.Exists(path))
        {
            ui_Sound._UNEQUIP_ITEM_function();
            File.Delete(path);
            UpdatePlayerUI(num);
        }
    }

    void Game_Start(int num)
    {
        PlayerData playerData = LoadPlayerData(num);

        if (playerData != null)
        {
            ui_Sound._EQUIP_ITEM_function();
            SceneMove.fadeOut_ui();

            if (!string.IsNullOrEmpty(playerData.save_Scene))
            {
                SceneManager.LoadScene(playerData.save_Scene);
            }
        }
    }

    void sound_Manager()
    {
        if (current == 1 && Input.GetKeyDown(KeyCode.DownArrow))
        {
            ui_Sound._CHANGE_SELECTION_function();
        }
        else if (current == 2 && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            ui_Sound._CHANGE_SELECTION_function();
        }
        else if (current == 3 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            ui_Sound._CHANGE_SELECTION_function();
        }
    }

    void esc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ui_Sound._UNEQUIP_ITEM_function();
            SceneMove.fadeOut_ui();
            StartCoroutine(BackPage());
        }
    }

    IEnumerator BackPage()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("1.menu_ui");
    }

    string GetSavePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    void SavePlayerData(PlayerData data, int playerNumber)
    {
        string path = GetSavePath($"player{playerNumber}.json");
        string jsonString = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, jsonString);
    }

    PlayerData LoadPlayerData(int playerNumber)
    {
        string path = GetSavePath($"player{playerNumber}.json");
        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            return JsonUtility.FromJson<PlayerData>(jsonString);
        }
        return null;
    }
}