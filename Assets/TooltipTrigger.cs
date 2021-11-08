using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string content;
    public string header;
    public RectTransform rect;
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Show(rect, content, header);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }

    void OnDestroy()
    {
        TooltipSystem.Hide();
    }
    void OnDisable()
    {
        TooltipSystem.Hide();
    }

    
}
