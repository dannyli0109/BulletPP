using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentHUD : MonoBehaviour
{
    public TooltipTrigger tooltipTrigger;
    public void Populate(int id, int level)
    {
        string name = AugmentManager.current.augmentDatas[id].title;
        string description = AugmentManager.current.augmentDatas[id].descriptions[level];
        tooltipTrigger.header = name;
        tooltipTrigger.content = description;
    }
}
