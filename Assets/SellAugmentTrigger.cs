using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SellAugmentTrigger : MonoBehaviour, IPointerClickHandler
{
    public Player player;
    public int index;

    public void Init(Player player, int index)
    {
        this.player = player;
        this.index = index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) {
            player.SellAugment(index);
        }
    } 

}
