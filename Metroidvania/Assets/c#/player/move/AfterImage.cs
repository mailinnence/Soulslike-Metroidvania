using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public float ghostDelay;
    private float ghostDelaySeconds;
    public GameObject ghost;
    public bool makeGhost = false;



    void Start()
    {
        ghostDelaySeconds = ghostDelay;
    }

        
    void Update()
    {
        if (makeGhost)
        {
            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                // generate a ghost
                Vector3 offset = new Vector3(0f, 0f, 0f); 
                GameObject currentGhost = Instantiate(ghost, transform.position + offset, transform.rotation);
                SpriteRenderer currentSpriteRenderer = GetComponent<SpriteRenderer>();
                Sprite currentSprite = currentSpriteRenderer.sprite;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                
                // Flip x if needed
                bool flipX = currentSpriteRenderer.flipX; // Get the flipX value from the original sprite
                currentGhost.GetComponent<SpriteRenderer>().flipX = flipX; // Set the flipX value for the ghost

                ghostDelaySeconds = ghostDelay;
                Destroy(currentGhost, 1f);
            }
        }
    }
}
