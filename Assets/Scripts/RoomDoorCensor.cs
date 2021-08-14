using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDoorCensor : MonoBehaviour
{
   public MapGeneration mapGenerationScipt;
    public Direction thisDirection;

    void Start()
    {
        mapGenerationScipt = FindObjectOfType<MapGeneration>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
      if(  other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            mapGenerationScipt.ReceiveDoorInput(thisDirection);

        }
    }
}
