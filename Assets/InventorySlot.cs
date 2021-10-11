﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Image icon;
    public Image outline;
    public int id;
    public int index;
    public InventoryHUD inventoryHUD;
    public SellAugmentTrigger sellAugmentTrigger;
    public TooltipTrigger tooltipTrigger;
    Vector2 anchorPosition;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pululate(int id, int index)
    {
        this.id = id;
        this.index = index;
        icon.gameObject.SetActive(true);
        icon.sprite = AugmentManager.current.augments[id].augmentIcon;
        sellAugmentTrigger.Init(inventoryHUD.player, index);
        sellAugmentTrigger.disabled = false;
        tooltipTrigger.header = AugmentManager.current.augments[id].augmentName;
        tooltipTrigger.content = AugmentManager.current.augments[id].description;
    }

    public void PululateEmpty(int index)
    {
        this.id = -1;
        this.index = index;
        outline.gameObject.SetActive(true);
        icon.gameObject.SetActive(false);
        sellAugmentTrigger.disabled = true;
        tooltipTrigger.content = "Empty slot";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
        anchorPosition = icon.rectTransform.anchoredPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        icon.rectTransform.anchoredPosition = anchorPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        icon.rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");

        InventorySlot slot = eventData.pointerDrag.GetComponent<InventorySlot>();

        if (slot)
        {
            if (slot.id != -1)
            {
                //inventoryHUD.player.inventory[]
                Augment formAugment = inventoryHUD.player.inventory[index];
                Augment toAugment = inventoryHUD.player.inventory[slot.index];

                inventoryHUD.player.inventory[index] = toAugment;
                inventoryHUD.player.inventory[slot.index] = formAugment;
                inventoryHUD.Populate();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
