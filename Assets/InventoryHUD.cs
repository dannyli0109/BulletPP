using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHUD : MonoBehaviour
{
    public static InventoryHUD current;
    public Player player;
    public List<InventorySlot> inventorySlots;

    // Start is called before the first frame update

    private void Awake()
    {
        current = this;
    }
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
            inventorySlots[i].Populate(i);
        }

        for (int i = player.inventory.Count; i < player.inventory.capacity; i++)
        {
            inventorySlots[i].PopulateEmpty(i);
        }
    }

    public void AnimateDamageText(int index)
    {
        inventorySlots[index].AnimateDamage();
    }

    public void AnimateAmountText(int index)
    {
        inventorySlots[index].AnimateAmount();
    }
}
