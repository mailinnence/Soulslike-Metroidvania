using System.Collections;
using UnityEngine;

public class black : playerStatManager
{
    public Color originalColor;
    public bool triggerChange = false;  // This flag can be used to trigger the color change manually

    void Awake()
    {
        // Get the SpriteRenderer component from this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {

        if (!boss_2 && !triggerChange)
        {
            triggerChange = true;

            StartCoroutine(ChangeColorCoroutine());
        }
    }

    // Coroutine to change the color to black and then restore it
    private IEnumerator ChangeColorCoroutine()
    {
        if (spriteRenderer != null)
        {
    
            spriteRenderer.color = Color.black;

            yield return new WaitForSeconds(1f);
            spriteRenderer.color = originalColor;
        }

    }
}
