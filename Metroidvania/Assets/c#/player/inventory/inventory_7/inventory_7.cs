using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class inventory_7 : playerStatManager
{


    [Header("Choice_asset")]
    public Image targetImage; // 이동시킬 UI 이미지

    private Animator animator;

    [Header("TextColor_change")]
    public TextColor TextColor1;
    public TextColor TextColor2;
    public TextColor TextColor3;


    [Header("Ui_sound")]
    public ui_Sound ui_Sound;

    [Header("back_sound_off")]
    public back_sound_stage_1 dead_man;
    public back_sound_stage_1 main_tema;
    public back_sound_stage_1 boss;
    


    void Start()
    {
        current = 1;
        choiceList = new List<int> { 1 , 2 , 3 };
        currentIndex = 0;

        UpdateCurrent();

        animator = GetComponent<Animator>();

        // 만약 Inspector에서 이미지를 할당하지 않았다면 자동으로 찾습니다.
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }
    
    }


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }



    void Update()
    {

        if (Inventory_7_bool)
        {
            choice_move();
            sound_Manager(); // 사운드 처리
            
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveToPrevious();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveToNext();
            }
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



    public void choice_move()
    {
        if(current == 1)
        {
            MoveTo1();
            TextColor1.ChangeToBrightYellow();
            TextColor2.ChangeToWhite();
            TextColor3.ChangeToWhite();

            // if (Input.GetKeyDown(KeyCode.Return))
            // {
            //    ui_Sound._EQUIP_ITEM_function();
            //    inventory = false;
            // }


        }

        else if(current == 2)
        {
            MoveTo2();
            TextColor2.ChangeToBrightYellow();
            TextColor1.ChangeToWhite();
            TextColor3.ChangeToWhite();


            if (Input.GetKeyDown(KeyCode.Return))
            {
               ui_Sound._EQUIP_ITEM_function();

            }

        }

        else if(current == 3)
        {
            MoveTo3();
            TextColor3.ChangeToBrightYellow();
            TextColor1.ChangeToWhite();
            TextColor2.ChangeToWhite();


            if (Input.GetKeyDown(KeyCode.Return))
            {
               player_setting_init(); // 나갈떄 변수들을 초기화 해주어야 한다.

               ui_Sound._EQUIP_ITEM_function();
               SceneManager.LoadScene("1.menu_ui");
            }

        }
    }



    public void MoveTo1()
    {
        if (targetImage != null)
        {
            RectTransform rectTransform = targetImage.rectTransform;
            Vector2 currentPosition = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = new Vector2(currentPosition.x, 169f);
        }
    }

    public void MoveTo2()
    {
        if (targetImage != null)
        {
            RectTransform rectTransform = targetImage.rectTransform;
            Vector2 currentPosition = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = new Vector2(currentPosition.x, 89f);
        }
    }

    public void MoveTo3()
    {
        if (targetImage != null)
        {
            RectTransform rectTransform = targetImage.rectTransform;
            Vector2 currentPosition = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = new Vector2(currentPosition.x, 9f);
        }
    }


    // 사운드 소리
    void sound_Manager()
    {
        if (current == 1 && Input.GetKeyDown(KeyCode.DownArrow))
        {
            ui_Sound._CHANGE_SELECTION_function();
        }

        else if (current == 2 && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            ui_Sound._CHANGE_SELECTION_function();
        }

        else if (current == 3 && Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            ui_Sound._CHANGE_SELECTION_function();
        }
    }







    public void player_setting_init()
    {
        damaged = false;              
        damagedMove = false;          
        isSlope = false;                       
        jumpAble = true;                       
        jump_verticalattack = false;       
        jump_ghost = false;        
        jump_high = 0;          
        gravity_anim_ = false;          
        gravity_hit = false;
        high_landing_ = false;   
        slidingExcept = false;        
        isSliding = false;            
        isSliding_stabbing = false;            
        stabbing_ = false;   
        airdooring = false;
        acting = false;
        actingButMove = false;
        alive = true;      
        hurt = false;            
        attacking = false;                             
        attackAble = true;                                        
        itemUsingState = false;                        
        textAble = false;                                      
        isLadder = false;             
        isLadderDown = false;           
        DownKeyLimit = false;            
        hangAble = false;
        parrying_action = false;
        parrying_counter = false;
    }


}
