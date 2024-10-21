using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_start : playerStatManager
{
    public GameObject fire_coulnm;

    void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

    }



    public void fire_start_()
    {
        Vector3 offset = new Vector3(0, 0f, 0f);
        GameObject effectInstance = Instantiate(fire_coulnm, transform.position + offset, transform.rotation);
        Destroy(gameObject);
    }

}
