using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AugmentUI : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI descriptions;
    public TextMeshProUGUI rarity;
    public GameObject synergyContainer;
    public GameObject synergyUIPrefab;
    public TextMeshProUGUI cost;
    public Image background;
    public Outline outline;
    public Shop shop;
    public HUDManager HUDManager;
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
        shop.level = 3;
        gameObject.SetActive(false);
        HUDManager.PopulateAugmentListUI(true);
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
            descriptions.richText = true;
            descriptions.text = augmentManager.augments[id].description;
            rarity.text = augmentManager.rarityTexts[augmentManager.augments[id].rarity];
            rarity.color = augmentManager.colors[augmentManager.augments[id].rarity];
            cost.text = "$ " + augmentManager.augments[id].cost;
            outline.effectColor = augmentManager.augments[id].color;

        }

        //for (int i = 0; i < augmentManager.augmentDatas[id].synergies.Count; i++)
        //{
        //    GameObject synergyUI = Instantiate(synergyUIPrefab);
        //    int synergyId = augmentManager.augmentDatas[id].synergies[i].id;
        //    int breakPoint = -1;
        //    for (int j = 0; j < shop.player.synergies.Count; j++)
        //    {
        //        if (shop.player.synergies[j].id == synergyId)
        //        {
        //            breakPoint = shop.player.synergies[j].breakPoint;
        //            break;
        //        }
        //    }
        //    synergyUI.GetComponent<SynergyUI>().Populate(synergyId, breakPoint);
        //    synergyUI.transform.SetParent(synergyContainer.transform);
        //    synergyUI.transform.localScale = new Vector3(1, 1, 1);
        //}
    }
}
