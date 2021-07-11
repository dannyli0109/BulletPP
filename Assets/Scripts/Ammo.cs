using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public Character owner;

    #region stats
    public int TimesBounced = 0;
    public float CurrentTimeTillRemoval = 10;
    #endregion stats

    void Start()
    {
        //grab defaults
    }

    void Update()
    {
        
    }
}
