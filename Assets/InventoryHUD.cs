using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHUD : MonoBehaviour
{
    public Player player;
    public List<InventorySlot> inventorySlots;
    // Start is called before the first frame update
    void Start()
    {
        Populate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Populate()
    {
        for (int i = 0; i < player.inventory.Count; i++)
        {
            inventorySlots[i].Pululate(player.inventory[i].id, i);
        }

        for (int i = player.inventory.Count; i < player.inventory.capacity; i++)
        {
            inventorySlots[i].PululateEmpty(i);
        }
    }
}
