using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SellAugmentTrigger : MonoBehaviour, IPointerClickHandler
{
    public Player player;
    public int index;
    public bool disabled;

    public void Init(Player player, int index)
    {
        this.player = player;
        this.index = index;
        this.disabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !disabled) {
            if (player.gold >= 2 || player.inventory.Count > 1)
            {
                player.SellAugment(index);
            }
        }
    } 

}
