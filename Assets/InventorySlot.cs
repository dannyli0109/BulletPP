using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class InventorySlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Canvas shopCanvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    public Image icon;
    public Image outline;
    public bool empty;
    public int index;
    public InventoryHUD inventoryHUD;
    public SellAugmentTrigger sellAugmentTrigger;
    public TooltipTrigger tooltipTrigger;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI amountText;
    public GameObject damageContainer;
    public GameObject amountContainer;
    public Outline imageOutline;
    Vector2 anchorPosition;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponent<Canvas>();
        empty = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!empty)
        {
            damageText.text = inventoryHUD.player.inventory[index].GetDamage(inventoryHUD.player, index).ToString();
            amountText.text = inventoryHUD.player.inventory[index].GetAmounts(inventoryHUD.player, index).ToString();
        }
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

        string description = "<b>Selling Price: " + 1 + "</b>" + "\n";
        description += inventoryHUD.player.inventory[index].description;
        description += "\n<b>Right click to sell</b>";

        tooltipTrigger.content = description;
        imageOutline.effectColor = inventoryHUD.player.inventory[index].GetColor(inventoryHUD.player, index);
    }

    public void PululateEmpty(int index)
    {
        empty = true;
        this.index = index;
        outline.gameObject.SetActive(true);
        icon.gameObject.SetActive(false);
        sellAugmentTrigger.disabled = true;
        tooltipTrigger.header = "";
        tooltipTrigger.content = "Empty slot";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //transform.SetAsLastSibling();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 999;
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
        anchorPosition = icon.rectTransform.anchoredPosition;
        TooltipSystem.current.gameObject.SetActive(false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvas.overrideSorting = false;
        //canvas.sortingOrder = 999;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        icon.rectTransform.anchoredPosition = anchorPosition;
        TooltipSystem.current.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        icon.rectTransform.anchoredPosition += eventData.delta / shopCanvas.scaleFactor;
        InventorySlot slot = eventData.pointerDrag.GetComponent<InventorySlot>();

        if (slot)
        {
            if (!empty)
            {
                Debug.Log("swapping");
                Debug.Log("from: " + index + " to: " + slot.index);
                //inventoryHUD.player.inventory[]
                Augment formAugment = inventoryHUD.player.inventory[index];
                Augment toAugment = inventoryHUD.player.inventory[slot.index];

                inventoryHUD.player.inventory[index] = toAugment;
                inventoryHUD.player.inventory[slot.index] = formAugment;
                inventoryHUD.Populate();
            }
        }
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
                Debug.Log("from: " + index + " to: " + slot.index);
                Augment formAugment = inventoryHUD.player.inventory[index];
                Augment toAugment = inventoryHUD.player.inventory[slot.index];

                inventoryHUD.player.inventory[index] = toAugment;
                inventoryHUD.player.inventory[slot.index] = formAugment;
                inventoryHUD.Populate();
            }
        }
    }

}
