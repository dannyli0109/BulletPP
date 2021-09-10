using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SynergyHUD : MonoBehaviour
{
    public TooltipTrigger tooltipTrigger;
    public Image icon;
    public TextMeshProUGUI text;

    public void Populate(int id, int breakPoint, int count)
    {
        AugmentManager augmentManager = AugmentManager.current;
        string name = augmentManager.synergyDatas[id].title;
        string description = augmentManager.synergyDatas[id].descriptions[breakPoint + 1];
        tooltipTrigger.header = name;
        tooltipTrigger.content = description;
        icon.sprite = augmentManager.synergyDatas[id].iconSprite;

        if ((breakPoint + 1) < augmentManager.synergyDatas[id].breakpoints.Count)
        {
            text.text = count.ToString() + "/" + augmentManager.synergyDatas[id].breakpoints[breakPoint + 1].ToString();
        }
        else
        {
            text.text = count.ToString() + "/" + augmentManager.synergyDatas[id].breakpoints[augmentManager.synergyDatas[id].breakpoints.Count - 1].ToString();
        }
    }
}
