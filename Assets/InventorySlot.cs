using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Image icon;
    public Image outline;
    public bool empty;
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

    public void Pululate(int index)
    {
        empty = false;
        this.index = index;
        icon.gameObject.SetActive(true);
        icon.sprite = inventoryHUD.player.inventory[index].augmentIcon;
        sellAugmentTrigger.Init(inventoryHUD.player, index);
        sellAugmentTrigger.disabled = false;
        tooltipTrigger.header = inventoryHUD.player.inventory[index].augmentName;
        tooltipTrigger.content = inventoryHUD.player.inventory[index].description;
    }

    public void PululateEmpty(int index)
    {
        empty = true;
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
            if (!empty)
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

}
