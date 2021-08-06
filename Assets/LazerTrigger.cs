﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerTrigger : MonoBehaviour
{
    public Character Owner;
    void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 12 )
        {
            if(other.GetType() == typeof(BoxCollider)){

                other.gameObject.GetComponent<Character>().hp -= Owner.laserStats.damage.value*Time.deltaTime;
            // for lazers
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}