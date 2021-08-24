using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SynergyUI : MonoBehaviour
{
    public TextMeshProUGUI nameUI;
    public TooltipTrigger tooltipTrigger;

    public void Populate(int id, int breakPoint)
    {
        string name = AugmentManager.current.synergyDatas[id].title;
        string description = AugmentManager.current.synergyDatas[id].descriptions[breakPoint + 1];
        nameUI.text = AugmentManager.current.synergyDatas[id].title;
        tooltipTrigger.header = name;
        tooltipTrigger.content = description;
    }
}
