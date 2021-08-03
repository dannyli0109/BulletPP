using System.Collections;
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
            Debug.Log("Laser 2" + other);

            // for lazers
            EventManager.current.OnLaserHit(0.1f,Owner, other.gameObject);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
