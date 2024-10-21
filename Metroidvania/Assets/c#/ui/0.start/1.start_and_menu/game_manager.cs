using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class game_manager : MonoBehaviour
{
    [Header("UI")]
    public TextColor TextColor1;
    public TextColor TextColor2;
    public choice_move choice_move;

    [Header("UI_fadeOut")]
    public SceneMove SceneMove;

    [Header("Sound")]
    public ui_Sound ui_Sound;


    [Header("menu")]
    public int current;
    public List<int> choiceList = new List<int> { 1, 2 };

    private int currentIndex = 0;

    void Start()
    {
        current = 1;
        UpdateCurrent();

        
    }


    void Update()
    {
        changeUi();
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


    void sound_Manager()
    {
        if (current == 1 && Input.GetKeyDown(KeyCode.DownArrow))
        {
            ui_Sound._CHANGE_SELECTION_function();
        }
        else if (current == 2 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            ui_Sound._CHANGE_SELECTION_function();
        }

    }



    // ui 변경
    void changeUi()
    {
        if (current == 1)
        {
            choice_move.MoveToUp();
            TextColor2.ChangeToWhite();
            TextColor1.ChangeToBrightYellow();

           // 나가기때 엔터를 누르면 게임 종류
            if (Input.GetKeyDown(KeyCode.Return)) 
            { 
                ui_Sound._EQUIP_ITEM_function();
                SceneMove.fadeOut_ui();
                StartCoroutine(LoadScene()); 
            }

        }


        else if(current == 2)
        {
            choice_move.MoveToDown();
            TextColor1.ChangeToWhite();
            TextColor2.ChangeToBrightYellow();

            // 나가기때 엔터를 누르면 게임 종류
            if (Input.GetKeyDown(KeyCode.Return)) 
            { 
                // choice_move.StopAnimation();
                ui_Sound._EQUIP_ITEM_function();
                SceneMove.fadeOut_ui();
                StartCoroutine(QuitGame()); 
            }
        }
    }



    IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(0.5f); 
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


    IEnumerator LoadScene()
    {
        yield return null; 
        SceneManager.LoadScene("2.save_File");
    }


    // 마우스 숨기기
    void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


}