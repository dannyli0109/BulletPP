using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AugmentUI : MonoBehaviour
{
    public TextMeshProUGUI title;
    //public TextMeshProUGUI descriptions;
    public TooltipTrigger tooltipTrigger;
    public TextMeshProUGUI rarity;
    public Image icon;
    public TextMeshProUGUI cost;
    public Image background;
    public Outline outline;
    public Shop shop;
    public HUDManager hudManager;

    public List<Image> synergyIcons;
    public Image separator;

    public GameEvent buyAugment;
    int id;

    // Start is called before the first frame update
    void Start()
    {
        //title = 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Click()
    {
        //AugmentManager augmentManager = AugmentManager.current;
        int state = shop.player.BuyAugment(id);
        if (state == -1) return;
        //shop.level = 3;
        gameObject.SetActive(false);
        buyAugment?.Invoke();
        shop.UpdateText();
        //shop.augmentIds = augmentManager.GetAugmentIdList();

        //if (state == 1)
        //{
        //    // obtained one that owned but remains the same level
        //    HUDManager.PopulateAugmentListUI(true);
        //    gameObject.SetActive(false);
        //}
        //else if (state == 2)
        //{
        //    // obtained a new augment
        //    shop.player.RemoveAllModifiers();

        //    for (int i = 0; i < shop.player.inventory.augments.Count; i++)
        //    {
        //        augmentManager.OnAugmentAttached(shop.player.inventory.augments[i].id, shop.player.inventory.augments[i].level);
        //    }

        //    for (int i = 0; i < shop.player.synergies.Count; i++)
        //    {
        //        augmentManager.OnSynergyAttached(shop.player.synergies[i].id, shop.player.synergies[i].breakPoint);
        //    }

        //    HUDManager.PopulateAugmentListUI(true);
        //    HUDManager.PopulateSynergyListUI();
        //    gameObject.SetActive(false);
        //}
        //else if (state == 3)
        //{
        //    shop.player.RemoveAllModifiers();

        //    for (int i = 0; i < shop.player.inventory.augments.Count; i++)
        //    {
        //        augmentManager.OnAugmentAttached(shop.player.inventory.augments[i].id, shop.player.inventory.augments[i].level);
        //    }

        //    for (int i = 0; i < shop.player.synergies.Count; i++)
        //    {
        //        augmentManager.OnSynergyAttached(shop.player.synergies[i].id, shop.player.synergies[i].breakPoint);
        //    }

        //    HUDManager.PopulateAugmentListUI(true);
        //    gameObject.SetActive(false);
        //}
        //shop.UpdateText();
    }

    public void Populate(int id)
    {
        this.id = id;
        UpdateText();
    }

    public void UpdateText()
    {
        AugmentManager augmentManager = AugmentManager.current;
        {
            //int level = 0;
            //for (int i = 0; i < shop.player.inventory.augments.Count; i++)
            //{
            //    if (shop.player.inventory.augments[i].id == id)
            //    {
            //        level = shop.player.inventory.augments[i].level;
            //        break;
            //    }
            //}
            //title.text = augmentManager.augmentDatas[id].title;
            //descriptions.richText = true;
            //descriptions.text = augmentManager.augmentDatas[id].descriptions[level];
            //cost.text = "$ " + augmentManager.costs[augmentManager.augmentDatas[id].rarity];
            //outline.effectColor = augmentManager.colors[augmentManager.augmentDatas[id].rarity];

            //foreach (Transform child in synergyContainer.transform)
            //{
            //    Destroy(child.gameObject);
            //}

            title.text = augmentManager.augments[id].augmentName;
            tooltipTrigger.header = augmentManager.augments[id].augmentName;
            tooltipTrigger.content = augmentManager.augments[id].description;
            tooltipTrigger.rect = GetComponent<RectTransform>();
            rarity.text = augmentManager.rarityTexts[augmentManager.augments[id].rarity];
            rarity.color = augmentManager.colors[augmentManager.augments[id].rarity];
            cost.text = "$ " + augmentManager.augments[id].cost;
            outline.effectColor = augmentManager.augments[id].color;
            icon.sprite = augmentManager.augments[id].augmentIcon;

            for (int i = 0; i < augmentManager.augments[id].synergies.Count; i++)
            {
                SynergyData data = augmentManager.augments[id].synergies[i];
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

            for (int i = augmentManager.augments[id].synergies.Count; i < synergyIcons.Count; i++)
            {
                synergyIcons[i].gameObject.SetActive(false);
            }

            if (augmentManager.augments[id].synergies.Count == 0)
            {
                separator.gameObject.SetActive(false);
            }
            else
            {
                separator.gameObject.SetActive(true);
            }
        }

    }
}
