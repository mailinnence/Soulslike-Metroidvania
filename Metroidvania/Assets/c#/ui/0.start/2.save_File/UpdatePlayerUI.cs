using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class UpdatePlayerUI : MonoBehaviour
{
    public TextMeshProUGUI player_loc_1;
    public TextMeshProUGUI player_last_time_1;
    public TextMeshProUGUI player_loc_2;
    public TextMeshProUGUI player_last_time_2;
    public TextMeshProUGUI player_loc_3;
    public TextMeshProUGUI player_last_time_3;

    void Start()
    {
        UpdatePlayerText(1);
        UpdatePlayerText(2);
        UpdatePlayerText(3);
    }

    void UpdatePlayerText(int playerNumber)
    {
        string filePath = GetSavePath($"player{playerNumber}.json");

        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonString);

            TextMeshProUGUI locText, timeText;
            GetPlayerTextFields(playerNumber, out locText, out timeText);

            locText.text = playerData.save_Location;
            timeText.text = playerData.last_play_time;
        }
        else
        {
            TextMeshProUGUI locText, timeText;
            GetPlayerTextFields(playerNumber, out locText, out timeText);

            locText.text = "";
            timeText.text = "";
        }
    }

    void GetPlayerTextFields(int playerNumber, out TextMeshProUGUI locText, out TextMeshProUGUI timeText)
    {
        switch (playerNumber)
        {
            case 1:
                locText = player_loc_1;
                timeText = player_last_time_1;
                break;
            case 2:
                locText = player_loc_2;
                timeText = player_last_time_2;
                break;
            case 3:
                locText = player_loc_3;
                timeText = player_last_time_3;
                break;
            default:
                locText = null;
                timeText = null;
                break;
        }
    }

    string GetSavePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
}