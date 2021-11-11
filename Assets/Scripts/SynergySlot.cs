using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SynergySlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI countText;
    public InventoryHUD inventoryHUD;

    public void Populate(int index)
    {
        SynergyData data = AugmentManager.current.synergies[inventoryHUD.player.synergies[index].id];
        icon.sprite = AugmentManager.current.synergies[data.id].synergyIcon;
        countText.text = inventoryHUD.player.synergies[index].count.ToString();
        TooltipTrigger synergyTrigger = icon.gameObject.GetComponent<TooltipTrigger>();
        synergyTrigger.content = "";
        for (int j = 0; j < data.breakpoints.Count; j++)
        {
            synergyTrigger.content += data.synergyName + " " + data.breakpoints[j] + "\n";
            synergyTrigger.content += data.descriptions[j];
            if (j != data.breakpoints.Count - 1)
            {
                synergyTrigger.content += "\n";
            }
            synergyTrigger.header = data.synergyName;
            synergyTrigger.rect = GetComponent<RectTransform>();
        }
    }
}
