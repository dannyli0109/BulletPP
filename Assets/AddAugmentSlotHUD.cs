using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAugmentSlotHUD : MonoBehaviour
{
    public TooltipTrigger tooltipTrigger;
    public Player player;
    public GameEvent addAugmentSlotEvent;
    int times = 0;

    public HUDManager thisHUDManager;
    public GameObject Visual;

    public void Start()
    {
        int cost = 10 * times + 10;
        tooltipTrigger.header = "Buy a new augment slot: $" + cost.ToString();
    }

    public void Update()
    {
        if(GameManager.current.GetState()== GameState.Shop)
        {
            Visual.SetActive(true);
        }
        else
        {
            Visual.SetActive(false);
        }
    }

    public void Click()
    {
        int cost = 10 * times + 10;
        if (player.gold >= cost)
        {
            player.gold -= cost;
            player.inventory.capacity++;
            tooltipTrigger.header = "Buy a new augment slot: $" + cost.ToString();
            times++;
            addAugmentSlotEvent?.Invoke();
            thisHUDManager.PopulateAugmentListUI(true);

        }
    }
}
