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
    public Animator damageTextAnimator;
    public Animator amountTextAnimator;
    public List<Image> synergyIcons;
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

    public void Populate(int index)
    {
        empty = false;
        this.index = index;
        icon.gameObject.SetActive(true);
        icon.sprite = inventoryHUD.player.inventory[index].augmentIcon;
        sellAugmentTrigger.Init(inventoryHUD.player, index);
        sellAugmentTrigger.disabled = false;
        tooltipTrigger.header = inventoryHUD.player.inventory[index].augmentName;

        tooltipTrigger.rect = GetComponent<RectTransform>();

        string description = "<b>Selling Price: " + 1 + "</b>" + "\n";
        description += inventoryHUD.player.inventory[index].description;
        description += "\n<b>Right click to sell</b>";

        tooltipTrigger.content = description;
        imageOutline.effectColor = inventoryHUD.player.inventory[index].GetColor(inventoryHUD.player, index);

        for (int i = 0; i < inventoryHUD.player.inventory[index].synergies.Count; i++)
        {
            SynergyData data = inventoryHUD.player.inventory[index].synergies[i];
            synergyIcons[i].gameObject.SetActive(true);
            synergyIcons[i].sprite = data.synergyIcon;
            TooltipTrigger synergyTrigger = synergyIcons[i].gameObject.GetComponent<TooltipTrigger>();
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

        for (int i = inventoryHUD.player.inventory[index].synergies.Count; i < synergyIcons.Count; i++)
        {
            synergyIcons[i].gameObject.SetActive(false);
        }
    }


    public void PopulateEmpty(int index)
    {
        empty = true;
        this.index = index;
        //outline.gameObject.SetActive(true);
        icon.gameObject.SetActive(false);
        sellAugmentTrigger.disabled = true;
        tooltipTrigger.header = "";
        tooltipTrigger.content = "Empty slot";
        tooltipTrigger.rect = GetComponent<RectTransform>();

        for (int i = 0; i < synergyIcons.Count; i++)
        {
            synergyIcons[i].gameObject.SetActive(false);
        }

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvas.overrideSorting = true;
        canvas.sortingOrder = 999;
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
        anchorPosition = icon.rectTransform.anchoredPosition;
        TooltipSystem.current.gameObject.SetActive(false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        canvas.overrideSorting = false;
        //canvas.sortingOrder = 999;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        icon.gameObject.SetActive(true);
        icon.rectTransform.anchoredPosition = anchorPosition;
        TooltipSystem.current.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        icon.rectTransform.anchoredPosition += eventData.delta / shopCanvas.scaleFactor;
        //InventorySlot fromSlot = eventData.pointerDrag.GetComponent<InventorySlot>();
        RaycastResult result = eventData.pointerCurrentRaycast;

        //if (result.isValid)
        //{
        //    InventorySlot slot = result.gameObject.GetComponent<InventorySlot>();
        //    if (slot)
        //    {
        //        if (!slot.empty)
        //        {
        //            int fromIndex = dragIndex;
        //            int toIndex = slot.index;
        //            Debug.Log("from: " + fromIndex + " to: " + toIndex);
        //            if (fromIndex != toIndex)
        //            {
        //                Swap(dragIndex, slot.index);
        //            }
        //        }
        //    }
        //}

    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlot slot = eventData.pointerDrag.GetComponent<InventorySlot>();

        if (slot)
        {
            if (!empty)
            {
                int fromIndex = slot.index;
                int toIndex = index;

                Swap(fromIndex, toIndex);
            }
            else
            {
                int fromIndex = slot.index;
                AppendToEnd(fromIndex);
            }
        }

        //inventoryHUD.PopulateActive();
    }

    void Swap(int fromIndex, int toIndex)
    {
        Augment formAugment = inventoryHUD.player.inventory[fromIndex];
        Augment toAugment = inventoryHUD.player.inventory[toIndex];

        inventoryHUD.player.inventory[fromIndex] = toAugment;
        inventoryHUD.player.inventory[toIndex] = formAugment;
        inventoryHUD.Populate();
    }

    void AppendToEnd(int fromIndex)
    {
        int toIndex = inventoryHUD.player.inventory.Count - 1;
        Augment formAugment = inventoryHUD.player.inventory[fromIndex];

        for (int i = fromIndex; i < inventoryHUD.player.inventory.Count - 1; i++)
        {
            inventoryHUD.player.inventory[i] = inventoryHUD.player.inventory[i + 1];
        }
        inventoryHUD.player.inventory[toIndex] = formAugment;
        inventoryHUD.Populate();
    }

    public void AnimateDamage()
    {
        damageTextAnimator.SetTrigger("statChanges");

    }

    public void AnimateAmount()
    {
        amountTextAnimator.SetTrigger("statChanges");
    }

}
