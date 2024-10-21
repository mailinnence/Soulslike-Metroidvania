using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_death : playerStatManager
{
    public GameObject fire_death_coulnm;

    void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

    }


    public void fire_start_()
    {

        Vector3 offset = new Vector3(0f, 0.3f, 0f);
        GameObject effectInstance = Instantiate(fire_death_coulnm, transform.position + offset, transform.rotation);
        Destroy(gameObject);
    }



}
