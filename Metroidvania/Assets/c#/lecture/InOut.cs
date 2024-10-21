using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InOut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // OnTriggerEnter2D is called when the Collider2D other enters the trigger.
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Print a message when colliding with an object with the "Player" tag
            Debug.Log("충돌");
        }
    }
}
