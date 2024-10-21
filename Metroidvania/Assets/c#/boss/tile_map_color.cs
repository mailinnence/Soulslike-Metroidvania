using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tile_map_color : playerStatManager
{
    public Tilemap tilemap1;
    public Tilemap tilemap2;
    public Tilemap tilemap3;
    public Tilemap tilemap4;
    public Tilemap tilemap5;
    public Tilemap tilemap6;
    public Tilemap tilemap7;

    public GameObject background_white;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Background_Out()
    {
        Color newColor = new Color(0f, 0f, 0f, 255f);
        tilemap1.color = newColor;
        tilemap2.color = newColor;
        tilemap3.color = newColor;
        tilemap4.color = newColor;
        tilemap5.color = newColor;
        tilemap6.color = newColor;
        tilemap7.color = newColor;



        SpriteRenderer renderer = background_white.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            Color color = renderer.color;
            color.a = 1; // 알파값 변경 (0 = 완전 투명, 1 = 완전 불투명)
            renderer.color = color;
        }
        

    }


    public void Background_In()
    {
        Color newColor = new Color(255f, 255f, 255f, 255f);
        tilemap1.color = newColor;
        tilemap2.color = newColor;
        tilemap3.color = newColor;
        tilemap4.color = newColor;
        tilemap5.color = newColor;
        tilemap6.color = newColor;
        tilemap7.color = newColor;

        SpriteRenderer renderer = background_white.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            Color color = renderer.color;
            color.a = 0; // 알파값 변경 (0 = 완전 투명, 1 = 완전 불투명)
            renderer.color = color;
        }
    }    

}
