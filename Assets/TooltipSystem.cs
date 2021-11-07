using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem current;
    public Tooltip tooltip;

    public void Awake()
    {
        current = this;
    }

    public static void Show(RectTransform rect, string content, string header = "")
    {
        current.tooltip.SetText(rect, content, header);
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        if (current.tooltip)
        {
            current.tooltip.gameObject.SetActive(false);
        }
    }


}
